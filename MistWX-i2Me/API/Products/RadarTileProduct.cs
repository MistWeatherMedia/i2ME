using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;
using MistWX_i2Me;
using NetVips;

namespace MistWX_i2Me.API.Products;

public class RadarTileProduct : Base
{
    public RadarTileProduct(int time, double x, double y, string radarType)
    {
        RecordName = "RadarTile";
        DataUrl =
            $"https://api.weather.com/v3/TileServer/tile?product={radarType}&ts={time}&xyz={x}:{y}:6&apiKey={Config.config.APIConfig.TwcApiKey}";
    }

    public async Task<Image> Populate()
    {
        Client.Timeout = Timeout.InfiniteTimeSpan;
        return await GetTileData();
    }
}
