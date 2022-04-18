using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Echo
{
   
    class EchoServer
    {
        
        [Obsolete]
        static void Main(string[] args)
        {

            Console.CancelKeyPress += delegate
            {
                System.Environment.Exit(0);
            };

            TcpListener ServerSocket = new TcpListener(5000);
            ServerSocket.Start();

            Console.WriteLine("Server started.");
            while (true)
            {
                TcpClient clientSocket = ServerSocket.AcceptTcpClient();
                handleClient client = new handleClient();
                client.startClient(clientSocket);
            }


        }
    }

    public class handleClient
    {
        static string HTTP_ROOT = "../../../www/";
        TcpClient clientSocket;
        public void startClient(TcpClient inClientSocket)
        {
            this.clientSocket = inClientSocket;
            Thread ctThread = new Thread(Echo);
            ctThread.Start();
        }

        private void Echo()
        {
            NetworkStream stream = clientSocket.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);
            Console.WriteLine(writer);
            while (true)
            {
                string str = reader.ReadString();
                Console.WriteLine(str);
                string[] sub_str = str.Split(' ');
                string file_name = sub_str[1];
                string path = HTTP_ROOT + file_name;
                writer.Write("HTTP/1.0 200 OK \n\n" + File.ReadAllText(path));
            }
        }

    }

}