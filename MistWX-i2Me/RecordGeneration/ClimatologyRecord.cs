using System.Xml;
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
            if (result.ParsedData.temperatureAverageMax != null)
            {
                cliRec.AvgHigh = result.ParsedData.temperatureAverageMax.First();
            }
            if (result.ParsedData.temperatureAverageMin != null)
            {
               cliRec.AvgLow = result.ParsedData.temperatureAverageMin.First(); 
            }
            if (result.ParsedData.temperatureRecordMax != null)
            {
                cliRec.RecHigh = result.ParsedData.temperatureRecordMax.First();
            }
            if (result.ParsedData.temperatureRecordMin != null)
            {
                cliRec.RecLow = result.ParsedData.temperatureRecordMin.First();
            }
            if (result.ParsedData.almanacRecordYearMax != null)
            {
                cliRec.RecHighYear = result.ParsedData.almanacRecordYearMax.First();
            }
            if (result.ParsedData.almanacRecordYearMin != null)
            {
                cliRec.RecLowYear = result.ParsedData.almanacRecordYearMin.First();
            }
            
            cliRec.Year = System.DateTime.Now.Year;
            cliRec.Month = System.DateTime.Now.Month;
            cliRec.Day = System.DateTime.Now.Day;
            cliRecRes.ClimoRec = cliRec;

            XmlSerializer serializer = new XmlSerializer(typeof(ClimatologyRecordResponse));
            StringWriter sw = new StringWriter();
            XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment, 
            });
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(xw, cliRecRes, ns);
            sw.Close();

            recordScript += 
                $"<ClimatologyRecord>" +
                $"<Key>{result.Location.cliStn}</Key>{xw.ToString()}</ClimatologyRecord>";
        }
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}