using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;

namespace MistWX_i2Me.RecordGeneration;

public class TropicalAdvisoryRecord : I2Record
{
    public async Task<string> MakeRecord(List<GenericResponse<TropicalAdvisoryResponse>> results)
    {
        Log.Info("Creating Tropical Advisory record.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "TropicalAdvisory.xml");
        string recordScript = "<Data type=\"TropicalAdvisory\">";

        foreach (var result in results)
        {
            recordScript += 
                $"<TropicalAdvisory id=\"000000000\" locationKey=\"0\" isWxScan=\"0\">" +
                $"{result.RawResponse}<clientKey>0</clientKey></TropicalAdvisory>";
        }
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}