using System.Xml;
using System.Xml.Serialization;
using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.RecordGeneration;

public class DHRecord : I2Record
{
    public async Task<string> MakeRecord(List<GenericResponse<HourlyForecastResponse>> results)
    {
        Log.Info("Creating DHRecord.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "DHRecord.xml");
        string recordScript = "<Data type=\"DHRecord\">";

        foreach (var result in results)
        {
            DHRecordResponse dHRecRes = new DHRecordResponse();
            
            DHRecordHeader dHRecHdr = new DHRecordHeader();
            dHRecHdr.CoopId = result.Location.coopId;
            dHRecHdr.ILevel = 2;
            dHRecHdr.StnNm = result.Location.prsntNm;
            dHRecHdr.ProcTm = System.DateTime.Now.ToString("yyyyMMddHHmmss");

            HourlyForecast fcst = result.ParsedData.Forecasts;
            DHRecordData dHRecData = new DHRecordData();
            dHRecData.hourNum = fcst.Num - 1;
            dHRecData.dayOfWeek = fcst.Dow;
            DateTimeOffset fcstValid = DateTimeOffset.FromUnixTimeSeconds(fcst.FcstValid);
            dHRecData.FcstValidGMT = fcst.FcstValid;
            dHRecData.FcstValidDay = fcstValid.ToString("yyyyddMM");
            dHRecData.FcstValidTime = fcstValid.ToString("HHmmss");
            dHRecData.LocValidDay = fcstValid.ToString("yyyyddMM");
            dHRecData.LocValidTime = fcstValid.ToString("HHmmss");
            dHRecData.LocalValidTime = $"{fcstValid.ToString("ddd dd MMM yyyy hh:mm:ss tt ")}GMT";
            dHRecData.TempF = fcst.Temp;
            dHRecData.TempC = Convert.ToInt32((fcst.Temp - 32) / 1.8);
            // No data for sky ceiling I assume?
            dHRecData.Sky = 27;
            dHRecData.SkyX = 2700;
            dHRecData.SensibleWx = fcst.Phrase32char;
            dHRecData.TSensibleWx = fcst.Phrase22char;
            dHRecData.ProbsOfPrecip = fcst.Pop;
            // No cPOS data
            dHRecData.chanceProbsOfSnow = 0;
            dHRecData.RelativeHumidity = fcst.Rh;
            dHRecData.WindSpeedMiles = fcst.Wspd;
            dHRecData.WindSpeedKm = Convert.ToInt32(fcst.Wspd * 1.60934);
            dHRecData.WindSpeedKnots = Convert.ToInt32(fcst.Wspd * 0.868976);
            dHRecData.windDir = fcst.Wdir;
            dHRecData.windDirCardinal = fcst.WdirCardinal;
            dHRecData.heatIndexF = fcst.Hi;
            dHRecData.heatIndexC = Convert.ToInt32((fcst.Hi - 32) / 1.8);
            dHRecData.windChillF = fcst.Wc;
            dHRecData.windChillC = Convert.ToInt32((fcst.Wc - 32) / 1.8);
            dHRecData.visiblityMiles = Convert.ToInt32(fcst.Vis);
            dHRecData.visiblityKm = Convert.ToInt32(fcst.Vis * 1.60934);
            dHRecData.cloudCover = fcst.Clds;
            dHRecData.dewPointF = fcst.Dewpt;
            dHRecData.dewPointC = Convert.ToInt32((fcst.Dewpt - 32) / 1.8);
            dHRecData.uvIndex = fcst.UvIndex;
            dHRecData.uvIndexRaw = fcst.UvIndexRaw;
            dHRecData.GolfIndex = fcst.GolfIndex;
            dHRecData.GolfCategory = fcst.GolfCategory;
            dHRecData.PrecipType = fcst.PrecipType;
            // Day/Night not provided
            if (DateTime.Now.Hour >= 18)
            {
               if (DateTime.Now.Hour < 6)
                {
                    dHRecData.dayNightIdx = "N"; 
                } 
            } else
            {
                dHRecData.dayNightIdx = "D";
            }
            dHRecData.wxman = fcst.Wxman;
            dHRecData.SubphrasePt1 = fcst.SubphrasePt1;
            dHRecData.SubphrasePt2 = fcst.SubphrasePt2;
            dHRecData.SubphrasePt3 = fcst.SubphrasePt3;
                
            // wtf is with HourlyForecast schema
            dHRecRes.DHRecordData.Add(dHRecData);

            XmlSerializer serializer = new XmlSerializer(typeof(DHRecordResponse));
            StringWriter sw = new StringWriter();
            XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment, 
            });
            xw.WriteWhitespace("");
            serializer.Serialize(xw, dHRecRes);
            sw.Close();

            recordScript += 
                $"<DHRecord>" +
                $"<Key>{result.Location.cliStn}</Key>{sw.ToString()}</DHRecord>";
        }
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}