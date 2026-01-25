using System.Xml;
using System.Xml.Serialization;
using MistWX_i2Me.API;
using MistWX_i2Me.Communication;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.RecordGeneration;

public class BulletinCrawlsGen : I2Record
{

    private readonly static Dictionary<string, int> priorities = new() {
        {"A", 150},
        {"B", 50},
        {"E", 500},
        {"L", 25},
        {"M", 20},
        {"O", 25},
        {"R", 45},
        {"S", 350},
        {"Y", 50},
        {"W", 450}
    };

    private readonly static Dictionary<string, int> visualstyles = new() {
        {"A", 2},
        {"B", 2},
        {"E", 0},
        {"L", 1},
        {"M", 1},
        {"O", 1},
        {"R", 2},
        {"S", 2},
        {"Y", 2},
        {"W", 0}
    };

    private readonly static Dictionary<string, int> audiostyles = new() {
        {"A", 0},
        {"B", 0},
        {"E", 24},
        {"L", 0},
        {"M", 0},
        {"O", 0},
        {"R", 0},
        {"S", 0},
        {"Y", 0},
        {"W", 24}
    };

    private readonly static Dictionary<string, int> durations = new() {
        {"A", 3600},
        {"B", 3600},
        {"E", 10800},
        {"L", 3600},
        {"M", 3600},
        {"O", 3600},
        {"R", 3600},
        {"S", 3600},
        {"Y", 3600},
        {"W", 7200}
    };
    public async Task MakeRecord(BERecordRoot results, UdpSender sender)
    {
        Log.Info("Creating BulletinRecords.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "BulletinRecords.xml");

        BulletinCrawlsResponse response = new();
        BulletinCrawls bCrawls = new()
        {
            Bulletins = new()
        };
        response.BulletinCrawls = bCrawls;

        int key = 0;
        if (results.BERecord != null)
        {
            foreach (var result in results.BERecord)
            {
                Bulletin bulletin = new()
                {
                    Index = key,
                    Label = ((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).EDesc ?? "",
                    Text = ((result.BEData ?? new BEData()).BNarrTxt ?? new BNarrTxt()).BLn ?? "",
                    VisualStyle = visualstyles[((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A"],
                    AudioStyle = audiostyles[((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A"],
                    Priority = priorities[((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A"],
                    Significance = ((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A",
                    Phenomena = ((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).EPhenom ?? "CF"
                };
                bCrawls.Bulletins.Add(bulletin);
                key += 1;
            }
            // Sort by priority
            bCrawls.Bulletins = bCrawls.Bulletins.OrderByDescending(a => a.Priority).ToList();
        }
            

        XmlSerializer serializer = new(typeof(HeadlinesResponse));
        StringWriter sw = new();
        XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            ConformanceLevel = ConformanceLevel.Fragment, 
        });
        xw.WriteWhitespace("");
        serializer.Serialize(xw, response);
        sw.Close();

        await File.WriteAllTextAsync(recordPath, ValidateXml(sw.ToString()));
        sender.SendFile(recordPath, "storeData(QGROUP=__BulletinCrawls__,Feed=BulletinCrawls)");
        sender.SendCommand($"loadRunPres(Duration={durations[response.BulletinCrawls.Bulletins.First().Significance]},PresentationId=BCrawl1,Flavor=BulletinCrawl)");
    }
}
