using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.Xml;

namespace MistWX_i2Me;

/// <summary>
/// Class for building the config.xml file used to configure the software
/// </summary>
[XmlRoot("Config")]
public class Config
{
    
    [XmlAnyElement("VersionComment")]
    public XmlComment VersionComment { get { return new XmlDocument().CreateComment("DO NOT CHANGE THIS!"); } set { } }
    [XmlElement]
    public int Version {get; set;} = 1;

    [XmlAnyElement("LLevelComment")]
    public XmlComment LLevelComment { get { return new XmlDocument().CreateComment("This sets how verbose you would like i2ME to be. (ex: debug, info, warning, error)"); } set { } }
    [XmlElement] public string LogLevel { get; set; } = "info";

    // Used to process what locations to generate
    [XmlAnyElement("MPCComment")]
    public XmlComment MPCComment { get { return new XmlDocument().CreateComment("Path to your MachineProductCfg."); } set { } }
    public string MachineProductConfig { get; set; } =
        "C:\\Program Files (x86)\\TWC\\i2\\Managed\\Config\\MachineProductCfg.xml";

    // Config Elements \\
    [XmlElement("APIKeyConfig")] public APIKeyConfig APIConfig { get; set; } = new APIKeyConfig();
    [XmlElement("DataConfig")] public DataConfig DConfig { get; set; } = new DataConfig();
    [XmlElement("AlertConfig")] public AlertConfig AConfig { get; set; } = new AlertConfig();
    [XmlElement("LocationConfig")] public LocConfig LocationConfig { get; set; } = new LocConfig();
    [XmlElement("UnitConfig")] public NetworkConfig UnitConfig { get; set; } = new NetworkConfig();
    [XmlElement("LocalConfig")] public LocalConfig LocalStarConfig { get; set; } = new LocalConfig();
    [XmlElement("RadarConfig")] public RadarConfig RadarConfiguration { get; set; } = new RadarConfig();
    [XmlElement("RecordConfig")] public DataEndpointConfig EndpointConfig { get; set; } = new DataEndpointConfig();

    // Actual configuration setup \\
    
    public static Config config = new Config();



    
    /// <summary>
    /// Loads values from the configuration file into the software
    /// </summary>
    /// <returns>Config object</returns>
    public static Config Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "config.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(Config));
        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
        namespaces.Add("", "");
        
        

        // Create the temp directory if none exists
        if (!Directory.Exists(Path.Combine(AppContext.BaseDirectory, "temp")))
        {
            Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "temp"));
        }
        
        // Create a base config if none exists
        if (!File.Exists(path))
        {
            config = new Config();
            serializer.Serialize(File.Create(path), config, namespaces);

            return config;
        }

        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            var deserializedConfig = serializer.Deserialize(stream);

            if (deserializedConfig != null && deserializedConfig is Config cfg)
            {
                config = cfg;

                // Possibly bullshit code incoming! - PB

                // Add tide locations
                if (cfg.LocationConfig.UseTideLocs)
                {
                    cfg.LocationConfig.LocationList.AddRange(new List<string>
                    {
                        "TideStation1",
                        "TideStation2",
                        "TideStation3",
                        "TideStation4",
                        "TideStation5",
                        "TideStation6",
                        "TideStation7",
                        "TideStation8",
                    }
                    );
                    if (cfg.LocationConfig.UseSubset1Locs)
                    {
                        cfg.LocationConfig.LocationList.AddRange(new List<string>
                        {
                            "TideStation1_1",
                            "TideStation2_1",
                            "TideStation3_1",
                            "TideStation4_1",
                            "TideStation5_1",
                            "TideStation6_1",
                            "TideStation7_1",
                            "TideStation8_1",
                        }
                        );
                    }
                    if (cfg.LocationConfig.UseSubset2Locs)
                    {
                        cfg.LocationConfig.LocationList.AddRange(new List<string>
                        {
                            "TideStation1_2",
                            "TideStation2_2",
                            "TideStation3_2",
                            "TideStation4_2",
                            "TideStation5_2",
                            "TideStation6_2",
                            "TideStation7_2",
                            "TideStation8_2",
                        }
                        );
                    }
                }
                // Add subset 1 locations
                if (cfg.LocationConfig.UseSubset1Locs)
                {
                    cfg.LocationConfig.LocationList.AddRange(new List<string>
                    {
                        "PrimaryLocation_1",
                        "NearbyLocation1_1",
                        "NearbyLocation2_1",
                        "NearbyLocation3_1",
                        "NearbyLocation4_1",
                        "NearbyLocation5_1",
                        "NearbyLocation6_1",
                        "NearbyLocation7_1",
                        "NearbyLocation8_1",
                        "MetroMapCity1_1",
                        "MetroMapCity2_1",
                        "MetroMapCity3_1",
                        "MetroMapCity4_1",
                        "MetroMapCity5_1",
                        "MetroMapCity6_1",
                        "MetroMapCity7_1",
                        "MetroMapCity8_1",
                    }
                    );
                }
                // Add subset 2 locations
                if (cfg.LocationConfig.UseSubset2Locs)
                {
                    cfg.LocationConfig.LocationList.AddRange(new List<string>
                    {
                        "PrimaryLocation_2",
                        "NearbyLocation1_2",
                        "NearbyLocation2_2",
                        "NearbyLocation3_2",
                        "NearbyLocation4_2",
                        "NearbyLocation5_2",
                        "NearbyLocation6_2",
                        "NearbyLocation7_2",
                        "NearbyLocation8_2",
                        "MetroMapCity1_2",
                        "MetroMapCity2_2",
                        "MetroMapCity3_2",
                        "MetroMapCity4_2",
                        "MetroMapCity5_2",
                        "MetroMapCity6_2",
                        "MetroMapCity7_2",
                        "MetroMapCity8_2",
                    }
                    );
                }
                // add map locations
                if (cfg.LocationConfig.UseMapLocs)
                {
                    cfg.LocationConfig.LocationList.AddRange(new List<string>
                    {
                        "MapCity1",
                        "MapCity2",
                        "MapCity3",
                        "MapCity4",
                        "MapCity5",
                        "MapCity6",
                        "MapCity7",
                        "MapCity8",
                    }
                    );
                    if (cfg.LocationConfig.UseSubset1Locs)
                    {
                        cfg.LocationConfig.LocationList.AddRange(new List<string>
                        {
                            "MapCity1_1",
                            "MapCity2_1",
                            "MapCity3_1",
                            "MapCity4_1",
                            "MapCity5_1",
                            "MapCity6_1",
                            "MapCity7_1",
                            "MapCity8_1",
                        }
                        );
                    }
                    if (cfg.LocationConfig.UseSubset2Locs)
                    {
                        cfg.LocationConfig.LocationList.AddRange(new List<string>
                        {
                            "MapCity1_2",
                            "MapCity2_2",
                            "MapCity3_2",
                            "MapCity4_2",
                            "MapCity5_2",
                            "MapCity6_2",
                            "MapCity7_2",
                            "MapCity8_2",
                        }
                        );
                    }
                }
                // add travel locations
                if (cfg.LocationConfig.UseTravelLocs)
                {
                    cfg.LocationConfig.LocationList.AddRange(new List<string>
                    {
                        "TravelCity1",
                        "TravelCity2",
                        "TravelCity3",
                        "TravelCity4",
                        "TravelCity5",
                        "TravelCity6",
                        "TravelCity7",
                        "TravelCity8",
                    }
                    );
                    if (cfg.LocationConfig.UseSubset1Locs)
                    {
                        cfg.LocationConfig.LocationList.AddRange(new List<string>
                        {
                            "TravelCity1_1",
                            "TravelCity2_1",
                            "TravelCity3_1",
                            "TravelCity4_1",
                            "TravelCity5_1",
                            "TravelCity6_1",
                            "TravelCity7_1",
                            "TravelCity8_1",
                        }
                        );
                    }
                    if (cfg.LocationConfig.UseSubset2Locs)
                    {
                        cfg.LocationConfig.LocationList.AddRange(new List<string>
                        {
                            "TravelCity1_2",
                            "TravelCity2_2",
                            "TravelCity3_2",
                            "TravelCity4_2",
                            "TravelCity5_2",
                            "TravelCity6_2",
                            "TravelCity7_2",
                            "TravelCity8_2",
                        }
                        );
                    }
                }
                // add winter getaway locations
                if (cfg.LocationConfig.UseWinterGLocs)
                {
                    cfg.LocationConfig.LocationList.AddRange(new List<string>
                    {
                        "WinterGetawayLocation1",
                        "WinterGetawayLocation2",
                        "WinterGetawayLocation3",
                    }
                    );
                    if (cfg.LocationConfig.UseSubset1Locs)
                    {
                        cfg.LocationConfig.LocationList.AddRange(new List<string>
                        {
                            "WinterGetawayLocation1_1",
                            "WinterGetawayLocation2_1",
                            "WinterGetawayLocation3_1",
                        }
                        );
                    }
                    if (cfg.LocationConfig.UseSubset2Locs)
                    {
                        cfg.LocationConfig.LocationList.AddRange(new List<string>
                        {
                            "WinterGetawayLocation2_1",
                            "WinterGetawayLocation2_2",
                            "WinterGetawayLocation3_2",
                        }
                        );
                    }
                }
                return config;
            }

            return new Config();
        }
        
    }

    [XmlRoot("LocationConfig")]
    public class LocConfig
    {
        [XmlAnyElement("TideComment")]
        public XmlComment TideComment { get { return new XmlDocument().CreateComment("Grab tide locations."); } set { } }
        [XmlElement] public bool UseTideLocs { get; set; } = false;

        [XmlAnyElement("MapComment")]
        public XmlComment MapComment { get { return new XmlDocument().CreateComment("Grab map locations."); } set { } }
        [XmlElement] public bool UseMapLocs { get; set; } = false;

        [XmlAnyElement("TravelComment")]
        public XmlComment TravelComment { get { return new XmlDocument().CreateComment("Grab travel locations."); } set { } }
        [XmlElement] public bool UseTravelLocs { get; set; } = false;

        [XmlAnyElement("WinterGComment")]
        public XmlComment WinterGComment { get { return new XmlDocument().CreateComment("Grab Winter Getaway locations."); } set { } }
        [XmlElement] public bool UseWinterGLocs { get; set; } = false;

        [XmlAnyElement("SummerGComment")]
        public XmlComment SummerGComment { get { return new XmlDocument().CreateComment("Grab Summer Getaway locations."); } set { } }
        [XmlElement] public bool UseSummerGLocs { get; set; } = false;

        [XmlAnyElement("Subset1Comment")]
        public XmlComment Subset1Comment { get { return new XmlDocument().CreateComment("Grab Subset 1/_1 locations."); } set { } }
        [XmlElement] public bool UseSubset1Locs { get; set; } = false;

        [XmlAnyElement("Subset2Comment")]
        public XmlComment Subset2Comment { get { return new XmlDocument().CreateComment("Grab Subset 2/_2 locations."); } set { } }
        [XmlElement] public bool UseSubset2Locs { get; set; } = false;


        [XmlAnyElement("NationalComment")]
        public XmlComment NationalComment { get { return new XmlDocument().CreateComment("Grab all the national locations, not the ones from the MachineProductCfg."); } set { } }
        [XmlElement] public bool UseNationalLocations { get; set; } = false;

        [XmlIgnore] 
        // Base locations.
        public List<string> LocationList = new List<string> {
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
    }

    [XmlRoot("UnitConfig")]
    public class NetworkConfig
    {
        [XmlAnyElement("RoutineComment")]
        public XmlComment RoutineComment { get { return new XmlDocument().CreateComment("Port for routine messages. Redundant if UseExecInstead is turned on."); } set { } }
        [XmlElement] public int RoutineMsgPort { get; set; } = 7787;

        [XmlAnyElement("PriorityComment")]
        public XmlComment PriorityComment { get { return new XmlDocument().CreateComment("Port for priority messages. Redundant if UseExecInstead is turned on."); } set { } }
        [XmlElement] public int PriorityMsgPort { get; set; } = 7788;

        [XmlAnyElement("i2AddrComment")]
        public XmlComment i2AddrComment { get { return new XmlDocument().CreateComment("UDP multicast address to i2Service. Redundant if UseExecInstead is turned on."); } set { } }
        [XmlElement] public string I2MsgAddress { get; set; } = "224.1.1.77";

        [XmlAnyElement("IntrAddrComment")]
        public XmlComment IntrAddrComment { get { return new XmlDocument().CreateComment("Interface address, can be 127.0.0.1. Redundant if UseExecInstead is turned on."); } set { } }
        [XmlElement] public string InterfaceAddress { get; set; } = "127.0.0.1";

        [XmlAnyElement("ExecComment")]
        public XmlComment ExecComment { get { return new XmlDocument().CreateComment("Use a usually more reliable way of sending data to i2Service. Is enabled by default."); } set { } }
        [XmlElement] public bool UseExecInstead { get; set; } = true;
    }

    [XmlRoot("RadarConfig")]
    public class RadarConfig
    {
        [XmlAnyElement("UseRadarServerComment")]
        public XmlComment UseRadarServerComment { get { return new XmlDocument().CreateComment("Use a radar server to retrieve radar data."); } set { } }
        [XmlElement] public bool UseRadarServer { get; set; } = false;

        [XmlAnyElement("RadarServerComment")]
        public XmlComment RadarServerComment { get { return new XmlDocument().CreateComment("Radar server url."); } set { } }
        [XmlElement] public string RadarServerUrl { get; set; } = "REPLACE_ME";

        [XmlAnyElement("RadarEnableComment")]
        public XmlComment RadarEnableComment { get { return new XmlDocument().CreateComment("Enable radar?"); } set { } }
        [XmlElement] public bool RadarEnable { get; set; } = true;

        [XmlAnyElement("SatRadEnableComment")]
        public XmlComment SatRadEnableComment { get { return new XmlDocument().CreateComment("Enable satellite radar?"); } set { } }
        [XmlElement] public bool SatRadEnable { get; set; } = true;

        [XmlAnyElement("RadarDefComment")]
        public XmlComment RadarDefComment { get { return new XmlDocument().CreateComment("Set radar cut. Default is Radar-US."); } set { } }
        [XmlElement] public string RadarDef { get; set; } = "Radar-US";

        [XmlAnyElement("SatRadDefComment")]
        public XmlComment SatRadDefComment { get { return new XmlDocument().CreateComment("Set satellite radar cut. Default is SatRad-US."); } set { } }
        [XmlElement] public string SatRadDef { get; set; } = "SatRad-US";

        [XmlAnyElement("RadarIntComment")]
        public XmlComment RadarIntComment { get { return new XmlDocument().CreateComment("Set how often should radar update. Default is 300."); } set { } }
        [XmlElement] public int RadarInt { get; set; } = 300;

        [XmlAnyElement("LocalRadarComment")]
        public XmlComment LocalRadarComment { get { return new XmlDocument().CreateComment("Grab the tiles nearby the center of your map instead of the entire map. Set to true since downloading every single tile for the national map takes a lot of time."); } set { } }
        [XmlElement] public bool LocalRadar { get; set; } = true;

        [XmlAnyElement("LocalRadarRadiusComment")]
        public XmlComment LocalRadarRadiusComment { get { return new XmlDocument().CreateComment("ONLY WORKS WITH LOCALRADAR! Add extra tiles. For example, a radius of 1 results in a 3x3 radius, a radius of 2 results in a 5x5, etc"); } set { } }
        [XmlElement] public int LocalRadarRadius { get; set; } = 1;

    }

    [XmlRoot("AlertConfig")]
    public class AlertConfig
    {
        [XmlAnyElement("GetAlertsComment")]
        public XmlComment GetAlertsComment { get { return new XmlDocument().CreateComment("Sets if you want i2ME to get alerts for your i2. Can be true or false."); } set { } }
        [XmlElement] public bool GetAlerts { get; set; } = true;

        [XmlAnyElement("CheckAlertTimeComment")]
        public XmlComment CheckAlertTimeComment { get { return new XmlDocument().CreateComment("Sets how long should i2ME wait before grabbing alerts."); } set { } }
        [XmlElement] public int CheckAlertTimeSeconds { get; set; } = 600;      // Defaults to 10 minutes
    }

    [XmlRoot("LocalConfig")]
    public class LocalConfig
    {
        [XmlAnyElement("UnitComment")]
        public XmlComment UnitComment { get { return new XmlDocument().CreateComment("Sets the unit of the data sent to the i2."); } set { } }
        [XmlAnyElement("Unit1Comment")]
        public XmlComment Unit1Comment { get { return new XmlDocument().CreateComment("This doesn't change the units displayed on the i2 - so if it's 0 degrees C, it gets shown as 0 degrees F on the i2."); } set { } }
        [XmlAnyElement("Unit2Comment")]
        public XmlComment Unit2Comment { get { return new XmlDocument().CreateComment("Values can be e, m, h, or s. For more information, check out the Weather Company Data API Common Usage Guide: https://www.ibm.com/docs/en/environmental-intel-suite?topic=apis-weather-company-data-api-common-usage-guide"); } set { } }
        [XmlElement] public string Unit { get; set; } = "e";
        
        [XmlAnyElement("LangComment")]
        public XmlComment LangComment { get { return new XmlDocument().CreateComment("Sets the language of the data sent to the i2."); } set { } }
        [XmlAnyElement("Lang1Comment")]
        public XmlComment Lang1Comment { get { return new XmlDocument().CreateComment("The i2 doesn't typically display other languages - so expect some problems. It also has most values hardcoded to English."); } set { } }
        [XmlAnyElement("Lang2Comment")]
        public XmlComment Lang2Comment { get { return new XmlDocument().CreateComment("Values are defined in the Weather Company Data API Common Usage Guide: https://www.ibm.com/docs/en/environmental-intel-suite?topic=apis-weather-company-data-api-common-usage-guide"); } set { } }
        [XmlElement] public string Language { get; set; } = "en-US";
    }

    [XmlRoot("ApiKeyConfig")]
    public class APIKeyConfig
    {
        [XmlAnyElement("TWCApiKeyComment")]
        public XmlComment TWCApiKeyComment { get { return new XmlDocument().CreateComment("The API key given to you by The Weather Company. This is required!"); } set { } }
        [XmlElement] public string TwcApiKey { get; set; } = "REPLACE_ME";
    }

    [XmlRoot("DataConfig")]
    public class DataConfig
    {
        [XmlAnyElement("RecordGenTimeComment")]
        public XmlComment RecordGenTimeComment { get { return new XmlDocument().CreateComment("Sets how long should i2ME wait before grabbing data."); } set { } }
        [XmlElement] public int RecordGenTimeSeconds { get; set; } = 1800;      // Defaults to 30 minutes
    }

    [XmlRoot("RecordConfig")]
    public class DataEndpointConfig
    {
        [XmlAnyElement("DataEComment")]
        public XmlComment DataEComment { get { return new XmlDocument().CreateComment("This defines which data record to generate and send to the i2. For more information, check out the MistWX-i2ME wiki."); } set { } }
        [XmlElement] public bool CurrentConditions { get; set; } = true;
        [XmlElement] public bool MosquitoActivity { get; set; } = true;
        [XmlElement] public bool DrySkin { get; set; } = true;
        [XmlElement] public bool DailyForecast { get; set; } = true;
        [XmlElement] public bool HourlyForecast { get; set; } = true;
        [XmlElement] public bool AirQuality { get; set; } = false;
        [XmlElement] public bool AchesAndPains { get; set; } = true;
        [XmlElement] public bool Breathing { get; set; } = true;
        [XmlElement] public bool HeatingAndCooling { get; set; } = true;
        [XmlElement] public bool PollenForecast { get; set; } = true;
        [XmlElement] public bool WateringNeeds { get; set; } = true;
        [XmlElement] public bool TideForecast { get; set; } = true;
        [XmlElement] public bool PollenObservations { get; set; } = true;
        [XmlElement] public bool TropicalAdvisory { get; set; } = true;
        [XmlElement] public bool ClimatologyRecord { get; set; } = true;
        [XmlElement] public bool DHRecord { get; set; } = false;
        [XmlElement] public bool HolidayMapping { get; set; } = true;
        [XmlElement] public bool TIRecord { get; set; } = true;
        [XmlElement] public bool AirportDelays { get; set; } = true;
    }
}
