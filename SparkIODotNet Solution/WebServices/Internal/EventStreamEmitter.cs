using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using SparkIO.WebServices.JSON;
using SparkIO.WebServices.CustomEventArgs;

namespace SparkIO.WebServices.Internal
{
    internal class EventStreamEmitter
    {
        private ConcurrentQueue<String> queue;
        private string filterEventName = null;
        private bool exactMatch = false;
        
        private volatile bool _shouldStop = false;

        #region Custom Event Classes
        /*
        public class SSEEventArgs : EventArgs
        {
            #region Fields
            public string EventName { get; set; }
            public string Data { get; set; }
            public int    TTL { get; set; }
            public DateTime PublishedAt { get; set; }
            public string CoreID { get; set; }
            #endregion Fields

            #region Constructors
            public SSEEventArgs(string eventName, string data, int ttl, DateTime publishedAt, string coreID)
            {
                EventName = eventName;
                Data = data;
                TTL = ttl;
                PublishedAt = publishedAt;
                CoreID = coreID;
            }
            #endregion Constructors
        }
        */
        #endregion

        #region Events
	    public event EventHandler<SSEEventArgs> EventReceived;
	    #endregion Events

        private EventStreamEmitter() { }

        public EventStreamEmitter(ConcurrentQueue<String> _queue, string _eventName = null, bool _exactMatch = false)
        {
            queue = _queue;
            filterEventName = _eventName;
            exactMatch = _exactMatch;
        }


        public void DoWork()
        {
            string line;
            string type;
            bool filter = false;

            if (exactMatch == true && filterEventName != null && filterEventName.Length > 0)
            {
                filter = true;
            }

            Object objResponse;
            EventDataVar eventData;
            string eventName = string.Empty;

            // JSON Deserializer for event info
            DataContractJsonSerializer jsonSerializerData = new DataContractJsonSerializer(typeof(EventDataVar));

            while (!_shouldStop)
            {
                if(queue.TryDequeue(out line))  // we have lines of events
                {
                    if (line.Length >= 6)  // valid data line is atleast 6 long
                    {
                        type = line.Substring(0, line.IndexOf(' '));  // what type of line is it?
                        line = line.Substring(line.IndexOf(' ') + 1);
                        if (type == "data:" && eventName != string.Empty)
                        {
                            if (filter && filterEventName != eventName)
                            {}
                            else
                            {
                                // Get the data from the line
                                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(line));
                                objResponse = jsonSerializerData.ReadObject(ms);
                                eventData = objResponse as EventDataVar;

                                // We've got all of the data - raise the event
                                EventHandler<SSEEventArgs> handler = EventReceived;
                                if (handler != null)
                                {
                                    handler(this, new SSEEventArgs(eventName, eventData.Data, eventData.TTL,
                                        DateTime.Parse(eventData.PublishedAt), eventData.CoreID));
                                }
                                // reset the event name
                                eventName = string.Empty;
                            }
                        }
                        else if (type == "event:" && eventName == string.Empty)
                        {
                            eventName = line;
                        }
                        else
                        {
                            eventName = string.Empty;
                        }
                    }
                }
            }

        }

        public void RequestStop()
        {
            _shouldStop = true;
        }

    }
}
