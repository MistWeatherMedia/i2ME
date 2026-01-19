using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName = "imperial")]
public class Imperial
{
    [XmlElement(ElementName = "wspd")] public int Wspd { get; set; }

    [XmlElement(ElementName = "gust")] public object? Gust { get; set; }

    [XmlElement(ElementName = "vis")] public double Vis { get; set; }

    [XmlElement(ElementName = "mslp")] public string? Mslp { get; set; }

    [XmlElement(ElementName = "altimeter")]
    public double Altimeter { get; set; }

    [XmlElement(ElementName = "temp")] public int Temp { get; set; }

    [XmlElement(ElementName = "dewpt")] public int Dewpt { get; set; }

    [XmlElement(ElementName = "rh")] public int Rh { get; set; }

    [XmlElement(ElementName = "wc")] public int Wc { get; set; }

    [XmlElement(ElementName = "hi")] public int Hi { get; set; }

    [XmlElement(ElementName = "temp_change_24hour")]
    public int TempChange24hour { get; set; }

    [XmlElement(ElementName = "temp_max_24hour")]
    public int TempMax24hour { get; set; }

    [XmlElement(ElementName = "temp_min_24hour")]
    public int TempMin24hour { get; set; }

    [XmlElement(ElementName = "pchange")] public double Pchange { get; set; }

    [XmlElement(ElementName = "feels_like")]
    public int FeelsLike { get; set; }

    [XmlElement(ElementName = "snow_1hour")]
    public double Snow1hour { get; set; }

    [XmlElement(ElementName = "snow_6hour")]
    public double Snow6hour { get; set; }

    [XmlElement(ElementName = "snow_24hour")]
    public double Snow24hour { get; set; }

    [XmlElement(ElementName = "snow_mtd")] public double SnowMtd { get; set; }

    [XmlElement(ElementName = "snow_season")]
    public double SnowSeason { get; set; }

    [XmlElement(ElementName = "snow_ytd")] public double SnowYtd { get; set; }

    [XmlElement(ElementName = "snow_2day")]
    public double Snow2day { get; set; }

    [XmlElement(ElementName = "snow_3day")]
    public double Snow3day { get; set; }

    [XmlElement(ElementName = "snow_7day")]
    public double Snow7day { get; set; }

    [XmlElement(ElementName = "ceiling")] public object? Ceiling { get; set; }

    [XmlElement(ElementName = "precip_1hour")]
    public double Precip1hour { get; set; }

    [XmlElement(ElementName = "precip_6hour")]
    public double Precip6hour { get; set; }

    [XmlElement(ElementName = "precip_24hour")]
    public double Precip24hour { get; set; }

    [XmlElement(ElementName = "precip_mtd")]
    public double PrecipMtd { get; set; }

    [XmlElement(ElementName = "precip_ytd")]
    public double PrecipYtd { get; set; }

    [XmlElement(ElementName = "precip_2day")]
    public double Precip2day { get; set; }

    [XmlElement(ElementName = "precip_3day")]
    public double Precip3day { get; set; }

    [XmlElement(ElementName = "precip_7day")]
    public double Precip7day { get; set; }

    [XmlElement(ElementName = "obs_qualifier_100char")]
    public object? ObsQualifier100char { get; set; }

    [XmlElement(ElementName = "obs_qualifier_50char")]
    public object? ObsQualifier50char { get; set; }

    [XmlElement(ElementName = "obs_qualifier_32char")]
    public object? ObsQualifier32char { get; set; }
}

[XmlRoot(ElementName = "observation")]
public class Observation
{
    [XmlElement(ElementName = "class")] public string? Class { get; set; }

    [XmlElement(ElementName = "expire_time_gmt")]
    public int ExpireTimeGmt { get; set; }

    [XmlElement(ElementName = "obs_time")] public int ObsTime { get; set; }

    [XmlElement(ElementName = "obs_time_local")]
    public string? ObsTimeLocal { get; set; }

    [XmlElement(ElementName = "wdir")] public int Wdir { get; set; }

    [XmlElement(ElementName = "icon_code")]
    public int IconCode { get; set; }

    [XmlElement(ElementName = "icon_extd")]
    public int IconExtd { get; set; }

    [XmlElement(ElementName = "sunrise")] public string? Sunrise { get; set; }

    [XmlElement(ElementName = "sunset")] public string? Sunset { get; set; }

    [XmlElement(ElementName = "day_ind")] public string? DayInd { get; set; }

    [XmlElement(ElementName = "uv_index")] public int UvIndex { get; set; }

    [XmlElement(ElementName = "uv_warning")]
    public int UvWarning { get; set; }

    [XmlElement(ElementName = "wxman")] public string? Wxman { get; set; }

    [XmlElement(ElementName = "obs_qualifier_code")]
    public object? ObsQualifierCode { get; set; }

    [XmlElement(ElementName = "ptend_code")]
    public int PtendCode { get; set; }

    [XmlElement(ElementName = "dow")] public string? Dow { get; set; }

    [XmlElement(ElementName = "wdir_cardinal")]
    public string? WdirCardinal { get; set; }

    [XmlElement(ElementName = "uv_desc")] public string? UvDesc { get; set; }

    [XmlElement(ElementName = "phrase_12char")]
    public string? Phrase12char { get; set; }

    [XmlElement(ElementName = "phrase_22char")]
    public string? Phrase22char { get; set; }

    [XmlElement(ElementName = "phrase_32char")]
    public string? Phrase32char { get; set; }

    [XmlElement(ElementName = "ptend_desc")]
    public string? PtendDesc { get; set; }

    [XmlElement(ElementName = "sky_cover")]
    public string? SkyCover { get; set; }

    [XmlElement(ElementName = "clds")] public string? Clds { get; set; }

    [XmlElement(ElementName = "obs_qualifier_severity")]
    public object? ObsQualifierSeverity { get; set; }

    [XmlElement(ElementName = "vocal_key")]
    public string? VocalKey { get; set; }

    [XmlElement(ElementName = "imperial")] public Imperial? Imperial { get; set; }
}

[XmlRoot(ElementName = "currentObservationsResponse")]
public class CurrentObservationsResponse
{
    [XmlElement(ElementName = "metadata")] public CurrentObservationsMetadata? Metadata { get; set; }

    [XmlElement(ElementName = "observation")]
    public Observation? Observation { get; set; }
}