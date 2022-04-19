using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Service1 : IService1
    {
        ProxyCache<JCDecauxItem> proxyCache = new Proxy.ProxyCache<JCDecauxItem>();
        public List<Station> stations;
        RequeteJCDecaux requete = new RequeteJCDecaux();

        public Service1()
        {
            this.stations = this.GetListStations();
        }
        public Station GetStation(int number, string contratName)
        {
            string KEY = number.ToString() + "_" + contratName;
            return proxyCache.Get(KEY, 90).root;
        }

        public List<Station> GetListStations()
        {
            return requete.GetListStations().Result;
        }

        public string GetAll()
        {
            return "ok";
        }

    }
}

