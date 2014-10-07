using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace PubSubTest
{
    public class SSEvent
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }

    class Program
    {
        public static List<string> Queue = new List<string>(1024);


        public static void Main()
        {
            Console.Write("Hello World\n");
            Console.Write("Attempting to open stream\n");

            var response = Program.OpenSSEStream("https://api.spark.io/v1/events");//?access_token=bfb875b7145b4cebf697c2dba394c434c14fe915>");
            Console.Write("Success! \n");
        }


        public static Stream OpenSSEStream(string url)
        {
            
            //Optionally ignore certificate errors
            ServicePointManager.ServerCertificateValidationCallback =
            new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            

            var request = WebRequest.Create(new Uri(url));
            request.Headers.Add("Authorization: Bearer bfb875b7145b4cebf697c2dba394c434c14fe915");
            ((HttpWebRequest)request).AllowReadStreamBuffering = false;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();

            Program.ReadStreamForever(stream);

            return stream;
        }

        public static void ReadStreamForever(Stream stream)
        {
            var encoder = new UTF8Encoding();
            var buffer = new byte[2048];
            while (true)
            {
                //TODO: Better evented handling of the response stream

                if (stream.CanRead)
                {
                    int len = stream.Read(buffer, 0, 2048);
                    if (len > 0)
                    {
                        var text = encoder.GetString(buffer, 0, len);
                        Console.WriteLine("Buffer: " + text);
                        Program.Push(text);
                    }
                }
                //System.Threading.Thread.Sleep(250);
            }
        }

        public static void Push(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Blank");
                return;
            }

            var lines = text.Trim().Split('\n');
            Program.Queue.AddRange(lines);

            if (text.Contains("data:"))
            {
                Program.ProcessLines();
            }
        }

        public static void ProcessLines()
        {
            var lines = Program.Queue;

            SSEvent lastEvent = null;
            int index = 0;
            int lastEventIdx = -1;

            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine("Raw: " + lines[i]);
                var line = lines[i];
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                line = line.Trim();

                if (line.StartsWith("event:"))
                {
                    lastEvent = new SSEvent()
                    {
                        Name = line.Replace("event:", String.Empty)
                    };
                }
                else if (line.StartsWith("data:"))
                {
                    if (lastEvent == null)
                    {
                        continue;
                    }


                    lastEvent.Data = line.Replace("data:", String.Empty);

                    Console.WriteLine("Found event: " + index);
                    Console.WriteLine("Name was: " + lastEvent.Name);
                    Console.WriteLine("Data was: " + lastEvent.Data);
                    index++;
                    lastEventIdx = i;
                }
            }
            //trim previously processed events
            if (lastEventIdx >= 0)
            {
                lines.RemoveRange(0, lastEventIdx);
            }

        }

        /*
        Optionally ignore certificate errors
 
        */
        public static bool AcceptAllCertifications(object sender,
        System.Security.Cryptography.X509Certificates.X509Certificate cert,
        System.Security.Cryptography.X509Certificates.X509Chain chain,
        System.Net.Security.SslPolicyErrors errors)
        {
            return true;
        }
    }
}
