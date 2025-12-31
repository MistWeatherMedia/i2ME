using System.Xml.Serialization;
using System.Xml
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.API.Products;

public class HolidayMappingProduct : Base
{
    public HolidayMappingProduct()
    {
        this.RecordName = "HolidayMapping";
    }

    public async Task<HolidayMappingResponse> Populate()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(HolidayMappingResponse));
        StreamReader reader = new StreamReader(Path.Combine(AppContext.BaseDirectory, "Data", "Mapping", "HolidayMapping.xml"));
        return (HolidayMappingResponse)serializer.Deserialize(reader);
    }
}