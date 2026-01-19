using System.Xml;
using System.Xml.Serialization;
using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.RecordGeneration;

public class LFRecord : I2Record
{
    public async Task<string> MakeRecord(LFRecordLocation result)
    {
        if (Directory.Exists(Path.Combine(AppContext.BaseDirectory, "temp", "LFRecords")) != true)
        {
            Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "temp", "LFRecords"));
        }
        Log.Info($"Creating LFRecord for {result.prsntNm}.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "LFRecords", $"LFRecord-{result.cntryCd}-{result.locType}-{result.locId}-{result.cityNm}.xml");
        string recordScript = "<Data type=\"LFRecord\">";

        LFRecordResponse LFRecRes = new()
        {
            LFRecordHeader = new LFRecordHeader()
            {
                LocId = result.locId,
                LocType = result.locType,
                ProcTm = System.DateTime.Now.ToString("yyyyMMddHHmmss")
            },
            LFRecordData = new LFRecordData()
            {
                active = result.active,
                arptId = result.arptId,
                cityNm = result.cityNm,
                cliStn = result.cliStn,
                clsRad = result.clsRad,
                cntryCd = result.cntryCd,
                cntyFips = result.cntyFips,
                cntyId = result.cntyId,
                cntyNm = result.cntyNm,
                coopId = result.coopId,
                dmaCd = result.dmaCd,
                dySTAct = result.dySTAct,
                dySTInd = result.dySTInd,
                elev = result.elev,
                epaId = result.epaId,
                gmtDiff = result.gmtDiff,
                lat = result.lat,
                lon = result.@long,
                lsRad = result.lsRad,
                mrnZoneId = result.mrnZoneId,
                obsStn = result.obsStn,
                pllnId = result.pllnId,
                primTecci = result.primTecci,
                prsntNm = result.prsntNm,
                regSat = result.regSat,
                secObsStn = result.secObsStn,
                secTecci = result.secTecci,
                siteId = result.siteId,
                skiId = result.skiId,
                ssRad = result.ssRad,
                stCd = result.stCd,
                tertObsStn = result.tertObsStn,
                tertTecci = result.tertTecci,
                tideId = result.tideId,
                tmZnNm = result.tmZnNm,
                tmZnAbbr = result.tmZnAbbr,
                tPrsntNm = result.tPrsntNm,
                wmoId = result.wmoId,
                wrlsPrsntNm = result.wrlsPrsntNm,
                zip2locId = result.zip2locId,
                zoneId = result.zoneId,
                zoneNm = result.cntyNm,
            }
        };

        XmlSerializer serializer = new XmlSerializer(typeof(LFRecordResponse));
        StringWriter sw = new StringWriter();
        XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            ConformanceLevel = ConformanceLevel.Fragment, 
        });
        xw.WriteWhitespace("");
        serializer.Serialize(xw, LFRecRes);
        sw.Close();

        recordScript += sw.ToString();
        
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}