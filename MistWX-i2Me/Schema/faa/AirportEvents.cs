namespace MistWX_i2Me.Schema.faa;

public class AirportEvent
{
    public string almanacInterval { get; set; }
}
public class AirportEventsResponse
{
    public List<AirportEvent> almanacInterval { get; set; }
}