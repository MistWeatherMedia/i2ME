using System.Xml.Serialization;
using MistWX_i2Me.RecordGeneration;

namespace MistWX_i2Me.Schema.twc;

[XmlRoot(ElementName="ImageSequenceDef")]
public class ImageSequenceDef { 

	[XmlAttribute(AttributeName="type")] 
	public string? type { get; set; } 

    [XmlAttribute(AttributeName="area")] 
	public string? area { get; set; } 

    [XmlElement(ElementName="LowerLeftLong")]
    public double LowerLeftLong { get; set; }

    [XmlElement(ElementName="LowerLeftLat")]
    public double LowerLeftLat { get; set; }

    [XmlElement(ElementName="UpperRightLong")]
    public double UpperRightLong { get; set; }

    [XmlElement(ElementName="UpperRightLat")]
    public double UpperRightLat { get; set; }

    [XmlElement(ElementName="VerticalAdjustment")]
    public double VerticalAdjustment { get; set; }

    [XmlElement(ElementName="OriginalImageWidth")]
    public int OriginalImageWidth { get; set; }

    [XmlElement(ElementName="OriginalImageHeight")]
    public int OriginalImageHeight { get; set; }

    [XmlElement(ElementName="MaxImages")]
    public int MaxImages { get; set; }

    [XmlElement(ElementName="Gap")]
    public int Gap { get; set; }

    [XmlElement(ElementName="ImagesInterval")]
    public int ImagesInterval { get; set; }

    [XmlElement(ElementName="Expiration")]
    public int Expiration { get; set; }

    [XmlElement(ElementName="DeletePadding")]
    public int DeletePadding { get; set; }

    [XmlElement(ElementName="FileNameDateFormat")]
    public string? FileNameDateFormat { get; set; }

    public Point<double> GrabUpperRight()
    {
        return new Point<double>(UpperRightLat, UpperRightLong);
    }

    public Point<double> GrabLowerLeft()
    {
        return new Point<double>(LowerLeftLat, LowerLeftLong);
    }

    public Point<double> GrabUpperLeft()
    {
        return new Point<double>(UpperRightLat, LowerLeftLong);
    }

    public Point<double> GrabLowerRight()
    {
        return new Point<double>(LowerLeftLat, UpperRightLong);
    }
    
}

[XmlRoot(ElementName="ImageSequenceDefs")]
public class ImageSequenceDefs
{
    [XmlElement(ElementName ="ImageSequenceDef")] 
	public List<ImageSequenceDef>? ImageSequenceDef { get; set; } 
}