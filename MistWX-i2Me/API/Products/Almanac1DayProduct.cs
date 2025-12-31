using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class Almanac1DayProduct : Base
{
    public Almanac1DayProduct()
    {
        RecordName = "Almanac1Day";
        DataUrl =
            "https://api.weather.com/v3/wx/almanac/daily/1day?geocode={geocode}&format=json&units={unit}&day={day}&month={month}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<Almanac1DayResponse>>> Populate(string[] locations)
    {
        return await GetJsonData<Almanac1DayResponse>(locations);
    }
}
