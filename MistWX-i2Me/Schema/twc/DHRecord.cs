using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="DHHdr")]
public class DHRecordHeader { 

	[XmlAttribute(AttributeName="iLevel")] 
	public int ILevel { get; set; } 

    [XmlElement(ElementName="coopId")]
    public string CoopId { get; set; }

    [XmlElement(ElementName="stnNm")]
    public string StnNm { get; set; }

    [XmlElement(ElementName="procTm")]
    public string ProcTm { get; set; }
}

[XmlRoot(ElementName="DHData")]
public class DHRecordData { 

    [XmlElement(ElementName="hrNum")]
    public int hourNum { get; set; }

    [XmlElement(ElementName="dow")]
    public string dayOfWeek { get; set; }

    [XmlElement(ElementName="fcstValGMT")]
    public string FcstValidGMT { get; set; }

    [XmlElement(ElementName="DHlclValTm")]
    public string LocalValidTime { get; set; }

    [XmlElement(ElementName="fcstValDay")]
    public string FcstValidDay { get; set; }

    [XmlElement(ElementName="fcstValTm")]
    public string FcstValidTime { get; set; }

    [XmlElement(ElementName="locValDay")]
    public string LocValidDay { get; set; }

    [XmlElement(ElementName="locValTm")]
    public string LocValidTime { get; set; }

    [XmlElement(ElementName="tmpF")]
    public int TempF { get; set; }

    [XmlElement(ElementName="tmpC")]
    public int TempC { get; set; }

    [XmlElement(ElementName="sky")]
    public int Sky { get; set; }

    [XmlElement(ElementName="skyX")]
    public int SkyX { get; set; }

    [XmlElement(ElementName="snsblWx")]
    public string SensibleWx { get; set; }

    [XmlElement(ElementName="tSnsblWx")]
    public string TSensibleWx { get; set; }

    [XmlElement(ElementName="wrlsWx")]
    public string WrlsWx { get; set; }

    [XmlElement(ElementName="pOP")]
    public int ProbsOfPrecip { get; set; }

    [XmlElement(ElementName="cPOS")]
    public int chanceProbsOfSnow { get; set; }

    [XmlElement(ElementName="rH")]
    public int RelativeHumidity { get; set; }

    [XmlElement(ElementName="wSpdM")]
    public int WindSpeedMiles { get; set; }

    [XmlElement(ElementName="wSpdK")]
    public int WindSpeedKm { get; set; }

    [XmlElement(ElementName="wSpdKn")]
    public int WindSpeedKnots { get; set; }

    [XmlElement(ElementName="wDir")]
    public int windDir { get; set; }

    [XmlElement(ElementName="wDirAsc")]
    public string windDirCardinal { get; set; }

    [XmlElement(ElementName="hIF")]
    public int heatIndexF { get; set; }

    [XmlElement(ElementName="hIC")]
    public int heatIndexC { get; set; }

    [XmlElement(ElementName="wCF")]
    public int windChillF { get; set; }

    [XmlElement(ElementName="wCC")]
    public int windChillC { get; set; }

    [XmlElement(ElementName="visM")]
    public int visiblityMiles { get; set; }

    [XmlElement(ElementName="visK")]
    public int visiblityKm { get; set; }

    [XmlElement(ElementName="clds")]
    public int cloudCover { get; set; }

    [XmlElement(ElementName="dwptF")]
    public int dewPointF { get; set; }

    [XmlElement(ElementName="dwptC")]
    public int dewPointC { get; set; }

    [XmlElement(ElementName="uvIdx")]
    public int uvIndex { get; set; }

    [XmlElement(ElementName="uvIdxRw")]
    public double uvIndexRaw { get; set; }

    [XmlElement(ElementName="glfIdx")]
    public string GolfIndex { get; set; }

    [XmlElement(ElementName="glfCat")]
    public string GolfCategory { get; set; }

    [XmlElement(ElementName="precipTyp")]
    public string PrecipType { get; set; }

    [XmlElement(ElementName="dyNghtInd")]
    public string dayNightIdx { get; set; }

    [XmlElement(ElementName="wxMan")]
    public string wxman { get; set; }

    [XmlElement(ElementName="subPhrsPrt1")]
    public string SubphrasePt1 { get; set; }

    [XmlElement(ElementName="subPhrsPrt2")]
    public string SubphrasePt2 { get; set; }

    [XmlElement(ElementName="subPhrsPrt3")]
    public string SubphrasePt3 { get; set; }

}

[XmlRoot(ElementName="DHRecord")]
public class DHRecordResponse { 

	[XmlAttribute(AttributeName="id")] 
	public string Id { get; set; } 

    [XmlElement(ElementName="action")]
    public int Action { get; set; }

    [XmlElement(ElementName="DHHdr")]
    public DHRecordHeader DHRecordHeader { get; set; }

    [XmlElement(ElementName="DHData")]
    public List<DHRecordData> DHRecordData { get; set; }
}