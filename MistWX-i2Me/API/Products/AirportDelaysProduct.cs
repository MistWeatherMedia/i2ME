using MistWX_i2Me.Schema.faa;

namespace MistWX_i2Me.API.Products;

public class AirportDelaysProduct : Base
{
    public AirportDelaysProduct()
    {
        RecordName = "AirportDelays";
        DataUrl =
            "https://nasstatus.faa.gov/api/airport-events";
    }

    public async Task<GenericResponse<List<AirportEvent>>?> Populate()
    {
        return await GetJsonDataLFR<List<AirportEvent>>(new Schema.System.LFRecordLocation());
    }
}