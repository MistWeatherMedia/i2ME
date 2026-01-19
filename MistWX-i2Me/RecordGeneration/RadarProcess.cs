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
using System.Diagnostics;
using System.Xml.Serialization;
using MistWX_i2Me.Schema.twc;
using MistWX_i2Me.Schema.System;
using System.ComponentModel.DataAnnotations;
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
    public async Task Run(string radar_type, int[] timestamps, UdpSender sender, string radar_product)
    {
        Log.Info("Creating radar frames...");


        // check if maps exist
        string mapDirPath = Path.Combine(AppContext.BaseDirectory, "temp", "maps");
        string mapTypeDirPath = Path.Combine(mapDirPath, radar_type);

        if (!Directory.Exists(mapDirPath))
        {
            Directory.CreateDirectory(mapDirPath);
            Log.Debug("Map dir doesn't exist");
            
        } else
        {
            Log.Debug("Passed map directory checks!");
        }
        if (!Directory.Exists(mapTypeDirPath))
        {
            Log.Debug("Map type dir doesn't exist");
            Directory.CreateDirectory(mapTypeDirPath);
        }else
        {
            Log.Debug("Passed map type directory checks!");
        }

        DirectoryInfo mapTypeDir = new(mapTypeDirPath);
        
        // Delete all frames from the maptype.
        foreach(System.IO.FileInfo file in mapTypeDir.GetFiles()) file.Delete();
        foreach(System.IO.DirectoryInfo subDirectory in mapTypeDir.GetDirectories()) subDirectory.Delete(true);
        

        ImageSequenceDef boundaries = BoundariesFromJSON(radar_type);
        Log.Debug("Imported boundaries from JSON");
        Point<double> upperRight = boundaries.GrabUpperRight();
        Point<double> lowerLeft = boundaries.GrabLowerLeft();
        Point<double> upperLeft = boundaries.GrabUpperLeft();
        Point<double> lowerRight = boundaries.GrabLowerRight();

        // slice timestamps
        timestamps = new ArraySegment<int>(timestamps, 0, boundaries.MaxImages - 1).ToArray();
        List<int> tempTses = new();

        foreach (int ts in timestamps)
        {
            if (ts % boundaries.ImagesInterval != 0)
            {
                Log.Debug($"Ignoring {ts}, not at correct frame interval");
                continue;
            }

            if (ts < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - (boundaries.Expiration)) 
            {
                Log.Debug($"Ignoring {ts}, expired");
            }
            tempTses.Add(ts);
            
        }
        timestamps = tempTses.ToArray();
        // Calculate bounds
        TileImageBounds bounds = CalculateBounds(upperRight,upperLeft,lowerLeft,lowerRight);

        // Calculate frame tile coords.
        Log.Debug("Calculating frame tile coordinates");

        List<Point<int>> combinedCoords = new();

        //throw new Exception($"YStart: {bounds.YStart}, YEnd: {bounds.YEnd}, XStart: {bounds.XStart}, XEnd: {bounds.XEnd}");
        
        int GrabXStart = bounds.XStart;
        int GrabYStart = bounds.YStart;
        int GrabXEnd = bounds.XEnd;
        int GrabYEnd = bounds.YEnd;

        // If LocalRadar is turned on, set range to be nearby the center of the map in terms of tiles.
        if (Config.config.RadarConfiguration.LocalRadar)
        {
            // Grab MachineProductConfig.
            XmlSerializer serializer = new(typeof(MachineProductConfig));
            StreamReader reader = new(Path.Combine(AppContext.BaseDirectory, "MachineProductConfig.xml"));
            MachineProductConfig? mpc = null;
            if (reader != null)
            {
                mpc = (MachineProductConfig?)serializer.Deserialize(reader);
            }
            
            if (mpc != null)
            {
                if (mpc.ConfigDef != null)
                {
                    if (mpc.ConfigDef.ConfigItems != null)
                    {
                        if (mpc.ConfigDef.ConfigItems.ConfigItem != null)
                        {
                            foreach (ConfigItem cfgItm in mpc.ConfigDef.ConfigItems.ConfigItem)
                            {
                                if (cfgItm.Key == "PrimaryLatitudeLongitude")
                                {
                                    Point<double> latlong = new((double)0.0, (double)0.0);
                                    string[] split = (cfgItm.Value ?? "-").Split("-");
                                    string split0Dir = split[0].Substring(0, 1);
                                    string split1Dir = split[1].Substring(0, 1);

                                    if (split0Dir == "W")
                                    {
                                        latlong.Y = Convert.ToDouble($"-{split[0].Substring(1)}");
                                    } else
                                    {
                                        latlong.Y = Convert.ToDouble(split[0].Substring(1));
                                    }

                                    if (split1Dir == "N")
                                    {
                                        latlong.X = Convert.ToDouble(split[1].Substring(1));
                                    } else
                                    {
                                        latlong.X = Convert.ToDouble($"-{split[1].Substring(1)}");
                                    }

                                    Point<int> tile = LongLatToTile(latlong);
                                    GrabXStart = tile.X - Config.config.RadarConfiguration.LocalRadarRadius;
                                    GrabXEnd = tile.X + Config.config.RadarConfiguration.LocalRadarRadius;
                                    GrabYStart = tile.Y - Config.config.RadarConfiguration.LocalRadarRadius;
                                    GrabYEnd = tile.Y + Config.config.RadarConfiguration.LocalRadarRadius;
                                    //throw new Exception($"{GrabXStart} \n {GrabXEnd} \n {GrabYStart} \n {GrabYEnd}");
                                    break;
                                } 
                            }
                        }
                    }
                }
                
            } else
            {
                Log.Warning("Could not read MachineProductCfg!");
            }
        }

        foreach (int y in Enumerable.Range(GrabYStart, GrabYEnd))
        {
            if (y <= GrabYEnd) {
                foreach (int x in Enumerable.Range(GrabXStart, GrabXEnd))
                {
                    if (x  <= GrabXEnd)
                    {
                        combinedCoords.Add(new Point<int>(x, y));
                    }
                }
            }
        }

        Dictionary<int, Task<Image[]>> images = new();
        // Grab all images
        foreach (int ts in timestamps)
        {
            List<Task<Image>> tileTaskList = new();
            foreach (Point<int> coords in combinedCoords)
            {
                Log.Debug($"Added new request to download frame for timestamp {ts}");
                tileTaskList.Add(new RadarTileProduct(ts, coords.X, coords.Y, radar_product).Populate());
            }
            images[ts] = Task.WhenAll(tileTaskList); 
        }
        Dictionary<int, Image[]> actualImages = new();
        foreach ((int ts, Task<Image[]> imageTask) in images)
        {
            actualImages[ts] = await imageTask;
        }
        Log.Debug("Sent requests to grab all images");

        // List of radar frames.
        Dictionary<int, Image> radarFrames = new();

        // Generate all radar frames.
        foreach (int ts in timestamps)
        {
            radarFrames[ts] = Image.Black(Math.Abs(bounds.ImageWidth), Math.Abs(bounds.ImageHeight));
            Log.Debug($"Generated new frame for {ts}");
        }

        // List of radar generation tasks.
        List<Task> taskList = new();

        foreach ((int ts, Image[] imageList) in actualImages)
        {
            taskList.Add(ProcessRadarFrame(imageList, radarFrames[ts], combinedCoords.ToArray(), ts, new Point<int>(bounds.XStart, bounds.XEnd), mapTypeDirPath, sender, radar_type));
            Log.Debug($"Added task to process radar frame {ts}");
        }

        await Task.WhenAll(taskList);
        Log.Debug("Awaited all tasks");
    }

    public static async Task ProcessRadarFrame(Image[] imgs, Image frame, Point<int>[] coords, int ts, Point<int> tileStart, string dir_path, UdpSender sender, string radar_type)
    {
        Log.Debug($"Processing frame {ts}");

        // Composite all tiles to frame.
        int[] xSet = coords.Select(p => Math.Abs((p.X - tileStart.X) * 256)).ToArray();
        int[] ySet = coords.Select(p => Math.Abs((p.Y - tileStart.Y) * 256)).ToArray();

        frame = frame.Composite(imgs, new Enums.BlendMode[]{Enums.BlendMode.Add}, xSet, ySet).Flatten();
        Log.Debug($"Frame {ts} stitched");
        // Frame recolor
        frame = PaletteConvert(frame);
        Log.Debug($"Frame {ts} recolored");
        
        // Save frame.
        string framePath = Path.Combine(dir_path, $"{ts}.tiff");
        frame.WriteToFile(framePath);
        Log.Debug($"Frame {ts} saved to {framePath}");

        // Split radar type.
        string[] splitRadarType = radar_type.Split("-");

        // Send command to i2 to ingest radar frame.
        sender.SendFile(framePath, $"storePriorityImage(FileExtension=.tiff, Location={splitRadarType[1]},ImageType={splitRadarType[0]},IssueTime={ts})");
        Log.Debug($"Frame {ts} sent");
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

    public static Point<double> LatLongProject(Point<double> point)
    {
        double sin_y = Math.Sin(point.X * Math.PI / 180);
        sin_y = Math.Min(Math.Max(sin_y, -0.9999), 0.9999);
        double x = (double)(256 * (0.5 + point.Y / 360));
        double y = (double)(256 * (0.5 - Math.Log((1 + sin_y) / (1 - sin_y)) / (4 * Math.PI)));

        return new Point<double>(x, y);
    }
    
    public static Point<int> LongLatToTile(Point<double> longlat)
    {
        int scale = 1 << 6;
        Point<double> coords = LatLongProject(longlat);

        return new Point<int>(
            (int)Math.Floor(coords.X * scale / 255),
            (int)Math.Floor(coords.Y * scale / 255)
        );
    }

    public static Point<int> CoordToTile(Point<double> longlat)
    {
        int scale = 1 << 6;

        return new Point<int>(
            (int)Math.Floor(longlat.X * scale / 256),
            (int)Math.Floor(longlat.Y * scale / 256)
        );
    }

    public static Point<int> CoordToPixel(Point<double> longlat)
    {
        int scale = 1 << 6;

        return new Point<int>(
            (int)Math.Floor(longlat.X * scale),
            (int)Math.Floor(longlat.Y * scale)
        );
    }

    class TileImageBounds
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

    static TileImageBounds CalculateBounds(Point<double> upper_right, Point<double> upper_left, Point<double> lower_left, Point<double> lower_right)
    {
        Point<int> UpperRightTile = CoordToTile(LatLongProject(upper_right));
        Point<int> UpperLeftTile = CoordToTile(LatLongProject(upper_left));
        Point<int> LowerRightTile = CoordToTile(LatLongProject(lower_right));
        Point<int> LowerLeftTile = CoordToTile(LatLongProject(lower_left));

        Point<int> UpperLeftPixels = CoordToPixel(LatLongProject(upper_left));
        Point<int> LowerRightPixels = CoordToPixel(LatLongProject(lower_right));

        int UpperLeftX = UpperLeftPixels.X - UpperLeftTile.X * 256;
        int UpperLeftY = UpperLeftPixels.Y - UpperLeftTile.Y * 256;
        int LowerRightX = LowerRightPixels.X - UpperLeftTile.X * 256;
        int LowerRightY = LowerRightPixels.Y - UpperLeftTile.Y * 256;

        int XStart = UpperLeftTile.X;
        int XEnd = UpperRightTile.X;
        int YStart = UpperLeftTile.Y;
        int YEnd = LowerLeftTile.Y;

        int XTiles = XEnd - XStart;
        int YTiles = YEnd - YStart;

        int ImageWidth = 256 * (XTiles + 1);
        int ImageHeight = 256 * (YTiles + 1);

        TileImageBounds imageBounds = new();
        imageBounds.UpperLeftX = UpperLeftX;
        imageBounds.UpperLeftY = UpperLeftY;
        imageBounds.LowerRightX = LowerRightX;
        imageBounds.LowerRightY = LowerRightY;
        imageBounds.XStart = XStart;
        imageBounds.XEnd = XEnd;
        imageBounds.YStart = YStart;
        imageBounds.YEnd = YEnd;
        imageBounds.XTiles = XTiles;
        imageBounds.YTiles = YTiles;
        imageBounds.ImageWidth = ImageWidth;
        imageBounds.ImageHeight = ImageHeight;

        return imageBounds;
    }

    static ImageSequenceDef BoundariesFromJSON(string maptype)
    {
        Log.Debug("Looking for boundaries...");
        StreamReader reader = new StreamReader(Path.Combine(AppContext.BaseDirectory, "Custom", "Config", "ImageSequenceDefs.xml"));
        XmlSerializer serializer = new XmlSerializer(typeof(ImageSequenceDefs));
        ImageSequenceDefs? output = (ImageSequenceDefs?)serializer.Deserialize(reader);

        Log.Debug("Boundaries deserialized.");
        if (output != null)
        {
            if (output.ImageSequenceDef != null)
            {
                foreach (ImageSequenceDef imageSequenceDef in output.ImageSequenceDef)
                {
                    string tempMT = $"{imageSequenceDef.type}-{imageSequenceDef.area}";
                    if (maptype == tempMT)
                    {
                        return imageSequenceDef;
                    } 
                }
                Log.Warning("Cannot find the ImageSequenceDef!");
                return new ImageSequenceDef();
        
            } else {
                Log.Warning("ImageSequenceDefs is null!");
                return new ImageSequenceDef();
            }
            
        } else {
            Log.Warning("ImageSequenceDefs is null!");
            return new ImageSequenceDef();
        }
    }
}