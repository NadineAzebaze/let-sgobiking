using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.Device.Location;
using System.Threading.Tasks;

namespace ServeurRouting
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IRouting
    {

        /* [OperationContract]
         [WebInvoke(Method = "GET", UriTemplate = "Add?x={a}&y={b}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Json)]

         Station ComputeClosestStation(Position position);

         [OperationContract]
         [WebInvoke(Method = "GET", UriTemplate = "Add?x={a}&y={b}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Json)]

         Boolean ComputeAvailibility(Station station);

         [OperationContract]
         [WebInvoke(Method = "GET", UriTemplate = "Add?x={a}&y={b}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Json)]

         void StoreItinerary(Itinerary itinerary);*/

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ComputeItinerary?origin={origin}&destination={destination}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Json)]
        Task<string> ComputeItinerary(string origin, string destination);



        // TODO: ajoutez vos opérations de service ici
    }

    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    // Vous pouvez ajouter des fichiers XSD au projet. Une fois le projet généré, vous pouvez utiliser directement les types de données qui y sont définis, avec l'espace de noms "ServeurRouting.ContractType".


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Position
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

        internal double getDistance(Position p, ServiceReference1.Position position)
        {
            double distance = 0;
            if (p != null && position != null)
            {
                GeoCoordinate g1 = new GeoCoordinate(p.latitude, p.longitude);

                GeoCoordinate g2 = new GeoCoordinate(position.latitude, position.longitude);
                distance = g1.GetDistanceTo(g2);
            }


            return distance;
        }
    }


    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }
   
    public class Properties
    {
        public string label { get; set; }
        public double score { get; set; }
        public string housenumber { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string postcode { get; set; }
        public string citycode { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public string city { get; set; }
        public string context { get; set; }
        public double importance { get; set; }
        public string street { get; set; }
    }

    public class PropertiesOpenRoute
    {
        public List<Segment> segments { get; set; }
        public Summary summary { get; set; }
        public List<int> way_points { get; set; }
    }


    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class FeatureOpenRoute
    {
        public List<double> bbox { get; set; }
        public string type { get; set; }
        public PropertiesOpenRoute properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public string version { get; set; }
        public List<Feature> features { get; set; }
        public string attribution { get; set; }
        public string licence { get; set; }
        public string query { get; set; }
        public int limit { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Step
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public int type { get; set; }
        public string instruction { get; set; }
        public string name { get; set; }
        public List<int> way_points { get; set; }
    }

    public class Segment
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public List<Step> steps { get; set; }
    }

    public class Summary
    {
        public double distance { get; set; }
        public double duration { get; set; }
    }
 

    public class Query
    {
        public List<List<double>> coordinates { get; set; }
        public string profile { get; set; }
        public string format { get; set; }
    }

    public class Engine
    {
        public string version { get; set; }
        public DateTime build_date { get; set; }
        public DateTime graph_date { get; set; }
    }

    public class MetadataOpenRoute
    {
        public string attribution { get; set; }
        public string service { get; set; }
        public long timestamp { get; set; }
        public Query query { get; set; }
        public Engine engine { get; set; }
    }

    public class Openroute
    {
        public string type { get; set; }
        public List<FeatureOpenRoute> features { get; set; }
        public List<double> bbox { get; set; }
        public MetadataOpenRoute metadata { get; set; }
    }


}
