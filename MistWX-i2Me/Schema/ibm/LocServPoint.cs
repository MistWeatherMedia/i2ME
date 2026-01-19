namespace MistWX_i2Me.Schema.ibm;

public class LocServPointLocale
{
    public string? locale1 { get; set; }

    public string? locale2 { get; set; }

    public string? locale3 { get; set; }

    public string? locale4 { get; set; }
}

public class LocServPointLocation
{
    public double latitude { get; set; }

    public double longitude { get; set; }

    public string? city { get; set; }

    public LocServPointLocale? locale { get; set; }

    public string? neighborhood { get; set; }

    public string? adminDistrict { get; set; }

    public string? adminDistrictCode { get; set; }

    public string? postalCode { get; set; }

    public string? postalKey { get; set; }

    public string? country { get; set; }

    public string? countryCode { get; set; }

    public string? ianaTimeZone { get; set; }

    public string? displayName { get; set; }

    public string? dstEnd { get; set; }

    public string? dstStart { get; set; }

    public string? dmaCd { get; set; }

    public string? placeId { get; set; }

    public bool? disputedArea { get; set; }

    public List<string>? disputedCountries { get; set; }

    public List<string>? disputedCountryCodes { get; set; }

    public List<object>? disputedCustomers { get; set; }

    public List<bool>? disputedShowCountry { get; set; }

    public string? canonicalCityId { get; set; }

    public string? countyId { get; set; }

    public string? locId { get; set; }

    public string? locationCategory { get; set; }

    public string? pollenId { get; set; }

    public string? pwsId { get; set; }

    public string? regionalSatellite { get; set; }

    public string? tideId { get; set; }

    public string? type { get; set; }

    public string? zoneId { get; set; }
}

public class LocServPointResponse
{
    public LocServPointLocation? location { get; set; }
}