using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class PollenForecastProduct : Base
{
    public PollenForecastProduct()
    {
        RecordName = "PollenForecast";
        DataUrl =
            "https://api.weather.com/v2/indices/pollen/daypart/7day?geocode={geocode}&language={lang}&format=xml&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<PollenResponse>>> Populate(string[] locations)
    {
        return await GetData<PollenResponse>(locations);
    }
}