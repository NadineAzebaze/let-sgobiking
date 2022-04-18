using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServeurRouting
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Routing)))
            {
                host.Open();
                Console.WriteLine("**************** RoutingService ****************");
                Console.WriteLine("Sart time : " + DateTime.Now.ToString());
                Console.WriteLine("The service is ready at {0}", host.BaseAddresses[0]);
                Console.WriteLine("Press <Enter> key to stop ...");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
