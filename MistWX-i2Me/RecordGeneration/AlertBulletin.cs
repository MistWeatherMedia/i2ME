using System.Security.Cryptography.X509Certificates;

using Dapper;
using MistWX_i2Me.API;
using MistWX_i2Me.API.Products;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;

namespace MistWX_i2Me.RecordGeneration;

public class AlertBulletin : I2Record
{
    /// <summary>
    /// Dictionary converting alert type keys into IntelliStar 2 vocal keys.
    /// </summary>
    private readonly static Dictionary<string, string> _vocalCodes = new Dictionary<string, string>()
    {
        { "HU_W", "HE001" },
        { "HU_S", "HE001" }, // USA (not defined)
        { "TROPCYCLONE-STD_S", "HE001" }, // Australia
        { "hurricane_W", "HE001" }, // Canada
        { "TTP_S", "HE001" }, // USA (not defined)

        { "TY_W", "HE002" },
        { "TY_S", "HE002" }, // USA (not defined)
        { "TROPCYCLONE-STD_W", "HE002" }, // Australia
        { "TROPCYCLONE-STD_A", "HE002" }, // Australia

        { "HI_W", "HE003" },

        { "TO_A", "HE004" },
        { "tornado_A", "HE004" }, // Canada

        { "SV_A", "HE005" },
        { "SV_W", "HE005" }, // USA (not defined)
        { "thunderstorm_A", "HE005" }, // Canada
        { "thunderstorm_W", "HE005" }, // Canada
        { "TTS_A", "HE005" }, // Europe
        { "TTS_W", "HE005" }, // Europe
        { "TTS_S", "HE005" }, // Europe
        { "TDS_S", "HE005" }, // Japan
        { "STW-STD", "HE005" }, // Australia

        { "HU_A", "HE006" },
        { "hurricane_A", "HE006" }, // Canada

        { "TY_A", "HE007" },

        { "TR_W", "HE008" },
        { "tropStorm_W", "HE008" }, // Canada
        { "tropStorm_S", "HE008" }, // Canada

        { "TR_A", "HE009" },
        { "tropStorm_A", "HE009" }, // Canada

        { "TI_W", "HE010" },

        { "HI_A", "HE011" },

        { "TI_A", "HE012" },

        { "BZ_W", "HE013" },
        { "blizzard_W", "HE013" }, // Canada

        { "IS_W", "HE014" },

        { "WS_W", "HE015" },
        { "winterStorm_W", "HE015" }, // Canada
        { "SNS_W", "HE015" }, // Japan
        { "SNS_E", "HE015" }, // Japan
        { "TSI_W", "HE015" }, // Europe

        { "HW_W", "HE016" },
        { "EW_W", "HE016" }, // USA (not defined)
        { "lsWind_W", "HE016" }, // Canada
        { "strongWind_W", "HE016" }, // Canada
        { "wind_W", "HE016" }, // Canada
        { "whWind_W", "HE016" }, // Canada
        { "TWA_W", "HE016" }, // Europe
        { "WS_E", "HE016" }, // Japan
        { "RWA-W-STD_W", "HE016" }, // Australia
        { "RWA-W+-STD_W", "HE016" }, // Australia

        { "LE_W", "HE017" },

        { "ZR_Y", "HE018" },

        { "CF_W", "HE019" },

        { "LS_W", "HE020" },

        { "WW_Y", "HE021" },

        { "LB_Y", "HE022" },

        { "LE_Y", "HE023" },

        { "BZ_A", "HE024" },

        { "WS_A", "HE025" },
        { "TSI_A", "HE025" }, // Europe
        { "TSI_S", "HE025" }, // Europe

        { "FF_A", "HE026" },
        { "FF_W", "HE026" }, // USA (not defined)
        { "FF_S", "HE026" }, // USA (not defined)
        { "STW-FF-STD_W", "HE026" }, // Australia
        { "SWW-FF-STD_W", "HE026" }, // Australia

        { "FA_A", "HE027" },
        { "FA_B", "HE027" }, // USA (not defined)
        { "TFL_A", "HE027" }, // Europe
        { "TRF_A", "HE027" }, // Europe
        { "FLA-WATCH_A", "HE027" }, // Australia
        { "FLA-UNK_A", "HE027" }, // Australia
        { "FLA-BLWMIN_A", "HE027" }, // Australia
        { "FLA-MAJ_A", "HE027" }, // Australia
        { "FLA-MIN_A", "HE027" }, // Australia
        { "FLA-MINMAJ_A", "HE027" }, // Australia
        { "FLA-MINMOD_A", "HE027" }, // Australia
        { "FLA-MOD_A", "HE027" }, // Australia
        { "FLA-MODMAJ_A", "HE027" }, // Australia
        { "FLA-FINAL_A", "HE027" }, // Australia

        { "FA_Y", "HE028" },
        { "FL_Y", "HE028" }, // Japan
        { "FL_W", "HE028" }, // Japan
        { "FA_W", "HE028" }, // USA (not defined)
        { "TFL_W", "HE028" }, // Europe
        { "TRF_W", "HE028" }, // Europe
        { "FLW-UNCL_W", "HE028" }, // Australia
        { "FLA-BLWMIN_W", "HE028" }, // Australia
        { "FLA-MAJ_W", "HE028" }, // Australia
        { "FLA-MIN_W", "HE028" }, // Australia
        { "FLA-MOD_W", "HE028" }, // Australia
        { "RWA-FL-STD_W", "HE028" }, // Australia
        { "FLA-FINAL_W", "HE028" }, // Australia

        { "HW_A", "HE029" },
        { "BW_Y", "HE029" }, // USA (not defined)
        { "TWA_S", "HE029" }, // Europe
        { "TWA_A", "HE029" }, // Europe

        { "LE_A", "HE030" },
        { "SU_W", "HE031" },
        { "TS_W", "HE031" }, // USA (not defined)
        { "tsunami_W", "HE031" }, // Canada
        { "HW_E", "HE031" }, // Japan
        { "HSW-STD_W", "HE031" }, // Australia
        { "SWW_HT_STD_W", "HE031" }, // Australia
        { "SWW_DS_STD_W", "HE031" }, // Australia
        { "TSUNAMI-WARN_W", "HE031" }, // Australia

        { "LS_Y", "HE032" },
        { "LS_S", "HE032" }, // USA (not defined)

        { "CF_A", "HE033" },
        { "CF_S", "HE033" }, // USA (not defined)
        { "CF_Y", "HE033" }, // USA (not defined)

        { "ZF_Y", "HE034" },

        { "FG_Y", "HE035" },
        { "other_Y", "HE035" }, // Canada
        { "TFA_A", "HE035" }, // Europe
        { "TFA_W", "HE035" }, // Europe
        { "TFA_S", "HE035" }, // Europe
        { "DFG_Y", "HE035" }, // Japan
        { "RWA-FG-STD_Y", "HE035" }, // Australia

        { "SM_Y", "HE036" },
        { "RWA-SM-STD_Y", "HE036" },

        { "EC_W", "HE037" },
        { "extremeCold_W", "HE037" }, // Canada
        { "TLT_W", "HE037" }, // Europe

        { "EH_W", "HE038" },
        { "EXTREMEHEAT-WARN_W", "HE038" }, // Australia
        { "BWA-CH-STD_W", "HE038" }, // Australia
        { "extremeHeat_W", "HE038" }, // Canada
        { "THT_W", "HE038" }, // Europe

        { "HZ_W", "HE039" },
        { "flashFreeze_W", "HE039" }, // Canada

        { "FZ_W", "HE040" },

        { "HT_Y", "HE041" },

        { "WC_Y", "HE042" },

        { "FR_Y", "HE043" },

        { "frost_Y", "HE043" }, // Canada
        { "FRW-STD_W", "HE043" }, // Australia
        { "FRW-SEV_W", "HE043" }, // Australia

        { "EC_A", "HE044" },
        { "TLT_A", "HE044" }, // Europe
        { "TLT_S", "HE044" }, // Europe
        { "LT_Y", "HE044" }, // Japan

        { "EH_A", "HE045" },
        { "THT_A", "HE045" }, // Europe
        { "THT_S", "HE045" }, // Europe
        { "EXTREMEHEAT-WATCH_A", "HE045" }, // Australia

        { "HZ_A", "HE046" },

        { "DS_W", "HE047" },

        { "WI_Y", "HE048" },

        { "SU_Y", "HE049" },
        { "TS_Y", "HE049" }, // USA (not defined)
        { "TS_B", "HE049" }, // USA (not defined)
        { "TS_A", "HE049" }, // USA (not defined)
        { "tsunami_A", "HE049" }, // Canada
        { "tsunami_Y", "HE049" }, // Canada
        { "TSUNAMI-STD_S", "HE049" }, // Australia
        { "TSUNAMI-NOTHRT_S", "HE049" }, // Australia
        { "TSUNAMI-WATCH_A", "HE049" }, // Australia

        { "AS_Y", "HE050" },

        { "WC_W", "HE051" },
        { "articOut_W", "HE051" }, // Canada

        { "FZ_A", "HE052" },

        { "WC_A", "HE053" },

        { "AF_W", "HE054" },
        { "MH_W", "HE054" }, // USA (not defined)


        { "AF_Y", "HE055" },
        { "MH_Y", "HE055" }, // USA (not defined)

        { "DU_Y", "HE056" },
        { "RWA-DU-STD_W", "HE056" }, // Australia

        { "LW_Y", "HE057" },

        { "LS_A", "HE058" },

        { "HF_W", "HE059" },
        { "hurricFrcWnd_W", "HE059" }, // Canada
        { "MWW-HURR_W", "HE059" }, // Australia

        { "SR_W", "HE060" },
        { "stormFrcWnd_W", "HE060" }, // Canada

        { "GL_W", "HE061" },
        { "galeWind_W", "HE061" }, // Canada
        { "MWW-GALE_W", "HE061" }, // Australia

        { "HF_A", "HE062" },

        { "UP_W", "HE063" },

        { "SE_W", "HE064" },

        { "SR_A", "HE065" },

        { "GL_A", "HE066" },

        { "MF_Y", "HE067" },

        { "MS_Y", "HE068" },

        { "SC_Y", "HE069" },
        { "MWW-SMCRFT_Y", "HE069" }, // Australia

        { "UP_Y", "HE070" },
        { "ZY_Y", "HE070" }, // USA (not defined)

        { "LO_Y", "HE071" },

        { "AF_V", "HE075" },

        { "UP_A", "HE076" },

        { "TAV_W", "HE077" },
        { "TAA_W", "HE077" }, // Europe

        { "TAV_A", "HE078" },
        { "TAA_A", "HE078" }, // Europe
        { "TAA_S", "HE078" }, // Europe
        { "AVL_Y", "HE078" }, // Japan

        { "TO_W", "HE0110" },
        { "tornado_W", "HE0110" }, // Canada
        { "SWW-TO-STD_W", "HE0110" } // Australia
    };
    
    /// <summary>
    /// Dictionary of state abbreviations converted into their full names.
    /// </summary>
    private readonly static Dictionary<string, string> _states = new Dictionary<string, string>
    {
        { "AL", "Alabama" },
        { "AK", "Alaska" },
        { "AZ", "Arizona" },
        { "AR", "Arkansas" },
        { "CA", "California" },
        { "CO", "Colorado" },
        { "CT", "Connecticut" },
        { "DE", "Delaware" },
        { "FL", "Florida" },
        { "GA", "Georgia" },
        { "HI", "Hawaii" },
        { "ID", "Idaho" },
        { "IL", "Illinois" },
        { "IN", "Indiana" },
        { "IA", "Iowa" },
        { "KS", "Kansas" },
        { "KY", "Kentucky" },
        { "LA", "Louisiana" },
        { "ME", "Maine" },
        { "MD", "Maryland" },
        { "DC", "District of Columbia" },
        { "MA", "Massachusetts" },
        { "MI", "Michigan" },
        { "MN", "Minnesota" },
        { "MS", "Mississippi" },
        { "MO", "Missouri" },
        { "MT", "Montana" },
        { "NE", "Nebraska" },
        { "NV", "Nevada" },
        { "NH", "New Hampshire" },
        { "NJ", "New Jersey" },
        { "NM", "New Mexico" },
        { "NY", "New York" },
        { "NC", "North Carolina" },
        { "ND", "North Dakota" },
        { "OH", "Ohio" },
        { "OK", "Oklahoma" },
        { "OR", "Oregon" },
        { "PA", "Pennsylvania" },
        { "RI", "Rhode Island" },
        { "SC", "South Carolina" },
        { "SD", "South Dakota" },
        { "TN", "Tennessee" },
        { "TX", "Texas" },
        { "UT", "Utah" },
        { "VT", "Vermont" },
        { "VA", "Virginia" },
        { "WA", "Washington" },
        { "WV", "West Virginia" },
        { "WI", "Wisconsin" },
        { "WY", "Wyoming" },
        { "INTL", "International" },
    };


    /// <summary>
    /// Utility for matching event & severity codes to their corresponding vocal key
    /// </summary>
    /// <param name="vocalCheck">Event & severity check from the AlertDetails endpoint</param>
    /// <returns>The vocal code as a string</returns>
    private string? MapVocalCode(string vocalCheck)
    {
        string? output = "";
        try
        {
            if (_vocalCodes.TryGetValue(vocalCheck, out output))
            {
                string vocalKey = output;
                return vocalKey;
            }
            else
            {
                Log.Warning($"Vocal check failed for key {vocalCheck}.");
                return "";
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Vocal check failed for key {vocalCheck}.");
            Log.Debug(ex.Message);
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                Log.Debug(ex.StackTrace);
            }
            return "";
        }
    }

    public async Task<BERecordRoot?> MakeRecord(List<GenericResponse<AlertDetailResponse>> alertDetails)
    {
        
        List<BERecord> alerts = new();
        BERecordRoot root = new()
        {
            Type = "BERecord",
            BERecord = alerts
        };

        if (alertDetails.Count < 1)
        {
            return null;
        }

        int twcIdIdx = 0;

        foreach (var details in alertDetails)
        {
            AlertDetail? detail = details.ParsedData.alertDetail;
            LFRecordLocation locationInfo = details.Location;
            
            // Timestamp parsing
            if (detail != null)
            {
                string endTime = DateTimeOffset.FromUnixTimeSeconds((int)(detail.expireTimeUTC ?? 0)).ToString("yyyy MM dd HH mm").Replace(" ", "");
                string expireTime = DateTimeOffset.FromUnixTimeSeconds((int)(detail.expireTimeUTC?? 0)).ToString("yyyy MM dd HH mm").Replace(" ", "");
                string issueTime = DateTime.Parse(detail.issueTimeLocal ?? "").ToString("yyyy MM dd HH mm").Replace(" ", "");
                string processTime = DateTimeOffset.FromUnixTimeSeconds((int)(detail.processTimeUTC ?? 0)).ToString("yyyy MM dd HH mm ss").Replace(" ", "");

                EActionCd eActionCd = new() {EActionPriority = (int)(detail.messageTypeCode ?? 0)};

                switch (detail.messageType)
                {
                    case "Update":
                        eActionCd.Text = "CON";
                        break;
                    case "New":
                        eActionCd.Text = "NEW";
                        break;
                }
                
                EOfficeId eOfficeId = new() {EOfficeNm = detail.officeName, Text = detail.officeCode};

                BEvent bEvent = new()
                {
                    ETWCIId = twcIdIdx.ToString(),
                    EActionCd = eActionCd,
                    EOfficeId = eOfficeId,

                    EPhenom = detail.phenomena,
                    EETN = detail.eventTrackingNumber,
                    EDesc = detail.eventDescription,
                    ESpnshDesc = detail.eventDescription,
                    EEndTmUTC = endTime,
                    ESvrty = (int)(detail.severityCode ?? 0),
                    EExpTmUTC = expireTime,
                    EStTmUTC = locationInfo.gmtDiff ?? "",
                    ESgnfcnc = (detail.significance ?? "")[0..1]
                };
                BStCd stateInfo = new()
                    {
                        BSt = _states["INTL"],
                        Text = "INTL"
                    };
                try
                {
                    stateInfo = new()
                    {
                        BSt = _states[locationInfo.stCd],
                        Text = locationInfo.stCd
                    };
                } catch
                {
                    stateInfo = new()
                    {
                        BSt = _states["INTL"],
                        Text = "INTL"
                    };
                }
                

                BLocCd loc = new()
                {
                    BLoc = detail.areaName,
                    BLocTyp = detail.areaTypeCode,
                    Text = detail.areaId
                };

                BLocations locations = new()
                {
                    BStCd = stateInfo,
                    BLocCd = loc,
                    BCntryCd = detail.countryCode,
                    BTzAbbrv = detail.effectiveTimeLocalTimeZone,
                    BUTCDiff = locationInfo.gmtDiff ?? ""
                };

                BEHdr header = new()
                {
                    BPIL = detail.productIdentifier,
                    BEvent = bEvent,
                    BSgmntChksum = detail.identifier,
                    ProcTm = processTime,
                    BLocations = locations
                };

                BHdln headline = new()
                {
                    BHdlnTxt = detail.headlineText,
                    BVocHdlnCd = MapVocalCode($"{detail.phenomena}_{(detail.significance ?? "")[0..1]}")
                };

                BNarrTxt narrative = new()
                {
                    BNarrTxtLang = "en_US"
                };

                BEData data = new()
                {
                    BIssueTmUTC = issueTime,
                    BHdln = headline,
                    BNarrTxt = narrative
                };

                BERecord record = new() 
                {
                    Id = "0000000",
                    LocationKey = $"{detail.areaId}_{detail.phenomena}_{(detail.significance ?? "")[0..1]}_{detail.eventTrackingNumber}_{detail.officeCode}",
                    Action = "1",
                    ClientKey = $"{detail.areaId}_{detail.phenomena}_{(detail.significance ?? "")[0..1]}_{detail.eventTrackingNumber}_{detail.officeCode}",
                    BEHdr = header,
                    BEData = data
                };       

                if (detail.texts != null)
                {
                    string narrtxt = detail.texts[0].description ?? "".Replace("\n", "");
                    if (narrtxt.Count() >= 764)
                    {
                        narrtxt = narrtxt.Substring(0, 764);
                        narrtxt += "...";
                    }
                    narrative.BLn = detail.texts[0].description ?? "".Replace("\n", "");
                }
                
                
                twcIdIdx += 1;
                alerts.Add(record);
            }
            
        }

        
        
        

        return root;
    }
    
}
