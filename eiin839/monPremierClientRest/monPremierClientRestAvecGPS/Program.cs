using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace monPremierClientRestAvecGPS
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
            public Position position { get; set; }
        }

        public  class Position
        {
            public double latitude { get; set; }
            public double longitude { get; set; }

            public Position(double latitude, double longitude)
            {
                this.latitude = latitude;
                this.longitude = longitude;
            }
        }

        public class GPS
        {
            public double getDistance(Position p1, Position p2)
            {
                double distance = 0;
                if (p1 != null && p2 != null)
                {
                    GeoCoordinate g1 = new GeoCoordinate(p1.latitude, p1.longitude);
                    GeoCoordinate g2 = new GeoCoordinate(p2.latitude, p2.longitude);
                    distance = g1.GetDistanceTo(g2);
                }
                

                return distance;
            }
        }

        static async Task Main(string[] args)

        {
            // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
            HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string param = Console.ReadLine();
                string link = "https://api.jcdecaux.com/vls/v3/stations?contract=" + param + "&apiKey=847fe2621dfc49202c3db3e62c32bc308d0af982";
                
                HttpResponseMessage response = await client.GetAsync(link);
             
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(responseBody);


                // Above three lines can be replaced with new helper method below
                List<Station> stations = JsonSerializer.Deserialize<List<Station>>(responseBody);
                Position p1 = new Position(50.0, 47.0);
                GPS gps = new GPS();
                Station closest=new Station();

                foreach (Station station in stations)
                {
                    double distance = -1;
                   
                    double distProvisoire = gps.getDistance(p1,station.position);
                    distance = distProvisoire;
                    if(distProvisoire <= distance || distance == -1)
                    {
                        closest = station;
                        distance = distProvisoire;
                       
                    }

                }
                Console.WriteLine($"contractName: {closest.contractName}");
                Console.WriteLine($"name: {closest.name}");
                Console.WriteLine($"address: {closest.address}");
                Console.WriteLine($"number: {closest.number}");
                Console.WriteLine($"Position: { closest.position}");

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
