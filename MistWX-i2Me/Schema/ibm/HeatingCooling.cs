using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName="metadata")]
public class HeatCoolMetadata { 

	[XmlElement(ElementName="language")] 
	public string? Language { get; set; } 

	[XmlElement(ElementName="transactionId")] 
	public string? TransactionId { get; set; } 

	[XmlElement(ElementName="version")] 
	public int Version { get; set; } 

	[XmlElement(ElementName="latitude")] 
	public double? Latitude { get; set; } 

	[XmlElement(ElementName="longitude")] 
	public double? Longitude { get; set; } 

	[XmlElement(ElementName="expireTimeGmt")] 
	public int ExpireTimeGmt { get; set; } 

	[XmlElement(ElementName="statusCode")] 
	public int StatusCode { get; set; } 
}

[XmlRoot(ElementName="heatingCoolingIndex")]
public class HeatingCoolingIndex { 

	[XmlElement(ElementName="heatingCoolingIndex")] 
	public List<int>? HeatingCoolingIdex { get; set; } 
}

[XmlRoot(ElementName="heatingCoolingCategory")]
public class HeatingCoolingCategory { 

	[XmlElement(ElementName="heatingCoolingCategory")] 
	public List<string>? HeatingCoolingCat { get; set; } 
}

[XmlRoot(ElementName="heatingCoolingIndex12hour")]
public class HeatingCoolingIndex12Hour { 

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

	[XmlElement(ElementName="heatingCoolingIndex")] 
	public HeatingCoolingIndex? HeatingCoolingIndex { get; set; } 

	[XmlElement(ElementName="heatingCoolingCategory")] 
	public HeatingCoolingCategory? HeatingCoolingCategory { get; set; } 
}

[XmlRoot(ElementName="daypartForecastResponse")]
public class HeatingCoolingResponse { 

	[XmlElement(ElementName="metadata")] 
	public HeatCoolMetadata? Metadata { get; set; } 

	[XmlElement(ElementName="heatingCoolingIndex12hour")] 
	public HeatingCoolingIndex12Hour? HeatingCoolingIndex12hour { get; set; } 
}
