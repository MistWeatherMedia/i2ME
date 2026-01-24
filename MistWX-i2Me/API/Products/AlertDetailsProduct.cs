using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using MistWX_i2Me.Schema.ibm;
using System.Data.SqlTypes;
using System.Data.SQLite;
using MistWX_i2Me.Schema.System;
using System.Text.Json.Serialization;

namespace MistWX_i2Me.API.Products;

public class AlertDetailsProduct : Base
{
    public AlertDetailsProduct()
    {
        RecordName = "AlertDetails";
        DataUrl = "N/A";    // Handled differently due to the endpoints used
    }

    public async Task<List<GenericResponse<AlertDetailResponse>>> Populate(List<GenericResponse<HeadlineResponse>> headlines)
    {
        IMemoryCache alertsCache = Globals.AlertsCache;
        List<string> alertDetailKeys = Globals.AlertDetailKeys;
        List<GenericResponse<AlertDetailResponse>> results = new();
        
        foreach (var headline in headlines)
        {
            if (headline.ParsedData.alerts != null)
            {
                foreach (Alert alert in headline.ParsedData.alerts)
                {
                    string url =
                        $"https://api.weather.com/v3/alerts/detail?alertId={alert.detailKey}&format=json&language={Config.config.LocalStarConfig.Language}&apiKey={Config.config.APIConfig.TwcApiKey}";
                    byte[]? resbyte = await DownloadRecord(url);
                    
                    if (resbyte == null)
                    {
                        continue;
                    }

                    string res = Encoding.UTF8.GetString(resbyte);
                    

                    using (var stream = StreamFromString(res))
                    {
                        AlertDetailResponse? response = await JsonSerializer.DeserializeAsync<AlertDetailResponse?>(stream, new JsonSerializerOptions() {DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull});
                        if (response != null)
                        {
                            results.Add(new GenericResponse<AlertDetailResponse>(headline.Location, res, response));
                        }
                    }
                }
            }
            
        }
        
        return results;
    }
}