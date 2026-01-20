using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="HTStrmId")]
public class DerivedHTRecordStrmId
{
    [XmlAttribute(AttributeName = "HTStrmNm")]
    public string? StrmNm { get; set; }

    [XmlText]
    public string? StrmId { get; set; }
}

[XmlRoot(ElementName="HTAdvsTmLcl")]
public class DerivedHTRecordAdvsTmLcl
{
    [XmlAttribute(AttributeName = "HTDyLcl")]
    public string? Dow { get; set; }

    [XmlAttribute(AttributeName = "HTMnthLcl")]
    public string? Month { get; set; }

    [XmlAttribute(AttributeName = "HTTmLcl")]
    public string? Time { get; set; }

    [XmlAttribute(AttributeName = "HTTzAbbrv")]
    public string? TZAbbrv { get; set; }

    [XmlText]
    public string? Timestamp { get; set; }
}

[XmlRoot(ElementName="HTHdr")]
public class DerivedHTRecordHeader
{
    [XmlElement(ElementName = "HTPil")]
    public string? Pil { get; set; }

    [XmlElement(ElementName = "HTStrmId")]
    public DerivedHTRecordStrmId? StrmId { get; set; }

    [XmlElement(ElementName = "HTAdvsTmUTC")]
    public string? AdvsTmUTC { get; set; }

    [XmlElement(ElementName = "procTm")]
    public string? ProcTm { get; set; }
}

[XmlRoot(ElementName="HTData")]
public class DerivedHTRecordData
{
    [XmlElement(ElementName = "HTSgmntChksum")]
    public string? SgmntChksum { get; set; }

    [XmlElement(ElementName = "HTWMOHdr")]
    public string? WMOHdr { get; set; }

    [XmlElement(ElementName = "HTIssueTmUTC")]
    public string? IssueTmUTC { get; set; }

    [XmlElement(ElementName = "HTIssueOffc")]
    public string? IssueOffc { get; set; }

    [XmlElement(ElementName = "HTAdvsTmLcl")]
    public DerivedHTRecordAdvsTmLcl? AdvsTmLcl { get; set; }

    [XmlElement(ElementName = "HTLat")]
    public double Lat { get; set; }

    [XmlElement(ElementName = "HTLatHmsphr")]
    public string? LatHmsphr { get; set; }

    [XmlElement(ElementName = "HTLon")]
    public double Lon { get; set; }

    [XmlElement(ElementName = "HTLonHmsphr")]
    public string? LonHmsphr { get; set; }

    [XmlElement(ElementName = "HTPrssrMB")]
    public int PressureMB { get; set; }

    [XmlElement(ElementName = "HTPrssrIn")]
    public double PressureIn { get; set; }

    [XmlElement(ElementName = "HTMxWndSpdMPH")]
    public int MaxWindSpeedMPH { get; set; }

    [XmlElement(ElementName = "HTMxWndGstMPH")]
    public int MaxWindGustMPH { get; set; }

    [XmlElement(ElementName = "HTCat")]
    public string? Cat { get; set; }

    [XmlElement(ElementName = "HTHdngDirDeg")]
    public int HeadingDirDeg { get; set; }

    [XmlElement(ElementName = "HTHdngDirCrdnl")]
    public string? HeadingDirCardinal { get; set; }

    [XmlElement(ElementName = "HTHdngSpdMPH")]
    public int HeadingSpdMPH { get; set; }

    [XmlElement(ElementName = "HTDstnc1")]
    public int Distance1 { get; set; }

    [XmlElement(ElementName = "HTDir1")]
    public string? Direction1 { get; set; }

    [XmlElement(ElementName = "HTLoc1")]
    public string? LocName1 { get; set; }

    [XmlElement(ElementName = "HTCtyNm1")]
    public string? CityName1 { get; set; }

    [XmlElement(ElementName = "HTDstnc2")]
    public string? Distance2 { get; set; }

    [XmlElement(ElementName = "HTDir1")]
    public string? Direction2 { get; set; }

    [XmlElement(ElementName = "HTLoc1")]
    public string? LocName2 { get; set; }

    [XmlElement(ElementName = "HTCtyNm1")]
    public string? CityName2 { get; set; }

    [XmlElement(ElementName = "HTBasin")]
    public string? Basin { get; set; }

    [XmlElement(ElementName = "HTLstUpd")]
    public int? LastUpdate { get; set; }
}

[XmlRoot(ElementName="DerivedHTRecord")]
public class DerivedHTRecordResponse
{
    [XmlElement(ElementName = "index")]
    public int Index { get; set; }

    [XmlElement(ElementName = "action")]
    public int Action { get; set; }

    [XmlElement(ElementName = "HTHdr")]
    public DerivedHTRecordHeader? Header { get; set; }

    [XmlElement(ElementName = "HTData")]
    public DerivedHTRecordData? Data { get; set; }
}