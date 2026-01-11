using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class PollenObservationsProduct : Base
{
    public PollenObservationsProduct()
    {
        this.RecordName = "PollenObservations";
        this.DataUrl =
            "https://api.weather.com/v1/geocode/{lat}/{long}/observations/pollen.xml?language={lang}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<PollenObservationsResponse>>> Populate(string[] locations)
    {
        return await GetData<PollenObservationsResponse>(locations);
    }
}