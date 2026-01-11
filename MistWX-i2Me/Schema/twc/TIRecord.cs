using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="TIHdr")]
public class TIRecordHeader { 

	[XmlAttribute(AttributeName="iLevel")] 
	public int ILevel { get; set; } 

    [XmlElement(ElementName="TIstnId")]
    public string? TIstnId { get; set; }

    [XmlElement(ElementName="procTm")]
    public string? ProcTm { get; set; }
}

[XmlRoot(ElementName="TIData")]
public class TIRecordData { 

    [XmlElement(ElementName="TItdTm")]
    public string? TItdTm { get; set; }

    [XmlElement(ElementName="TItdDST")]
    public string? TItdDST { get; set; }

    [XmlElement(ElementName="TItdTyp")]
    public string? TItdTyp { get; set; }

    [XmlElement(ElementName="TItdHght")]
    public float TItdHght { get; set; }
}

[XmlRoot(ElementName="TIRecord")]
public class TIRecordResponse { 

	[XmlAttribute(AttributeName="id")] 
	public string? Id { get; set; } 

    [XmlElement(ElementName="action")]
    public int Action { get; set; }

    [XmlElement(ElementName="TIHdr")]
    public TIRecordHeader? TIRecordHeader { get; set; }

    [XmlElement(ElementName="TIData")]
    public List<TIRecordData>? TIRecordData { get; set; }
}