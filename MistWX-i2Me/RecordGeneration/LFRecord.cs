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

        LFRecordResponse LFRecRes = new LFRecordResponse();

        LFRecordHeader LFRecHdr = new LFRecordHeader();
        LFRecHdr.LocId = result.locId;
        LFRecHdr.LocType = result.locType;
        LFRecHdr.ProcTm = System.DateTime.Now.ToString("yyyyMMddHHmmss");

        LFRecRes.LFRecordHeader = LFRecHdr;

        // Set the LFRecord values
        LFRecordData LFRecData = new LFRecordData();
        LFRecData.active = result.active;
        LFRecData.arptId = result.arptId;
        LFRecData.cityNm = result.cityNm;
        LFRecData.cliStn = result.cliStn;
        LFRecData.clsRad = result.clsRad;
        LFRecData.cntryCd = result.cntryCd;
        LFRecData.cntyFips = result.cntyFips;
        LFRecData.cntyId = result.cntyId;
        LFRecData.cntyNm = result.cntyNm;
        LFRecData.coopId = result.coopId;
        LFRecData.dmaCd = result.dmaCd;
        LFRecData.dySTAct = result.dySTAct;
        LFRecData.dySTInd = result.dySTInd;
        LFRecData.elev = result.elev;
        LFRecData.epaId = result.epaId;
        LFRecData.gmtDiff = result.gmtDiff;
        LFRecData.lat = result.lat;
        LFRecData.lon = result.@long;
        LFRecData.lsRad = result.lsRad;
        LFRecData.mrnZoneId = result.mrnZoneId;
        LFRecData.obsStn = result.obsStn;
        LFRecData.pllnId = result.pllnId;
        LFRecData.primTecci = result.primTecci;
        LFRecData.prsntNm = result.prsntNm;
        LFRecData.regSat = result.regSat;
        LFRecData.secObsStn = result.secObsStn;
        LFRecData.secTecci = result.secTecci;
        LFRecData.siteId = result.siteId;
        LFRecData.skiId = result.skiId;
        LFRecData.ssRad = result.ssRad;
        LFRecData.stCd = result.stCd;
        LFRecData.tertObsStn = result.tertObsStn;
        LFRecData.tertTecci = result.tertTecci;
        LFRecData.tideId = result.tideId;
        LFRecData.tmZnNm = result.tmZnNm;
        LFRecData.tmZnAbbr = result.tmZnAbbr;
        LFRecData.tPrsntNm = result.tPrsntNm;
        LFRecData.wmoId = result.wmoId;
        LFRecData.wrlsPrsntNm = result.wrlsPrsntNm;
        LFRecData.zip2locId = result.zip2locId;
        LFRecData.zoneId = result.zoneId;
        LFRecData.zoneNm = result.cntyNm;
        
        LFRecRes.LFRecordData = LFRecData;

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