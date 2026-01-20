using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="Arrival")]
public class Arrival
{
    [XmlAttribute(AttributeName="cancellations")]
    public int Cancellations { get; set; }

    [XmlAttribute(AttributeName="percentage_cancelled")]
    public int PercentCancelled { get; set; }

    [XmlAttribute(AttributeName="total")]
    public int Total { get; set; }
}

[XmlRoot(ElementName="AirportDelays")]
public class AirportDelays
{
    [XmlAttribute(AttributeName="key")]
    public string? Key { get; set; }

    [XmlAttribute(AttributeName="icao_code")]
    public string? ICAOCode { get; set; }

    [XmlAttribute(AttributeName="iata_code")]
    public string? IATACode { get; set; }

    [XmlAttribute(AttributeName="faa_code")]
    public string? FAACode { get; set; }

    [XmlAttribute(AttributeName="airport_name")]
    public string? AirportName { get; set; }

    [XmlAttribute(AttributeName="local_airport_name")]
    public string? LocalAirportName { get; set; }

    [XmlAttribute(AttributeName="arrival")]
    public Arrival? Arrival { get; set; }

    [XmlAttribute(AttributeName="departure")]
    public Arrival? Departure { get; set; }

    [XmlAttribute(AttributeName="process_time_gmt")]
    public int ProcessTimeGmt { get; set; }
}