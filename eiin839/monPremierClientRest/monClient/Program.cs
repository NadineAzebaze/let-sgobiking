using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace monClient
{
    class EchoClient
    {
        static TcpClient clientSocket;
        public static void Write()
        {
            BinaryWriter writer = new BinaryWriter(clientSocket.GetStream());

            while (true)
            {
                string str = Console.ReadLine();
                writer.Write(str);
            }
        }
        public static void Read()
        {
            BinaryReader reader = new BinaryReader(clientSocket.GetStream());

            while (true)
            {
                string str = "response: ";
                str = str + reader.ReadString();
                Console.WriteLine(str);
            }

        }

        static void Main(string[] args)
        {
            clientSocket = new TcpClient("localhost", 5000);
            Thread ctThreadWrite = new Thread(Write);
            Thread ctThreadRead = new Thread(Read);
            ctThreadRead.Start();
            ctThreadWrite.Start();
        }
    }
}
