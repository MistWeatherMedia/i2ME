using System.Data;
using System.Xml;
using System.Xml.Serialization;
using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.RecordGeneration;

public class DerivedHTRecord : I2Record
{
    public async Task<string> MakeRecord(List<GenericResponse<TropicalAdvisoryResponse>> results)
    {
        Log.Info("Creating DerivedHTRecord.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "DerivedHTRecord.xml");
        string recordScript = "<Data type=\"DerivedHTRecord\">";

        int HTIdx = 0;
        List<string> addedStrmId = new();
        foreach (var loc in results)
        {
            if (loc.ParsedData.AdvisoryInfo != null)
            {
                if (loc.ParsedData.AdvisoryInfo.AdvisoryInfo != null)
                {
                    foreach (var result in loc.ParsedData.AdvisoryInfo.AdvisoryInfo)
                    {
                        if (!addedStrmId.Contains(result.StormID ?? "N/A"))
                        {
                            addedStrmId.Add(result.StormID ?? "N/A");
                            XmlSerializer serializer = new(typeof(DerivedHTRecordResponse));
                            StringWriter sw = new();
                            XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
                            {
                                OmitXmlDeclaration = true,
                                ConformanceLevel = ConformanceLevel.Fragment, 
                            });
                            xw.WriteWhitespace("");
                            DateTime advDt = DateTime.Parse(result.AdvDtTm ?? "2026-01-19T04:00:00+04:00", null, System.Globalization.DateTimeStyles.None);
                            DateTime issueDt = DateTime.Parse(result.IssueDtTm ?? "2026-01-19T04:00:00+04:00", null, System.Globalization.DateTimeStyles.None);
                            serializer.Serialize(xw, new DerivedHTRecordResponse()
                            {
                                Index = HTIdx,
                                Action = 1,
                                Header = new DerivedHTRecordHeader()
                                {
                                    Pil = result.PIL,
                                    StrmId = new DerivedHTRecordStrmId()
                                    {
                                        StrmNm = $"{(result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).StormType ?? "".ToUpper()} {result.StormName ?? "".ToUpper()}",
                                        StrmId = result.StormID,
                                    },
                                    AdvsTmUTC = DateTime.UtcNow.ToString("yyyyMMddHHmm"),
                                    ProcTm = DateTime.Now.ToString("yyyyMMddHHmmss")
                                },
                                Data = new DerivedHTRecordData()
                                {
                                    WMOHdr = result.WMOID,
                                    IssueTmUTC = issueDt.ToString("yyyyMMddHHmm"),
                                    IssueOffc = result.IssueOffice,
                                    AdvsTmLcl = new DerivedHTRecordAdvsTmLcl()
                                    {
                                        Dow = advDt.ToString("dddd"),
                                        Month = advDt.ToString("MMMM"),
                                        Time = advDt.ToString("h tt"),
                                        TZAbbrv = result.AdvDtTmTzAbbrv,
                                        Timestamp = advDt.ToString("yyyyMMddHHmm")
                                    },
                                    Lat = (result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).Latitude,
                                    LatHmsphr = (result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).LatitudeHemisphere,
                                    Lon = (result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).Longitude,
                                    LonHmsphr = (result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).LongitudeHemisphere,
                                    PressureMB = (result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).MinPressure ?? "0",
                                    PressureIn = (result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).MinPressure ?? "0",
                                    MaxWindSpeedMPH = (result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).MaxSustainedWind,
                                    MaxWindGustMPH = (result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).WindGust,
                                    Cat = (result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).StormTypeCd,
                                    HeadingDirDeg = ((result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).Heading ?? new TropicalAdvisoryHeading()).StormDir,
                                    HeadingDirCardinal = ((result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).Heading ?? new TropicalAdvisoryHeading()).StormDirCardinal,
                                    HeadingSpdMPH = ((result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).Heading ?? new TropicalAdvisoryHeading()).StormSpd,
                                    Distance1 = ((result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).NearbyLoc ?? new TropicalAdvisoryNearbyLoc()).Dist,
                                    Direction1 = ((result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).NearbyLoc ?? new TropicalAdvisoryNearbyLoc()).DirCardinal,
                                    LocName1 = ((result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).NearbyLoc ?? new TropicalAdvisoryNearbyLoc()).LocNm,
                                    CityName1 = ((result.CurrentPosition ?? new TropicalAdvisoryCurrentPosition()).NearbyLoc ?? new TropicalAdvisoryNearbyLoc()).CityNm,
                                    Basin = result.Basin,
                                    LastUpdate = 0
                                }
                            });
                            sw.Close();

                            recordScript += sw.ToString();
                            HTIdx += 1;
                            }
                        }
                }
                
                    
            }
        }
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}