using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.System;

[XmlRoot(ElementName = "ConfigItem")]
public class ConfigItem
{
    [XmlAttribute(AttributeName = "key")] public string? Key { get; set; }
    [XmlAttribute(AttributeName = "value")] public string? Value { get; set; }
}

[XmlRoot(ElementName = "ConfigItems")]
public class ConfigItems
{
    [XmlElement(ElementName = "ConfigItem")]
    public List<ConfigItem>? ConfigItem { get; set; }
}

[XmlRoot(ElementName = "ConfigDef")]
public class ConfigDef
{
    [XmlElement(ElementName = "ConfigItems")]
        public ConfigItems? ConfigItems { get; set; }
}

[XmlRoot(ElementName = "Config")]
public class MachineProductConfig
{
    [XmlElement(ElementName = "ConfigDef")]
    public ConfigDef? ConfigDef { get; set; }
}