using System.ComponentModel.Design.Serialization;
using System.Xml;
using System.Xml.Serialization;
using MistWX_i2Me.API;
using MistWX_i2Me.API.Products;
using MistWX_i2Me.Communication;
using MistWX_i2Me.Schema.faa;
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.RecordGeneration;

public class AirportDelays : I2Record
{
 
    public static string GenerateReason(string reason)
    {
        if (reason.IndexOf("wx", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("weather", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (reason.IndexOf("thunderstorm", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("tstorm", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("thunder", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Thunderstorm";
                } else if (reason.IndexOf("snow", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("blizzard", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Snow";
                } else if (reason.IndexOf("rain", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("drizzle", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Rain";
                } else if (reason.IndexOf("fog", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Fog";
                } else if (reason.IndexOf("wind", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Wind";
                } else if (reason.IndexOf("visibility", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Visibility";
                } else if (reason.IndexOf("cloud", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("ceiling", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Cloud Ceiling";
                } else if (reason.IndexOf("ice", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("icing", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Icing";
                } else if (reason.IndexOf("storm", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Storm";
                } else if (reason.IndexOf("hurricane", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Hurricane";
                } else if (reason.IndexOf("tornado", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    reason = "Tornado";
                } else
                {
                    reason = "Weather";
                }
            } else if (reason.IndexOf("traffic", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("volume", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("congesetion", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("flow", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("vol", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                reason = "Traffic Volume";
            } else if (reason.IndexOf("staff", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("staffing", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                reason = "Staffing";
            } else if (reason.IndexOf("equipment", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("equip", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                reason = "Equipment";
            } else if (reason.IndexOf("runway", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("run", StringComparison.OrdinalIgnoreCase) >= 0 || reason.IndexOf("rwy", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                reason = "Runway";
            } else if (reason.IndexOf("security", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                reason = "Security";
            } else
            {
                reason = "Other";
            }

        if (string.IsNullOrEmpty(reason))
        {
            reason = "Other";
        }
        
        return reason;
    }
    public static Delays GenerateDelay(AirportEvent ae)
    {
        string category;
        string color;
        int delay_sec;
        string reason;

        if (ae.arrivalDelay != null)
        {
            reason = GenerateReason(ae.arrivalDelay.impactingCondition ?? "");
            delay_sec = Convert.ToInt32(ae.arrivalDelay.averageDelay) * 60;
            color = "yellow";
            category = "arrival";
        } else if (ae.departureDelay != null) {
            reason = GenerateReason(ae.departureDelay.impactingCondition ?? "");
            delay_sec = Convert.ToInt32(ae.departureDelay.averageDelay) * 60;
            color = "yellow";
            category = "departure";
        } else if (ae.groundStop != null) {
            reason = GenerateReason(ae.groundStop.impactingCondition ?? "");
            delay_sec = 0;
            color = "yellow";
            category = "departure";
        } else if (ae.groundDelay != null) {
            reason = GenerateReason(ae.groundDelay.impactingCondition ?? "");
            delay_sec = Convert.ToInt32(ae.groundDelay.avgDelay);
            color = "yellow";
            category = "departure";
        } else if (ae.freeForm != null) {
            reason = ae.freeForm.text ?? "";
            delay_sec = 0;
            color = "red";
            category = "airport closure";
        } else if (ae.airportClosure != null) {
            reason = "Airport is closed";
            delay_sec = 0;
            color = "red";
            category = "airport closure";
        } else
        {
            reason = "";
            delay_sec = 0;
            color = "yellow";
            category = "arrival";
        }
        

        return new()
        {
            Category = category,
            Color = color,
            DelaySec = delay_sec,
            ReasonsAll = new ReasonsAll()
            {
                Delay = new ()
                {
                    new()
                    {
                        Category = category,
                        Color = color,
                        DelaySec = delay_sec,
                        Reason = reason
                    }
                }
            }
        };
    }
    
    public async Task MakeRecord(GenericResponse<List<AirportEvent>>? result, UdpSender sender)
    {
        Log.Info("Creating AirportDelays.");
        if (result != null)
        {
                string ADIdx = "";
                string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "AirportDelays.xml");
                string recordScript = "<Data type=\"AirportDelays\" >";
                DateTime expiration = DateTime.UtcNow;
                expiration.AddHours(2);
                foreach (AirportEvent AE in result.ParsedData)
                { 
                    AirportResult apresult = new()
                    {
                        Key = AE.airportId,
                        ICAOCode = $"K{AE.airportId}",
                        IATACode = AE.airportId,
                        FAACode = AE.airportId,
                        AirportName = AE.airportLongName,
                        LocalAirportName = null,
                        Delays = GenerateDelay(AE),
                        Arrival = new()
                        {
                            Cancellations = 0,
                            PercentCancelled = 100,
                            Total = 1000
                        },
                        Departure = new()
                        {
                            Cancellations = 0,
                            PercentCancelled = 100,
                            Total = 1000
                        },
                        ProcessTimeGmt = (int)((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
                        ExpireTimeGmt = (int)((DateTimeOffset)expiration).ToUnixTimeSeconds()
                    };
                    AirportDelaysMetadata metadata = new()
                    {
                        airport_code = AE.airportId,
                        expire_time_gmt = (int)((DateTimeOffset)expiration).ToUnixTimeSeconds()
                    };
                    Schema.twc.AirportDelays product = new()
                    {
                        ProcessTimeGmtAttr = (int)((DateTimeOffset)expiration).ToUnixTimeSeconds(),
                        KeyAttr = AE.airportId,
                        locationKey = AE.airportId,
                        airport_result = apresult,
                        clientKey = AE.airportId
                    };
                    ADIdx += $"{AE.airportId},";
                    XmlSerializer serializer = new(typeof(Schema.twc.AirportDelays));
                    StringWriter sw = new();
                    XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
                    {
                        OmitXmlDeclaration = true,
                        ConformanceLevel = ConformanceLevel.Fragment, 
                    });
                    xw.WriteWhitespace("");
                    serializer.Serialize(xw, product);
                    sw.Close();

                    recordScript += sw.ToString();
                }
                recordScript += "</Data>";
                await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));
                sender.SendFile(recordPath, "storeData(QGROUP=__AirportDelays__,Feed=AirportDelays)");
                // Make airport delay index
                recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "AirportDelaysIndexes.xml");
                recordScript = "<Data type=\"AirportDelayIndexes\">";
                
                AirportDelayIndexes ADI = new()
                {
                    Key = "indexes",
                    Indexes = ADIdx[0..(ADIdx.Length - 1)]
                };
                XmlSerializer ADISerial = new(typeof(AirportDelayIndexes));
                StringWriter ADISw = new();
                XmlWriter ADIXw = XmlWriter.Create(ADISw, new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    ConformanceLevel = ConformanceLevel.Fragment, 
                });
                ADIXw.WriteWhitespace("");
                ADISerial.Serialize(ADIXw, ADI);
                ADISw.Close();
                recordScript += ADISw.ToString();
                recordScript += "</Data>";
                await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));
                sender.SendFile(recordPath, "storeData(QGROUP=__AirportDelayIndexes__,Feed=AirportDelayIndexes)");
            
        }
    }
}
