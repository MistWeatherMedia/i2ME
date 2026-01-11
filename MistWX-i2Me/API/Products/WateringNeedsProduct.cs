using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class WateringNeedsProduct : Base
{
    public WateringNeedsProduct()
    {
        RecordName = "WateringNeeds";
        DataUrl =
            "https://api.weather.com/v2/indices/wateringNeeds/daypart/7day?geocode={geocode}&format=xml&language={lang}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<WateringNeedsResponse>>> Populate(string[] locations)
    {
        return await GetData<WateringNeedsResponse>(locations);
    }
}
