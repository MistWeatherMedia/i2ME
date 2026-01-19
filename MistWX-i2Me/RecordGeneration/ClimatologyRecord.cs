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
            ClimatologyRecordResponse cliRecRes = new();
            List<ClimatologyRec> cliRecList = new();
            if (result.Location.cliStn != null)
            {
                cliRecRes.Key = result.Location.cliStn;
            } else
            {
                cliRecRes.Key = "0";
                Log.Warning($"{result.Location.locId} doesn't have a cliStn!");
            }
            cliRecRes.ClimoRec = cliRecList;

            if (result.ParsedData.almanacInterval != null)
            {
                for (var i = 0; i < result.ParsedData.almanacInterval.Count; i++)
                {
                    ClimatologyRec cliRec = new()
                    {
                        Loc = result.Location.cliStn ?? "0",
                        AvgHigh = (result.ParsedData.temperatureAverageMax ?? Enumerable.Repeat<int?>(null, result.ParsedData.almanacInterval.Count).ToList())[i] ?? 0,
                        AvgLow = (result.ParsedData.temperatureAverageMin ?? Enumerable.Repeat<int?>(null, result.ParsedData.almanacInterval.Count).ToList())[i] ?? 0,
                        RecHigh = (result.ParsedData.temperatureRecordMax ?? Enumerable.Repeat<int?>(null, result.ParsedData.almanacInterval.Count).ToList())[i] ?? 0,
                        RecLow = (result.ParsedData.temperatureRecordMin ?? Enumerable.Repeat<int?>(null, result.ParsedData.almanacInterval.Count).ToList())[i] ?? 0,
                        RecHighYear = (result.ParsedData.almanacRecordYearMax ?? Enumerable.Repeat<int?>(null, result.ParsedData.almanacInterval.Count).ToList())[i] ?? 2026,
                        RecLowYear = (result.ParsedData.almanacRecordYearMin ?? Enumerable.Repeat<int?>(null, result.ParsedData.almanacInterval.Count).ToList())[i] ?? 2026,
                        Year = System.DateTime.Now.Year,
                        Month = (result.ParsedData.almanacRecordDate ?? Enumerable.Repeat<string>("1220", result.ParsedData.almanacInterval.Count).ToList())[i][..2],
                        Day = (result.ParsedData.almanacRecordDate ?? Enumerable.Repeat<string>("1220", result.ParsedData.almanacInterval.Count).ToList())[i][3..]
                    };
                    
                    cliRecList.Add(cliRec);
                    if (i == result.ParsedData.almanacInterval.Count - 1)
                    {
                        ClimatologyRec todayCliRec = new()
                        {
                            Loc = result.Location.cliStn ?? "0",
                            AvgHigh = cliRec.AvgHigh,
                            AvgLow = cliRec.AvgLow,
                            RecHigh = cliRec.RecHigh,
                            RecHighYear = cliRec.RecHighYear,
                            RecLow = cliRec.RecLow,
                            RecLowYear = cliRec.RecLowYear,
                            Year = System.DateTime.Now.Year,
                            Month = System.DateTime.Now.ToString("MM"),
                            Day = System.DateTime.Now.ToString("dd")
                        };

                        cliRecList.Add(todayCliRec);
                    }
                }
            }
            
            XmlSerializer serializer = new(typeof(ClimatologyRecordResponse));
            StringWriter sw = new();
            XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment, 
            });
            xw.WriteWhitespace("");
            serializer.Serialize(xw, cliRecRes);
            sw.Close();

            recordScript += sw.ToString();
        }
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}