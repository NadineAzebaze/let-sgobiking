using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using serveurSoap.ServiceReference1;

namespace serveurSoap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.MathsOperationsClient mathsOperationsClient = new ServiceReference1.MathsOperationsClient();
            int sum = mathsOperationsClient.add(3, 4);
            Console.WriteLine("sum 3+4 = ");
            Console.WriteLine( sum);
            int multiply = mathsOperationsClient.multiply(3, 4);
            Console.WriteLine( "multiply 3*4 = ");
            Console.WriteLine(multiply);
            int substract = mathsOperationsClient.substract(7, 4);
            Console.WriteLine("substract 7-4 = ");
            Console.WriteLine(substract);
            Console.ReadLine();
        }
    }
}
