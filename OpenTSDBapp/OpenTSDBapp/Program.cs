using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;




namespace OpenTSDBapp
{
    class Timeseriesclass
    {
        public string Tag { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
        public int Status { get; set; }
        public DateTime EventProcessedUTCtime { get; set; }
        public int PartitionId { get; set; }
        public DateTime EventEnqueuedUtcTime { get; set; }
    }
    class Program
    {
        static HttpClient client = new HttpClient();

      
        static void Main(string[] args)
        {
            LoadJson();
            
            Console.ReadLine();
        }
        public static void LoadJson()
        {
            Timeseriesclass tsc = new Timeseriesclass();
            string sFullFilePath = @"C:\TS\file3.json";
            using (StreamReader r = new StreamReader(sFullFilePath))
            {
                string jsonfile = r.ReadToEnd();
                List<Timeseriesclass> series= JsonConvert.DeserializeObject<List<Timeseriesclass>>(jsonfile);
                RunAsync(series).GetAwaiter().GetResult();
                //foreach(var item in items)
                //{
                //    Console.WriteLine("{0} - {1} - {2}", item.,;
                //}

            }
        }
        static async Task<Uri> CreateRecords(Timeseriesclass tsc)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/put", tsc);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
        static async Task RunAsync(List<Timeseriesclass> tsclassseries)
        {
            client.BaseAddress = new Uri("https://opentsdbcluster1-tdb.apps.azurehdinsight.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (Timeseriesclass ts in tsclassseries)
            {
                var newrecord = await CreateRecords(ts);
            }
            



        }
    }
}
