using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HeavyClientproject.ServiceReference1;

namespace HeavyClientproject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("enter first adress");
            var origin = Console.ReadLine();
            Console.WriteLine("enter last adress");
            var destination = Console.ReadLine();
            Console.WriteLine("I'm computing your itinerary");
            ServiceReference1.RoutingClient router = new ServiceReference1.RoutingClient();
            ServiceReference1.Openroute[] s = router.ComputeItinerary(origin, destination);
            Statistiques[] list = router.FeedBackStatistiques();
            List<string> result = new List<string>();

            Console.WriteLine("================= LET's GO BIKING=====================");

            Console.WriteLine("==============from the origin to the station ==========>");
            foreach (Step step in s[0].features[0].properties.segments[0].steps)
            {
                result.Add(step.instruction);
                Console.WriteLine(step.instruction);
            }
            Console.WriteLine("============from the first station to the last station ==========>");
            foreach (Step step in s[1].features[0].properties.segments[0].steps)
            {
                result.Add(step.instruction);
                Console.WriteLine(step.instruction);
            }

            Console.WriteLine("===========from the last station to the arrival ==========>");
            foreach (Step step in s[2].features[0].properties.segments[0].steps)
                {
                    result.Add(step.instruction);
                    Console.WriteLine(step.instruction);
                }

              
            Console.WriteLine(" #########Performances############ ");

             Console.WriteLine("Waiting for services to start up...");
             Thread.Sleep(5000);
             Console.WriteLine("Starting benchmark...");


             Bench(() => router.ComputeItinerary(origin, destination));

             Console.WriteLine("Done");
            Console.ReadLine();

            void Bench<T>(Expression<Func<T>> code)
            {
                var comp = code.Compile();
                Console.WriteLine(code.Body);
                var watch = new Stopwatch();
                for (var i = 0; i < 5; i++)
                {
                    Console.Write("[{0}] ", i);
                    watch.Start();
                    comp();
                    watch.Stop();
                    Console.WriteLine("{0} ms", watch.ElapsedMilliseconds);
                    watch.Reset();
                }
            }
        }
    }

}
