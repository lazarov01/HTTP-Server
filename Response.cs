using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_sever
{
    public class Response
    {
        public Byte[] data = null;
        private Response(Byte[] data)
        {
            this.data = data;
        }
        public static Response From(Request request)
        {
            if (request == null)
                return Make("400", "Bad Request");//Null request

            if (request.Type == "GET")
            {
                String file = Environment.CurrentDirectory + HTTPServer.WEB_DIR + request.URL;
                FileInfo f = new FileInfo(file);
                if (f.Exists && f.Extension.Contains("."))
                {
                    return Make();//Make from file
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(f + "/");
                    if (!di.Exists)
                        return Make("404", "Page Not Found");

                    FileInfo[] fi = di.GetFiles();
                    foreach (FileInfo ff in fi)
                    {
                        String n = ff.Name;
                        if (n.Contains("default.html") || n.Contains("default.htm") || n.Contains("index.html") || n.Contains("index.htm"))
                            return Make();//Make from file
                        
                    }
                }
            }

            else
                return Make("405", "Method not allowed");//Method not allowed
            
            return Make("404", "Page Not Found");//page not found
        }

        private static Response Make()
        {
            Byte[] b;
            b = Jobs.d;
            return new Response(b);
        }
        
        private static Response Make(String error, String msg)
        {
            String file = Environment.CurrentDirectory + HTTPServer.MSG_DIR + error + ".html";
            FileInfo f = new FileInfo(file);
            FileStream fs = f.OpenRead();
            BinaryReader br = new BinaryReader(fs);
            Byte[] b = new Byte[fs.Length];
            br.Read(b, 0, b.Length);
            fs.Close();
            String res = error + " " + msg;
            return new Response(b);
        }

        public void Post(NetworkStream stream)
        {
            String msg = "HTTP/1.1 \r\nUser-Agent: C# program\r\n" + "Connection: close\r\nAccept: text/html\r\n\r\n";
            stream.Write(Encoding.UTF8.GetBytes(msg), 0, msg.Length);
            stream.Write(data , 0, data.Length);
            

        }
    }
}
