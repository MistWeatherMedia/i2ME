using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName="metadata")]
public class PollenMetadata { 

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

[XmlRoot(ElementName="fcstValid")]
public class FcstValid { 

	[XmlElement(ElementName="fcstValid")] 
	public List<int>? ForecastValid { get; set; } 
}

[XmlRoot(ElementName="fcstValidLocal")]
public class FcstValidLocal { 

	[XmlElement(ElementName="fcstValidLocal")] 
	public List<string>? ForecastValidLocal { get; set; } 
}

[XmlRoot(ElementName="dayInd")]
public class DayInd { 

	[XmlElement(ElementName="dayInd")] 
	public List<string>? DayIndex { get; set; } 
}

[XmlRoot(ElementName="num")]
public class Num { 

	[XmlElement(ElementName="num")] 
	public List<int>? Number { get; set; } 
}

[XmlRoot(ElementName="daypartName")]
public class DaypartName { 

	[XmlElement(ElementName="daypartName")] 
	public List<string>? DpartName { get; set; } 
}

[XmlRoot(ElementName="grassPollenIndex")]
public class GrassPollenIndex { 

	[XmlElement(ElementName="grassPollenIndex")] 
	public List<int>? GrassPollenIdex { get; set; } 
}

[XmlRoot(ElementName="grassPollenCategory")]
public class GrassPollenCategory { 

	[XmlElement(ElementName="grassPollenCategory")] 
	public List<string>? GrassPollenCat { get; set; } 
}

[XmlRoot(ElementName="treePollenIndex")]
public class TreePollenIndex { 

	[XmlElement(ElementName="treePollenIndex")] 
	public List<int>? TreePollenIdex { get; set; } 
}

[XmlRoot(ElementName="treePollenCategory")]
public class TreePollenCategory { 

	[XmlElement(ElementName="treePollenCategory")] 
	public List<string>? TreePollenCat { get; set; } 
}

[XmlRoot(ElementName="ragweedPollenIndex")]
public class RagweedPollenIndex { 

	[XmlElement(ElementName="ragweedPollenIndex")] 
	public List<int>? RagweedPollenIdex { get; set; } 
}

[XmlRoot(ElementName="ragweedPollenCategory")]
public class RagweedPollenCategory { 

	[XmlElement(ElementName="ragweedPollenCategory")] 
	public List<string>? RagweedPollenCat { get; set; } 
}

[XmlRoot(ElementName="pollenForecast12hour")]
public class PollenForecast12Hour { 

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

	[XmlElement(ElementName="grassPollenIndex")] 
	public GrassPollenIndex? GrassPollenIndex { get; set; } 

	[XmlElement(ElementName="grassPollenCategory")] 
	public GrassPollenCategory? GrassPollenCategory { get; set; } 

	[XmlElement(ElementName="treePollenIndex")] 
	public TreePollenIndex? TreePollenIndex { get; set; } 

	[XmlElement(ElementName="treePollenCategory")] 
	public TreePollenCategory? TreePollenCategory { get; set; } 

	[XmlElement(ElementName="ragweedPollenIndex")] 
	public RagweedPollenIndex? RagweedPollenIndex { get; set; } 

	[XmlElement(ElementName="ragweedPollenCategory")] 
	public RagweedPollenCategory? RagweedPollenCategory { get; set; } 
}

[XmlRoot(ElementName="daypartForecastResponse")]
public class PollenResponse { 

	[XmlElement(ElementName="metadata")] 
	public PollenMetadata? Metadata { get; set; } 

	[XmlElement(ElementName="pollenForecast12hour")] 
	public PollenForecast12Hour? PollenForecast12hour { get; set; } 
}
