using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeavyClientproject.ServiceReference1;

namespace HeavyClientproject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var origin = Console.ReadLine();
            var destination = Console.ReadLine();
            var route = new ServiceReference1.RoutingClient();
            var s = await route.ComputeItineraryAsync(origin, destination);
            Console.WriteLine(s[0]);
            Console.ReadLine();
        }
    }

}
