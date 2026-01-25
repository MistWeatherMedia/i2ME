using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="arrival")]
public class Arrival
{
    [XmlElement(ElementName="cancellations")]
    public int Cancellations { get; set; }

    [XmlElement(ElementName="percentage_cancelled")]
    public int PercentCancelled { get; set; }

    [XmlElement(ElementName="total")]
    public int Total { get; set; }
}

[XmlRoot(ElementName="departure")]
public class Departure
{
    [XmlElement(ElementName="cancellations")]
    public int Cancellations { get; set; }

    [XmlElement(ElementName="percentage_cancelled")]
    public int PercentCancelled { get; set; }

    [XmlElement(ElementName="total")]
    public int Total { get; set; }
}

[XmlRoot(ElementName="delay")]
public class Delay
{
    [XmlElement(ElementName="category")]
    public string? Category { get; set; }

    [XmlElement(ElementName="color")]
    public string? Color { get; set; }

    [XmlElement(ElementName="delay_sec")]
    public int DelaySec { get; set; }

    [XmlElement(ElementName="reason")]
    public string Reason = "Other";
}

[XmlRoot(ElementName="reasons_all")]
public class ReasonsAll
{
    [XmlElement(ElementName="delay")]
    public List<Delay>? Delay { get; set; }
}

[XmlRoot(ElementName="delays")]
public class Delays
{
    [XmlElement(ElementName="category")]
    public string? Category { get; set; }

    [XmlElement(ElementName="color")]
    public string? Color { get; set; }

    [XmlElement(ElementName="delay_sec")]
    public int DelaySec { get; set; }

    [XmlElement(ElementName="reasons_all")]
    public ReasonsAll? ReasonsAll { get; set; }
}

[XmlRoot(ElementName="airport_result")]
public class AirportResult
{
    [XmlElement(ElementName = "class")] 
    public string Class = "airport_delays";

    [XmlElement(ElementName = "source")] 
    public string Source = "flightaware.com";

    [XmlElement(ElementName="key")]
    public string? Key { get; set; }

    [XmlElement(ElementName="icao_code")]
    public string? ICAOCode { get; set; }

    [XmlElement(ElementName="iata_code")]
    public string? IATACode { get; set; }

    [XmlElement(ElementName="faa_code")]
    public string? FAACode { get; set; }

    [XmlElement(ElementName="airport_name")]
    public string? AirportName { get; set; }

    [XmlElement(ElementName="local_airport_name")]
    public string? LocalAirportName { get; set; }

    [XmlElement(ElementName="delays")]
    public Delays? Delays { get; set; }

    [XmlElement(ElementName="arrival")]
    public Arrival? Arrival { get; set; }

    [XmlElement(ElementName="departure")]
    public Departure? Departure { get; set; }

    [XmlElement(ElementName="process_time_gmt")]
    public int ProcessTimeGmt { get; set; }

    [XmlElement(ElementName="expire_time_gmt")]
    public int ExpireTimeGmt { get; set; }
    
}

public class AirportDelaysMetadata
{
    [XmlElement(ElementName="language")]
    public string language = "en-US";

    [XmlElement(ElementName="transaction_id")]
    public string transaction_id = "i2ME";

    [XmlElement(ElementName="version")]
    public string version = "1";

    [XmlElement(ElementName="airport_code")]
    public string airport_code {get; set;}

    [XmlElement(ElementName="expire_time_gmt")]
    public int expire_time_gmt {get; set;}

    [XmlElement(ElementName="status_code")]
    public int status_code = 200;
}

[XmlRoot(ElementName="AirportDelays")]
public class AirportDelays
{
    [XmlAttribute(AttributeName = "process_time_gmt")] 
    public int ProcessTimeGmtAttr {get; set;}

    [XmlAttribute(AttributeName = "locationKey")] 
    public string? locationKey {get; set;}

    [XmlAttribute(AttributeName = "key")] 
    public string? KeyAttr {get; set;}

    [XmlElement(ElementName="airport_result")]
    public AirportResult? airport_result { get; set; }

    [XmlElement(ElementName="metadata")]
    public AirportDelaysMetadata? metadata { get; set; }

    [XmlElement(ElementName="clientKey")]
    public string? clientKey { get; set; }
}