namespace MistWX_i2Me.Schema.ibm;

public class LocServNearObsLocation
{
    public List<string?> adminDistrictCode { get; set; }

    public List<string?> stationName { get; set; }

    public List<string?> countryCode { get; set; }

    public List<string?> stationId { get; set; }

    public List<string?> ianaTimeZone { get; set; }

    public List<string?> obsType { get; set; }

    public List<double> latitude { get; set; }

    public List<double> longitude { get; set; }

    public List<double?> distanceKm { get; set; }

    public List<double?> distanceMi { get; set; }
}

public class LocServNearObsResponse
{
    public LocServNearObsLocation location { get; set; }
}