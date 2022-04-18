using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    internal class JCDecauxItem
    {
        public Station root;
        RequeteJCDecaux requete = new RequeteJCDecaux();
        //créer une classe externe pour faire la requête à l'intérieur et résoudre le problème de async si tu n'y arrives pas demande à Denis
        public JCDecauxItem(string KEY)
        {
            string[] vs = KEY.Split('_');
            string contratName = vs[1];
            int number = int.Parse(vs[0]);
            root = requete.CreatingObtectJCDecaux(number, contratName).Result;
        }

    }
}
