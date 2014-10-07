using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparkIO.WebServices;

namespace ConsoleApplication1
{
    class Program
    {
        //public static ConcurrentQueue<String> queue = new ConcurrentQueue<String>();
        //public static Stream stream;

        static void Event_Received(object sender, SSEEventArgs e)
        {
            Console.WriteLine("Event:  " + e.EventName);
            Console.WriteLine("Data:   " + e.Data);
            Console.WriteLine("TTL:    " + e.TTL);
            Console.WriteLine("PubAt:  " + e.PublishedAt);
            Console.WriteLine("CoreID: " + e.CoreID);
        }


        static void Main(string[] args)
        {
            EventsAPI core = new EventsAPI("core", "token"); // MUTANT_COWBOY

            core.EventReceived += Event_Received;

            core.Start("temp");

            System.Threading.Thread.Sleep(5000);

            core.Stop();

            /*
            stream = core.GetEventsByName();

            EventStreamReader reader = new EventStreamReader(stream, queue);
            EventStreamEmitter emitter = new EventStreamEmitter(queue);

            emitter.EventReceived += Event_Received;

            Task task1 = Task.Factory.StartNew(() => reader.DoWork(), TaskCreationOptions.LongRunning);

            Task task2 = Task.Factory.StartNew(() => emitter.DoWork(), TaskCreationOptions.LongRunning);
            
            System.Threading.Thread.Sleep(5000);

            Console.WriteLine("Request Reader Stop.");
            reader.RequestStop();
            Console.WriteLine("Reader Join.");
            task1.Wait();

            Console.WriteLine("Request Emitter Stop.");
            emitter.RequestStop();
            Console.WriteLine("Emitter Join.");
            task2.Wait();

            Console.WriteLine("Closing Stream.");
            stream.Close();
            */

            Console.WriteLine("Done!");

            Console.ReadKey();
        }

        public static void ReadStreamForever()
        { }

        public static void ReadStreamForever(Stream stream, List<string> queue)
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

                        var lines = text.Trim().Split('\n');
                        queue.AddRange(lines);
                    }
                }
                System.Threading.Thread.Sleep(0);
            }
        }

    }
}
