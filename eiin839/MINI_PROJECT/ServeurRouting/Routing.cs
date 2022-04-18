using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ServeurRouting.ServiceReference1;

namespace ServeurRouting
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Routing : IRouting
    {
       Service1Client cache = new Service1Client() ;
       List<Station> stations = new List<Station>();
        public Routing()
        {
            stations = cache.GetListStation();
        }
        public async Task<List<double>> ComputePositionAsync(string adress)
        {
            HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string link = "https://api-adresse.data.gouv.fr/search/?q=" + adress;
                HttpResponseMessage response = await client.GetAsync(link);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);


                // Above three lines can be replaced with new helper method below
                Root root = JsonConvert.DeserializeObject<Root>(responseBody);

                return root.features[0].geometry.coordinates;
            }
            catch (Exception)
            {
                Console.WriteLine("error error");
                return null;
            }
        }

        public async Task<string> ComputeItinerary(string origin, string destination)
        {
            double latitudeOrigin, longitudeOrigin;
            double latitudeDestination, longitudeDestination;

            List<double> coordinatesOrigin;
            List<double> coordinatesDestination;

            coordinatesOrigin = await ComputePositionAsync(origin);
            coordinatesDestination = await ComputePositionAsync(destination);

            latitudeOrigin = coordinatesOrigin[0];
            longitudeOrigin = coordinatesOrigin[1];
            latitudeDestination = coordinatesDestination[0];
            longitudeDestination = coordinatesDestination[1];

            Station closestToOrigin = ComputeAvailableOneOrigin(latitudeOrigin, longitudeOrigin);
            Station closestToDestination = ComputeAvailableOneDestination(latitudeDestination, longitudeDestination);

            //Openroute firstway = this.ComputingRouteAsync(latitudeOrigin, longitudeOrigin,closestToOrigin.position.latitude,closestToOrigin.position.longitude,"foot-walking").Result;
            //Openroute middleway = this.ComputingRouteAsync(closestToOrigin.position.latitude, closestToOrigin.position.longitude, closestToDestination.position.latitude, closestToDestination.position.longitude, "cycling-electric").Result;
            //Openroute endway = this.ComputingRouteAsync(closestToDestination.position.latitude, closestToDestination.position.longitude, latitudeDestination,longitudeDestination, "foot-walking").Result;
            

            return "yes";
        }

        public Station ComputeAvailableOneOrigin(double latitude, double longitude)
        {

            // Call asynchronous network methods in a try/catch block to handle exceptions.

            Position p = new Position(latitude, longitude);
            GPS gps = new GPS();
            Station closest = new Station();
            closest = this.ComputeClosestStation(this.stations, p, gps);
            Station available = new Station();
            if (closest.totalStands.availabilities.bikes > 1)
            {
                available = closest;
            }
            else
            {
                stations.Remove(closest);
                closest = this.ComputeAvailableOneOrigin(latitude, longitude);
            }
            return available;
                
        }

        public Station ComputeAvailableOneDestination(double latitude, double longitude)
        {

            // Call asynchronous network methods in a try/catch block to handle exceptions.

            Position p = new Position(latitude, longitude);
            GPS gps = new GPS();
            Station closest = new Station();
            closest = this.ComputeClosestStation(stations, p, gps);
            Station available = new Station();
            if (closest.totalStands.availabilities.stands > 0)
            {
                available = closest;
            }
            else
            {
                stations.Remove(closest);
                closest = this.ComputeAvailableOneDestination(latitude, longitude);
            }
            return available;

        }

        public Station ComputeClosestStation(List<Station> stations,Position p,GPS gps)
        {
        
                Station closest = new Station();
                List<double> vs = new List<double>();
                foreach (Station station in stations)
                {
                    vs.Add(gps.getDistance(p, station.position));
                }
                double min = vs.Min();

                closest = stations[vs.IndexOf(min)];

                return closest;
        }

    public async Task<Openroute> ComputingRouteAsync(double startLongitude, double startLatitude, double endLongitude, double endLatitude, string goingby)
    {
            Openroute route = new Openroute();
            var baseAddress = new Uri("https://api.openrouteservice.org/v2/directions/"+goingby
                +"?api_key=5b3ce3597851110001cf6248feed7c380d514571ae25d19073013485&start="
                +startLatitude.ToString()+","+startLongitude.ToString()
                +"&end="+endLatitude.ToString()+","+endLongitude.ToString());

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");

                using (var response = await httpClient.GetAsync("directions"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                   route = JsonConvert.DeserializeObject<Openroute>(responseData);
                    
                }
            }

            return route;
        }

       /* string IRouting.ComputeItinerary(string origin, string destination)
        {
            throw new NotImplementedException();
        }*/
    }
}




