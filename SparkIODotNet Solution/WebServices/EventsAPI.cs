using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;

namespace SparkIO.WebServices
{
    public class EventsAPI : BaseAPI
    {
        #region Variables & Constants
        private const string addrEventsAllPublicYourPrivate = sparkURL + "/events?access_token={0}"; // {0} = Access Token
        private const string addrEventsAllPublicYourPrivateByName = sparkURL + "/events/{0}?access_token={1}";  // {0} = Name {1} = Access Token
        private const string addrEventsYourDevicesPublicPrivate = sparkURL + "/devices/events?access_token={0}"; // {0} = Access Token
        private const string addrEventsYourDevicesPublicPrivateByName = sparkURL + "/devices/events/{0}?access_token={1}";  // {0} = Name {1} = Access Token
        private const string addrEventsDeviceAllPublicYourPrivate = sparkURL + "/devices/{0}/events?access_token={1}"; // {0} = Device {1} = Access Token
        private const string addrEventsDeviceAllPublicYourPrivateByName = sparkURL + "/devices/{0}/events/{1}?access_token={2}";  // {0} = Device {1} = Name {2} = Access Token

        private bool isStarted = false;
        public static ConcurrentQueue<String> queue = new ConcurrentQueue<String>();

        EventStreamReader reader;
        EventStreamEmitter emitter;

        Task taskReader;
        Task taskEmitter;

        Stream stream;
        #endregion

        #region Events
        public event EventHandler<SSEEventArgs> EventReceived;
        #endregion Events

        #region Constructors
        public EventsAPI(String accessToken) :
            base(String.Empty, accessToken)
        { }

        public EventsAPI(String accessToken, WebProxy webProxy) :
            base(String.Empty, accessToken, webProxy)
        { }

        public EventsAPI(String coreID, String accessToken) :
            base(coreID, accessToken)
        {}

        public EventsAPI(String coreID, String accessToken, WebProxy webProxy) :
            base(coreID, accessToken, webProxy)
        {}
        #endregion

        #region Event Subscriptions

        private Stream GetEventsByName(string eventName = null)
        {
            string address;

            if (eventName == null || eventName.Length == 0)
            {
                address = string.Format(addrEventsAllPublicYourPrivate, AccessToken);
            }
            else
            {
                address = string.Format(addrEventsAllPublicYourPrivateByName, eventName, AccessToken);
            }

            HttpWebRequest request = GetHttpWebRequest(webMethod.GET, String.Format(address, AccessToken), null);
            request.AllowReadStreamBuffering = false;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            return stream;
        }

        #endregion

        #region Start Commands
        public void Start()
        {
            if (!isStarted)
            {
                stream = GetEventsByName();

                CheckSSEStreamIsOK(stream);

                StartEvents(stream);
                isStarted = true;
            }
        }

        public void Start(string eventName)
        {
            if (!isStarted)
            {
                stream = GetEventsByName(eventName);

                CheckSSEStreamIsOK(stream);

                StartEvents(stream);
                isStarted = true;
            }
        }
        #endregion

        public void Stop()
        {
            if (isStarted)
            {
                // stop reading the stream
                reader.RequestStop();
                taskReader.Wait();

                // close the stream
                stream.Close();

                // and all queued events should have stopped by now
                emitter.RequestStop();
                taskEmitter.Wait();

                isStarted = false;
            }
        }

        private void StartEvents(Stream stream)
        {
            // instantiate the stream reader and event emitter
            reader = new EventStreamReader(stream, queue);
            emitter = new EventStreamEmitter(queue);

            // wire up the emitter to our event
            emitter.EventReceived += this.EventReceived;

            // start the task reader : stream -> queue
            taskReader = Task.Factory.StartNew(() => reader.DoWork(), TaskCreationOptions.LongRunning);

            // start the task emitter : queue -> events
            taskEmitter = Task.Factory.StartNew(() => emitter.DoWork(), TaskCreationOptions.LongRunning);
        }

        private void CheckSSEStreamIsOK(Stream stream)
        {
            var encoder = new UTF8Encoding();
            var buffer = new byte[5];

            int len = stream.Read(buffer, 0, buffer.Length);

            if (len > 0)
            {
                var text = encoder.GetString(buffer, 0, len);
                
                if(text == ":ok\n\n")
                    { return; }
            }
            throw new ApplicationException("SSE Stream not :ok");
        }

    }
}
