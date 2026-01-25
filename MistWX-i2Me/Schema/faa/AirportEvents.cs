using System.Dynamic;

namespace MistWX_i2Me.Schema.faa;

public class ArrivalDelay
{
    public string? impactingCondition { get; set; }

    public string? averageDelay { get; set; }
}

public class GroundStop
{
    public string? impactingCondition { get; set; }
}

public class GroundDelay
{
    public string? impactingCondition { get; set; }

    public float avgDelay { get; set; }
}

public class FreeForm
{
    public string? text { get; set; }
}

public class AirportEvent
{
    public string? airportId { get; set; }
    public ArrivalDelay? arrivalDelay { get; set; }
    public ArrivalDelay? departureDelay { get; set; }
    public GroundStop? groundStop { get; set; }
    public GroundDelay? groundDelay { get; set; }
    public FreeForm? freeForm { get; set; }
    public object? airportClosure { get; set; }
    public string? airportLongName { get; set; }
}
public class AirportEventsResponse
{
    public List<AirportEvent>? airportEvents { get; set; }
}