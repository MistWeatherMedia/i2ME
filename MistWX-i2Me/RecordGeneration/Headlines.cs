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

    // Maps a phenomena with a LOT8 icon, however is not intended i2 behivaior and requires a patch
    private readonly static Dictionary<string, string> alerttolficon = new() {
        // Air Quality
        {"TAP", "alertpollution"},
        {"TAQ", "alertpollution"},
        {"AS", "alertpollution"},
        {"DS", "alertdust"},
        {"TOZ", "alertpollution"},
        {"airQuality", "alertpollution"},
        {"smog", "alertpollution"},
        {"DAR", "alertwindy"},
        // Avalanche
        {"TAV", "alertavalanche"},
        {"TAA", "alertavalanche"},
        {"AVL", "alertavalanche"},
        // Coastal
        {"CF", "alertflood"},
        {"TCL", "alertflood"},
        {"ZY", "alertfreeze"},
        {"CL", "alertfreeze"},
        {"GL", "alertwindy"},
        {"SE", "alertsurge"},
        {"UP", "alertfreeze"},
        {"SU", "alertsurge"},
        {"HF", "alertwindy"},
        {"LW", "alertwindy"},
        {"LS", "alertflood"},
        {"LO", "alertflood"},
        {"MA", "alertsurge"},
        {"RP", "alertsurge"},
        {"SC", "alertsurge"},
        {"SR", "alertstorm"},
        {"TS", "alertsurge"},
        {"freezngSpray", "alertfreeze"},
        {"galeWind", "alertwindy"},
        {"highWater", "alertflood"},
        {"hurricFrcWnd", "alertwindy"},
        {"icePressure", "alertwindy"},
        {"rpdCloseLead", "alertwindy"},
        {"icePressure", "alertwindy"},
        {"spclIce", "alertfrost"},
        {"spclMarine", "alertsurge"},
        {"squall", "alertwindy"},
        {"stormFrcWind", "alertwindy"},
        {"tsunami", "alertsurge"},
        {"waterspout", "alerttornado"},
        {"TCW", "alertsurge"},
        {"GA", "alertwindy"},
        {"HW", "alertsurge"},
        {"MWW-GALE", "alertwindy"},
        {"HSW-STD", "alertsurge"},
        {"MWW-HURR", "alerthurricane"},
        {"MWW", "alertwindy"},
        {"SWW-HT-STD", "alertsurge"},
        {"SWW-DS-STD", "alertsurge"},
        {"MWW-STO", "alertstorm"},
        {"MWW-STR", "alertwindy"},
        {"TSUNAMI-STD", "alertsurge"},
        {"TSUNAMI-WARN", "alertsurge"},
        {"TSUNAMI-WATCH", "alertsurge"},
        // Miscellaneous
        {"TNO", "alertgeneric"},
        {"TAD", "alertgeneric"},
        {"TAW", "alertgeneric"},
        {"TCA", "alertgeneric"},
        {"TCD", "alertgeneric"},
        {"TCE", "alertgeneric"},
        {"TEQ", "alertearthquake"},
        {"TEV", "alertgeneric"},
        {"TFI", "alertfire"},
        {"FW", "alertfire"},
        {"THM", "alerthazard"},
        {"TLA", "alertgeneric"},
        {"TLC", "alertgeneric"},
        {"TNM", "alertgeneric"},
        {"TNU", "alerthazard"},
        {"TRH", "alerthazard"},
        {"TSP", "alertgeneric"},
        {"TSL", "alertwindy"},
        {"TVO", "alertvolcano"},
        {"TWX", "alertwindy"},
        {"weather", "alertwindy"},
        {"TFF", "alertfire"},
        {"OT", "alertgeneric"},
        {"BRA-STV", "alerthazard"},
        {"BWA-STD", "alertgeneric"},
        {"FWW-CAT", "alertfire"},
        {"DMA-STD", "alerthazard"},
        {"FWW-EXT", "alertfire"},
        {"FWW-STD", "alertfire"},
        {"FWW-MARG", "alertfire"},
        {"RWA-STD", "alertgeneric"},
        {"RWA-SL-STD", "alertgeneric"},
        {"BRA-SEV", "alerthazard"},
        {"DMA-SEV", "alerthazard"},
        {"FWW-SEV", "alertfire"},
        {"SHW-SEV", "alertgeneric"},
        {"SHW-STD", "alertgeneric"},
        {"FWW-VH", "alertfire"},
        // Rain/Flood
        {"FF", "alertflood"},
        {"FA", "alertflood"},
        {"TSF", "alertflood"},
        {"HY", "alertflood"},
        {"FL", "alertflood"},
        {"rainfall", "alertflood"},
        {"TFL", "alertflood"},
        {"TRA", "alertflood"},
        {"TRF", "alertflood"},
        {"RF", "alertflood"},
        {"FLW-FINAL", "alertflood"},
        {"FLA-FINAL", "alertflood"},
        {"FLW-UNCL", "alertflood"},
        {"FLA-WATCH", "alertflood"},
        {"FLA-UNK", "alertflood"},
        {"FLW-BLWMIN", "alertflood"},
        {"FLA-BLWMIN", "alertflood"},
        {"FLW-MAJ", "alertflood"},
        {"FLA-MAJ", "alertflood"},
        {"FLW-MIN", "alertflood"},
        {"FLA-MIN", "alertflood"},
        {"FLA-MINMAJ", "alertflood"},
        {"FLA-MINMOD", "alertflood"},
        {"FLW-MOD", "alertflood"},
        {"FLA-MOD", "alertflood"},
        {"FLA-MODMAJ", "alertflood"},
        {"RWA-FL-STD", "alertflood"},
        {"RWA-RA-STD", "alertflood"},
        {"STW-R+-STD", "alertflood"},
        {"SWW-FF-STD", "alertflood"},
        {"SWW-R-STD", "alertflood"},
        {"SWW-R+-STD", "alertflood"},
        // Temperature
        {"EH", "alertheat"},
        {"EC", "alertcold"},
        {"FZ", "alertfreeze"},
        {"FR", "alertfrost"},
        {"HZ", "alertfreeze"},
        {"HT", "alertheat"},
        {"WC", "alertwindy"},
        {"articOut", "alertwindy"},
        {"extremeCold", "alertcold"},
        {"flashFreeze", "alertfreeze"},
        {"frost", "alertfrost"},
        {"extremeHeat", "alertheat"},
        {"THT", "alertheat"},
        {"TLT", "alertcold"},
        {"LT", "alertcold"},
        {"BWA-CH-STD", "alertcold"},
        {"EXTREMEHEAT-WARN", "alertheat"},
        {"EXTREMEHEAT-WATCH", "alertheat"},
        {"FRW-STD", "alertfrost"},
        {"FRW-SEV", "alertfrost"},
        // Thunderstorm
        {"SV", "alertstorm"},
        {"TSA", "alertstorm"},
        {"TO", "alerttornado"},
        {"thunderstorm", "alertstorm"},
        {"thunder", "alerttornado"},
        {"TTS", "alertstorm"},
        {"TDS", "alertstorm"},
        {"STW-STD", "alertstorm"},
        {"STW-W-STD", "alertstorm"},
        {"STW-W+-STD", "alertstorm"},
        {"STW-GH-STD", "alertsnow"},
        {"STW-LH-STD", "alertsnow"},
        {"STW-TO-STD", "alerttornado"},
        {"SWW-STD", "alertstorm"},
        {"SWW-W-STD", "alertstorm"},
        {"SWW-W+-STD", "alertstorm"},
        {"SWW-LH-STD", "alertsnow"},
        {"SWW-TO-STD", "alerttornado"},
        // Hurricane
        {"HU", "alerthurricane"},
        {"SS", "alertsurge"},
        {"TR", "alerttropicalstorm"},
        {"TTP", "alerttropicalstorm"},
        {"TY", "alerthurricane"},
        {"hurricane", "alerthurricane"},
        {"tropStorm", "alerttropicalstorm"},
        {"SSG", "alertsurge"},
        {"TROPCYCLONE-STD", "alerthurricane"},
        {"TROPCYCLONE-WARN", "alerthurricane"},
        {"TROPCYCLONE-WATCH", "alerthurricane"},
        // Visibility
        {"AF", "alertash"},
        {"MH", "alertash"},
        {"DU", "alertdust"},
        {"FG", "alertfog"},
        {"MF", "alertfog"},
        {"MS", "alertash"},
        {"SM", "alertash"},
        {"DS", "alertdust"},
        {"dustStorm", "alertdust"},
        {"other", "alertfog"},
        {"TFA", "alertfog"},
        {"DFG", "alertfog"},
        {"RWA-DU-STD", "alertdust"},
        {"RWA-FG-STD", "alertfog"},
        {"RWA-SM-STD", "alertash"},
        // Wind
        {"BW", "alertwindy"},
        {"EW", "alertwindy"},
        {"HW", "alertwindy"},
        {"WI", "alertwindy"},
        {"lsWind", "alertwindy"},
        {"strongWind", "alertwindy"},
        {"wind", "alertwindy"},
        {"whWind", "alertwindy"},
        {"TWA", "alertwindy"},
        {"WS", "alertwindy"},
        {"RWA-W-STD", "alertwindy"},
        {"RWA-W+-STD", "alertwindy"},
        // Winter Precipitation
        {"BZ", "alertsnow"},
        {"ZF", "alertfog"},
        {"LE", "alertsnow"},
        {"SQ", "alertsnow"},
        {"WS", "alertsnow"},
        {"WW", "alertsnow"},
        {"blizzard", "alertsnow"},
        {"blowingSnow", "alertsnow"},
        {"freezeDrzl", "alertsnow"},
        {"freezeRain", "alertsnow"},
        {"snowSquall", "alertsnow"},
        {"snowfall", "alertsnow"},
        {"winterStorm", "alertsnow"},
        {"TSI", "alertsnow"},
        {"FR", "alertfrost"},
        {"SNG", "alertsnow"},
        {"SNF", "alertsnow"},
        {"ICA", "alerticy"},
        {"SNA", "alertsnow"},
        {"SNS", "alertsnow"},
        {"SNM", "alertsnow"},
        {"BWA-SN-STD", "alertsnow"},
        {"RWA-IC-STD", "alerticy"},
        {"RWA-SN-STD", "alertsnow"},
        {"SWW-BZ-STD", "alertsnow"},
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
        // Grab a list of significances Headlines should receive.
        string[] significances = Config.config.AConfig.HeadlineSig.Split(",");

        int key = 0;
        if (results.BERecord != null)
        {
            List<String> addedAlerts = new();
            foreach (var result in results.BERecord)
            {
                string alertCheck = (((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).EPhenom ?? "") + "_" + (((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A");
                if (!addedAlerts.Contains(alertCheck)) {
                    if (!significances.Contains(((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A"))
                    {
                        continue;
                    }
                    string alerticon = "alertgeneric";
                    /*
                    if (alerttolficon.ContainsKey(((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).EPhenom ?? ""))
                    {
                        alerticon = alerttolficon[((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).EPhenom ?? ""];
                    }
                    */

                    Headline headline = new()
                    {
                        key = key,
                        procTm = System.DateTime.Now.ToString("yyyyMMddHHmmss"),
                        expiration = (((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).EExpTmUTC ?? "0") + "00",
                        vocalCd = ((result.BEData ?? new BEData()).BHdln ?? new BHdln()).BVocHdlnCd ?? "",
                        priority = priorities[((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A"],
                        significance = ((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).ESgnfcnc ?? "A",
                        text = ((result.BEData ?? new BEData()).BHdln ?? new BHdln()).BHdlnTxt ?? "",
                        phenomena = ((result.BEHdr ?? new BEHdr()).BEvent ?? new BEvent()).EPhenom ?? "",
                        lficon = alerticon,
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
                        },
                        
                    };
                    HlList.Add(headline);
                    key += 1;
                    addedAlerts.Add(alertCheck);
                }
            }
            // Sort by priority
            HlList = HlList.OrderByDescending(a => a.priority).ToList();
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
