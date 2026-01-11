using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class CurrentObservationsProduct : Base
{
    public CurrentObservationsProduct()
    {
        this.RecordName = "CurrentObservations";
        this.DataUrl =
            "https://api.weather.com/v1/geocode/{lat}/{long}/observations/current.xml?language={lang}&units={unit}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<CurrentObservationsResponse>>> Populate(string[] locations)
    {
        return await GetData<CurrentObservationsResponse>(locations);
    }
}