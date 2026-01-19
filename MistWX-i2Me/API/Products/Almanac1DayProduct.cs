using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;

namespace MistWX_i2Me.API.Products;

public class Almanac1DayProduct : Base
{
    public Almanac1DayProduct()
    {
        RecordName = "Almanac1Day";
        DataUrl =
            "https://api.weather.com/v3/wx/almanac/daily/45day?geocode={geocode}&format=json&units={unit}&startDay={startDay45Day}&startMonth={startMonth45Day}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<Almanac1DayResponse>>> Populate(string[] locations)
    {
        return await GetJsonData<Almanac1DayResponse>(locations);
    }

    public async Task<GenericResponse<Almanac1DayResponse>?> Receive(LFRecordLocation location)
    {
        return await GetJsonDataLFR<Almanac1DayResponse>(location);
    }
}
