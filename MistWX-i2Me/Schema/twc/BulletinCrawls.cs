using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="Bulletin")]
public class Bulletin { 

    [XmlAttribute(AttributeName="index")] 
	public int Index { get; set; }

    [XmlElement(ElementName="Label")] 
	public string Label { get; set; }

    [XmlElement(ElementName="Text")] 
	public string Text { get; set; }

    [XmlElement(ElementName="VisualStyle")] 
	public int VisualStyle { get; set; }

    [XmlElement(ElementName="AudioStyle")] 
	public int AudioStyle { get; set; }

    [XmlElement(ElementName="Priority")] 
	public int Priority { get; set; }

    [XmlElement(ElementName="Phenomena")] 
	public string Phenomena { get; set; }

    [XmlElement(ElementName="Significance")] 
	public string Significance { get; set; }

    [XmlElement(ElementName="TtsAudioClip")] 
	public string TtsAudioClip = "";

    [XmlElement(ElementName="TtsAudioClipSeconds")] 
	public string TtsAudioClipSeconds = "0";

}

[XmlRoot(ElementName="BulletinCrawls")]
public class BulletinCrawls { 

    [XmlAttribute(AttributeName="key")] 
	public string key = "active";

    [XmlAttribute(AttributeName="expiration")] 
	public string expiration { get; set; }

    [XmlElement(ElementName="Bulletin")]
    public List<Bulletin> Bulletins { get; set; }

}

[XmlRoot(ElementName="Data")]
public class BulletinCrawlsResponse { 

	[XmlAttribute(AttributeName="type")] 
	public string Type = "BulletinCrawls";

    [XmlElement(ElementName="BulletinCrawls")]
    public BulletinCrawls BulletinCrawls { get; set; }
}