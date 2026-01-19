namespace MistWX_i2Me.Schema.ibm;

public class AlertDetail
{
    public string? detailKey { get; set; }
    public int messageTypeCode { get; set; }
    public string? messageType { get; set; }
    public string? productIdentifier { get; set; }
    public string? phenomena { get; set; }
    public string? significance { get; set; }
    public string? eventTrackingNumber { get; set; }
    public string? officeCode { get; set; }
    public string? officeName { get; set; }
    public string? officeAdminDistrict { get; set; }
    public string? officeAdminDistrictCode { get; set; }
    public string? officeCountryCode { get; set; }
    public string? eventDescription { get; set; }
    public int severityCode { get; set; }
    public string? severity { get; set; }
    public List<Category>? categories { get; set; }
    public List<ResponseType>? responseTypes { get; set; }
    public string? urgency { get; set; }
    public string? urgencyCode { get; set; }
    public string? certainty { get; set; }
    public string? certaintyCode { get; set; }
    public DateTime? effectiveTimeLocal { get; set; }
    public string? effectiveTimeLocalTimeZone { get; set; }
    public DateTime? expireTimeLocal { get; set; }
    public string? expireTimeLocalTimeZone { get; set; }
    public long expireTimeUTC { get; set; }
    public DateTime? onsetTimeLocal { get; set; }
    public string? onsetTimeLocalTimeZone { get; set; }
    public Flood? flood { get; set; }
    public string? areaTypeCode { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public string? areaId { get; set; }
    public string? areaName { get; set; }
    public string? ianaTimeZone { get; set; }
    public string? adminDistrictCode { get; set; }
    public string? adminDistrict { get; set; }
    public string? countryCode { get; set; }
    public string? countryName { get; set; }
    public string? headlineText { get; set; }
    public string? source { get; set; }
    public object? disclaimer { get; set; }
    public string? issueTimeLocal { get; set; }
    public string? issueTimeLocalTimeZone { get; set; }
    public string? identifier { get; set; }
    public long processTimeUTC { get; set; }
    public DateTime? endTimeLocal { get; set; }
    public string? endTimeLocalTimeZone { get; set; }
    public long endTimeUTC { get; set; }
    public List<Text>? texts { get; set; }
    public List<Polygon>? polygon { get; set; }
    public string? synopsis { get; set; }
    public Dictionary<string, string>? supplement { get; set; }
}

public class Category
{
    public string? category { get; set; }
    public int? categoryCode { get; set; }
}

public class Flood
{
    public string? floodLocationId { get; set; }
    public string? floodLocationName { get; set; }
    public string? floodSeverityCode { get; set; }
    public string? floodSeverity { get; set; }
    public string? floodImmediateCauseCode { get; set; }
    public string? floodImmediateCause { get; set; }
    public string? floodRecordStatusCode { get; set; }
    public string? floodRecordStatus { get; set; }
    public string? floodStartTimeLocal { get; set; }
    public string? floodStartTimeLocalTimeZone { get; set; }
    public string? floodCrestTimeLocal { get; set; }
    public string? floodCrestTimeLocalTimeZone { get; set; }
    public string? floodEndTimeLocal { get; set; }
    public string? floodEndTimeLocalTimeZone { get; set; }
}

public class Polygon
{
    public double? lat { get; set; }
    public double? lon { get; set; }
}

public class ResponseType
{
    public string? responseType { get; set; }
    public int? responseTypeCode { get; set; }
}

public class AlertDetailResponse
{
    public AlertDetail? alertDetail { get; set; }
}

public class Text
{
    public string? languageCode { get; set; }
    public string? description { get; set; }
    public string? instruction { get; set; }
    public string? overview { get; set; }
}