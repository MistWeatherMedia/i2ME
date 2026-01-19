using System.Xml.Serialization;

namespace MistWX_i2Me.Schema.ibm;

[XmlRoot(ElementName="metadata")]
public class TropicalAdvisoryMetadata { 

	[XmlElement(ElementName="language")] 
	public string? Language { get; set; } 

	[XmlElement(ElementName="transactionId")] 
	public string? TransactionId { get; set; } 

	[XmlElement(ElementName="version")] 
	public int Version { get; set; } 

	[XmlElement(ElementName="nautical")] 
	public bool Nautical { get; set; } 

	[XmlElement(ElementName="format")] 
	public string? Format { get; set; }

	[XmlElement(ElementName="units")] 
	public string? Units { get; set; }  

	[XmlElement(ElementName="basin")] 
	public string? Basin { get; set; } 

	[XmlElement(ElementName="source")] 
	public string? Source { get; set; }

	[XmlElement(ElementName="expireTimeGmt")] 
	public int ExpireTimeGmt { get; set; } 

	[XmlElement(ElementName="statusCode")] 
	public int StatusCode { get; set; } 
}

[XmlRoot(ElementName="headline")]
public class TropicalAdvisoryHeadline { 

	[XmlElement(ElementName="item")] 
	public List<string>? Item { get; set; } 
}

[XmlRoot(ElementName="heading")]
public class TropicalAdvisoryHeading { 

	[XmlElement(ElementName="storm_dir")] 
	public int StormDir { get; set; } 

	[XmlElement(ElementName="storm_dir_cardinal")] 
	public string? StormDirCardinal { get; set; } 

	[XmlElement(ElementName="storm_spd")] 
	public int StormSpd { get; set; } 
}

[XmlRoot(ElementName="nearby_loc")]
public class TropicalAdvisoryNearbyLoc { 

	[XmlElement(ElementName="loc_nm")] 
	public string? LocNm { get; set; } 

	[XmlElement(ElementName="dist")] 
	public int Dist { get; set; } 

	[XmlElement(ElementName="dir_cardinal")] 
	public string? DirCardinal { get; set; } 

	[XmlElement(ElementName="city_nm")] 
	public string? CityNm { get; set; } 

	[XmlElement(ElementName="st_cd")] 
	public string? StateCd { get; set; } 
}

[XmlRoot(ElementName="wind_radii")]
public class TropicalAdvisoryWindRadii { 

	[XmlElement(ElementName="radii_wspd")] 
	public int RadiiWspd { get; set; }  

	[XmlElement(ElementName="NE")] 
	public int NE { get; set; } 

	[XmlElement(ElementName="SE")] 
	public int SE { get; set; } 

	[XmlElement(ElementName="SW")] 
	public int SW { get; set; } 

	[XmlElement(ElementName="NW")] 
	public int NW { get; set; } 
}

[XmlRoot(ElementName="sea_radii")]
public class TropicalAdvisorySeaRadii { 

	[XmlElement(ElementName="wave_height")] 
	public int RadiiWspd { get; set; }  

	[XmlElement(ElementName="NE")] 
	public int NE { get; set; } 

	[XmlElement(ElementName="SE")] 
	public int SE { get; set; } 

	[XmlElement(ElementName="SW")] 
	public int SW { get; set; } 

	[XmlElement(ElementName="NW")] 
	public int NW { get; set; } 

	[XmlElement(ElementName="dist")] 
	public int Dist { get; set; }
}

[XmlRoot(ElementName="currentposition")]
public class TropicalAdvisoryCurrentPosition { 

	[XmlElement(ElementName="lat")] 
	public double Latitude { get; set; } 

	[XmlElement(ElementName="lat_hemisphere")] 
	public string? LatitudeHemisphere { get; set; } 

	[XmlElement(ElementName="lon")] 
	public double Longitude { get; set; } 

	[XmlElement(ElementName="lon_hemisphere")] 
	public string? LongitudeHemisphere { get; set; }

	[XmlElement(ElementName="storm_type_cd")] 
	public string? StormTypeCd { get; set; }

	[XmlElement(ElementName="storm_type")] 
	public string? StormType { get; set; }

	[XmlElement(ElementName="headline")] 
	public TropicalAdvisoryHeadline? Headline { get; set; }

	[XmlElement(ElementName="min_pressure")] 
	public double MinPressure { get; set; }

	[XmlElement(ElementName="max_sustained_wind")] 
	public int MaxSustainedWind { get; set; }

	[XmlElement(ElementName="wind_gust")] 
	public int WindGust { get; set; }

	[XmlElement(ElementName="heading")] 
	public TropicalAdvisoryHeading? Heading { get; set; }

	[XmlElement(ElementName="nearby_loc")] 
	public TropicalAdvisoryNearbyLoc? NearbyLoc { get; set; }

	[XmlElement(ElementName="wind_radii")] 
	public List<TropicalAdvisoryWindRadii>? WindRadii { get; set; }

	[XmlElement(ElementName="sea_radii")] 
	public List<TropicalAdvisorySeaRadii>? SeaRadii { get; set; }
}

[XmlRoot(ElementName="advisoryinfo")]
public class TropicalAdvisoryInfo { 

	[XmlElement(ElementName="storm_key")] 
	public string? StormKey { get; set; } 

	[XmlElement(ElementName="storm_id")] 
	public string? StormID { get; set; } 

	[XmlElement(ElementName="storm_number")] 
	public int StormNumber { get; set; } 

	[XmlElement(ElementName="storm_name")] 
	public string? StormName { get; set; } 

	[XmlElement(ElementName="alternative_storm_name")] 
	public string? AltStormID { get; set; } 

	[XmlElement(ElementName="source")] 
	public string? Source { get; set; }

	[XmlElement(ElementName="dsgnt_source")] 
	public bool DsgntSource { get; set; }  

	[XmlElement(ElementName="basin")] 
	public string? Basin { get; set; } 

	[XmlElement(ElementName="issue_office")] 
	public string? IssueOffice { get; set; } 

	[XmlElement(ElementName="wmo_id")] 
	public string? WMOID { get; set; } 

	[XmlElement(ElementName="pil")] 
	public string? PIL { get; set; } 

	[XmlElement(ElementName="bulletin_id")] 
	public string? BulletinID { get; set; } 

	[XmlElement(ElementName="related_bulletin_id")] 
	public string? RelBulletinID { get; set; } 

	[XmlElement(ElementName="issue_dt_tm")] 
	public string? IssueDtTm { get; set; } 

	[XmlElement(ElementName="issue_dt_tm_tz_cd")] 
	public string? IssueDtTmTzCd { get; set; } 

	[XmlElement(ElementName="issue_dt_tm_tz_abbrv")] 
	public string? IssueDtTmTzAbbrv { get; set; } 

	[XmlElement(ElementName="adv_num")] 
	public int AdvNum { get; set; } 

	[XmlElement(ElementName="adv_dt_tm")] 
	public string? AdvDtTm { get; set; } 

	[XmlElement(ElementName="adv_dt_tm_tz_cd")] 
	public string? AdvDtTmTzCd { get; set; } 

	[XmlElement(ElementName="adv_dt_tm_tz_abbrv")] 
	public string? AdvDtTmTzAbbrv { get; set; } 

	[XmlElement(ElementName="nxt_cmplt_advsry_dt_tm")] 
	public string? NxtCmpltAdvsryDtTm { get; set; } 

	[XmlElement(ElementName="nxt_cmplt_advsry_tz_cd")] 
	public string? NxtCmpltAdvsryDtTmTzCd { get; set; } 

	[XmlElement(ElementName="nxt_cmplt_advsry_tz_abbrv")] 
	public string? NxtCmpltAdvsryDtTmTzAbbrv { get; set; } 

	[XmlElement(ElementName="nxt_intrmdt_advsry_dt_tm")] 
	public string? NxtIntrmdtAdvsryDtTm { get; set; } 

	[XmlElement(ElementName="nxt_intrmdt_advsry_tz_cd")] 
	public string? NxtIntrmdtAdvsryDtTmTzCd { get; set; } 

	[XmlElement(ElementName="nxt_intrmdt_advsry_tz_abbrv")] 
	public string? NxtIntrmdtAdvsryDtTmTzAbbrv { get; set; } 

	[XmlElement(ElementName="process_time_gmt")] 
	public int ProcessTimeGmt { get; set; } 

	[XmlElement(ElementName="expire_time_gmt")] 
	public int ExpireTimeGmt { get; set; } 

	[XmlElement(ElementName="final_advisory")] 
	public bool FinalAdvisory { get; set; } 

	[XmlElement(ElementName="alternate_final_advisory")] 
	public bool AltFinalAdvisory { get; set; } 

	[XmlElement(ElementName="currentposition")] 
	public TropicalAdvisoryCurrentPosition? CurrentPosition { get; set; } 
}

[XmlRoot(ElementName="response")]
public class TropicalAdvisoryResponse { 

	[XmlElement(ElementName="metadata")] 
	public TropicalAdvisoryMetadata? Metadata { get; set; } 

	[XmlElement(ElementName="advisoryinfo")] 
	public List<TropicalAdvisoryInfo>? AdvisoryInfo { get; set; }
 
}