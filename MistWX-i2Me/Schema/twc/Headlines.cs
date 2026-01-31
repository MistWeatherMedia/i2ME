using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="audioSeq")]
public class HeadlinesAudioSeq { 

    [XmlAttribute(AttributeName="code")] 
	public string code { get; set; } 

    [XmlElement(ElementName = "audioClip")]
    public HeadlinesAudioClip audioClip { get; set; } 
}

[XmlRoot(ElementName="audioClip")]
public class HeadlinesAudioClip { 

    [XmlAttribute(AttributeName="path")] 
	public string path { get; set; } 
}

[XmlRoot(ElementName="vocalSeq")]
public class HeadlinesVocalSeq { 

    [XmlElement(ElementName="audioSeq")] 
	public HeadlinesAudioSeq audioSeq { get; set; } 
}

[XmlRoot(ElementName="Headline")]
public class Headline { 

    [XmlElement(ElementName="key")] 
	public int key { get; set; } 

    [XmlElement(ElementName="procTm")] 
	public string procTm { get; set; } 

	[XmlElement(ElementName="text")] 
	public string text { get; set; } 

    [XmlElement(ElementName="expiration")] 
	public string expiration { get; set; } 

    [XmlElement(ElementName="vocalCd")] 
	public string vocalCd { get; set; } 

    [XmlElement(ElementName="priority")] 
	public int priority { get; set; } 

    [XmlElement(ElementName="phenomena")] 
	public string phenomena { get; set; } 

    [XmlElement(ElementName="significance")] 
	public string significance { get; set; } 

    [XmlElement(ElementName="lficon")] 
	public string lficon { get; set; } 

    [XmlElement(ElementName="vocalSeq")] 
	public HeadlinesVocalSeq vocalSeq { get; set; }
}

[XmlRoot(ElementName="Data")]
public class HeadlinesResponse { 

	[XmlAttribute(AttributeName="type")] 
	public string Type = "Headline";

    [XmlElement(ElementName="Headline")]
    public List<Headline> Headlines { get; set; }
}