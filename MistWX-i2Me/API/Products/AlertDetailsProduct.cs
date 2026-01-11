using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using MistWX_i2Me.Schema.ibm;

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
            foreach (Alert alert in headline.ParsedData.alerts)
            {
                string url =
                    $"https://api.weather.com/v3/alerts/detail?alertId={alert.detailKey}&format=json&language={Config.config.LocalStarConfig.Language}&apiKey={Config.config.APIConfig.TwcApiKey}";
                string? res = await DownloadRecord(url);
                
                if (res == null)
                {
                    continue;
                }
                
                if (alertsCache.TryGetValue(alert.detailKey, out int expireTime))
                {
                    continue;
                }

                using (var stream = StreamFromString(res))
                {
                    AlertDetailResponse? response = await JsonSerializer.DeserializeAsync<AlertDetailResponse?>(stream);
                    results.Add(new GenericResponse<AlertDetailResponse>(headline.Location, res, response));
                    alertsCache.Set(alert.detailKey, alert.expireTimeUTC);
                    alertDetailKeys.Add(alert.detailKey);
                    Log.Debug($"Alert ID {alert.detailKey} cached, expires @ {alert.expireTimeUTC} UTC.");
                }
            }
        }
        
        return results;
    }
}