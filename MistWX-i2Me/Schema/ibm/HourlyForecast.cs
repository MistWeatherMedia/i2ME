using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName = "forecast")]
public class HourlyForecast
{
    [XmlElement(ElementName = "class")] public string Class { get; set; }

    [XmlElement(ElementName = "expire_time_gmt")]
    public int ExpireTimeGmt { get; set; }

    [XmlElement(ElementName = "fcst_valid")]
    public int FcstValid { get; set; }

    [XmlElement(ElementName = "fcst_valid_local")]
    public string FcstValidLocal { get; set; }

    [XmlElement(ElementName = "num")] public int Num { get; set; }

    [XmlElement(ElementName = "day_ind")] public string DayInd { get; set; }

    [XmlElement(ElementName = "temp")] public int Temp { get; set; }

    [XmlElement(ElementName = "dewpt")] public int Dewpt { get; set; }

    [XmlElement(ElementName = "hi")] public int Hi { get; set; }

    [XmlElement(ElementName = "wc")] public int Wc { get; set; }

    [XmlElement(ElementName = "feels_like")]
    public int FeelsLike { get; set; }

    [XmlElement(ElementName = "icon_extd")]
    public int IconExtd { get; set; }

    [XmlElement(ElementName = "wxman")] public string Wxman { get; set; }

    [XmlElement(ElementName = "icon_code")]
    public int IconCode { get; set; }

    [XmlElement(ElementName = "dow")] public string Dow { get; set; }

    [XmlElement(ElementName = "phrase_12char")]
    public string Phrase12char { get; set; }

    [XmlElement(ElementName = "phrase_22char")]
    public string Phrase22char { get; set; }

    [XmlElement(ElementName = "phrase_32char")]
    public string Phrase32char { get; set; }

    [XmlElement(ElementName = "subphrase_pt1")]
    public string SubphrasePt1 { get; set; }

    [XmlElement(ElementName = "subphrase_pt2")]
    public string SubphrasePt2 { get; set; }

    [XmlElement(ElementName = "subphrase_pt3")]
    public string SubphrasePt3 { get; set; }

    [XmlElement(ElementName = "pop")] public int Pop { get; set; }

    [XmlElement(ElementName = "precip_type")]
    public string PrecipType { get; set; }

    [XmlElement(ElementName = "qpf")] public double Qpf { get; set; }

    [XmlElement(ElementName = "snow_qpf")] public double SnowQpf { get; set; }

    [XmlElement(ElementName = "rh")] public int Rh { get; set; }

    [XmlElement(ElementName = "wspd")] public int Wspd { get; set; }

    [XmlElement(ElementName = "wdir")] public int Wdir { get; set; }

    [XmlElement(ElementName = "wdir_cardinal")]
    public string WdirCardinal { get; set; }

    [XmlElement(ElementName = "gust")] public object Gust { get; set; }

    [XmlElement(ElementName = "clds")] public int Clds { get; set; }

    [XmlElement(ElementName = "vis")] public double Vis { get; set; }

    [XmlElement(ElementName = "mslp")] public double Mslp { get; set; }

    [XmlElement(ElementName = "uv_index_raw")]
    public double UvIndexRaw { get; set; }

    [XmlElement(ElementName = "uv_index")] public int UvIndex { get; set; }

    [XmlElement(ElementName = "uv_warning")]
    public int UvWarning { get; set; }

    [XmlElement(ElementName = "uv_desc")] public string UvDesc { get; set; }

    [XmlElement(ElementName = "golf_index")]
    public int GolfIndex { get; set; }

    [XmlElement(ElementName = "golf_category")]
    public string GolfCategory { get; set; }

    [XmlElement(ElementName = "severity")] public int Severity { get; set; }
}

[XmlRoot(ElementName = "forecasts")]
public class HourlyForecasts
{
    [XmlElement(ElementName = "forecast")] public List<Forecast> Forecast { get; set; }
}

[XmlRoot(ElementName = "document")]
public class HourlyForecastResponse
{
    [XmlElement(ElementName = "metadata")] public HourlyForecastMetadata Metadata { get; set; }

    [XmlElement(ElementName = "forecasts")]
    public HourlyForecast Forecasts { get; set; }
}