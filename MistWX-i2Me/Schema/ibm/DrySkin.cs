using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName="metadata")]
public class DrySkinMetadata { 

	[XmlElement(ElementName="language")] 
	public string? Language { get; set; } 

	[XmlElement(ElementName="transactionId")] 
	public string? TransactionId { get; set; } 

	[XmlElement(ElementName="version")] 
	public int Version { get; set; } 

	[XmlElement(ElementName="latitude")] 
	public double Latitude { get; set; } 

	[XmlElement(ElementName="longitude")] 
	public double Longitude { get; set; } 

	[XmlElement(ElementName="expireTimeGmt")] 
	public int ExpireTimeGmt { get; set; } 

	[XmlElement(ElementName="statusCode")] 
	public int StatusCode { get; set; } 
}


[XmlRoot(ElementName="drySkinIndex")]
public class drySkinIndex { 

	[XmlElement(ElementName="drySkinIndex")] 
	public List<int>? drySkinIdex { get; set; } 
}

[XmlRoot(ElementName="drySkinCategory")]
public class drySkinCategory { 

	[XmlElement(ElementName="drySkinCategory")] 
	public List<string>? drySkinCat { get; set; } 
}

[XmlRoot(ElementName="drySkinIndex12hour")]
public class drySkinIndex12Hour { 

	[XmlElement(ElementName="fcstValid")] 
	public FcstValid? FcstValid { get; set; } 

	[XmlElement(ElementName="fcstValidLocal")] 
	public FcstValidLocal? FcstValidLocal { get; set; } 

	[XmlElement(ElementName="dayInd")] 
	public DayInd? DayInd { get; set; } 

	[XmlElement(ElementName="num")] 
	public Num? Num { get; set; } 

	[XmlElement(ElementName="daypartName")] 
	public DaypartName? DaypartName { get; set; } 

	[XmlElement(ElementName="drySkinIndex")] 
	public drySkinIndex? drySkinIndex { get; set; } 

	[XmlElement(ElementName="drySkinCategory")] 
	public drySkinCategory? drySkinCategory { get; set; } 
}

[XmlRoot(ElementName="daypartForecastResponse")]
public class DrySkinResponse { 

	[XmlElement(ElementName="metadata")] 
	public DrySkinMetadata? Metadata { get; set; } 

	[XmlElement(ElementName="drySkinIndex12hour")] 
	public drySkinIndex12Hour? drySkinIndex12hour { get; set; } 
}
