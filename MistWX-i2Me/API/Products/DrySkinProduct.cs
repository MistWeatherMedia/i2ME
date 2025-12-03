using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class DrySkinProduct : Base
{
    public DrySkinProduct()
    {
        RecordName = "DrySkin";
        DataUrl =
            "https://api.weather.com/v2/indices/drySkin/daypart/7day?geocode={geocode}&format=xml&language=en-US&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<DrySkinResponse>>> Populate(string[] locations)
    {
        return await GetData<DrySkinResponse>(locations);
    }
}
