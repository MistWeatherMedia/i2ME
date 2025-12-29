using System.Xml.Serialization;
using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.RecordGeneration;

public class ClimatologyRecord : I2Record
{
    public async Task<string> MakeRecord(List<GenericResponse<Almanac1DayResponse>> results)
    {
        Log.Info("Creating Climatology Record.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "ClimatologyRecord.xml");
        string recordScript = "<Data type=\"ClimatologyRecord\">";

        foreach (var result in results)
        {
            ClimatologyRecordResponse cliRecRes = new ClimatologyRecordResponse();
            ClimatologyRec cliRec = new ClimatologyRec();
            cliRecRes.Key = result.Location.cliStn;
            cliRec.AvgHigh = result.ParsedData.temperatureAverageMax.FirstOrDefault();
            cliRec.AvgLow = result.ParsedData.temperatureAverageMin.FirstOrDefault();
            cliRec.RecHigh = result.ParsedData.temperatureRecordMax.FirstOrDefault();
            cliRec.RecLow = result.ParsedData.temperatureRecordMin.FirstOrDefault();
            cliRec.RecHighYear = result.ParsedData.almanacRecordYearMax.FirstOrDefault();
            cliRec.RecLowYear = result.ParsedData.almanacRecordYearMin.FirstOrDefault();
            cliRec.Year = System.DateTime.Now.Year;
            cliRec.Month = System.DateTime.Now.Month;
            cliRec.Day = System.DateTime.Now.Day;
            cliRecRes.ClimoRec = cliRec;

            XmlSerializer serializer = new XmlSerializer(typeof(ClimatologyRecordResponse));
            StringWriter sw = new StringWriter();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(sw, cliRecRes, ns);
            sw.Close();

            recordScript += 
                $"<ClimatologyRecord>" +
                $"<Key>{result.Location.cliStn}</Key>{sw.ToString()}</ClimatologyRecord>";
        }
        
        recordScript += "</Data>";
        recordScript.Replace('<?xml version="1.0" encoding="utf-16"?>', String.Empty);
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}