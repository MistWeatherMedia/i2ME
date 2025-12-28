using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using MistWX_i2Me;
using MistWX_i2Me.API;
using MistWX_i2Me.API.Products;
using MistWX_i2Me.Communication;
using MistWX_i2Me.RecordGeneration;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.System;

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

        if (config.TwcApiKey == "REPLACE_ME" || String.IsNullOrEmpty(config.TwcApiKey))
        {
            Log.Error("No weather.com API key is currently set.");
            Log.Info("A valid weather.com API key needs to be set in Config.xml.");
            Log.Info("If this is your first time running i2ME, the file has been generated in the program's root folder.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
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
            if (config.LocationConfig.NationalLocations == null) 
            {
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
                locations = config.LocationConfig.NationalLocations;
            }
        }
        else
        {
            locations = await GetMachineLocations(config);
        }
        
        Task checkAlerts = TimedTasks.CheckForAlerts(locations, prioritySender, config.CheckAlertTimeSeconds);
        Task recordGenTask = TimedTasks.RecordGenTask(locations, routineSender, config.RecordGenTimeSeconds);
        Task clearAlertsCache = TimedTasks.ClearExpiredAlerts();
        
        await Task.WhenAll(checkAlerts, recordGenTask, clearAlertsCache);

    }

    /// <summary>
    /// Runs through the pre-existing MachineProductConfig.xml file to scrape what locations that need
    /// weather information collected.
    /// </summary>
    private static async Task<string[]> GetMachineLocations(Config config)
    {
        List<string> locations = new List<string>();

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

        MachineProductConfig mpc;
        
        using (var reader = new StreamReader(copyPath))
        {
            mpc = (MachineProductConfig) new XmlSerializer(typeof(MachineProductConfig)).Deserialize(reader);
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

        foreach (ConfigItem i in mpc.ConfigDef.ConfigItems.ConfigItem)
        {
            if (configLocationKeys.Contains(i.Key))
            {
                Log.Debug($"{i.Key}: {i.Value}");
                if (string.IsNullOrEmpty(i.Value.ToString()))
                {
                    continue;
                }
                try
                {
                    if (Regex.IsMatch(i.Value.ToString(), @"^\d\d\d\d\d(?:,\d\d\d\d\d)*$")) {
                        string[] choppedValues = i.Value.ToString().Split(",");
                        foreach (string choppedValue in choppedValues)
                        {
                            locations.Add(choppedValue);
                        }
                    }
                    else
                    {
                        string choppedValue = i.Value.ToString().Split("_")[2];

                    // Avoid duplicate locations from being added to the location list
                    if (locations.Contains(choppedValue))
                    {
                        continue;
                    }
                        locations.Add(choppedValue);
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
        
        return locations.ToArray();
    }
}