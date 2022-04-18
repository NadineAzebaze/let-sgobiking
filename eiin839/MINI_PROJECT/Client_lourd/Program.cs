using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceReference1;

namespace Client_lourd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServeurRouting.ServiceReference1.RoutingClient routingClient = new ServeurRouting.ServiceReference1.RoutingClient();
            Console.WriteLine("yes");
            Console.ReadLine();
        }
    }
}
