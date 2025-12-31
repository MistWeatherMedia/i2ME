using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace MistWX_i2Me;

/// <summary>
/// Class for building the config.xml file used to configure the software
/// </summary>
[XmlRoot("Config")]
public class Config
{
    // Config Elements \\
    
    [XmlElement] public string TwcApiKey { get; set; } = "REPLACE_ME";
    [XmlElement] public string LogLevel { get; set; } = "info";
    [XmlElement] public bool GetAlerts { get; set; } = true;



    // Used to process what locations to generate
    [XmlElement]
    public string MachineProductConfig { get; set; } =
        "C:\\Program Files (x86)\\TWC\\i2\\Managed\\Config\\MachineProductCfg.xml";

    [XmlElement] public int RecordGenTimeSeconds { get; set; } = 1800;      // Defaults to 30 minutes
    [XmlElement] public int CheckAlertTimeSeconds { get; set; } = 600;      // Defaults to 10 minutes
    
    [XmlElement("LocationConfig")] public LocConfig LocationConfig { get; set; } = new LocConfig();
    [XmlElement("UnitConfig")] public NetworkConfig UnitConfig { get; set; } = new NetworkConfig();

    [XmlElement("LocalConfig")] public LocalConfig LocalStarConfig { get; set; } = new LocalConfig();
    [XmlElement("RadarConfig")] public RadarConfig RadarConfiguration { get; set; } = new RadarConfig();
    [XmlElement("DataConfig")] public DataEndpointConfig DataConfig { get; set; } = new DataEndpointConfig();

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

        [XmlElement] public bool UseTideLocs { get; set; } = false;
        [XmlElement] public bool UseMapLocs { get; set; } = false;
        [XmlElement] public bool UseTravelLocs { get; set; } = false;
        [XmlElement] public bool UseWinterGLocs { get; set; } = false;
        [XmlElement] public bool UseSummerGLocs { get; set; } = false;
        [XmlElement] public bool UseSubset1Locs { get; set; } = false;
        [XmlElement] public bool UseSubset2Locs { get; set; } = false;


        [XmlElement] public bool UseNationalLocations { get; set; } = false;
        [XmlArray("NationalLocations")] 
        [XmlArrayItem("Location")] 
        public string[] NationalLocations { get; set; } = {
                "USNM0004", "USGA0028", "USMD0018", "USME0017", "USMT0031", "USAL0054", "USND0037", "USID0025",
                "USMA0046", "USNY0081", "USVT0033", "USNC0121", "USIL0225", "USOH0188", "USOH0195", "USTX0327",
                "USCO0105", "USIA0231", "USMI0229", "USAZ0068", "USSC0140", "USCT0094", "USTX0617", "USIN0305",
                "USFL0228", "USMO0460", "USNV0049", "USAR0337", "USCA0638", "USKY1096", "USTN0325", "USFL0316",
                "USWI0455", "USMN0503", "USTN0357", "USLA0338", "USNY0996", "USNJ0355", "USVA0557", "USOK0400",
                "USNE0363", "USFL0372", "USPA1276", "USAZ0166", "USPA1290", "USME0328", "USOR0275", "USNC0558",
                "USSD0283", "USNV0076", "USCA0967", "USUT0225", "USTX1200", "USCA0982", "USCA0987", "USWA0395",
                "USWA0422", "USMO0787", "USFL0481", "USOK0537"
            };

        [XmlArray("Locations")] 
        [XmlArrayItem("Location")] 
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
        [XmlElement] public int RoutineMsgPort { get; set; } = 7787;
        [XmlElement] public int PriorityMsgPort { get; set; } = 7788;
        [XmlElement] public string I2MsgAddress { get; set; } = "224.1.1.77";
        [XmlElement] public string InterfaceAddress { get; set; } = "127.0.0.1";
        [XmlElement] public bool UseExecInstead { get; set; } = true;
    }

    [XmlRoot("RadarConfig")]
    public class RadarConfig
    {
        [XmlElement] public bool UseRadarServer { get; set; } = false;
        [XmlElement] public string RadarServerUrl { get; set; } = "REPLACE_ME";
    }

    [XmlRoot("LocalConfig")]
    public class LocalConfig
    {
        [XmlElement] public string Unit { get; set; } = "e";
    }

    [XmlRoot("DataConfig")]
    public class DataEndpointConfig
    {
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
    }
}
