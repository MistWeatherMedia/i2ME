using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName="metadata")]
public class MosquitoActivityMetadata { 

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


[XmlRoot(ElementName="mosquitoIndex")]
public class mosquitoIndex { 

	[XmlElement(ElementName="mosquitoIndex")] 
	public List<int>? mosquitoIdex { get; set; } 
}

[XmlRoot(ElementName="mosquitoCategory")]
public class mosquitoCategory { 

	[XmlElement(ElementName="mosquitoCategory")] 
	public List<string>? mosquitoCat { get; set; } 
}

[XmlRoot(ElementName="mosquitoIndex12hour")]
public class mosquitoIndex12Hour { 

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

	[XmlElement(ElementName="mosquitoIndex")] 
	public mosquitoIndex? mosquitoIndex { get; set; } 

	[XmlElement(ElementName="mosquitoCategory")] 
	public mosquitoCategory? mosquitoCategory { get; set; } 
}

[XmlRoot(ElementName="daypartForecastResponse")]
public class MosquitoActivityResponse { 

	[XmlElement(ElementName="metadata")] 
	public MosquitoActivityMetadata? Metadata { get; set; } 

	[XmlElement(ElementName="mosquitoIndex12hour")] 
	public mosquitoIndex12Hour? mosquitoIndex12hour { get; set; } 
}
