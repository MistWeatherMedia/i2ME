using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;

namespace MistWX_i2Me.API.Products;

public class RadarImageryProduct : Base
{
    public RadarImageryProduct()
    {
        RecordName = "RadarImagery";
        DataUrl =
            "https://api.weather.com/v3/TileServer/series/productSet?apiKey={apiKey}&filter=twcRadarMosaic,sat";
    }

    public async Task<GenericResponse<RadarImageryResponse>?> Populate()
    {
        return await GetJsonDataLFR<RadarImageryResponse>(new LFRecordLocation());
    }
}
