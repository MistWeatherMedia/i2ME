using System.Xml;
using System.Xml.Serialization;
using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.RecordGeneration;

public class Headlines : I2Record
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

    private readonly static Dictionary<string, string> _vocalCodes = new Dictionary<string, string>()
    {
        { "HU_W", "HE001" },
        { "TY_W", "HE002" },
        { "HI_W", "HE003" },
        { "TO_A", "HE004" },
        { "SV_A", "HE005" },
        { "HU_A", "HE006" },
        { "TY_A", "HE007" },
        { "TR_W", "HE008" },
        { "TR_A", "HE009" },
        { "TI_W", "HE010" },
        { "HI_A", "HE011" },
        { "TI_A", "HE012" },
        { "BZ_W", "HE013" },
        { "IS_W", "HE014" },
        { "WS_W", "HE015" },
        { "HW_W", "HE016" },
        { "LE_W", "HE017" },
        { "ZR_Y", "HE018" },
        { "CF_W", "HE019" },
        { "LS_W", "HE020" },
        { "WW_Y", "HE021" },
        { "LB_Y", "HE022" },
        { "LE_Y", "HE023" },
        { "BZ_A", "HE024" },
        { "WS_A", "HE025" },
        { "FF_A", "HE026" },
        { "FA_A", "HE027" },
        { "FA_Y", "HE028" },
        { "HW_A", "HE029" },
        { "LE_A", "HE030" },
        { "SU_W", "HE031" },
        { "LS_Y", "HE032" },
        { "CF_A", "HE033" },
        { "ZF_Y", "HE034" },
        { "FG_Y", "HE035" },
        { "SM_Y", "HE036" },
        { "EC_W", "HE037" },
        { "EH_W", "HE038" },
        { "HZ_W", "HE039" },
        { "FZ_W", "HE040" },
        { "HT_Y", "HE041" },
        { "WC_Y", "HE042" },
        { "FR_Y", "HE043" },
        { "EC_A", "HE044" },
        { "EH_A", "HE045" },
        { "HZ_A", "HE046" },
        { "DS_W", "HE047" },
        { "WI_Y", "HE048" },
        { "SU_Y", "HE049" },
        { "AS_Y", "HE050" },
        { "WC_W", "HE051" },
        { "FZ_A", "HE052" },
        { "WC_A", "HE053" },
        { "AF_W", "HE054" },
        { "AF_Y", "HE055" },
        { "DU_Y", "HE056" },
        { "LW_Y", "HE057" },
        { "LS_A", "HE058" },
        { "HF_W", "HE059" },
        { "SR_W", "HE060" },
        { "GL_W", "HE061" },
        { "HF_A", "HE062" },
        { "UP_W", "HE063" },
        { "SE_W", "HE064" },
        { "SR_A", "HE065" },
        { "GL_A", "HE066" },
        { "MF_Y", "HE067" },
        { "MS_Y", "HE068" },
        { "SC_Y", "HE069" },
        { "UP_Y", "HE070" },
        { "LO_Y", "HE071" },
        { "AF_V", "HE075" },
        { "UP_A", "HE076" },
        { "TAV_W", "HE077" },
        { "TAV_A", "HE078" },
        { "TO_W", "HE0110" },
        { "", "" }
    }.ToDictionary(x => x.Value, x=> x.Key);

    public async Task<string> MakeRecord(BERecordRoot results)
    {
        Log.Info("Creating Headlines.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "Headlines.xml");

        HeadlinesResponse response = new();
        List<Headline> HlList = new();
        response.Headlines = HlList;

        int key = 0;
        if (results.BERecord != null)
        {
            foreach (var result in results.BERecord)
            {
                Headline headline = new()
                {
                    key = key,
                    procTm = System.DateTime.Now.ToString("yyyyMMddHHmmss"),
                    expiration = ((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).EExpTmUTC ?? "0",
                    vocalCd = ((result.BEData ?? new BEData()).BHdln ?? new BHdln()).BVocHdlnCd ?? "",
                    priority = priorities[((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A"],
                    significance = ((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A",
                    vocalSeq = new()
                    {
                        audioSeq = new ()
                        {
                            code = "HE",
                            audioClip = new()
                            {
                                path = "domestic/vocalLocal/Cantore/Headline_Event_Phrases\\" + _vocalCodes[((result.BEData ?? new BEData()).BHdln ?? new BHdln()).BVocHdlnCd ?? ""] + ".wav"
                            }
                        }
                    }
                };
                HlList.Add(headline);
                key += 1;
            }
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

        return recordPath;
       
    }
}
