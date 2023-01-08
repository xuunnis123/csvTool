
using CsvHelper;
using CsvHelper.Configuration;
using csvTool;
using System.Globalization;

using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

public class Data
{

   
    public int Phone { get; set; }
    public string? Name { get; set; }

}
class Program {
    //static void Main(string[] args)
    static void Main(string[] args)
    {
        var readConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };

        //get
        using (var client = new HttpClient())
        {
            //string url = $"http://localhost:54586/refreshVisitor?id={i}&visitor_phone={data.Phone}&visitor_name={data.Name}";
            string url = "http://localhost:54586/getAllVisitors";
            var response = Task.Run(() => client.GetAsync(url)).Result;
            //var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully=" + response.Content);
            }
            else
            {
           
                Console.WriteLine("Error=" + response);

                // API 呼叫失敗，可以在這裡處理錯誤
            }
        }


        using (var reader = new StreamReader("../../../0108_visitors.csv"))
        using (var csv = new CsvReader(reader, readConfiguration))
        {


            var records = csv.GetRecords<Data>();
            int i = 1;

            foreach (var data in records)
            {

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    string url = "http://localhost:54586/refreshVisitor";
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        id = i,
                        visitor_phone = data.Phone,
                        visitor_name = data.Name
                    }), Encoding.UTF8, "application/json");
                    //var response = await client.PostAsync(url, content);
                    var response = Task.Run(() => client.PostAsync(url,content)).Result;
                    //var response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Successfully" + i + "_" + data.Phone + "_" + data.Name);
                    }
                    else
                    {
                        Console.WriteLine("response ="+ response);
                        Console.WriteLine("Error" + i + "_" + data.Phone + "_" + data.Name);

                        // API 呼叫失敗，可以在這裡處理錯誤
                    }
                }

                i++;
                //var records = csv.GetRecords<Data>();
                //int i = 1;
                //foreach (var data in records)
                //{
                //    var client = new HttpClient();
                //    Console.WriteLine( i+ "," +data.Phone+","+ data.Name);
                //    i++;
                //}
            }


        }
    }
}
