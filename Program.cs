using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_sever
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server on port: 8080");
            Jobs.Update();
            HTTPServer server = new HTTPServer(8080);
            server.Start();
        }
    }
}
