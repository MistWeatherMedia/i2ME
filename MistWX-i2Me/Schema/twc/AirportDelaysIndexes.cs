using System.Security.Policy;
using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="AirportDelayIndexes")]
public class AirportDelayIndexes
{
    [XmlElement(ElementName="key")]
    public string? Key { get; set; }

    [XmlElement(ElementName="indexes")]
    public string? Indexes { get; set; }
}