using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.RecordGeneration;

public class MosquitoActivityRecord : I2Record
{
    public async Task<string> MakeRecord(List<GenericResponse<MosquitoActivityResponse>> results)
    {
        Log.Info("Creating Mosquito Activity record.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "MosquitoActivity.xml");
        string recordScript = "<Data type=\"MosquitoActivity\">";

        foreach (var result in results)
        {
            recordScript += 
                $"<MosquitoActivity id=\"000000000\" locationKey=\"{result.Location.coopId}\" isWxScan=\"0\">" +
                $"{result.RawResponse}<clientKey>{result.Location.coopId}</clientKey></MosquitoActivity>";
        }
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}