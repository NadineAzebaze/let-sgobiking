using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace monPremierClientRest
{
    internal class Program

    {
        public class Contrat{
            public string contractName { get; set; }
            public string name { get; set; }
            public string address { get; set; }
        }

        static async Task Main()

        {
            // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
             HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v3/contracts?apiKey=847fe2621dfc49202c3db3e62c32bc308d0af982");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                // Above three lines can be replaced with new helper method below
                List<Contrat> contrats = JsonSerializer.Deserialize<List<Contrat>>(responseBody);

                foreach (Contrat contrat in contrats)
                {
                    Console.WriteLine($"ContractName: {contrat.contractName}");
                    Console.WriteLine($"name: {contrat.name}");
                    Console.WriteLine($"address: {contrat.address}");
                }

                Console.ReadLine();
                
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message {0} :  ", e.Message);

                Console.ReadLine();
            }

        }
        
    }
    

}