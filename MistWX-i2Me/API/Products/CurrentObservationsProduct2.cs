using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;

namespace MistWX_i2Me.API.Products;

public class CurrentObservationsProduct2 : Base
{
    public CurrentObservationsProduct2()
    {
        this.RecordName = "CurrentObservations2";
        this.DataUrl =
            "https://api.weather.com/v1/geocode/{lat}/{long}/observations.json?language={lang}&units={unit}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<CurrentObservations2Response>>> Populate(string[] locations)
    {
        return await GetJsonData<CurrentObservations2Response>(locations);
    }

    public async Task<GenericResponse<CurrentObservations2Response>?> Receive(LFRecordLocation locations)
    {
        return await GetJsonDataLFR<CurrentObservations2Response>(locations);
    }
}