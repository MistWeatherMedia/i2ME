using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName="metadata")]
public class WateringNeedsMetadata { 

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


[XmlRoot(ElementName="wateringNeedsIndex")]
public class wateringNeedsIndex { 

	[XmlElement(ElementName="wateringNeedsIndex")] 
	public List<int>? wtrNeedsIdx { get; set; } 
}

[XmlRoot(ElementName="wateringNeedsCategory")]
public class wateringNeedsCategory { 

	[XmlElement(ElementName="wateringNeedsCategory")] 
	public List<string>? wtrNeedsCat { get; set; } 
}

[XmlRoot(ElementName="wateringNeedsIndex12hour")]
public class wateringNeedsIndex12hour { 

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

	[XmlElement(ElementName="wateringNeedsIndex")] 
	public wateringNeedsIndex? wateringNeedsIndex { get; set; } 

	[XmlElement(ElementName="wateringNeedsCategory")] 
	public wateringNeedsCategory? wateringNeedsCategory { get; set; } 
}

[XmlRoot(ElementName="daypartForecastResponse")]
public class WateringNeedsResponse { 

	[XmlElement(ElementName="metadata")] 
	public WateringNeedsMetadata? Metadata { get; set; } 

	[XmlElement(ElementName="wateringNeedsIndex12hour")] 
	public wateringNeedsIndex12hour? wateringNeedsIndex12hour { get; set; } 
}
