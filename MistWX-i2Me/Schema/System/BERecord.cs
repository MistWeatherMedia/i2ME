using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.System;

[XmlRoot(ElementName = "eActionCd")]
public class EActionCd
{

    [XmlAttribute(AttributeName = "eActionPriority")]
    public int EActionPriority { get; set; }

    [XmlText]
    public string? Text { get; set; }
}

[XmlRoot(ElementName = "eOfficeId")]
public class EOfficeId
{

    [XmlAttribute(AttributeName = "eOfficeNm")]
    public string? EOfficeNm { get; set; }

    [XmlText]
    public string? Text { get; set; }
}

[XmlRoot(ElementName = "bEvent")]
public class BEvent
{

    [XmlElement(ElementName = "eActionCd")]
    public EActionCd? EActionCd { get; set; }

    [XmlElement(ElementName = "eOfficeId")]
    public EOfficeId? EOfficeId { get; set; }

    [XmlElement(ElementName = "ePhenom")]
    public string? EPhenom { get; set; }

    [XmlElement(ElementName = "eSgnfcnc")]
    public string? ESgnfcnc { get; set; }

    [XmlElement(ElementName = "eETN")]
    public string? EETN { get; set; }

    [XmlElement(ElementName = "eDesc")]
    public string? EDesc { get; set; }

    [XmlElement(ElementName = "eSpnshDesc")]
    public string? ESpnshDesc { get; set; }

    [XmlElement(ElementName = "eStTmUTC")]
    public string? EStTmUTC = "NOT_USED";

    [XmlElement(ElementName = "eEndTmUTC")]
    public string? EEndTmUTC { get; set; }

    [XmlElement(ElementName = "eSvrty")]
    public int ESvrty { get; set; }

    [XmlElement(ElementName = "eTWCIId")]
    public string? ETWCIId = "NOT_USED";

    [XmlElement(ElementName = "eExpTmUTC")]
    public string? EExpTmUTC { get; set; }
}

[XmlRoot(ElementName = "bLocCd")]
public class BLocCd
{

    [XmlAttribute(AttributeName = "bLoc")]
    public string? BLoc { get; set; }

    [XmlAttribute(AttributeName = "bLocTyp")]
    public string? BLocTyp { get; set; }

    [XmlText]
    public string? Text { get; set; }
}

[XmlRoot(ElementName = "bStCd")]
public class BStCd
{

    [XmlAttribute(AttributeName = "bSt")]
    public string? BSt { get; set; }

    [XmlText]
    public string? Text { get; set; }
}

[XmlRoot(ElementName = "bLocations")]
public class BLocations
{

    [XmlElement(ElementName = "bLocCd")]
    public BLocCd? BLocCd { get; set; }

    [XmlElement(ElementName = "bStCd")]
    public BStCd? BStCd { get; set; }

    [XmlElement(ElementName = "bUTCDiff")]
    public string? BUTCDiff = "NOT_USED";

    [XmlElement(ElementName = "bTzAbbrv")]
    public string? BTzAbbrv { get; set; }

    [XmlElement(ElementName = "bCntryCd")]
    public string? BCntryCd { get; set; }
}

[XmlRoot(ElementName = "BEHdr")]
public class BEHdr
{

    [XmlElement(ElementName = "bPIL")]
    public string? BPIL = "NOT_USED";

    [XmlElement(ElementName = "bWMOHdr")]
    public string BWMOHdr { get; set; } = "NOT_USED";

    [XmlElement(ElementName = "bEvent")]
    public BEvent? BEvent { get; set; }

    [XmlElement(ElementName = "bLocations")]
    public BLocations? BLocations { get; set; }

    [XmlElement(ElementName = "bSgmtChksum")]
    public string? BSgmntChksum { get; set; }

    [XmlElement(ElementName = "procTm")]
    public string? ProcTm { get; set; }
}

[XmlRoot(ElementName = "bHdln")]
public class BHdln
{

    [XmlElement(ElementName = "bHdlnTxt")]
    public string? BHdlnTxt { get; set; }

    [XmlElement(ElementName = "bVocHdlnCd")]
    public string? BVocHdlnCd { get; set; }
}

[XmlRoot(ElementName = "bNarrTxt")]
public class BNarrTxt
{

    [XmlElement(ElementName = "bLn")]
    public string? BLn { get; set; }

    [XmlAttribute(AttributeName = "bNarrTxtLang")]
    public string? BNarrTxtLang { get; set; }

    [XmlText]
    public string? Text { get; set; }
}

[XmlRoot(ElementName = "BEData")]
public class BEData
{

    [XmlElement(ElementName = "bIssueTmUTC")]
    public string? BIssueTmUTC { get; set; }

    [XmlElement(ElementName = "bHdln")]
    public BHdln? BHdln { get; set; }

    [XmlElement(ElementName = "bParameter")]
    public string BParameter = "NOT_USED";

    [XmlElement(ElementName = "bNarrTxt")]
    public BNarrTxt? BNarrTxt { get; set; }

    [XmlElement(ElementName = "bSrchRslt")]
    public string BSrchRslt = "NOT_USED";
}

[XmlRoot(ElementName = "BERecord")]
public class BERecord
{

    [XmlElement(ElementName = "action")]
    public string Action = "NOT_USED";

    [XmlElement(ElementName = "BEHdr")]
    public BEHdr? BEHdr { get; set; }

    [XmlElement(ElementName = "BEData")]
    public BEData? BEData { get; set; }

    [XmlElement(ElementName = "clientKey")]
    public string? ClientKey { get; set; }

    [XmlAttribute(AttributeName = "id")]
    public string? Id { get; set; }

    [XmlAttribute(AttributeName = "locationKey")]
    public string? LocationKey { get; set; }

    [XmlAttribute(AttributeName = "isWxscan")]
    public int IsWxscan { get; set; }

    [XmlText]
    public string? Text { get; set; }
}

[XmlRoot(ElementName = "Data")]
public class BERecordRoot
{

    [XmlElement(ElementName = "BERecord")]
    public List<BERecord>? BERecord { get; set; }

    [XmlAttribute(AttributeName = "type")]
    public string? Type { get; set; }

    [XmlText]
    public string? Text { get; set; }
}