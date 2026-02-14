using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName = "Event")]
public class Event
{
    [XmlElement(ElementName = "Text")]
    public string Text { get; set; } = "";
}

[XmlRoot(ElementName = "Events")]
public class Events
{
    [XmlElement(ElementName = "Event")]
    public List<Event> Event { get; set; } = new();

    [XmlAttribute(AttributeName = "StartRandom")]
    public string? StartRandom { get; set; } = null;

    [XmlAttribute(AttributeName = "Type")]
    public string? Type { get; set; } = null;

    [XmlAttribute(AttributeName = "FilePath")]
    public string? FilePath { get; set; } = null;

    [XmlAttribute(AttributeName = "DefaultFile")]
    public string? DefaultFile { get; set; } = null;
}