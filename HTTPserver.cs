using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace HTTP_sever
{
    public class HTTPServer
    {
        public const String VERSION = "HTTP/1.1";
        public const String NAME = "LAZSERVERv1";
        public const String MSG_DIR = "/root/msg/";
        public const String WEB_DIR = "/root/web/";
        
        private bool running = false;

        private TcpListener listener;

        public HTTPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        public void Run()
        {
            running = true;
            listener.Start();

            while (running)
            {
                Console.WriteLine("Waiting for connection...");

                TcpClient client = listener.AcceptTcpClient();
                listener.AcceptTcpClient();

                Console.WriteLine("Client connected!");

                HandleClient(client);

                client.Close();
            }
            listener.Stop();
            running = false;
        }

        private void HandleClient(TcpClient client)
        {
            
            Console.WriteLine("Getting jobs...");
            Jobs job = Jobs.GetJobs();
            Jobs.SaveJobs(job.jobnames);
            job.loadJobNames();

            StreamReader reader = new StreamReader(client.GetStream());

            String msg = "";
            while (reader.Peek() != -1)
            {
                msg += reader.ReadLine() + "\n";
            }
            Request req = Request.GetRequest(msg);
            Response res = Response.From(req);
            res.Post(client.GetStream());
        }

    }
}
