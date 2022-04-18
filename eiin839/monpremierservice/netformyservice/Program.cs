using netformyservice.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace netformyservice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.Service1Client service = new Service1Client();
            int sum = service.add(3, 4);
            Console.WriteLine(sum);
            Console.ReadLine();

        }
    }
}
