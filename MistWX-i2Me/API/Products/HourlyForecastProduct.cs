using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class HourlyForecastProduct : Base
{
    public HourlyForecastProduct()
    {
        RecordName = "HourlyForecast";
        DataUrl =
            "https://api.weather.com/v1/geocode/{lat}/{long}/forecast/hourly/48hour.xml?language={lang}&units={unit}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<HourlyForecastResponse>>> Populate(string[] locations)
    {
        return await GetData<HourlyForecastResponse>(locations);
    }
}