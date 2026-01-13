using System.Data.Common;
using System.Data.SQLite;
using Dapper;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Caching.Memory;
using MistWX_i2Me.Schema.System;

namespace MistWX_i2Me.API;

public class Base
{
    protected HttpClient Client = new HttpClient();
    protected string ApiKey = Config.config.APIConfig.TwcApiKey;

    protected string RecordName = String.Empty;
    protected string DataUrl = String.Empty;
    private readonly IMemoryCache locationCache = Globals.LocationCache;

    /// <summary>
    /// Downloads XML data from the specified URL
    /// </summary>
    /// <param name="url">API URL to send a GET request to</param>
    /// <returns>XML Document as a string object</returns>
    public async Task<string> DownloadRecord(string url)
    {
        Log.Debug(url);

        try
        {
            HttpResponseMessage response = await Client.GetAsync(url);
            // response.EnsureSuccessStatusCode();

            Log.Debug(response.StatusCode.ToString());

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Log.Debug("Bad Request issue!");

                return String.Empty;
            }
            
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return String.Empty;
            }


            byte[] content = await Client.GetByteArrayAsync(url);
            string contentString = Encoding.UTF8.GetString(content);

            return contentString;
        }
        catch (Exception ex)
        {
            
            Log.Debug($"Failed to Download Record for {url}");
            Log.Debug(ex.Message);
            // Print stacktrace to the debug console if applicable
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                Log.Debug(ex.StackTrace);
            }
            return String.Empty;
        }

    }

    
    public string GetInnerXml(string content)
    {
        XDocument doc = XDocument.Parse(content);
        XElement? root = doc.Root;
        string innerXml;
        
        if (root == null)
        {
            return String.Empty;
        }

        XmlReader reader = root.CreateReader();
        reader.MoveToContent();
        
        innerXml = reader.ReadInnerXml();
        reader.Close();
        
        return innerXml;
    }

    
    public string FormatUrl(LFRecordLocation location)
    {
        string url = DataUrl;
        url = url.Replace("{apiKey}", ApiKey);

        if (url.Contains("{lat}"))
        {
            url = url.Replace("{lat}", location.lat);
        }

        if (url.Contains("{long}"))
        {
            url = url.Replace("{long}", location.@long);
        }

        if (url.Contains("{geocode}"))
        {
            url = url.Replace("{geocode}", $"{location.lat},{location.@long}");
        }

        if (url.Contains("{zip}"))
        {
            url = url.Replace("{zip}", location.zip2locId);
        }

        if (url.Contains("{zone}"))
        {
            if (location.zoneId == "ZZZ999" || string.IsNullOrEmpty(location.zoneId))
            {
                url = url.Replace("{zone}", location.cntyId);
            }
            else
            {
                url = url.Replace("{zone}", location.zoneId);
            }
        }

        if (url.Contains("{cntyId}"))
        {
            url = url.Replace("{cntyId}", location.cntyId);
        }

        if (url.Contains("{locId}"))
        {
            url = url.Replace("{locId}", location.locId);
        }

        if (url.Contains("{locType}"))
        {
            url = url.Replace("{locType}", location.locType);
        }

        if (url.Contains("{cntryCd}"))
        {
            url = url.Replace("{cntryCd}", location.cntryCd);
        }
        
        if (url.Contains("{curDate}"))
        {
            url = url.Replace("{curDate}", DateTime.Now.ToString("yyyyMMdd"));
        }

        if (url.Contains("{curDatePlusFive}"))
        {
            url = url.Replace("{curDatePlusFive}", DateTime.Now.AddDays(5).ToString("yyyyMMdd"));
        }

        if (url.Contains("{day}"))
        {
            url = url.Replace("{day}", DateTime.Now.Day.ToString());
        }

        if (url.Contains("{month}"))
        {
            url = url.Replace("{month}", DateTime.Now.Month.ToString());
        }

        if (url.Contains("{unit}"))
        {
            url = url.Replace("{unit}", Config.config.LocalStarConfig.Unit);
        }

        if (url.Contains("{startDay45Day}"))
        {
            url = url.Replace("{startDay45Day}", DateTime.Now.Subtract(TimeSpan.FromDays(45)).ToString("dd"));
        }

        if (url.Contains("{startMonth45Day}"))
        {
            url = url.Replace("{startMonth45Day}", DateTime.Now.Subtract(TimeSpan.FromDays(45)).ToString("MM"));
        }

        if (url.Contains("{lang}"))
        {
            url = url.Replace("{lang}", Config.config.LocalStarConfig.Language);
        }

        return url;
    }


    public async Task<string?> DownloadLocationData(LFRecordLocation location)
    {
        Log.Debug($"Downloading {RecordName} for location {location.locId}");
        
        string url = FormatUrl(location);
        string response = await DownloadRecord(url);
        return response;
    }

    public async Task<LFRecordLocation> GetLocInfo(string locId)
    {
        if (locationCache.TryGetValue(locId, out LFRecordLocation? cachedLocation))
        {
            if (cachedLocation != null)
            {
                Log.Debug($"Pulled location {locId} from locations cache.");
                return cachedLocation;
            }
        }
        
        SQLiteConnection sqlite =
            new SQLiteConnection($"Data Source={Path.Combine(AppContext.BaseDirectory, "Data", "LFRecord.db")}", true);
        
        await sqlite.OpenAsync();

        LFRecordLocation location = await sqlite.QuerySingleAsync<LFRecordLocation>
            ($"SELECT * FROM LFRecord WHERE locId = '{locId}'");
        
        await sqlite.CloseAsync();
        
        locationCache.Set(locId, location);

        Log.Debug($"Location {locId} added to the location cache.");

        return location;
    }

    public static Stream StreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public async Task<List<GenericResponse<T>>> GetData<T>(string[] locations)
    {
        List<GenericResponse<T>> results = new List<GenericResponse<T>>();

        foreach (string location in locations)
        {
            LFRecordLocation locationInfo = await GetLocInfo(location);
            string? response = await DownloadLocationData(locationInfo);

            if (string.IsNullOrEmpty(response))
            {
                // Just keep going! - PB
                // return results;
                continue;
            }

            // For Current Observations they change the format of the XML depending on what unit you have - no go for i2
            if (response.Contains("<metric>"))
            {
                response = response.Replace("<metric>", "<imperial>");
                response = response.Replace("</metric>", "</imperial>");
            }

            if (response.Contains("<metric_si>"))
            {
                response = response.Replace("<metric_si>", "<imperial>");
                response = response.Replace("</metric_si>", "</imperial>");
            }

            if (response.Contains("<uk_hybrid>"))
            {
                response = response.Replace("<uk_hybrid>", "<imperial>");
                response = response.Replace("</uk_hybrid>", "</imperial>");
            }

            string data = GetInnerXml(response);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (StringReader reader = new StringReader(response))
                {
                    var deserializedData = (T?)serializer.Deserialize(reader);

                    if (deserializedData == null)
                    {
                        Log.Warning($"Failed to deserialize {RecordName} for location {location}");
                        continue;
                    }

                    results.Add(new GenericResponse<T>(locationInfo, data, deserializedData));
                }
            }
            catch (InvalidOperationException ex)
            {
                Log.Debug(ex.Message);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    Log.Debug(ex.StackTrace);
                }
                Log.Warning($"Location {location} has no data for {RecordName}, skipping..");
            }
        }

        return results;
    }

    /// <summary>
    /// Creates a list of GenericResponse objects for JSON endpoints.
    /// </summary>
    /// <param name="locations">Array of location IDs</param>
    /// <typeparam name="T">Type of API response</typeparam>
    /// <returns>A list of GenericResponse<T> objects.</returns>
    public async Task<List<GenericResponse<T>>> GetJsonData<T>(string[] locations)
    {
        List<GenericResponse<T>> results = new List<GenericResponse<T>>();

        foreach (string location in locations)
        {
            LFRecordLocation locationInfo = await GetLocInfo(location);
            string? response = await DownloadLocationData(locationInfo);

            if (!string.IsNullOrEmpty(response))
            {
                using (var stream = StreamFromString(response))
                {
                    try
                    {
                        T? deserializedData = await JsonSerializer.DeserializeAsync<T?>(stream);
                        results.Add(new GenericResponse<T>(locationInfo, response, deserializedData));
                        continue;
                    }
                    catch (JsonException exception)
                    {
                        Log.Error($"Failed to parse {RecordName} data for location {location}.");
                        Log.Debug(exception.Message);
                        
                        // Print stacktrace to the debug console if applicable
                        if (!string.IsNullOrEmpty(exception.StackTrace))
                        {
                            Log.Debug(exception.StackTrace);
                        }
                    }

                }
            }
            Log.Warning($"{RecordName} returned no data for location {location}.");
        }

        return results;
    }
}