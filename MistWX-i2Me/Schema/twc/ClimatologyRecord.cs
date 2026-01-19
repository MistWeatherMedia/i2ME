using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="ClimoRec")]
public class ClimatologyRec { 

	[XmlAttribute(AttributeName="loc")] 
	public string? Loc { get; set; }  

    [XmlAttribute(AttributeName="year")] 
	public int Year { get; set; } 

    [XmlAttribute(AttributeName="month")] 
	public string? Month { get; set; }

	[XmlAttribute(AttributeName="day")] 
	public string? Day { get; set; }

    [XmlAttribute(AttributeName="avgHigh")] 
	public int AvgHigh { get; set; }  

    [XmlAttribute(AttributeName="avgLow")] 
	public int AvgLow { get; set; } 

    [XmlAttribute(AttributeName="recHigh")] 
	public int RecHigh { get; set; }  

    [XmlAttribute(AttributeName="recLow")] 
	public int RecLow { get; set; } 

    [XmlAttribute(AttributeName="recHighYear")] 
	public int RecHighYear { get; set; }  

    [XmlAttribute(AttributeName="recLowYear")] 
	public int RecLowYear { get; set; }
}

[XmlRoot(ElementName="ClimatologyRecord")]
public class ClimatologyRecordResponse { 

	[XmlElement(ElementName="Key")] 
	public string? Key { get; set; } 

	[XmlElement(ElementName="ClimoRec")] 
	public List<ClimatologyRec>? ClimoRec { get; set; } 
}