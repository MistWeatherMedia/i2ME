using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class DailyForecastProduct : Base
{
    public DailyForecastProduct()
    {
        RecordName = "DailyForecast";
        DataUrl =
            "https://api.weather.com/v1/geocode/{lat}/{long}/forecast/daily/7day.xml?language=en-US&units={unit}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<DailyForecastResponse>>> Populate(string[] locations)
    {
        return await GetData<DailyForecastResponse>(locations);
    }
}