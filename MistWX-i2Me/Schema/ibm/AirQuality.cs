using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName = "pollutantinfo")]
public class Pollutantinfo
{
    [XmlElement(ElementName = "primary_pollutant_ind")]
    public string? PrimaryPollutantInd { get; set; }

    [XmlElement(ElementName = "pollutant")]
    public string? Pollutant { get; set; }

    [XmlElement(ElementName = "pollutant_phrase")]
    public string? PollutantPhrase { get; set; }

    [XmlElement(ElementName = "air_quality_idx")]
    public double AirQualityIdx { get; set; }

    [XmlElement(ElementName = "air_quality_cat")]
    public string? AirQualityCat { get; set; }

    [XmlElement(ElementName = "air_quality_cat_idx")]
    public int AirQualityCatIdx { get; set; }
}

[XmlRoot(ElementName = "pollutants")]
public class Pollutants
{
    [XmlElement(ElementName = "pollutantinfo")]
    public Pollutantinfo? Pollutantinfo { get; set; }
}

[XmlRoot(ElementName = "airqualityreport")]
public class Airqualityreport
{
    [XmlElement(ElementName = "class")] public string? Class { get; set; }

    [XmlElement(ElementName = "key")] public string? Key { get; set; }

    [XmlElement(ElementName = "area_id")] public string? AreaId { get; set; }

    [XmlElement(ElementName = "area_name")]
    public string? AreaName { get; set; }

    [XmlElement(ElementName = "state_cd")] public string? StateCd { get; set; }

    [XmlElement(ElementName = "country_cd")]
    public string? CountryCd { get; set; }

    [XmlElement(ElementName = "latitude")] public double Latitude { get; set; }

    [XmlElement(ElementName = "longitude")]
    public double Longitude { get; set; }

    [XmlElement(ElementName = "source")] public string? Source { get; set; }

    [XmlElement(ElementName = "rpt_dt")] public string? RptDt { get; set; }

    [XmlElement(ElementName = "valid_time_lap")]
    public string? ValidTimeLap { get; set; }

    [XmlElement(ElementName = "process_time_gmt")]
    public int ProcessTimeGmt { get; set; }

    [XmlElement(ElementName = "action_day_ind")]
    public string? ActionDayInd { get; set; }

    [XmlElement(ElementName = "air_quality_cmnt")]
    public object? AirQualityCmnt { get; set; }

    [XmlElement(ElementName = "data_type")]
    public string? DataType { get; set; }

    [XmlElement(ElementName = "pollutants")]
    public Pollutants? Pollutants { get; set; }
}

[XmlRoot(ElementName = "airquality")]
public class Airquality
{
    [XmlElement(ElementName = "airqualityreport")]
    public List<Airqualityreport>? Airqualityreport { get; set; }
}

[XmlRoot(ElementName = "metadata")]
public class Metadata
{
    [XmlElement(ElementName = "language")] public string? Language { get; set; }

    [XmlElement(ElementName = "transaction_id")]
    public string? TransactionId { get; set; }

    [XmlElement(ElementName = "version")] public int Version { get; set; }

    [XmlElement(ElementName = "location_id")]
    public string? LocationId { get; set; }

    [XmlElement(ElementName = "expire_time_gmt")]
    public int ExpireTimeGmt { get; set; }

    [XmlElement(ElementName = "status_code")]
    public int StatusCode { get; set; }
}

[XmlRoot(ElementName = "response")]
public class AirQualityResponse
{
    [XmlElement(ElementName = "airquality")]
    public Airquality? Airquality { get; set; }

    [XmlElement(ElementName = "metadata")] public Metadata? Metadata { get; set; }

    [XmlAttribute(AttributeName = "xmlns")]
    public string? Xmlns { get; set; }

    [XmlText] public string? Text { get; set; }
}