namespace MistWX_i2Me.Schema.ibm;


public class Alert
{
    public string? detailKey { get; set; }
    public int? messageTypeCode { get; set; }
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
    public int? severityCode { get; set; }
    public string? severity { get; set; }
    public List<HeadlineCategory>? categories { get; set; }
    public List<HeadlineResponseType>? responseTypes { get; set; }
    public string? urgency { get; set; }
    public int? urgencyCode { get; set; }
    public string? certainty { get; set; }
    public int? certaintyCode { get; set; }
    public object? effectiveTimeLocal { get; set; }
    public object? effectiveTimeLocalTimeZone { get; set; }
    public DateTime? expireTimeLocal { get; set; }
    public string? expireTimeLocalTimeZone { get; set; }
    public int? expireTimeUTC { get; set; }
    public object? onsetTimeLocal { get; set; }
    public object? onsetTimeLocalTimeZone { get; set; }
    public object? flood { get; set; }
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
    public DateTime? issueTimeLocal { get; set; }
    public string? issueTimeLocalTimeZone { get; set; }
    public string? identifier { get; set; }
    public int? processTimeUTC { get; set; }
    public DateTime? endTimeLocal { get; set; }
    public string? endTimeLocalTimeZone { get; set; }
    public int? endTimeUTC { get; set; }
}

public class HeadlineCategory
{
    public string? category { get; set; }
    public int? categoryCode { get; set; }
}

public class HeadlineMetadata
{
    public object? next { get; set; }
}

public class HeadlineResponseType
{
    public string? responseType { get; set; }
    public int? responseTypeCode { get; set; }
}

public class HeadlineResponse
{
    public HeadlineMetadata? metadata { get; set; }
    public List<Alert>? alerts { get; set; }
}