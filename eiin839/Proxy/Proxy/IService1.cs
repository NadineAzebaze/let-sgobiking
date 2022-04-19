using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetStation?number={number}&contratName={contratName}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Json)]
        Station GetStation(int number, string contratName);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "get_all", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string GetAll();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "get_station", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]

        List<Station> GetListStations();
        // TODO: ajoutez vos opérations de service ici
    }

    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    // Vous pouvez ajouter des fichiers XSD au projet. Une fois le projet généré, vous pouvez utiliser directement les types de données qui y sont définis, avec l'espace de noms "Proxy.ContractType".

    [DataContract]
    public class Position
    {
        [DataMember]
        public double latitude { get; set; }

        [DataMember]
        public double longitude { get; set; }
    }


    [DataContract]
    public class Availabilities
    {

        [DataMember]
        public int bikes { get; set; }


        [DataMember]
        public int stands { get; set; }


        [DataMember]
        public int mechanicalBikes { get; set; }


        [DataMember]
        public int electricalBikes { get; set; }


        [DataMember]
        public int electricalInternalBatteryBikes { get; set; }


        [DataMember]
        public int electricalRemovableBatteryBikes { get; set; }
    }


    [DataContract]
    public class TotalStands
    {

        [DataMember]
        public Availabilities availabilities { get; set; }


        [DataMember]
        public int capacity { get; set; }
    }


    [DataContract]
    public class MainStands
    {

        [DataMember]
        public Availabilities availabilities { get; set; }


        [DataMember]
        public int capacity { get; set; }
    }


    [DataContract]
    public class Station
    {
        [DataMember]
        public int number { get; set; }

        [DataMember]
        public string contractName { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string address { get; set; }

        [DataMember]
        public Position position { get; set; }

        [DataMember]
        public bool banking { get; set; }

        [DataMember]
        public bool bonus { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public DateTime lastUpdate { get; set; }

        [DataMember]
        public bool connected { get; set; }

        [DataMember]
        public bool overflow { get; set; }

        /*[DataMember]
        public object shape { get; set; }*/

        [DataMember]
        public TotalStands totalStands { get; set; }

        [DataMember]
        public MainStands mainStands { get; set; }

        /*[DataMember]
        public object overflowStands { get; set; }*/
    }
}
