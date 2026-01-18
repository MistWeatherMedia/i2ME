using MistWX_i2Me.Schema.ibm;
using NetVips;
using System.Text.Encodings.Web;
using System.Text.Json;
using MistWX_i2Me;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Cryptography.X509Certificates;
using MistWX_i2Me.API.Products;
using System.Threading;
using System.Xml.Xsl;
using MistWX_i2Me.Communication;
using Dapper;
namespace MistWX_i2Me.RecordGeneration;

public class Point<T>
{
    public Point(T x, T y)
    {
        this.X = x;
        this.Y = y;
    }
    public T X { get; set; }
    public T Y { get; set; }
}
public class ImageBoundaries
{
    public float LowerLeftLong { get; set; }
    public float LowerLeftLat { get; set; }
    public float UpperRightLong { get; set; }
    public float UpperRightLat { get; set; }
    public float VerticalAdjustment { get; set; }
    public int OriginalImageWidth { get; set; }
    public int OriginalImageHeight { get; set; }
    public int MaxImages { get; set; }
    public int Gap { get; set; }
    public int ImagesInterval { get; set; }
    public int Expiration { get; set; }
    public int DeletePadding { get; set; }
    public string? FileNameDateFormat { get; set; }

    public Point<float> GrabUpperRight()
    {
        return new Point<float>(UpperRightLat, LowerLeftLong);
    }

    public Point<float> GrabUpperLeft()
    {
        return new Point<float>(LowerLeftLat, UpperRightLong);
    }

    public Point<float> GrabLowerRight()
    {
        return new Point<float>(UpperRightLat, LowerLeftLong);
    }

    public Point<float> GrabLowerLeft()
    {
        return new Point<float>(LowerLeftLat, LowerLeftLong);
    }
}
public class TileImageBounds
{
    public int UpperLeftX { get; set; }
    public int UpperLeftY { get; set; }
    public int LowerRightX { get; set; }
    public int LowerRightY { get; set; }
    public int XStart { get; set; }
    public int XEnd { get; set; }
    public int YStart { get; set; }
    public int YEnd { get; set; }
    public int XTiles { get; set; }
    public int YTiles { get; set; }
    public int ImageWidth { get; set; }
    public int ImageHeight { get; set; }
}
public class RadarProcess
{
    public async Task Run(string radar_type, int[] timestamps, UdpSender sender)
    {
        Log.Info("Creating radar frames...");

        // check if maps exist
        string mapDirPath = Path.Combine(AppContext.BaseDirectory, "temp", "maps");
        string mapTypeDirPath = Path.Combine(mapDirPath, radar_type);

        if (!Directory.Exists(mapDirPath))
        {
            Directory.CreateDirectory(mapDirPath);
            if (!Directory.Exists(mapTypeDirPath))
            {
                Directory.CreateDirectory(mapTypeDirPath);
            }
        }

        List<Point<int>> combinedCoords = new List<Point<int>>();

        ImageBoundaries boundaries = BoundariesFromJSON(radar_type);
        Point<float> upperRight = boundaries.GrabUpperRight();
        Point<float> lowerLeft = boundaries.GrabLowerLeft();
        Point<float> upperLeft = boundaries.GrabUpperLeft();
        Point<float> lowerRight = boundaries.GrabLowerRight();

        TileImageBounds tileImgBounds = CalculateBounds(upperRight, lowerLeft, upperLeft, lowerRight);

        // Calculate frame tile coords.
        foreach (int y in Enumerable.Range(tileImgBounds.YStart, tileImgBounds.YEnd)) {
            if (y <= tileImgBounds.YEnd)
            {
                foreach (int x in Enumerable.Range(tileImgBounds.XStart, tileImgBounds.XEnd))
                {
                    if (x <= tileImgBounds.XEnd)
                    {
                        combinedCoords.Add(new Point<int>(x, y));
                    }
                }
            }

        }

        Dictionary<int, List<Task<Image>>> images = new();
        // Grab all images
        foreach (int ts in timestamps)
        {
            foreach (Point<int> coords in combinedCoords)
            {
                images[ts].Add(new RadarTileProduct(ts, coords.X, coords.Y, radar_type).Populate());
            }
        }

        // List of radar frames.
        Dictionary<int, Image> radarFrames = new();

        // Generate all radar frames.
        foreach (int ts in timestamps)
        {
            radarFrames[ts] = Image.Black(tileImgBounds.ImageWidth, tileImgBounds.ImageHeight);
        }

        // List of radar generation tasks.
        List<Task> taskList = new();

        foreach ((int ts, List<Task<Image>> imageList) in images)
        {
            taskList.Add(ProcessRadarFrame(await Task.WhenAll(imageList), radarFrames[ts], combinedCoords.ToArray(), ts, tileImgBounds, mapTypeDirPath, sender, radar_type));
        }

        await Task.WhenAll(taskList);
    }

    public static async Task ProcessRadarFrame(Image[] imgs, Image frame, Point<int>[] coords, int ts, TileImageBounds tileImgBounds, string dir_path, UdpSender sender, string radar_type)
    {
        // Composite all tiles to frame.
        int[] xSet = coords.Select(p => (p.X - tileImgBounds.XStart) * 256).ToArray();
        int[] ySet = coords.Select(p => (p.Y - tileImgBounds.YStart) * 256).ToArray();
        frame.Composite(imgs, Enumerable.Repeat(Enums.BlendMode.Clear, imgs.Length).ToArray(), x: xSet, y: ySet);
        // Frame recolor
        frame = PaletteConvert(frame);
        
        // Save frame.
        string framePath = Path.Combine(dir_path, $"{ts}.tiff");
        frame.WriteToFile(framePath);

        // Split radar type.
        string[] splitRadarType = radar_type.Split("-");

        // Send command to i2 to ingest radar frame.
        sender.SendFile(framePath, $"storePriorityImage(FileExtension=.tiff, Location={splitRadarType[1]},ImageType={splitRadarType[0]},IssueTime={ts})");
    }

    public static Image PaletteConvert(Image img)
    {
        int[][] rainColors = new int[][] {
            new int[]{64,204,85}, // Lightest green
            new int[]{0,153,0}, // Medium green
            new int[]{0,102,0}, // Darkest green
            new int[]{191,204,85}, // Yellow
            new int[]{191,153,0}, // Orange
            new int[]{255,51,0}, // ...
            new int[]{191,51,0}, // Red
            new int[]{64,0,0}, // Dark red
        };

        int[][] mixColors = new int[][] {
            new int[]{253,130,215}, // Light purple
            new int[]{208,94,176}, // ...
            new int[]{190,70,150}, // ...
            new int[]{170,50,130}, // Dark purple
        };

        int[][] snowColors = new int[][] {
            new int[]{150,150,150}, // Dark grey
            new int[]{180,180,180}, // Light grey
            new int[]{210,210,210}, // Grey
            new int[]{230,230,230}, // White
        };

        // Time to replace all the colors.
        // Replace rain colors
        img = img.Equal(new int[] {99,235,99}).BandAnd().Ifthenelse(rainColors[0], img)
                 .Equal(new int[] {28,158,52}).BandAnd().Ifthenelse(rainColors[1], img)
                 .Equal(new int[] {0,63,0}).BandAnd().Ifthenelse(rainColors[2], img)
                 .Equal(new int[] {251,235,2}).BandAnd().Ifthenelse(rainColors[3], img)
                 .Equal(new int[] {238,109,2}).BandAnd().Ifthenelse(rainColors[4], img)
                 .Equal(new int[] {210,11,6}).BandAnd().Ifthenelse(rainColors[5], img)
                 .Equal(new int[] {169,5,3}).BandAnd().Ifthenelse(rainColors[6], img)
                 .Equal(new int[] {128,0,0}).BandAnd().Ifthenelse(rainColors[7], img)
                 // Replace mix colors
                 .Equal(new int[] {255,160,207}).BandAnd().Ifthenelse(mixColors[0], img)
                 .Equal(new int[] {217,110,163}).BandAnd().Ifthenelse(mixColors[1], img)
                 .Equal(new int[] {192,77,134}).BandAnd().Ifthenelse(mixColors[2], img)
                 .Equal(new int[] {174,51,112}).BandAnd().Ifthenelse(mixColors[3], img)
                 .Equal(new int[] {146,13,79}).BandAnd().Ifthenelse(mixColors[3], img)
                 // Replace snow colors
                 .Equal(new int[] {138,248,255}).BandAnd().Ifthenelse(snowColors[0], img)
                 .Equal(new int[] {110,203,212}).BandAnd().Ifthenelse(snowColors[1], img)
                 .Equal(new int[] {82,159,170}).BandAnd().Ifthenelse(snowColors[2], img)
                 .Equal(new int[] {40,93,106}).BandAnd().Ifthenelse(snowColors[3], img)
                 .Equal(new int[] {13,49,64}).BandAnd().Ifthenelse(snowColors[3], img);

        return img;
    }
    public static Point<int> WorldCoordToTile(Point<float> coord)
    {
        int scale = 1 << 6;

        return new Point<int>((int)Math.Floor(coord.X * scale / 255), (int)Math.Floor(coord.Y * scale / 255));
    }
    public static Point<int> WorldCoordToPixel(Point<float> coord)
    {
        int scale = 1 << 6;

        return new Point<int>((int)Math.Floor(coord.X * scale), (int)Math.Floor(coord.Y * scale));
    }
    public static Point<float> LatLongProject(float lat, float lon)
    {
        double sin_y = Math.Min(Math.Max(Math.Sin(lat * Math.PI / 180), -0.9999), 0.9999);

        return new Point<float>((float)(256 * (0.5 + lon / 360)), (float)(256 * (0.5 - Math.Log(1 + sin_y) / (1- sin_y) / (4*Math.PI))));
    }
    public static TileImageBounds CalculateBounds(Point<float> upper_right, Point<float> lower_left, Point<float> upper_left, Point<float> lower_right)
    {
        TileImageBounds bounds = new TileImageBounds();
        Point<int> UpperRightTile = WorldCoordToTile(LatLongProject(upper_right.X, upper_right.Y));
        Point<int> LowerRightTile = WorldCoordToTile(LatLongProject(lower_right.X, lower_right.Y));
        Point<int> UpperLeftTile = WorldCoordToTile(LatLongProject(upper_left.X, upper_left.Y));
        Point<int> LowerLeftTile = WorldCoordToTile(LatLongProject(lower_left.X, lower_right.Y));

        Point<int> UpperLeftPixels = WorldCoordToPixel(LatLongProject(upper_left.X, upper_left.Y));
        Point<int> LowerRightPixels = WorldCoordToPixel(LatLongProject(lower_right.X, lower_right.Y));

        Point<int> UpperLeft = new Point<int>(UpperLeftPixels.X - UpperLeftTile.X * 256, UpperLeftPixels.Y - UpperLeftTile.Y * 256);
        Point<int> LowerRight = new Point<int>(LowerRightPixels.X - LowerRightTile.X * 256, LowerRightPixels.Y - LowerRightTile.Y * 256);

        bounds.UpperLeftX = UpperLeft.X;
        bounds.UpperLeftY = UpperLeft.Y;
        bounds.LowerRightX = LowerRight.X;
        bounds.LowerRightY = LowerRight.Y;
        bounds.XStart = UpperLeftTile.X;
        bounds.XEnd = UpperRightTile.X;
        bounds.YStart = UpperLeftTile.Y;
        bounds.YEnd = LowerLeftTile.Y;
        bounds.ImageWidth = 256 * (UpperRightTile.X - UpperLeftTile.X + 1);
        bounds.ImageHeight = 256 * (LowerLeftTile.Y - UpperLeftTile.Y + 1);

        return bounds;
    }
    public static ImageBoundaries BoundariesFromJSON(string maptype)
    {
        StreamReader reader = new StreamReader(Path.Combine(AppContext.BaseDirectory, "Custom", "ImageSequenceDefs.json"));
        string? json = reader.ToString();

        if (json != null)
        {
            Dictionary<string, ImageBoundaries>? values = JsonSerializer.Deserialize<Dictionary<string, ImageBoundaries>>(json);
            if (values != null)
            {
                return values[maptype];
            } else {
                MistWX_i2Me.Log.Warning("There was a problem parsing the ImageSequenceDefs.");
                return new ImageBoundaries();
            }
        } else {
            MistWX_i2Me.Log.Warning("ImageSequenceDefs is null!");
            return new ImageBoundaries();
        }
    }
}