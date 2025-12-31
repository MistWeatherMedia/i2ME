using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class TideForecastProduct : Base
{
    public TideForecastProduct()
    {
        this.RecordName = "TidesForecast";
        this.DataUrl =
            "https://api.weather.com/v1/geocode/{lat}/{long}/forecast/tides.xml?language=en-US&startDate={curDate}&endDate={curDatePlusFive}&units={unit}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<TideForecastResponse>>> Populate(string[] locations)
    {
        return await GetData<TideForecastResponse>(locations);
    }
}