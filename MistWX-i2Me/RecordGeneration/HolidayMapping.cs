using System.Xml;
using System.Xml.Serialization;
using System.Globalization;
using MistWX_i2Me.API;
using MistWX_i2Me.Schema.ibm;
using MistWX_i2Me.Schema.twc;

namespace MistWX_i2Me.RecordGeneration;

public class HolidayMapping : I2Record
{
    public async Task<string> MakeRecord(HolidayMappingResponse result)
    {
        Log.Info("Creating Mapping.");
        string recordPath = Path.Combine(AppContext.BaseDirectory, "temp", "Mapping.xml");
        string recordScript = "<Data type=\"Mapping\">";

        CultureInfo provider = CultureInfo.InvariantCulture;

        if (result.Holidays != null)
        {
            foreach (var holiday in result.Holidays)
            {
                DateTime date = DateTime.ParseExact(holiday.Date ?? "20260119", "yyyyMMdd", provider);
                DateTime dateNew = new(DateTime.Now.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                DateTime now = DateTime.Now;

                Log.Debug($"It is currently {now.ToString()}");

                if (now > dateNew)
                {
                    Log.Debug($"Holiday {holiday.Name} has already past,");
                    DateTime newDate = new(dateNew.Year + 1, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                    Log.Debug($"new date is {newDate.ToString()}");
                    holiday.Date = newDate.ToString("yyyyMMdd");
                    holiday.DateFormatted = newDate.ToString("MM/dd/yyyy");
                    continue;
                }

                holiday.Date = dateNew.ToString("yyyyMMdd");
                holiday.DateFormatted = dateNew.ToString("MM/dd/yyyy");
            }
            // Sort by how recent the event was
            result.Holidays = result.Holidays.OrderBy(a => DateTime.ParseExact(a.Date, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MMddyyyy")).ToList();
        }
        

        XmlSerializer serializer = new XmlSerializer(typeof(HolidayMappingResponse));
        StringWriter sw = new StringWriter();
        XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            ConformanceLevel = ConformanceLevel.Fragment, 
        });
        xw.WriteWhitespace("");
        serializer.Serialize(xw, result);
        sw.Close();

        recordScript += sw.ToString();
        
        
        recordScript += "</Data>";
        
        await File.WriteAllTextAsync(recordPath, ValidateXml(recordScript));

        return recordPath;
    }
}