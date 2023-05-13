using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using System.Threading;

namespace RobotUICSharp
{

    public class RobotWebController
    {
        HttpListener listener;
        String URL;

        public RobotWebController(String url)
        {
            URL = url;
            listener = new HttpListener();
            listener.Prefixes.Add(URL);
            listener.Start();
            Console.WriteLine("Webcontroller listener start");
            Thread WebThread = new Thread(new ThreadStart(this.HandleWebserver));
            WebThread.Start();
            Console.WriteLine("Is this run?");
        }


        public void HandleWebserver()
        {
            while (true)
            {
                HttpListenerContext ctx = listener.GetContext();
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse response = ctx.Response;
                string absolutePath = req.Url.AbsolutePath;
                Console.WriteLine(absolutePath);
                DecodeAbsolutePath(absolutePath);
                string responseString = String.Format("<HTML><BODY> Last State: {0}</BODY></HTML>", absolutePath);
                
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

        public void DecodeAbsolutePath(String input)
        {
            foreach (Tuple<String, StateInterface> item in ActionConfiguration.GetURLConfig())
            {
                if (input == item.Item1)
                {
                    RobotAnimator.Instance.ChangeState(item.Item2);
                }
            }
        }

    }
} 

