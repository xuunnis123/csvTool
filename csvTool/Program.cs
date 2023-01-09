
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
    static async Task Main(string[] args)
    {
        while (true) {
            Console.WriteLine("(1)資料取出(2)資料儲存");
            string input = Console.ReadLine();

            // 將輸入的文字顯示在控制台上
            Console.WriteLine("input=" + input);
            if (string.IsNullOrEmpty(input)) { break; }
            else if (input.Equals("1"))
            {
                
                // GET
                using (var client = new HttpClient())
                {

                    //string url = "http://localhost:54586/getAllVisitors";
                    string url = "https://csvapi.azurewebsites.net/getAllVisitors";

                    var response = Task.Run(() => client.GetAsync(url)).Result;
                    
                    if (response.IsSuccessStatusCode)
                    {

                        
                        string content = await response.Content.ReadAsStringAsync();
                        

                        Console.WriteLine("GET Successfully!");
                        Console.WriteLine(content);
                    }
                    else
                    {
                        // API 呼叫失敗
                        Console.WriteLine("GET Failed,Error Code：" + response.StatusCode);

                        
                    }
                }
            }
            else
            {
                //DELETE
                using (var client = new HttpClient())
                {
                    //string url = "http://localhost:54586/deleteAllVisitors";
                    string url = "https://csvapi.azurewebsites.net/deleteAllVisitors";
                    // 發出 HTTP DELETE 請求

                    var deleteResponse = Task.Run(() => client.DeleteAsync(url)).Result;
                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine("DELETE Successfully!");

                        //POST


                        var readConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HasHeaderRecord = true
                        };


                        using (var reader = new StreamReader("../../../0108_visitors.csv"))
                        using (var csv = new CsvReader(reader, readConfiguration))
                        {


                            var records = csv.GetRecords<Data>();
                            int i = 1;

                            foreach (var data in records)
                            {

                                using (var postClient = new HttpClient())
                                {
                                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                                    //string postUrl = "http://localhost:54586/refreshVisitor";
                                    string postUrl = "https://csvapi.azurewebsites.net/refreshVisitor";
                                    var content = new StringContent(JsonConvert.SerializeObject(new
                                    {
                                        id = i,
                                        visitor_phone = data.Phone,
                                        visitor_name = data.Name
                                    }), Encoding.UTF8, "application/json");
                                   
                                    var postResponse = Task.Run(() => postClient.PostAsync(postUrl, content)).Result;

                                    if (postResponse.IsSuccessStatusCode)
                                    {
                                        Console.WriteLine( i + "," + data.Phone + "," + data.Name + " POST Successfully!");
                                    }
                                    else
                                    {
                                        // API 呼叫失敗
                                        Console.WriteLine("POST failed,Error Code:" + postResponse.StatusCode);
                                        Console.WriteLine("Error at data:" + i + "," + data.Phone + "," + data.Name);

                                       
                                    }
                                }

                                i++;

                            }


                        }


                    }
                    else
                    {
                        Console.WriteLine("DELETE Failed,Error Code:" + deleteResponse.StatusCode);
                    }
                }



            }
        }
        
        
        
    }
}
