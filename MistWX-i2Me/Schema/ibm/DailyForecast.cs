using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName = "forecast")]
public class Forecast
{
    [XmlElement(ElementName = "class")] public string? Class { get; set; }

    [XmlElement(ElementName = "expire_time_gmt")]
    public int ExpireTimeGmt { get; set; }

    [XmlElement(ElementName = "fcst_valid")]
    public int FcstValid { get; set; }

    [XmlElement(ElementName = "fcst_valid_local")]
    public string? FcstValidLocal { get; set; }

    [XmlElement(ElementName = "num")] public int Num { get; set; }

    [XmlElement(ElementName = "max_temp")] public string? MaxTemp { get; set; }

    [XmlElement(ElementName = "min_temp")] public int MinTemp { get; set; }

    [XmlElement(ElementName = "torcon")] public string? Torcon { get; set; }

    [XmlElement(ElementName = "stormcon")] public string? Stormcon { get; set; }

    [XmlElement(ElementName = "blurb")] public string? Blurb { get; set; }

    [XmlElement(ElementName = "blurb_author")]
    public string? BlurbAuthor { get; set; }

    [XmlElement(ElementName = "lunar_phase_day")]
    public int LunarPhaseDay { get; set; }

    [XmlElement(ElementName = "dow")] public string? Dow { get; set; }

    [XmlElement(ElementName = "lunar_phase")]
    public string? LunarPhase { get; set; }

    [XmlElement(ElementName = "lunar_phase_code")]
    public string? LunarPhaseCode { get; set; }

    [XmlElement(ElementName = "sunrise")] public string? Sunrise { get; set; }

    [XmlElement(ElementName = "sunset")] public string? Sunset { get; set; }

    [XmlElement(ElementName = "moonrise")] public string? Moonrise { get; set; }

    [XmlElement(ElementName = "moonset")] public string? Moonset { get; set; }

    [XmlElement(ElementName = "qualifier_code")]
    public string? QualifierCode { get; set; }

    [XmlElement(ElementName = "qualifier")]
    public string? Qualifier { get; set; }

    [XmlElement(ElementName = "narrative")]
    public string? Narrative { get; set; }

    [XmlElement(ElementName = "qpf")] public double Qpf { get; set; }

    [XmlElement(ElementName = "snow_qpf")] public double SnowQpf { get; set; }

    [XmlElement(ElementName = "snow_range")]
    public string? SnowRange { get; set; }

    [XmlElement(ElementName = "snow_phrase")]
    public string? SnowPhrase { get; set; }

    [XmlElement(ElementName = "snow_code")]
    public string? SnowCode { get; set; }

    [XmlElement(ElementName = "night")] public Day? Night { get; set; }

    [XmlElement(ElementName = "day")] public Day? Day { get; set; }
}

[XmlRoot(ElementName = "day")]
public class Day
{
    [XmlElement(ElementName = "fcst_valid")]
    public int FcstValid { get; set; }

    [XmlElement(ElementName = "fcst_valid_local")]
    public string? FcstValidLocal { get; set; }

    [XmlElement(ElementName = "day_ind")] public string? DayInd { get; set; }

    [XmlElement(ElementName = "thunder_enum")]
    public int ThunderEnum { get; set; }

    [XmlElement(ElementName = "daypart_name")]
    public string? DaypartName { get; set; }

    [XmlElement(ElementName = "long_daypart_name")]
    public string? LongDaypartName { get; set; }

    [XmlElement(ElementName = "alt_daypart_name")]
    public string? AltDaypartName { get; set; }

    [XmlElement(ElementName = "thunder_enum_phrase")]
    public string? ThunderEnumPhrase { get; set; }

    [XmlElement(ElementName = "num")] public int Num { get; set; }

    [XmlElement(ElementName = "temp")] public int Temp { get; set; }

    [XmlElement(ElementName = "hi")] public int Hi { get; set; }

    [XmlElement(ElementName = "wc")] public int Wc { get; set; }

    [XmlElement(ElementName = "pop")] public int Pop { get; set; }

    [XmlElement(ElementName = "icon_extd")]
    public int IconExtd { get; set; }

    [XmlElement(ElementName = "icon_code")]
    public int IconCode { get; set; }

    [XmlElement(ElementName = "wxman")] public string? Wxman { get; set; }

    [XmlElement(ElementName = "phrase_12char")]
    public string? Phrase12char { get; set; }

    [XmlElement(ElementName = "phrase_22char")]
    public string? Phrase22char { get; set; }

    [XmlElement(ElementName = "phrase_32char")]
    public string? Phrase32char { get; set; }

    [XmlElement(ElementName = "subphrase_pt1")]
    public string? SubphrasePt1 { get; set; }

    [XmlElement(ElementName = "subphrase_pt2")]
    public string? SubphrasePt2 { get; set; }

    [XmlElement(ElementName = "subphrase_pt3")]
    public string? SubphrasePt3 { get; set; }

    [XmlElement(ElementName = "precip_type")]
    public string? PrecipType { get; set; }

    [XmlElement(ElementName = "rh")] public int Rh { get; set; }

    [XmlElement(ElementName = "wspd")] public int Wspd { get; set; }

    [XmlElement(ElementName = "wdir")] public int Wdir { get; set; }

    [XmlElement(ElementName = "wdir_cardinal")]
    public string? WdirCardinal { get; set; }

    [XmlElement(ElementName = "clds")] public int Clds { get; set; }

    [XmlElement(ElementName = "pop_phrase")]
    public string? PopPhrase { get; set; }

    [XmlElement(ElementName = "temp_phrase")]
    public string? TempPhrase { get; set; }

    [XmlElement(ElementName = "accumulation_phrase")]
    public string? AccumulationPhrase { get; set; }

    [XmlElement(ElementName = "wind_phrase")]
    public string? WindPhrase { get; set; }

    [XmlElement(ElementName = "shortcast")]
    public string? Shortcast { get; set; }

    [XmlElement(ElementName = "narrative")]
    public string? Narrative { get; set; }

    [XmlElement(ElementName = "qpf")] public double Qpf { get; set; }

    [XmlElement(ElementName = "snow_qpf")] public double SnowQpf { get; set; }

    [XmlElement(ElementName = "snow_range")]
    public string? SnowRange { get; set; }

    [XmlElement(ElementName = "snow_phrase")]
    public string? SnowPhrase { get; set; }

    [XmlElement(ElementName = "snow_code")]
    public string? SnowCode { get; set; }

    [XmlElement(ElementName = "vocal_key")]
    public string? VocalKey { get; set; }

    [XmlElement(ElementName = "qualifier_code")]
    public string? QualifierCode { get; set; }

    [XmlElement(ElementName = "qualifier")]
    public string? Qualifier { get; set; }

    [XmlElement(ElementName = "uv_index_raw")]
    public double UvIndexRaw { get; set; }

    [XmlElement(ElementName = "uv_index")] public int UvIndex { get; set; }

    [XmlElement(ElementName = "uv_warning")]
    public int UvWarning { get; set; }

    [XmlElement(ElementName = "uv_desc")] public string? UvDesc { get; set; }

    [XmlElement(ElementName = "golf_index")]
    public string? GolfIndex { get; set; }

    [XmlElement(ElementName = "golf_category")]
    public string? GolfCategory { get; set; }
}

[XmlRoot(ElementName = "forecasts")]
public class Forecasts
{
    [XmlElement(ElementName = "forecast")] public List<Forecast>? Forecast { get; set; }
}

[XmlRoot(ElementName = "dailyForecastResponse")]
public class DailyForecastResponse
{
    [XmlElement(ElementName = "metadata")] public DailyForecastMetadata? Metadata { get; set; }

    [XmlElement(ElementName = "forecasts")]
    public Forecasts? Forecasts { get; set; }
}