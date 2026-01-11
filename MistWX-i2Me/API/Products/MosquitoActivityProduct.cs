using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class MosquitoActivityProduct : Base
{
    public MosquitoActivityProduct()
    {
        RecordName = "MosquitoActivity";
        DataUrl =
            "https://api.weather.com/v2/indices/mosquito/daily/7day?geocode={geocode}&format=xml&language={lang}&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<MosquitoActivityResponse>>> Populate(string[] locations)
    {
        return await GetData<MosquitoActivityResponse>(locations);
    }
}
