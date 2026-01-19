using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName="metadata")]
public class BreathingMetadata { 

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

[XmlRoot(ElementName="breathingIndex")]
public class BreathingIndex { 

	[XmlElement(ElementName="breathingIndex")] 
	public List<int>? BreathingIdex { get; set; } 
}

[XmlRoot(ElementName="breathingCategory")]
public class BreathingCategory { 

	[XmlElement(ElementName="breathingCategory")] 
	public List<string>? BreathingCat { get; set; } 
}

[XmlRoot(ElementName="breathingIndex12hour")]
public class BreathingIndex12Hour { 

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

	[XmlElement(ElementName="breathingIndex")] 
	public BreathingIndex? BreathingIndex { get; set; } 

	[XmlElement(ElementName="breathingCategory")] 
	public BreathingCategory? BreathingCategory { get; set; } 
}

[XmlRoot(ElementName="daypartForecastResponse")]
public class BreathingResponse { 

	[XmlElement(ElementName="metadata")] 
	public BreathingMetadata? Metadata { get; set; } 

	[XmlElement(ElementName="breathingIndex12hour")] 
	public BreathingIndex12Hour? BreathingIndex12hour { get; set; } 
}