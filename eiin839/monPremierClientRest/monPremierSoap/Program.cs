using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monPremierSoap.ServiceReference1;

namespace monPremierSoap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalculatorSoapClient calculatorSoapClient = new CalculatorSoapClient();
            int sum = calculatorSoapClient.Add(3, 4);
            Console.WriteLine(sum);
            Console.ReadLine();
        }
    }
}
