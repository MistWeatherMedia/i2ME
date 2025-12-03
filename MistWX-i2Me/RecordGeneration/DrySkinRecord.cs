using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.RecordGeneration;

public class DrySkinRecord : I2Record
{
    public async Task<string> MakeRecord(List<GenericResponse<DrySkinResponse>> results)
    {
        Log.Info("Creating Dry Skin record.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "DrySkin.xml");
        string recordScript = "<Data type=\"DrySkin\">";

        foreach (var result in results)
        {
            recordScript += 
                $"<DrySkin id=\"000000000\" locationKey=\"{result.Location.coopId}\" isWxScan=\"0\">" +
                $"{result.RawResponse}<clientKey>{result.Location.coopId}</clientKey></DrySkin>";
        }
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}