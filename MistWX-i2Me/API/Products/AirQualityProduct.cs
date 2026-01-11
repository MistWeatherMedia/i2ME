using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class AirQualityProduct : Base
{
    public AirQualityProduct()
    {
        RecordName = "AirQuality";
        DataUrl = "https://api.weather.com/v1/geocode/{lat}/{long}/airquality.xml?language={lang}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<AirQualityResponse>>> Populate(string[] locations)
    {
        return await GetData<AirQualityResponse>(locations);
    }
}