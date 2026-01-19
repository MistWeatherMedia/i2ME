using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName = "tide")]
public class Tide
{
    [XmlElement(ElementName = "class")] public string? Class { get; set; }
    [XmlElement(ElementName = "tide_id")] public int TideId { get; set; }
    [XmlElement(ElementName = "station_id")] public int StationId { get; set; }
    [XmlElement(ElementName = "present_nm")] public string? PresentNM { get; set; }
    [XmlElement(ElementName = "st_cd")] public string? StCD { get; set; }
    [XmlElement(ElementName = "country_cd")] public string? CountryCD { get; set; }
    [XmlElement(ElementName = "station_latitude")] public float StationLatitude { get; set; }
    [XmlElement(ElementName = "station_longitude")] public float StationLongitude { get; set; }
    [XmlElement(ElementName = "prediction_type")] public string? PredictionType { get; set; }
    [XmlElement(ElementName = "tide_tm")] public string? TideTM { get; set; }
    [XmlElement(ElementName = "tide_epoch")] public int TideEpoch { get; set; }
    [XmlElement(ElementName = "tide_type")] public string? TideType { get; set; }
    [XmlElement(ElementName = "tide_ht")] public float TideHt { get; set; }
    [XmlElement(ElementName = "annual_max_ht")] public float AnnualMaxHt { get; set; }
    [XmlElement(ElementName = "annual_min_ht")] public float AnnualMinHt { get; set; }
}

[XmlRoot(ElementName = "tides")]
public class Tides
{
    [XmlElement(ElementName = "tide")] public List<Tide>? Tide { get; set; }
}



[XmlRoot(ElementName = "tidesResponse")]
public class TideForecastResponse
{
    [XmlElement(ElementName = "metadata")] public TideForecastMetadata? Metadata { get; set; }

    [XmlElement(ElementName = "tides")]
    public Tides? Tides { get; set; }
}