using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName = "metadata")]
public class PollenObsMetadata
{
    [XmlElement(ElementName="language")] 
	public string? Language { get; set; } 

	[XmlElement(ElementName="transactionId")] 
	public string? TransactionId { get; set; } 

	[XmlElement(ElementName="version")] 
	public int Version { get; set; } 

	[XmlElement(ElementName="location_id")] 
	public string? Latitude { get; set; } 

	[XmlElement(ElementName="units")] 
	public string? Units { get; set; } 

	[XmlElement(ElementName="expireTimeGmt")] 
	public int ExpireTimeGmt { get; set; } 

	[XmlElement(ElementName="statusCode")] 
	public int StatusCode { get; set; } 
}

[XmlRoot(ElementName = "treenames")]
public class TreeNames
{
    [XmlElement(ElementName="tree_nm")] 
	public List<string>? TreeNM { get; set; } 
}

[XmlRoot(ElementName = "pollenobservation")]
public class PollenObservation
{
    [XmlElement(ElementName = "pollen_type")] public string? PollenType { get; set; }
    [XmlElement(ElementName = "pollen_idx")] public int PollenIdx { get; set; }
    [XmlElement(ElementName = "pollen_desc")] public string? PollenDesc { get; set; }
}

[XmlRoot(ElementName = "pollenobservations")]
public class PollenObservations
{
    [XmlElement(ElementName = "class")] public string? Class { get; set; }
    [XmlElement(ElementName = "loc_id")] public string? LocId { get; set; }
    [XmlElement(ElementName = "loc_nm")] public string? LocNM { get; set; }
    [XmlElement(ElementName = "rpt_dt")] public string? RptDt { get; set; }
    [XmlElement(ElementName = "process_time_gmt")] public int ProcessTimeGMT { get; set; }
    [XmlElement(ElementName = "treenames")] public TreeNames? TreeNames { get; set; }
    [XmlElement(ElementName = "total_pollen_cnt")] public int TotalPollenCnt { get; set; }
    [XmlElement(ElementName = "stn_cmnt_cd")] public string? StnCmntCd { get; set; }
    [XmlElement(ElementName = "stn_cmnt")] public string? StnCmnt { get; set; }
    [XmlElement(ElementName="pollenobservation")] 
	public List<PollenObservation>? PollenObservation { get; set; }
}

[XmlRoot(ElementName = "response")]
public class PollenObservationsResponse
{
    [XmlElement(ElementName = "metadata")] public PollenObsMetadata? Metadata { get; set; }
}

