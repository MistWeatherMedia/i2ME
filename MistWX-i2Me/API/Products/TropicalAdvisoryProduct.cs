using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.API.Products;

public class TropicalAdvisoryProduct : Base
{
    public TropicalAdvisoryProduct()
    {
        RecordName = "TropicalAdvisory";
        DataUrl =
            "https://api.weather.com/v2/tropical/currentposition?source=default&basin=all&units={unit}&language={lang}&format=xml&nautical=true&apiKey={apiKey}";
    }

    public async Task<List<GenericResponse<TropicalAdvisoryResponse>>> Populate()
    {
        var locations = new string[] {"USGA0267"};
        return await GetData<TropicalAdvisoryResponse>(locations);
    }
}