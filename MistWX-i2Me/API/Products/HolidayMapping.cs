using System.Xml.Serialization;
using MistWX_i2Me.Schema.ibm;
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
        XmlSerializer serializer = new(typeof(HolidayMappingResponse));
        StreamReader reader = new(Path.Combine(AppContext.BaseDirectory, "Custom", "Mapping", "HolidayMapping.xml"));
        HolidayMappingResponse? response = (HolidayMappingResponse?)serializer.Deserialize(reader);
        if (response != null)
        {
            return response;
        } else
        {
            return new HolidayMappingResponse();
        }
    }
}