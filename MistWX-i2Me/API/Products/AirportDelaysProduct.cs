using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class AirportDelaysProduct : Base
{
    public AirportDelaysProduct()
    {
        RecordName = "AirportDelays";
        DataUrl =
            "https://nasstatus.faa.gov/api/airport-events";
    }

    public async Task<List<GenericResponse<AirportDelaysResponse>>> Populate(string[] locations)
    {
        return await GetJsonData<AirportDelaysResponse>(locations);
    }
}