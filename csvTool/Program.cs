// See https://aka.ms/new-console-template for more information
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public class Data
{

    public int Id { get; set; }
    public string? Name { get; set; }

}
class Program {
    static void Main(string[] args)
    {
        var readConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };

        using (var reader = new StreamReader("./name.csv"))
        using (var csv = new CsvReader(reader, readConfiguration))
        {
            var records = csv.GetRecords<Data>();
            foreach (var data in records)
            {
                Console.WriteLine(data.Id + "," + data.Name);
            }
        }
    }
}