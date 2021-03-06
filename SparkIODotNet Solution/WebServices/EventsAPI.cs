﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;
using SparkIO.WebServices.Internal;
using SparkIO.WebServices.CustomEventArgs;
using System.Threading;

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
        public EventsAPI(String accessToken, String coreID = "", WebProxy webProxy = null,
                       bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress) :
            base(accessToken, coreID, webProxy, acceptAllCertificates, urlSparkCloud)
        {}

        public EventsAPI(String userName, String password, String coreID = "", WebProxy webProxy = null,
                       bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress) :
            base(userName, password, coreID, webProxy, acceptAllCertificates, urlSparkCloud)
        { }
        #endregion

        public bool IsStarted
        {
            get { return isStarted; }
        }

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

        private Stream GetOwnedDeviceEventsByName(string eventName = null)
        {
            string address;

            if (eventName == null || eventName.Length == 0)
            {
                address = string.Format(addrEventsYourDevicesPublicPrivate, AccessToken);
            }
            else
            {
                address = string.Format(addrEventsYourDevicesPublicPrivateByName, eventName, AccessToken);
            }

            HttpWebRequest request = GetHttpWebRequest(webMethod.GET, String.Format(address, AccessToken), null);
            request.AllowReadStreamBuffering = false;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            return stream;
        }

        private Stream GetDeviceEventsByName(string deviceName, string eventName = null)
        {
            string address;

            if (eventName == null || eventName.Length == 0)
            {
                address = string.Format(addrEventsDeviceAllPublicYourPrivate, deviceName, AccessToken);
            }
            else
            {
                address = string.Format(addrEventsDeviceAllPublicYourPrivateByName, deviceName, eventName, AccessToken);
            }

            HttpWebRequest request = GetHttpWebRequest(webMethod.GET, String.Format(address, AccessToken), null);
            request.AllowReadStreamBuffering = false;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            return stream;
        }
        #endregion

        #region Start and Stop Commands
        public void StartPublicEvents()
        {
            if (!isStarted)
            {
                stream = GetEventsByName();

                CheckSSEStreamIsOK(stream);

                StartEvents(stream);
                isStarted = true;
            }
        }

        public void StartPublicEventsByName(string eventName, bool exactMatch = false)
        {
            if (!isStarted)
            {
                stream = GetEventsByName(eventName);

                CheckSSEStreamIsOK(stream);

                StartEvents(stream, eventName, exactMatch);
                isStarted = true;
            }
        }

        public void StartOwnedDeviceEvents()
        {
            if (!isStarted)
            {
                stream = GetOwnedDeviceEventsByName();

                CheckSSEStreamIsOK(stream);

                StartEvents(stream);
                isStarted = true;
            }
            else
            {
                throw new ApplicationException("Event Stream Already Started.");
            }
        }

        public void StartOwnedDeviceEventsByName(string eventName, bool exactMatch = false)
        {
            if (!isStarted)
            {
                stream = GetOwnedDeviceEventsByName(eventName);

                CheckSSEStreamIsOK(stream);

                StartEvents(stream, eventName, exactMatch);
                isStarted = true;
            }
            else
            {
                throw new ApplicationException("Event Stream Already Started.");
            }
        }

        public void StartDeviceEvents()
        {
            if (CoreID == string.Empty || CoreID.Length == 0)
            {
                throw new ApplicationException("Cannot call this method without specifying a Core ID in the constructor.");
            }

            if (!isStarted)
            {
                stream = GetDeviceEventsByName(CoreID);

                CheckSSEStreamIsOK(stream);

                StartEvents(stream);
                isStarted = true;
            }
            else
            {
                throw new ApplicationException("Event Stream Already Started.");
            }
        }

        public void StartDeviceEvents(string deviceName)
        {
            if (!isStarted)
            {
                stream = GetDeviceEventsByName(deviceName);

                CheckSSEStreamIsOK(stream);

                StartEvents(stream);
                isStarted = true;
            }
            else
            {
                throw new ApplicationException("Event Stream Already Started.");
            }
        }

        public void StartDeviceEventsByName(string deviceName, string eventName, bool exactMatch = false)
        {
            if (!isStarted)
            {
                stream = GetDeviceEventsByName(deviceName, eventName);

                CheckSSEStreamIsOK(stream);

                StartEvents(stream, eventName, exactMatch);
                isStarted = true;
            }
            else
            {
                throw new ApplicationException("Event Stream Already Started.");
            }
        }

        public void StartDeviceEventsByName(string eventName, bool exactMatch = false)
        {
            if (CoreID == string.Empty || CoreID.Length == 0)
            {
                throw new ApplicationException("Cannot call this method without specifying a Core ID in the constructor.");
            }

            if (!isStarted)
            {
                stream = GetDeviceEventsByName(CoreID, eventName);

                CheckSSEStreamIsOK(stream);

                StartEvents(stream, eventName, exactMatch);
                isStarted = true;
            }
            else
            {
                throw new ApplicationException("Event Stream Already Started.");
            }
        }

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
        #endregion

        #region internal methods
        private void StartEvents(Stream stream, string eventName = null, bool exactMatch = false)
        {
            // create an event so reader can signal emitter
            AutoResetEvent emitterWaitHandle = new AutoResetEvent(true);

            // instantiate the stream reader and event emitter
            reader = new EventStreamReader(stream, queue, emitterWaitHandle);
            emitter = new EventStreamEmitter(queue,  this.raiseEvent, emitterWaitHandle, eventName, exactMatch);


            // start the task reader : stream -> queue
            taskReader = Task.Factory.StartNew(() => { Thread.CurrentThread.Name = "StreamReader"; reader.DoWork(); }, TaskCreationOptions.LongRunning);

            // start the task emitter : queue -> events
            taskEmitter = Task.Factory.StartNew(() => {Thread.CurrentThread.Name = "Event Emitter" ; emitter.DoWork();}, TaskCreationOptions.LongRunning);
        }

        protected void raiseEvent(SSEEventArgs eventArgs)
        {
            // We've got all of the data - raise the event
            if (EventReceived != null)
            {
                EventReceived(this, eventArgs);
            }
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
        #endregion
    }
}
