using Newtonsoft.Json;
using ServeurRouting.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServeurRouting
{
    public class Routing : IRouting
    {
        Service1Client cache = new Service1Client();
        List<Station> stations = null;
        List<Statistiques> statistiques = new List<Statistiques>();
        public Routing()
        {
            stations = cache.GetListStations();
        }
        
        /*
         * Cette méthode effectue le calcul complet de l'itinéraire
         */
        public async Task<List<Openroute>> ComputeItinerary(string origin, string destination)
        {
            double latitudeOrigin, longitudeOrigin;
            double latitudeDestination, longitudeDestination;

            List<double> coordinatesOrigin;
            List<double> coordinatesDestination;

            //Récupération des coordonnées de l'adresse de départ et de l'adresse d'arrivée
            coordinatesOrigin = await ComputePositionAsync(origin);
            coordinatesDestination = await ComputePositionAsync(destination);

            //Décomposition des coordonées
            latitudeOrigin = coordinatesOrigin[1];
            longitudeOrigin = coordinatesOrigin[0];
            latitudeDestination = coordinatesDestination[1];
            longitudeDestination = coordinatesDestination[0];

            //Calcul des stations les plus proches
            Station closestToOrigin = ComputeAvailableOneOrigin(latitudeOrigin, longitudeOrigin);
            Station closestToDestination = ComputeAvailableOneDestination(latitudeDestination, longitudeDestination);

            //Recomposition des coordonnées géographiques de l'origine et de la destination
            Position posOrigin = new Position(latitudeOrigin, longitudeOrigin);
            Position posDestination = new Position(latitudeDestination, longitudeDestination);

            //Vérification de la posiibilité de calculer un itinéraire
            Boolean cantgobyFeet = (this.isPreferable(posOrigin, closestToOrigin.position, posDestination, closestToDestination.position)) && (this.GetStationsAsync(origin).Result != null);
            List<Openroute> result = new List<Openroute>();

            if (cantgobyFeet)
            {
                Openroute firstway = this.ComputingRouteAsync(longitudeOrigin, latitudeOrigin, closestToOrigin.position.longitude, closestToOrigin.position.latitude, "foot-walking").Result;
                //Openroute middleway = this.ComputingRouteAsync(closestToOrigin.position.longitude, closestToOrigin.position.latitude, closestToDestination.position.longitude, closestToDestination.position.latitude, "foot-walking").Result;
                Openroute middleway = this.ComputingRouteAsync(closestToOrigin.position.longitude, closestToOrigin.position.latitude, closestToDestination.position.longitude, closestToDestination.position.latitude, "cycling-road").Result;
                Openroute endway = this.ComputingRouteAsync(closestToDestination.position.longitude, closestToDestination.position.latitude, longitudeDestination, latitudeDestination, "foot-walking").Result;
                result.Add(firstway);
                result.Add(middleway);
                result.Add(endway);
                Console.WriteLine("ok");
                Console.WriteLine(result);
            }
            else
            {
                result = null;
            }
            return result;
        }


        /*
         * Cette méthode vérifie qu'il y'a bien aumoins un vélo dans la station la plus proche de l'origine
         */
        public Station ComputeAvailableOneOrigin(double latitude, double longitude)
        {
            Position p = new Position(latitude, longitude);
            GPS gps = new GPS();
            List<Station> closest = new List<Station>();
            closest = this.ComputeClosestStation(stations, p, gps);
            Station close_station = new Station();
            foreach (Station s in closest)
            {
                if (s.totalStands.availabilities.bikes > 1)
                {
                    close_station = s;
                    break;
                }
            }
            this.updateStats(close_station);
            return close_station;

        }

        /*
         * Cette méthode vérifie qu'il y'a bien des stands disponibles dans la dernière station
         */
        public Station ComputeAvailableOneDestination(double latitude, double longitude)
        {
            Position p = new Position(latitude, longitude);
            GPS gps = new GPS();
            List<Station> closest = new List<Station>();
            closest = this.ComputeClosestStation(stations, p, gps);
            Station close_station = new Station();
            foreach (Station s in closest)
            {
                if (s.totalStands.availabilities.stands > 0)
                {
                    close_station = s;
                    break;
                }
            }
            this.updateStats(close_station);
            return close_station;


        }

        /*
         * Cette méthode permet de déterminer les stations les plus proches d'une position donnée
         * Elle retourne une liste de stations rangées de la plus proche à la plus éloignée
         */
        public List<Station> ComputeClosestStation(List<Station> stations, Position p, GPS gps)
        {

            Station closest = new Station();
            List<double> vs = new List<double>();
            List<Station> the_closest_ones = new List<Station>();
            foreach (Station station in stations)
            {
                vs.Add(gps.getDistance(p, station.position));
            }
            List<double> copy_vs = new List<double>(vs);
            List<double> sorted_vs = new List<double>();

            while (copy_vs.Count > 0)
            {
                double min = copy_vs.Min();
                sorted_vs.Add(min);
                copy_vs.Remove(min);
            }
            foreach (double distance in sorted_vs)
            {
                int index = 0;
                foreach(double dist_vs in vs)
                {
                    if(distance == dist_vs)
                    {
                        index = vs.IndexOf(dist_vs);
                    }
                }
                the_closest_ones.Add(stations[index]);

            }
            return the_closest_ones;
        }


        /*
         * Cette méthode retourne un objet OpenRoute qui est l'itinéraire entre deux points en utilisant leurs coordonnées géographiques
         * */
        public async Task<Openroute> ComputingRouteAsync(double startLongitude, double startLatitude, double endLongitude, double endLatitude, string goingby)
        {
            Openroute route = new Openroute();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");

            var baseAddress = new Uri("https://api.openrouteservice.org/v2/directions/"+goingby+"?start="
               + startLongitude.ToString().Replace(",", ".") + "," + startLatitude.ToString().Replace(",", ".")
               + "&end=" + endLongitude.ToString().Replace(",", ".") + "," + endLatitude.ToString().Replace(",", ".") + "&api_key=5b3ce3597851110001cf6248feed7c380d514571ae25d19073013485");

            try
            {
                HttpResponseMessage response = await client.GetAsync(baseAddress);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                route = JsonConvert.DeserializeObject<Openroute>(responseBody);

                return (Openroute)route;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!" + e.Message);
                return null;
            }
        }

        /*
         * Cette méthode permet de récupérer les coordonnées géographiques d'une adresse
         * */
        public async Task<List<double>> ComputePositionAsync(string adress)
        {
            HttpClient client = new HttpClient();
            try
            {
                string link = "https://api-adresse.data.gouv.fr/search/?q=" + adress;
                HttpResponseMessage response = await client.GetAsync(link);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Root root = JsonConvert.DeserializeObject<Root>(responseBody);

                return root.features[0].geometry.coordinates;
            }
            catch (Exception)
            {
                Console.WriteLine("error error");
                return null;
            }
        }


        /*
         * Cette méthode permet d'extraire la ville d'une adresse
         * */
        public async Task<String> GetCityAsync(string adress)
        {
            HttpClient client = new HttpClient();
            try
            {
                string link = "https://api-adresse.data.gouv.fr/search/?q=" + adress;
                HttpResponseMessage response = await client.GetAsync(link);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Root root = JsonConvert.DeserializeObject<Root>(responseBody);

                return root.features[0].properties.city;
            }
            catch (Exception)
            {
                Console.WriteLine("error error");
                return null;
            }
        }


        /*
         * Cette méthode permet de récupérer la liste des stations d'une ville selon l'adresse qui nous est fournie
         * */
        public async Task<List<Station>> GetStationsAsync(string adress)

        {
            String contratName = this.GetCityAsync(adress).Result;
            HttpClient client = new HttpClient();
            try
            {
                string link = "https://api.jcdecaux.com/vls/v3/stations?contract=" + contratName + "&apiKey=847fe2621dfc49202c3db3e62c32bc308d0af982";
                HttpResponseMessage response = await client.GetAsync(link);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Station> root = JsonConvert.DeserializeObject<List<Station>>(responseBody);

                return root;
            }
            catch (Exception)
            {
                Console.WriteLine("error error");
                return null;
            }
        }

        /*
         * Cette méthode permet d'évaluer si c'est mieux pour l'utilisateur d'aller en vélo ou d'aller autrement à sa destination; en comparant les distances qu'il parcourt avec le vélo et sans le vélo
         * */
        public Boolean isPreferable(Position origin, ServiceReference1.Position station1, Position destination, ServiceReference1.Position station2)
        {
            GPS gps = new GPS();
            double distOriginToStation = gps.getDistance(origin, station1);
            double cycleDistance = gps.getDistanceTo(origin, destination);
            double distDestinationStation = gps.getDistance2(station2, destination);
                if (distOriginToStation < cycleDistance || distDestinationStation < cycleDistance || distDestinationStation+distOriginToStation < cycleDistance)
                {
                    if(cycleDistance < 10000.0) { return true; }
                }
            return false;
        }


        /*
         * Cette méthode retourne le nombre de fois que les stations sont appelées et quelles sont les stations appelées
         * */
        public List<Statistiques> FeedBackStatistiques()
        {
            return this.statistiques;
        }

        /*
         * Cette méthode permet de mettre à jour les stats d'une station
         * */
        public void updateStats(Station s)
        {
            foreach(Statistiques stat in statistiques)
            {
                if(stat.mystation == s)
                {
                    stat.nbOfTimesItIsCalled = stat.nbOfTimesItIsCalled+1;
                    return;
                }
            }
            statistiques.Add(new Statistiques(s));

        }
    }
}




