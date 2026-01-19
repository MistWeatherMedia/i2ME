using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;
using System.Xml.Serialization;
using System.Xml;

namespace MistWX_i2Me.RecordGeneration;

public class DailyForecastRecord : I2Record
{
    public static double CMtoIN(double cm)
    {
        return cm / 2.54;
    }
    public static double CtoF(double c)
    {
        return (c * (9/5)) + 32;
    }
    public static string QualifierCheck(double snow, int iconCode, double temp, double rain)
    {
        string UnitConfig = Config.config.LocalStarConfig.Unit;
        // Unit checks
        if (UnitConfig != "e")
        {
            snow = CMtoIN(snow);
            temp = CtoF(temp);
        }
        // Icon code checks 
        if (iconCode == 43)
        {
            return "Q9030";
        } else if (iconCode == 4)
        {
            return "Q9610";
        } else if (iconCode == 1)
        {
            return "Q9810";
        } else if (iconCode == 2)
        {
            return "Q9820";
        }
        
        // Rain chceks 
        if (rain > 0.3)
        {
            return "Q9410";
        } else
        {
            
        }
        // Snow checks
        if (snow >= 3)
        {
            if (snow > 3 && snow <= 6)
            {
                return "Q9010";
            } else if (snow > 6 && snow <= 12)
            {
                return "Q9015";
            } else if (snow > 12)
            {
                return "Q9020";
            }
            return "Q9005";
        }
        // Temp checks
        if (temp <= 32){
            if (iconCode == 8)
            {
                return "Q9205";
            } else if (iconCode == 10)
            {
                return "Q9210";
            }
        }
        return "";
    }
    public async Task<string> MakeRecord(List<GenericResponse<DailyForecastResponse>> results)
    {
        Log.Info("Creating Daily Forecast Record");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "DailyForecast.xml");
        string recordScript = "<Data type=\"DailyForecast\">";

        foreach (var result in results)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DailyForecastResponse));
            StringWriter sw = new StringWriter();
            XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment, 
            });
            
            // check for potential severe weather qualifiers
            if (result.ParsedData.Forecasts != null)
            {  
                if (result.ParsedData.Forecasts.Forecast != null)
                {
                    foreach (Forecast fcst in result.ParsedData.Forecasts.Forecast)
                    {
                        fcst.QualifierCode = QualifierCheck(fcst.SnowQpf,0, fcst.MinTemp, fcst.Qpf);
                        if (fcst.Day != null)
                        {
                            fcst.Day.QualifierCode = QualifierCheck(fcst.Day.SnowQpf, fcst.Day.IconCode, fcst.Day.Temp, fcst.Day.Qpf);
                        }
                        if (fcst.Night != null)
                        {
                            fcst.Night.QualifierCode = QualifierCheck(fcst.Night.SnowQpf, fcst.Night.IconCode, fcst.Night.Temp, fcst.Night.Qpf);
                        }
                    }
                } 
            }
            
            xw.WriteWhitespace("");
            serializer.Serialize(xw, result.ParsedData);

            recordScript +=
                $"<DailyForecast id=\"000000000\" locationKey=\"{result.Location.coopId}\" isWxScan=\"0\">" +
                $"{sw}<clientKey>{result.Location.coopId}</clientKey></DailyForecast>";
        }

        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}