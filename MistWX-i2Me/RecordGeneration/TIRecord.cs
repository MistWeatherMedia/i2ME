using System.Xml;
using System.Xml.Serialization;
using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.RecordGeneration;

public class TIRecord : I2Record
{
    public async Task<string> MakeRecord(List<GenericResponse<TideForecastResponse>> results)
    {
        Log.Info("Creating TIRecord.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "TIRecord.xml");
        string recordScript = "<Data type=\"TIRecord\">";

        foreach (var result in results)
        {
            TIRecordResponse dHRecRes = new TIRecordResponse();
            List<TIRecordData> dhRecordDataList = new List<TIRecordData>();
            dHRecRes.TIRecordData = dhRecordDataList;

            TIRecordHeader dHRecHdr = new TIRecordHeader();
            dHRecHdr.TIstnId = result.Location.tideId;
            dHRecHdr.ILevel = 2;
            dHRecHdr.ProcTm = System.DateTime.Now.ToString("yyyyMMddHHmmss");

            dHRecRes.TIRecordHeader = dHRecHdr;

            foreach (var fcst in result.ParsedData.Tides.Tide)
            {
                TIRecordData dHRecData = new TIRecordData();
                DateTime time = DateTime.Parse(fcst.TideTM, null, System.Globalization.DateTimeStyles.RoundtripKind);
                dHRecData.TItdTm = time.ToString("MM/dd/yyyy HH:mm:ss");
                dHRecData.TItdHght = fcst.TideHt;
                dHRecData.TItdTyp = fcst.TideType;
                // wtf is with HourlyForecast schema
                dhRecordDataList.Add(dHRecData);
            }
            

            XmlSerializer serializer = new XmlSerializer(typeof(TIRecordResponse));
            StringWriter sw = new StringWriter();
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