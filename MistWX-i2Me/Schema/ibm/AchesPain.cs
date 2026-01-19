using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName="metadata")]
public class AchesPainMetadata { 

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


[XmlRoot(ElementName="achesPainsIndex")]
public class AchesPainsIndex { 

	[XmlElement(ElementName="achesPainsIndex")] 
	public List<int>? AchesPainsIdex { get; set; } 
}

[XmlRoot(ElementName="achesPainsCategory")]
public class AchesPainsCategory { 

	[XmlElement(ElementName="achesPainsCategory")] 
	public List<string>? AchesPainsCat { get; set; } 
}

[XmlRoot(ElementName="achesPainsIndex12hour")]
public class AchesPainsIndex12Hour { 

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

	[XmlElement(ElementName="achesPainsIndex")] 
	public AchesPainsIndex? AchesPainsIndex { get; set; } 

	[XmlElement(ElementName="achesPainsCategory")] 
	public AchesPainsCategory? AchesPainsCategory { get; set; } 
}

[XmlRoot(ElementName="daypartForecastResponse")]
public class AchesPainResponse { 

	[XmlElement(ElementName="metadata")] 
	public AchesPainMetadata? Metadata { get; set; } 

	[XmlElement(ElementName="achesPainsIndex12hour")] 
	public AchesPainsIndex12Hour? AchesPainsIndex12hour { get; set; } 
}