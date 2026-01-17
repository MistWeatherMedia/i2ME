namespace MistWX_i2Me.Schema.ibm;

public class LocServNearAirportLocation
{
    public List<string> latitude { get; set; }

    public List<string> longitude { get; set; }

    public List<string?> distanceKm { get; set; }

    public List<string?> distanceMi { get; set; }

    public List<string?> airportName { get; set; }

    public List<string?> iataCode { get; set; }

    public List<string?> icaoCode { get; set; }
}

public class LocServNearAirportResponse
{
    public LocServNearAirportLocation location { get; set; }
}