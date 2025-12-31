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

        foreach (var holiday in result.Holidays)
        {
            DateTime date = DateTime.ParseExact(holiday.Date, "yyyyMMdd", provider);
            DateTime dateNew = new DateTime(DateTime.Now.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            DateTime now = DateTime.Now;
            if (now > dateNew)
            {
                dateNew.AddYears(1);
            }

            holiday.Date = dateNew.ToString("yyyyMMdd");
            holiday.DateFormatted = dateNew.ToString("MM/dd/yyyy");
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