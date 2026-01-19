namespace MistWX_i2Me.Schema.ibm;

public class LocServNearSkiLocation
{
    public List<string?>? adminDistrictCode { get; set; }

    public List<string?>? countryCode { get; set; }

    public List<double?>? distanceKm { get; set; }

    public List<double?>? distanceMi { get; set; }

    public List<string?>? ianaTimeZone { get; set; }

    public List<double>? latitude { get; set; }

    public List<double>? longitude { get; set; }

    public List<string?>? skiId { get; set; }

    public List<string?>? skiName { get; set; }
}

public class LocServNearSkiResponse
{
    public LocServNearSkiLocation? location { get; set; }
}