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
            DHRecordResponse dHRecRes = new();
            List<DHRecordData> dhRecordDataList = new();
            dHRecRes.DHRecordData = dhRecordDataList;

            DHRecordHeader dHRecHdr = new()
            {
                CoopId = result.Location.coopId ?? "0",
                StnNm = result.Location.prsntNm ?? "",
                ProcTm = System.DateTime.Now.ToString("yyyyMMddHHmmss"),
                ILevel = 2
            };

            if (result.ParsedData.Forecasts != null) {
                if (result.ParsedData.Forecasts.Forecast != null)
                {
                    foreach (var fcst in result.ParsedData.Forecasts.Forecast)
                    {
                        DateTimeOffset fcstValid = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(fcst.FcstValid));
                        string UnitConfig = Config.config.LocalStarConfig.Unit;
                        DHRecordData dHRecData = new()
                        {
                            hourNum = fcst.Num - 1,
                            dayOfWeek = fcst.Dow ?? "",
                            FcstValidGMT = fcst.FcstValid ?? "",
                            FcstValidDay = fcstValid.ToString("yyyyddMM"),
                            FcstValidTime = fcstValid.ToString("yyyyddMM"),
                            LocValidDay = fcstValid.ToString("yyyyddMM"),
                            LocValidTime = fcstValid.ToString("HHmmss"),
                            LocalValidTime = $"{fcstValid.ToString("ddd dd MMM yyyy hh:mm:ss tt ")}GMT",
                            TempF = fcst.Temp,
                            TempC = Convert.ToInt32((fcst.Temp - 32) / 1.8),
                            // No data for sky ceiling I assume?
                            Sky = 27,
                            SkyX = 2700,
                            SensibleWx = fcst.Phrase32char ?? "",
                            TSensibleWx = fcst.Phrase22char ?? "",
                            ProbsOfPrecip = fcst.Pop,
                            // No cPOS data
                            chanceProbsOfSnow = 0,
                            RelativeHumidity = fcst.Rh,
                            WindSpeedMiles = fcst.Wspd,
                            WindSpeedKm = Convert.ToInt32(fcst.Wspd * 1.60934),
                            WindSpeedKnots = Convert.ToInt32(fcst.Wspd * 0.868976),
                            windDir = fcst.Wdir,
                            windDirCardinal = fcst.WdirCardinal ?? "",
                            heatIndexF = Convert.ToInt32(fcst.Hi),
                            windChillF = fcst.Wc,
                            windChillC = Convert.ToInt32((fcst.Wc - 32) / 1.8),
                            visiblityMiles = Convert.ToInt32(fcst.Vis),
                            visiblityKm = Convert.ToInt32(fcst.Vis * 1.60934),
                            cloudCover = fcst.Clds,
                            dewPointF = fcst.Dewpt,
                            dewPointC = Convert.ToInt32((fcst.Dewpt - 32) / 1.8),
                            uvIndex = fcst.UvIndex,
                            uvIndexRaw = fcst.UvIndexRaw,
                            GolfIndex = fcst.GolfIndex ?? "",
                            GolfCategory = fcst.GolfCategory ?? "",
                            PrecipType = fcst.PrecipType ?? "",
                            dayNightIdx = fcst.DayInd ?? "",
                            wxman = fcst.Wxman ?? "",
                            SubphrasePt1 = fcst.SubphrasePt1 ?? "",
                            SubphrasePt2 = fcst.SubphrasePt2 ?? "",
                            SubphrasePt3 = fcst.SubphrasePt3 ?? ""
                        };                 
                            
                        // wtf is with HourlyForecast schema
                        dhRecordDataList.Add(dHRecData);
                    }
                }
            }
            

            XmlSerializer serializer = new(typeof(DHRecordResponse));
            StringWriter sw = new();
            XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment, 
            });
            xw.WriteWhitespace("");
            serializer.Serialize(xw, dHRecRes);
            sw.Close();

            recordScript += sw.ToString();
        }
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}