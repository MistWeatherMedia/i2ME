using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

public class CurrentObservationsMetadata
{
    public string? language { get; set; }
    public string? transaction_id { get; set; }
    public string? version { get; set; }
    public string? location_id { get; set; }
    public string? units { get; set; }
    public int expire_time_gmt { get; set; }
    public int status_code { get; set; }
}

public class TideForecastMetadata
{
    public string? language { get; set; }
    public string? transaction_id { get; set; }
    public string? version { get; set; }
    public string? location_id { get; set; }
    public string? units { get; set; }
    public int status_code { get; set; }
}
public class DailyForecastMetadata
{

    [XmlElement(ElementName = "language")]
    public string? Language { get; set; }

    [XmlElement(ElementName = "transaction_id")]
    public string? TransactionId { get; set; }

    [XmlElement(ElementName = "version")]
    public int Version { get; set; }

    [XmlElement(ElementName = "latitude")]
    public double Latitude { get; set; }

    [XmlElement(ElementName = "longitude")]
    public double Longitude { get; set; }

    [XmlElement(ElementName = "units")]
    public string? Units { get; set; }

    [XmlElement(ElementName = "expire_time_gmt")]
    public int ExpireTimeGmt { get; set; }

    [XmlElement(ElementName = "status_code")]
    public int StatusCode { get; set; }
}

[XmlRoot(ElementName = "metadata")]
public class HourlyForecastMetadata
{

    [XmlElement(ElementName = "language")]
    public string? Language { get; set; }

    [XmlElement(ElementName = "transaction_id")]
    public string? TransactionId { get; set; }

    [XmlElement(ElementName = "version")]
    public int Version { get; set; }

    [XmlElement(ElementName = "location_id")]
    public string? LocationId { get; set; }

    [XmlElement(ElementName = "units")]
    public string? Units { get; set; }

    [XmlElement(ElementName = "expire_time_gmt")]
    public int ExpireTimeGmt { get; set; }

    [XmlElement(ElementName = "status_code")]
    public int StatusCode { get; set; }
}