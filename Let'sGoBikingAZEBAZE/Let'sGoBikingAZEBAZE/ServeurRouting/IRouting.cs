using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.Device.Location;
using System.Threading.Tasks;
using ServeurRouting.ServiceReference1;

namespace ServeurRouting
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IRouting
    {
         [OperationContract]
         [WebInvoke(Method = "GET", UriTemplate = "get", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Json)]
            List<Statistiques> FeedBackStatistiques();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "available?latitude={latitude}&longitude={longitude}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Json)]
        Station ComputeAvailableOneOrigin(double latitude, double longitude);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ComputeItinerary?origin={origin}&destination={destination}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Json)]
        Task<List<Openroute>> ComputeItinerary(string origin, string destination);

    }

    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    // Vous pouvez ajouter des fichiers XSD au projet. Une fois le projet généré, vous pouvez utiliser directement les types de données qui y sont définis, avec l'espace de noms "ServeurRouting.ContractType".

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

    [DataContract]
    public class Statistiques
    {
        [DataMember]
        public Station mystation { get; set; }

        [DataMember]
        public double nbOfTimesItIsCalled { get; set; }

       
        public Statistiques(Station mystation)
        {
            this.mystation = mystation;
            this.nbOfTimesItIsCalled = 1;
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

        internal double getDistance2(ServiceReference1.Position position, Position p)
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

        internal double getDistanceTo(Position p, Position position)
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
        //public Geometry geometry { get; set; }
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
        [DataMember]
        public double distance { get; set; }
        [DataMember]
        public double duration { get; set; }
        [DataMember]
        public int type { get; set; }
        [DataMember]
        public string instruction { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public List<int> way_points { get; set; }
    }

    public class Segment
    {
        [DataMember]
        public double distance { get; set; }
        [DataMember]
        public double duration { get; set; }
        [DataMember]
        public List<Step> steps { get; set; }
    }

    public class Summary
    {
        [DataMember]
        public double distance { get; set; }
        [DataMember]
        public double duration { get; set; }
    }
 

    public class Query
    {
        [DataMember]
        public List<List<double>> coordinates { get; set; }
        [DataMember]
        public string profile { get; set; }
        [DataMember]
        public string format { get; set; }
    }

    public class Engine
    {
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public DateTime build_date { get; set; }
        [DataMember]
        public DateTime graph_date { get; set; }
    }

    public class MetadataOpenRoute
    {
        [DataMember]
        public string attribution { get; set; }
        [DataMember]
        public string service { get; set; }
        [DataMember]
        public long timestamp { get; set; }
        [DataMember]
        public Query query { get; set; }
        [DataMember]
        public Engine engine { get; set; }
    }

    [DataContract]
    public class Openroute
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public List<FeatureOpenRoute> features { get; set; }
        [DataMember]
        public List<double> bbox { get; set; }
        [DataMember]
        public MetadataOpenRoute metadata { get; set; }

        public string toString()
        {
            return features.ToString();
        }
    }


}
