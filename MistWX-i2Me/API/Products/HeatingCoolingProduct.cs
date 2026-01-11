using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class HeatingCoolingProduct : Base
{
    public HeatingCoolingProduct()
    {
        RecordName = "HeatingCooling";
        DataUrl =
            "https://api.weather.com/v2/indices/heatCool/daypart/7day?geocode={geocode}&format=xml&language={lang}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<HeatingCoolingResponse>>> Populate(string[] locations)
    {
        return await GetData<HeatingCoolingResponse>(locations);
    }
}