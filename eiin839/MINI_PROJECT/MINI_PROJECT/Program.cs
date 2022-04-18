using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MINI_PROJECT
{
    public class Abonnes
    { 
        public string name { get; set; }
        public string duree { get; set; }
    }



    internal class Program
    {
        static async Task Main(string[] args)
        {
            // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
            HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                //args[0] = Console.ReadLine();
                HttpResponseMessage response = await client.GetAsync("htttps://nom" + args[0] + "duree" + args[1]+" ");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                List<Abonnes> abonnements = JsonSerializer.Deserialize<List<Abonnes>>(responseBody);

                foreach (Abonnes a in abonnements)
                {
                    if (a.duree == args[1])
                    {
                        Console.WriteLine($"name: {a.name}");
                    }
                }

                Console.ReadLine();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("ERROR ERROR ERROR");

                Console.ReadLine();
            }


        }
    }
}
