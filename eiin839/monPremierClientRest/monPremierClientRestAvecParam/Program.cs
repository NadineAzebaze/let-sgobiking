using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace monPremierClientRestAvecParam
{
    internal class Program
    {
            public class Contrat
        {
            public string commercial_name { get; set; }
            public string name { get; set; }
            public string country_code { get; set; }
        }

            public class Station
        {
            public string contractName { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public int number { get; set; }
        }

        static async Task Main(string[] args)

        {
            // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
            HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                //args[0] = Console.ReadLine();
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v3/stations?contract=" + args[0] + "&apiKey=847fe2621dfc49202c3db3e62c32bc308d0af982" );
                Console.WriteLine(args[0]);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                // Above three lines can be replaced with new helper method below
                List<Station> stations = JsonSerializer.Deserialize<List<Station>>(responseBody);

                foreach (Station station in stations)
                {
                    Console.WriteLine($"contractName: {station.contractName}");
                    Console.WriteLine($"name: {station.name}");
                    Console.WriteLine($"address: {station.address}");
                    Console.WriteLine($"number: {station.number}");
                }

                Console.ReadLine();


            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Contract:  ", e.Message);

                Console.ReadLine();
            }
        }

    }
}

