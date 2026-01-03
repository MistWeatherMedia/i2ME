using System.Diagnostics;
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
            List<ClimatologyRec> cliRecList = new List<ClimatologyRec>();
            cliRecRes.Key = result.Location.cliStn;
            cliRecRes.ClimoRec = cliRecList;

            for (var i = 0; i < result.ParsedData.almanacInterval.Count(); i++)
            {
                ClimatologyRec cliRec = new ClimatologyRec();
                cliRec.Loc = result.Location.cliStn;
                if (result.ParsedData.temperatureAverageMax != null)
                {
                    if (result.ParsedData.temperatureAverageMax[i] != null)
                    {
                        cliRec.AvgHigh = Convert.ToInt32(result.ParsedData.temperatureAverageMax[i]);
                    }
                    
                }
                if (result.ParsedData.temperatureAverageMin != null)
                {
                    if (result.ParsedData.temperatureAverageMin[i] != null)
                    {
                        cliRec.AvgLow = Convert.ToInt32(result.ParsedData.temperatureAverageMin[i]);
                    }
                    
                }
                if (result.ParsedData.temperatureRecordMax != null)
                {
                    if (result.ParsedData.temperatureRecordMax[i] != null)
                    {
                        cliRec.RecHigh = Convert.ToInt32(result.ParsedData.temperatureRecordMax[i]);
                    }
                    
                }
                if (result.ParsedData.temperatureRecordMin != null)
                {
                    if (result.ParsedData.temperatureRecordMin[i] != null)
                    {
                        cliRec.RecLow = Convert.ToInt32(result.ParsedData.temperatureRecordMin[i]);
                    }
                    
                }
                if (result.ParsedData.almanacRecordYearMax != null)
                {
                    if (result.ParsedData.almanacRecordYearMax[i] != null)
                    {
                        cliRec.RecHighYear = Convert.ToInt32(result.ParsedData.almanacRecordYearMax[i]);
                    }
                    
                }
                if (result.ParsedData.almanacRecordYearMin != null)
                {
                    if (result.ParsedData.almanacRecordYearMin[i] != null)
                    {
                        cliRec.RecLowYear = Convert.ToInt32(result.ParsedData.almanacRecordYearMin[i]);
                    }
                }
                
                cliRec.Year = System.DateTime.Now.Year;
                cliRec.Month = System.DateTime.Now.Month;
                cliRec.Day = System.DateTime.Now.Day;

                cliRecList.Add(cliRec);
            }
            

            XmlSerializer serializer = new XmlSerializer(typeof(ClimatologyRecordResponse));
            StringWriter sw = new StringWriter();
            XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment, 
            });
            xw.WriteWhitespace("");
            serializer.Serialize(xw, cliRecRes);
            sw.Close();

            recordScript += 
                $"<ClimatologyRecord>" +
                $"<Key>{result.Location.cliStn}</Key>{sw.ToString()}</ClimatologyRecord>";
        }
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}