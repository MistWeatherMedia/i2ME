namespace MistWX_i2Me.Schema.ibm;

public class Almanac1DayResponse
{
    public List<string> almanacInterval { get; set; }
    public List<string> almanacRecordDate { get; set; }
    public List<int?> almanacRecordPeriod { get; set; }
    public List<int?> almanacRecordYearMax { get; set; }
    public List<int?> almanacRecordYearMin { get; set; }
    public List<double?> precipitationAverage { get; set; }
    public List<double?> snowAccumulationAverage { get; set; }
    public List<string> stationId { get; set; }
    public List<string?> stationName { get; set; }
    public List<int?> temperatureAverageMax { get; set; }
    public List<int?> temperatureAverageMin { get; set; }
    public List<int?> temperatureAverageMean { get; set; }
    public List<int?> temperatureRecordMax { get; set; }
    public List<int?> temperatureRecordMin { get; set; }
}