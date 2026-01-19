using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;

namespace MistWX_i2Me.API.Products;

public class LocServNearAirportProduct : Base
{
    public LocServNearAirportProduct()
    {
        RecordName = "LocServNearAirport";
        DataUrl =
            "https://api.weather.com/v3/location/near?geocode={lat},{long}&product=airport&format=json&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<LocServNearAirportResponse>>> Populate(string[] locations)
    {
        return await GetJsonData<LocServNearAirportResponse>(locations);
    }

    public async Task<GenericResponse<LocServNearAirportResponse>?> Receive(LFRecordLocation locations)
    {
        return await GetJsonDataLFR<LocServNearAirportResponse>(locations);
    }
}
