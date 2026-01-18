namespace MistWX_i2Me.Schema.ibm;

public class RadarImageryCoordinates
{
    public string? lat { get; set; }

    public string? lng { get; set; }
}

public class RadarImageryBoundingBox
{
    public RadarImageryCoordinates? tl { get; set; }

    public RadarImageryCoordinates? br { get; set; }
}

public class RadarImageryTimestamps
{
    public int ts { get; set; }
}

public class RadarImageryLayer
{
    public int nativeZoom { get; set; }

    public int maxZoom { get; set; }

    public RadarImageryBoundingBox? bb { get; set; }

    public List<RadarImageryTimestamps>? series { get; set; }
}

public class RadarImagerySeriesInfo
{
    public RadarImageryLayer? sat { get; set; }
    public RadarImageryLayer? twcRadarMosaic { get; set; }
}

public class RadarImageryResponse
{
    public RadarImagerySeriesInfo? seriesInfo { get; set; }
}