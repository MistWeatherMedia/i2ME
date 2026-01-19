using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using MistWX_i2Me;
using MistWX_i2Me.API;
using MistWX_i2Me.API.Products;
using MistWX_i2Me.Communication;
using MistWX_i2Me.RecordGeneration;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;
using MistWX_i2Me.Schema.twc;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("d8b  .d8888b.  888b     d888 8888888888 \nY8P d88P  Y88b 8888b   d8888 888        \n           " +
                          "888 88888b.d88888 888        \n888      .d88P 888Y88888P888 8888888    \n888  .od888P" +
                          "\"  888 Y888P 888 888        " +
                          "\n888 d88P\"      888  Y8P  888 888        \n888 888\"       888   \"   888 888        " +
                          "\n888 888888888  888       " +
                          "888 8888888888 ");
        
        Console.WriteLine("(C) mewtek / MistWX 2025");
        Console.WriteLine("This project is licensed under the AGPL v3.0 license.");
        Console.WriteLine("Weather information collected from The National Weather Service & The Weather Company");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Log.Info("Starting i2ME...");

        Config config = Config.Load();

        if (config.APIConfig.TwcApiKey == "REPLACE_ME" || String.IsNullOrEmpty(config.APIConfig.TwcApiKey))
        {
            Log.Error("No weather.com API key is currently set.");
            Log.Info("A valid weather.com API key needs to be set in Config.xml.");
            Log.Info("If this is your first time running i2ME, the file has been generated in the program's root folder.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }
        
        // Check if custom folder exists
        if (Directory.Exists(Path.Combine(AppContext.BaseDirectory, "Custom")) != true)
        {
            Log.Info("Custom directory not made, making right now");
            Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Custom"));
            Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Custom", "Mapping"));
            Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Custom", "Config"));

            File.Copy(Path.Combine(AppContext.BaseDirectory, "Data","LFRecord.db"),Path.Combine(AppContext.BaseDirectory, "Custom","LFRecord.db"));
            File.Copy(Path.Combine(AppContext.BaseDirectory, "Data","Mapping","HolidayMapping.xml"),Path.Combine(AppContext.BaseDirectory, "Custom","Mapping","HolidayMapping.xml"));
            File.Copy(Path.Combine(AppContext.BaseDirectory, "Data","Config","ImageSequenceDefs.xml"),Path.Combine(AppContext.BaseDirectory, "Custom","Config","ImageSequenceDefs.xml"));
        }

        UdpSender routineSender = new UdpSender(config.UnitConfig.I2MsgAddress, config.UnitConfig.RoutineMsgPort,
                config.UnitConfig.InterfaceAddress);
        UdpSender prioritySender = new UdpSender(config.UnitConfig.I2MsgAddress, config.UnitConfig.PriorityMsgPort,
            config.UnitConfig.InterfaceAddress);

        Log.SetLogLevel(config.LogLevel);

        string[] locations;
        
        if (config.LocationConfig.UseNationalLocations)
        {
            Log.Warning("Collecting data for national locations can take a while.");

            locations = new string[]
            {
                "USNM0004", "USGA0028", "USMD0018", "USME0017", "USMT0031", "USAL0054", "USND0037", "USID0025",
                "USMA0046", "USNY0081", "USVT0033", "USNC0121", "USIL0225", "USOH0188", "USOH0195", "USTX0327",
                "USCO0105", "USIA0231", "USMI0229", "USAZ0068", "USSC0140", "USCT0094", "USTX0617", "USIN0305",
                "USFL0228", "USMO0460", "USNV0049", "USAR0337", "USCA0638", "USKY1096", "USTN0325", "USFL0316",
                "USWI0455", "USMN0503", "USTN0357", "USLA0338", "USNY0996", "USNJ0355", "USVA0557", "USOK0400",
                "USNE0363", "USFL0372", "USPA1276", "USAZ0166", "USPA1290", "USME0328", "USOR0275", "USNC0558",
                "USSD0283", "USNV0076", "USCA0967", "USUT0225", "USTX1200", "USCA0982", "USCA0987", "USWA0395",
                "USWA0422", "USMO0787", "USFL0481", "USOK0537"
            };

        }
        else
        {
            string[][] exlocs = await GetMachineLocations(config);
            var locs = new List<string>();
            // Check if custom LFRecord exists.
            string LFRPath = Path.Combine(AppContext.BaseDirectory, "Custom", "LFRecord.db");
            if (File.Exists(LFRPath) != true)
            {
                File.Copy(Path.Combine(AppContext.BaseDirectory, "Data", "LFRecord.db"), LFRPath);
            }
            // Check if all locations have a LFRecord.
            SQLiteConnection sqlite =
                new SQLiteConnection($"Data Source={LFRPath}", true);
            
            sqlite.Open();

            foreach (string[] loc in exlocs)
            {
                var cmd = sqlite.CreateCommand();
                cmd.CommandText = string.Format($"SELECT count(*) FROM LFRecord WHERE locId = \"{loc[2]}\" LIMIT 1");

                using (cmd)
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        // If location does not exist, generate one.
                        LFRecordLocation lflr = await GenLFRecordLocations(loc);

                        // Inserting to current LFRecord.
                        SQLiteCommand insertSQL = sqlite.CreateCommand();
                        insertSQL.CommandText = "INSERT INTO LFRecord (locId, locType, cityNm, stCd, prsntNm, cntryCd, coopId, lat, long, obsStn, secObsStn, tertObsStn, gmtDiff, regSat, cntyNm, zoneId, zoneNm, cntyFips, active, dySTInd, dmaCd, zip2locId, elev, cliStn, tmZnNm, tmZnAbbr, dySTAct, clsRad, metRad, ultRad, ssRad, siteId, idxId, primTecci, secTecci, tertTecci, arptId, mrnZoneId, pllnId, skiId, tideId, epaId, tPrsntNm, wrlsPrsntNm, wmoId) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    
                        insertSQL.Parameters.AddWithValue("locId", lflr.locId);
                        insertSQL.Parameters.AddWithValue("locType", lflr.locType);
                        insertSQL.Parameters.AddWithValue("cityNm", lflr.cityNm);
                        insertSQL.Parameters.AddWithValue("stCd",lflr.stCd);
                        insertSQL.Parameters.AddWithValue("prsntNm",lflr.prsntNm);
                        insertSQL.Parameters.AddWithValue("cntryCd",lflr.cntryCd);
                        insertSQL.Parameters.AddWithValue("coopId", lflr.coopId);
                        insertSQL.Parameters.AddWithValue("cntryCd", lflr.cntryCd);
                        insertSQL.Parameters.AddWithValue("coopId", lflr.coopId);
                        insertSQL.Parameters.AddWithValue("lat", lflr.lat);
                        insertSQL.Parameters.AddWithValue("long",lflr.@long);
                        insertSQL.Parameters.AddWithValue("obsStn",lflr.obsStn);
                        insertSQL.Parameters.AddWithValue("secObsStn",lflr.secObsStn);
                        insertSQL.Parameters.AddWithValue("tertObsStn", lflr.tertObsStn);
                        insertSQL.Parameters.AddWithValue("gmtDiff",lflr.gmtDiff);
                        insertSQL.Parameters.AddWithValue("regSat",lflr.regSat);
                        insertSQL.Parameters.AddWithValue("coopId",lflr.coopId);
                        insertSQL.Parameters.AddWithValue("cntyNm",lflr.cntyNm);
                        insertSQL.Parameters.AddWithValue("zoneId", lflr.zoneId);
                        insertSQL.Parameters.AddWithValue("zoneNm",lflr.cntyNm);
                        insertSQL.Parameters.AddWithValue("cntyFips", lflr.cntyFips);
                        insertSQL.Parameters.AddWithValue("active",lflr.active);
                        insertSQL.Parameters.AddWithValue("dySTInd",lflr.dySTInd);
                        insertSQL.Parameters.AddWithValue("dmaCd",lflr.dmaCd);
                        insertSQL.Parameters.AddWithValue("zip2locId",lflr.zip2locId);
                        insertSQL.Parameters.AddWithValue("elev",lflr.elev);
                        insertSQL.Parameters.AddWithValue("cliStn",lflr.cliStn);
                        insertSQL.Parameters.AddWithValue("tmZnNm",lflr.tmZnNm);
                        insertSQL.Parameters.AddWithValue("tmZnAbbr",lflr.tmZnAbbr);
                        insertSQL.Parameters.AddWithValue("dySTAct",lflr.dySTAct);
                        insertSQL.Parameters.AddWithValue("clsRad",lflr.clsRad);
                        insertSQL.Parameters.AddWithValue("metRad",lflr.metRad);
                        insertSQL.Parameters.AddWithValue("ultRad",lflr.ultRad);
                        insertSQL.Parameters.AddWithValue("ssRad",lflr.ssRad);
                        insertSQL.Parameters.AddWithValue("siteId",lflr.siteId);
                        insertSQL.Parameters.AddWithValue("idxId",lflr.idxId);
                        insertSQL.Parameters.AddWithValue("primTecci",lflr.primTecci);
                        insertSQL.Parameters.AddWithValue("secTecci",lflr.secTecci);
                        insertSQL.Parameters.AddWithValue("tertTecci",lflr.tertTecci);
                        insertSQL.Parameters.AddWithValue("arptId",lflr.arptId);
                        insertSQL.Parameters.AddWithValue("mrnZoneId",lflr.mrnZoneId);
                        insertSQL.Parameters.AddWithValue("pllnId", lflr.pllnId);
                        insertSQL.Parameters.AddWithValue("skiId", lflr.skiId);
                        insertSQL.Parameters.AddWithValue("tideId", lflr.tideId);
                        insertSQL.Parameters.AddWithValue("epaId",lflr.epaId);
                        insertSQL.Parameters.AddWithValue("tPrsntNm",lflr.tPrsntNm);
                        insertSQL.Parameters.AddWithValue("wrlsPrsntNm",lflr.wrlsPrsntNm);
                        insertSQL.Parameters.AddWithValue("wmoId",lflr.wmoId);

                        try {
                            insertSQL.ExecuteNonQuery();
                        }
                        catch (Exception ex) {
                            throw new Exception(ex.Message);
                        }

                        // Send to i2 LFRecord.
                        string response = await new LFRecord().MakeRecord(lflr);
                        if (response != null)
                        {
                            routineSender.SendFile(response, "storeData(QGROUP=__LFRecord__,Feed=LFRecord)");
                        }
                    }
                    
                }
                locs.Add(loc[2]);
            }
            // Close SQLite please
            sqlite.Close();
            // Convert locations to array.
            locations = locs.ToArray<string>();
        }

        
        Task checkAlerts = TimedTasks.CheckForAlerts(locations, prioritySender, config.AConfig.CheckAlertTimeSeconds);
        Task recordGenTask = TimedTasks.RecordGenTask(locations, routineSender, config.DConfig.RecordGenTimeSeconds);
        Task radarTask = TimedTasks.RadarTask(prioritySender, config.RadarConfiguration.RadarInt);
        Task clearAlertsCache = TimedTasks.ClearExpiredAlerts();
        
        await Task.WhenAll(checkAlerts, recordGenTask, radarTask, clearAlertsCache);

    }

    /// <summary>
    /// Runs through the pre-existing MachineProductConfig.xml file to scrape what locations that need
    /// weather information collected.
    /// </summary>
    private static async Task<string[][]> GetMachineLocations(Config config)
    {
        List<string[]> locations = new List<string[]>();

        Log.Info("Getting locations for this unit..");

        string copyPath = Path.Combine(AppContext.BaseDirectory, "MachineProductConfig.xml");

        if (File.Exists(copyPath))
        {
            File.Delete(copyPath);
        }

        if (!File.Exists(config.MachineProductConfig))
        {
            Log.Error("Unable to locate MachineProductConfig.xml");
            return locations.ToArray();
        }
        
        File.Copy(config.MachineProductConfig, copyPath);

        MachineProductConfig? mpc = null;
        
        using (StreamReader reader = new(copyPath))
        {
            XmlSerializer serializer = new(typeof(MachineProductConfig));
            if (reader != null)
            {
                mpc = (MachineProductConfig?)serializer.Deserialize(reader);
            } else
            {
                throw new Exception("MachineProductCfg could not be read!");
            }
            
        }

        //TODO: Find a better way to do this.
        var configLocationKeys = new List<string> 
        {
                "PrimaryLocation",
                "NearbyLocation1",
                "NearbyLocation2",
                "NearbyLocation3",
                "NearbyLocation4",
                "NearbyLocation5",
                "NearbyLocation6",
                "NearbyLocation7",
                "NearbyLocation8",
                "MetroMapCity1",
                "MetroMapCity2",
                "MetroMapCity3",
                "MetroMapCity4",
                "MetroMapCity5",
                "MetroMapCity6",
                "MetroMapCity7",
                "MetroMapCity8",
            };
        if (config.LocationConfig.LocationList != null) {
            configLocationKeys = config.LocationConfig.LocationList;
        }

        if (mpc != null)
        {
            if (mpc.ConfigDef != null)
            {
                if (mpc.ConfigDef.ConfigItems != null)
                {
                    if (mpc.ConfigDef.ConfigItems.ConfigItem != null)
                    {
                        foreach (ConfigItem i in mpc.ConfigDef.ConfigItems.ConfigItem)
                        {
                            if (i.Key != null)
                            {
                                if (configLocationKeys.Contains(i.Key))
                                {
                                    Log.Debug($"{i.Key}: {i.Value}");
                                    if (string.IsNullOrEmpty(i.Value))
                                    {
                                        continue;
                                    }
                                    try
                                    {
                                        if (Regex.IsMatch(i.Value.ToString(), @"^\d\d\d\d\d(?:,\d\d\d\d\d)*$")) {
                                            string[] choppedValues = i.Value.ToString().Split(",");
                                            locations.Add(choppedValues);
                                        }
                                        else
                                        {
                                            string[] choppedValues = i.Value.ToString().Split("_");

                                        // Avoid duplicate locations from being added to the location list
                                        if (locations.Contains(choppedValues.GetValue(2)))
                                        {
                                            continue;
                                        }
                                            locations.Add(choppedValues);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Debug($"Failed to configure locations for {i.Key}");
                                        Log.Debug(ex.Message);
                                        // Print stacktrace to the debug console if applicable
                                        if (!string.IsNullOrEmpty(ex.StackTrace))
                                        {
                                            Log.Debug(ex.StackTrace);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        
        return locations.ToArray();
    }

    /// <summary>
    /// Generate LFRecord values for unknown locations.
    /// </summary>
    private static async Task<LFRecordLocation> GenLFRecordLocations(string[] loc)
    {
        // Set current values.
        LFRecordLocation tempLF = new LFRecordLocation();
        tempLF.locType = loc[0];
        tempLF.cntryCd = loc[1];
        tempLF.locId = loc[2];
        string genTecci = tempLF.locType + "-" + tempLF.cntryCd + "-" + tempLF.locId;
        tempLF.primTecci = "T" + genTecci;
        tempLF.coopId = genTecci;

        // Fallback values
        tempLF.cityNm = tempLF.locId.ToUpper();
        tempLF.prsntNm = tempLF.locId;
        tempLF.tPrsntNm = tempLF.locId;
        tempLF.lat = "41.45";
        tempLF.@long = "-74.42";
        tempLF.wrlsPrsntNm = tempLF.locId;
        tempLF.regSat = null;
        tempLF.ssRad = null;
        tempLF.lsRad = null;
        tempLF.siteId = "INTL";
        tempLF.stCd = "INTL";
        tempLF.cntyNm = $"{tempLF.locId} County";
        tempLF.gmtDiff = "-5.00";
        tempLF.cliStn = "305310";
        tempLF.tmZnNm = "Eastern Standard Time";
        tempLF.tmZnAbbr = "EST";
        tempLF.wmoId = null;
        tempLF.arptId = null;
        tempLF.idxId = "KMGJ";
        tempLF.skiId = null;
        tempLF.active = "1";
        tempLF.cntyFips = "99999";
        tempLF.dySTInd = "N";
        tempLF.dySTAct = "0.00";
        tempLF.elev = "0";
        tempLF.dmaCd = null;

        // Grab data.
        GenericResponse<LocServPointResponse>? point = await new LocServPointProduct().Receive(tempLF);

        if (point != null)
        {
            if (point.ParsedData.location != null)
            {
                // Location... location.
                tempLF.lat = Math.Round(point.ParsedData.location.latitude, 2).ToString();
                tempLF.@long = Math.Round(point.ParsedData.location.longitude, 2).ToString();

                // Location Name
                // Set it to Display Name
                if (point.ParsedData.location.displayName != null)
                {
                    tempLF.cityNm = point.ParsedData.location.displayName.ToUpper();
                    tempLF.prsntNm = point.ParsedData.location.displayName;
                    tempLF.tPrsntNm = point.ParsedData.location.displayName;
                    if (point.ParsedData.location.displayName.Length >= 16)
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.displayName.Substring(0,15) + ".";
                    } else
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.displayName;
                    }
                // Set it to City
                } else if (point.ParsedData.location.city != null)
                {
                    tempLF.cityNm = point.ParsedData.location.city.ToUpper();
                    tempLF.prsntNm = point.ParsedData.location.city;
                    tempLF.tPrsntNm = point.ParsedData.location.city;
                    if (point.ParsedData.location.city.Length >= 16)
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.city.Substring(0,15) + ".";
                    } else
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.city;
                    }
                // Set it to Locale 2
                } else if (point.ParsedData.location.locale != null && point.ParsedData.location.locale.locale2 != null)
                {
                    tempLF.cityNm = point.ParsedData.location.locale.locale2.ToUpper();
                    tempLF.prsntNm = point.ParsedData.location.locale.locale2;
                    tempLF.tPrsntNm = point.ParsedData.location.locale.locale2;
                    if (point.ParsedData.location.locale.locale2.Length >= 16)
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.locale.locale2.Substring(0,15) + ".";
                    } else
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.locale.locale2;
                    }
                // Set it to Neighbourhood
                } else if (point.ParsedData.location.neighborhood != null)
                {
                    tempLF.cityNm = point.ParsedData.location.neighborhood.ToUpper();
                    tempLF.prsntNm = point.ParsedData.location.neighborhood;
                    tempLF.tPrsntNm = point.ParsedData.location.neighborhood;
                    if (point.ParsedData.location.neighborhood.Length >= 16)
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.neighborhood.Substring(0,15) + ".";
                    } else
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.neighborhood;
                    }
                // Set to postal code
                } else if (point.ParsedData.location.postalCode != null)
                {
                    tempLF.cityNm = point.ParsedData.location.postalCode.ToUpper();
                    tempLF.prsntNm = point.ParsedData.location.postalCode;
                    tempLF.tPrsntNm = point.ParsedData.location.postalCode;
                    if (point.ParsedData.location.postalCode.Length >= 16)
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.postalCode.Substring(0,15) + ".";
                    } else
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.postalCode;
                    }
                // Set it to Admin District
                } else if (point.ParsedData.location.adminDistrict != null)
                {
                    tempLF.cityNm = point.ParsedData.location.adminDistrict.ToUpper();
                    tempLF.prsntNm = point.ParsedData.location.adminDistrict;
                    tempLF.tPrsntNm = point.ParsedData.location.adminDistrict;
                    if (point.ParsedData.location.adminDistrict.Length >= 16)
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.adminDistrict.Substring(0,15) + ".";
                    } else
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.adminDistrict;
                    }
                // Set it to Country
                } else if (point.ParsedData.location.country != null)
                {
                    tempLF.cityNm = point.ParsedData.location.country.ToUpper();
                    tempLF.prsntNm = point.ParsedData.location.country;
                    tempLF.tPrsntNm = point.ParsedData.location.country;
                    if (point.ParsedData.location.country.Length >= 16)
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.country.Substring(0,15) + ".";
                    } else
                    {
                        tempLF.wrlsPrsntNm = point.ParsedData.location.country;
                    }
                } 
                
                // State code 
                // Set it to adminDistrictCode
                if (point.ParsedData.location.adminDistrictCode != null)
                {
                    tempLF.stCd = point.ParsedData.location.adminDistrictCode;
                }

                // County/Zone names
                // Set it to Locale 1
                if (point.ParsedData.location.locale != null && point.ParsedData.location.locale.locale1 != null)
                {
                    tempLF.cntyNm = point.ParsedData.location.locale.locale1;
                // Set it to admin district
                } else if (point.ParsedData.location.adminDistrict != null)
                {
                    tempLF.cntyNm = point.ParsedData.location.adminDistrict;
                // Set it to neighborhood
                } else if (point.ParsedData.location.neighborhood != null)
                {
                    tempLF.cntyNm = point.ParsedData.location.neighborhood;
                
                // Set it to display name
                } else if (point.ParsedData.location.displayName != null)
                {
                    tempLF.cntyNm = point.ParsedData.location.displayName;
                // Set it to city
                } else if (point.ParsedData.location.city != null)
                {
                    tempLF.cntyNm = point.ParsedData.location.city;
                // Set it to countyId
                } else if (point.ParsedData.location.countyId != null)
                {
                    tempLF.cntyNm = point.ParsedData.location.countyId;
                }

                // Set countyId
                if (point.ParsedData.location.countyId != null)
                {
                    tempLF.cntyId = point.ParsedData.location.countyId;
                } 

                // Set zoneId
                if (point.ParsedData.location.zoneId != null)
                {
                    tempLF.zoneId = point.ParsedData.location.zoneId;
                }
            
                // Set regionalSatellite
                if (point.ParsedData.location.regionalSatellite != null)
                {
                    tempLF.regSat = point.ParsedData.location.regionalSatellite;
                    tempLF.ssRad = point.ParsedData.location.regionalSatellite;
                    tempLF.lsRad = point.ParsedData.location.regionalSatellite;
                }

                // Set zip2locid
                if (point.ParsedData.location.postalCode != null)
                {
                    tempLF.zip2locId = point.ParsedData.location.postalCode;
                }

                // Set siteId and cntryCd
                if (point.ParsedData.location.countryCode != null)
                {
                    tempLF.siteId = point.ParsedData.location.countryCode;
                    tempLF.cntryCd = point.ParsedData.location.countryCode;
                }

                // Set timezone
                if (point.ParsedData.location.ianaTimeZone != null)
                {
                    TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(point.ParsedData.location.ianaTimeZone);
                    tempLF.tmZnNm = tz.StandardName;
                    tempLF.tmZnAbbr = Regex.Replace(tz.StandardName, "[^A-Z]", "");
                    tempLF.gmtDiff = $"{tz.BaseUtcOffset.Hours}.{tz.BaseUtcOffset.ToString("mm")}";
                }

                // Set wmoId and pllnId
                if (point.ParsedData.location.pollenId != null)
                {
                    tempLF.pllnId = point.ParsedData.location.pollenId;
                    tempLF.wmoId = point.ParsedData.location.pollenId;
                } 

                // Set dmaCd
                if (point.ParsedData.location.dmaCd != null)
                {
                    tempLF.dmaCd = point.ParsedData.location.dmaCd;
                }

                GenericResponse<LocServNearAirportResponse>? airport = await new LocServNearAirportProduct().Receive(tempLF);
                GenericResponse<LocServNearSkiResponse>? ski = await new LocServNearSki().Receive(tempLF);
                GenericResponse<LocServNearObsResponse>? obs = await new LocServNearObs().Receive(tempLF);
                GenericResponse<Almanac1DayResponse>? al = await new Almanac1DayProduct().Receive(tempLF);
                GenericResponse<CurrentObservations2Response>? cc = await new CurrentObservationsProduct2().Receive(tempLF);

                if (airport != null)
                {
                    if (airport.ParsedData.location != null)
                    {
                        if (airport.ParsedData.location.iataCode != null)
                        {
                            foreach (string? apId in airport.ParsedData.location.iataCode)
                            {
                                if (apId != null)
                                {
                                    tempLF.arptId = apId;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (ski != null)
                {
                    if (ski.ParsedData.location != null)
                    {
                        if (ski.ParsedData.location.skiId != null)
                        {
                            foreach (string? skiId in ski.ParsedData.location.skiId)
                            {
                                if (skiId != null)
                                {
                                    tempLF.skiId = skiId;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (obs != null)
                {
                    int obsIdx = 0;
                    if (obs.ParsedData.location != null)
                    {
                        if (obs.ParsedData.location.stationId != null)
                        {
                            foreach (string? obsId in obs.ParsedData.location.stationId)
                            {
                                if (obs.ParsedData.location != null)
                                {
                                if (obsId != null)
                                    {
                                        if (obsIdx == 0)
                                        {
                                            tempLF.obsStn = obsId;
                                            tempLF.idxId = obsId;
                                        } else if (obsIdx == 1)
                                        {
                                            tempLF.secObsStn = obsId;
                                        } else if (obsIdx == 2)
                                        {
                                            tempLF.tertObsStn = obsId;
                                        } else
                                        {
                                            break;
                                        }
                                        obsIdx += 1;
                                    } 
                                }
                            }
                        }
                    }
                }

                if (al != null)
                {
                    if (al.ParsedData.stationId != null)
                    {
                        tempLF.cliStn = al.ParsedData.stationId.First();
                    }
                }

                if (cc != null)
                {
                    if (cc.ParsedData.observation != null && cc.ParsedData.observation.key != null)
                    {
                        tempLF.coopId = cc.ParsedData.observation.key;
                        tempLF.primTecci = cc.ParsedData.observation.key;
                    }
                }

            } else
            {
                Log.Warning($"Location {tempLF.locId} has no location data, generating fallback ones");
            }
        }
        // Return the generated LFRecordLocation.
        return tempLF;
    }
}

