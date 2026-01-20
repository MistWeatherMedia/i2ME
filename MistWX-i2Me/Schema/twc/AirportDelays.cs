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

[XmlRoot(ElementName="AirportDelays")]
public class AirportDelays
{
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
}