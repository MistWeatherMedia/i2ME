using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;


[XmlRoot(ElementName="Holiday")]
public class Holiday { 

	[XmlAttribute(AttributeName="date")] 
	public string Date { get; set; } 

    [XmlElement(ElementName ="Name")] 
	public string Name { get; set; } 

    [XmlElement(ElementName ="Date")] 
	public string DateFormatted { get; set; } 

    [XmlElement(ElementName ="Icon")] 
	public string Icon { get; set; } 

    [XmlElement(ElementName ="RGBHero")] 
	public string RGBHero { get; set; } 

    [XmlElement(ElementName ="RGBLogo")] 
	public string RGBLogo { get; set; } 

    [XmlElement(ElementName ="RGBIcon")] 
	public string RGBIcon { get; set; } 
}

[XmlRoot(ElementName="Mapping")]
public class HolidayMappingResponse { 

	[XmlAttribute(AttributeName="id")] 
	public string Id { get; set; } 

    [XmlAttribute(AttributeName="name")] 
	public string Name { get; set; } 

    [XmlElement(ElementName ="Holiday")] 
	public List<Holiday> Holidays { get; set; } 

}