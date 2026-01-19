using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="LFHdr")]
public class LFRecordHeader { 

    [XmlElement(ElementName="locType")]
    public string? LocType { get; set; }

    [XmlElement(ElementName="locId")]
    public string? LocId { get; set; }

    [XmlElement(ElementName="procTm")]
    public string? ProcTm { get; set; }
}

[XmlRoot(ElementName="LFData")]
public class LFRecordData { 

    [XmlElement(ElementName="cityNm")]
    public string? cityNm { get; set; }

    [XmlElement(ElementName="stCd")]
    public string? stCd { get; set; }

    [XmlElement(ElementName="prsntNm")]
    public string? prsntNm { get; set; }

    [XmlElement(ElementName="cntryCd")]
    public string? cntryCd { get; set; }

    [XmlElement(ElementName="coopId")]
    public string? coopId { get; set; }

    [XmlElement(ElementName="lat")]
    public string? lat { get; set; }

    [XmlElement(ElementName="long")]
    public string? lon { get; set; }

    [XmlElement(ElementName="obsStn")]
    public string? obsStn { get; set; }

    [XmlElement(ElementName="secObsStn")]
    public string? secObsStn { get; set; }

    [XmlElement(ElementName="tertObsStn")]
    public string? tertObsStn { get; set; }

    [XmlElement(ElementName="gmtDiff")]
    public string? gmtDiff { get; set; }

    [XmlElement(ElementName="regSat")]
    public string? regSat { get; set; }

    [XmlElement(ElementName="cntyId")]
    public string? cntyId { get; set; }

    [XmlElement(ElementName="cntyNm")]
    public string? cntyNm { get; set; }

    [XmlElement(ElementName="zoneId")]
    public string? zoneId { get; set; }

    [XmlElement(ElementName="zoneNm")]
    public string? zoneNm { get; set; }

    [XmlElement(ElementName="cntyFips")]
    public string? cntyFips { get; set; }

    [XmlElement(ElementName="active")]
    public string? active { get; set; }

    [XmlElement(ElementName="dySTInd")]
    public string? dySTInd { get; set; }

    [XmlElement(ElementName="dmaCd")]
    public string? dmaCd { get; set; }

    [XmlElement(ElementName="zip2locId")]
    public string? zip2locId { get; set; }

    [XmlElement(ElementName="elev")]
    public string? elev { get; set; }

    [XmlElement(ElementName="cliStn")]
    public string? cliStn { get; set; }

    [XmlElement(ElementName="tmZnNm")]
    public string? tmZnNm { get; set; }

    [XmlElement(ElementName="tmZnAbbr")]
    public string? tmZnAbbr { get; set; }

    [XmlElement(ElementName="dySTAct")]
    public string? dySTAct { get; set; }

    [XmlElement(ElementName="clsRad")]
    public string? clsRad { get; set; }

    [XmlElement(ElementName="ssRad")]
    public string? ssRad { get; set; }

    [XmlElement(ElementName="lsRad")]
    public string? lsRad { get; set; }

    [XmlElement(ElementName="siteId")]
    public string? siteId { get; set; }

    [XmlElement(ElementName="primTecci")]
    public string? primTecci { get; set; }

    [XmlElement(ElementName="secTecci")]
    public string? secTecci { get; set; }

    [XmlElement(ElementName="tertTecci")]
    public string? tertTecci { get; set; }

    [XmlElement(ElementName="arptId")]
    public string? arptId { get; set; }

    [XmlElement(ElementName="mrnZoneId")]
    public string? mrnZoneId { get; set; }

    [XmlElement(ElementName="pllnId")]
    public string? pllnId { get; set; }

    [XmlElement(ElementName="skiId")]
    public string? skiId { get; set; }

    [XmlElement(ElementName="tideId")]
    public string? tideId { get; set; }

    [XmlElement(ElementName="epaId")]
    public string? epaId { get; set; }

    [XmlElement(ElementName="tPrsntNm")]
    public string? tPrsntNm { get; set; }

    [XmlElement(ElementName="wrlsPrsntNm")]
    public string? wrlsPrsntNm { get; set; }

    [XmlElement(ElementName="wmoId")]
    public string? wmoId { get; set; }

}

[XmlRoot(ElementName="LFRecord")]
public class LFRecordResponse { 

    [XmlElement(ElementName="action")]
    public int Action { get; set; }

    [XmlElement(ElementName="LFHdr")]
    public LFRecordHeader? LFRecordHeader { get; set; }

    [XmlElement(ElementName="LFData")]
    public LFRecordData? LFRecordData { get; set; }
}