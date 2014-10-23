using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SparkIO.WebServices.JSON;

namespace SparkIO.WebServices
{
    public class BaseAPI
    {
        #region Variables and Constants
        protected const string sparkAddress = "https://api.spark.io";
        protected const string sparkVersion = "v1";
        protected const string sparkURL = "/" + sparkVersion;
        protected const string contentType = "application/x-www-form-urlencoded";
        protected const int webrequestTimeout = 10000;

        protected const string addrBaseAPIListDevices = sparkURL + "/devices?access_token={0}"; // {0} = access token
        protected const string addrBaseAPIGetDeviceInfo = sparkURL + "/devices/{0}?access_token={1}"; // {1} = device id {0} = access token
        private const string payloadBaseAPIAccessToken = "access_token={0}"; // {0} = Access Token 

        protected enum webMethod
        {
            GET,
            POST
        }

        protected String CoreID = "";
        protected String AccessToken = "";
        protected WebProxy localProxy = null;
        protected bool AcceptAllCertificates = false;
        protected string URLSparkCloud = sparkAddress;
        #endregion

        #region Constructors
        public BaseAPI(String coreID, String accessToken, bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress)
        {
            CoreID = coreID;
            AccessToken = accessToken;
            AcceptAllCertificates = acceptAllCertificates;

            //Optionally ignore certificate errors
            if (acceptAllCertificates)
            {
                ServicePointManager.ServerCertificateValidationCallback =
                new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            }
        }

        public BaseAPI(String coreID, String accessToken, WebProxy webProxy, bool acceptAllCertificates = false, string urlSparkCloud = sparkURL)
        {
            localProxy = webProxy;
            CoreID = coreID;
            AccessToken = accessToken;
            AcceptAllCertificates = acceptAllCertificates;

            //Optionally ignore certificate errors
            if (acceptAllCertificates)
            {
                ServicePointManager.ServerCertificateValidationCallback =
                new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            }
        }
        #endregion

        #region Methods
        protected HttpWebRequest GetHttpWebRequest(webMethod method, string addr, string payload)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLSparkCloud + addr);

            request.Timeout = webrequestTimeout;

            if (localProxy != null)
            {
                request.Proxy = localProxy;
            }

            request.Method = method.ToString();
            request.ContentType = contentType;

            request.ContentLength = 0;

            if (!string.IsNullOrEmpty(payload) && method == webMethod.POST)
            {
                byte[] bytes = StringToUTF8Bytes(payload);

                request.ContentLength = bytes.Length;

                using (Stream writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            return request;
        }

        protected object GetHttpWebResponseAsJSONData(HttpWebRequest request, Type type)
        {
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    String responseValue = String.Empty;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        String message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                        throw new ApplicationException(message);
                    }

                    // grab the response
                    object objResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(type); 
                        // DateTime parsing issue http://stackoverflow.com/questions/9266435/datacontractjsonserializer-deserializing-datetime-within-listobject

                        objResponse = jsonSerializer.ReadObject(responseStream);
                    }

                    return objResponse;
                }
            }
            catch (WebException we)
            {
                throw new ApplicationException("A WebException has been thrown.  This usually indicates that the spark function name, core id or access token is incorrect, or the device is not online.", we);
            }
            catch
            {
                throw;
            }
        }

        public DeviceList[] GetCoreList()
        {
            HttpWebRequest request = GetHttpWebRequest(webMethod.GET, string.Format(addrBaseAPIListDevices, HttpUtility.UrlEncode(this.AccessToken)), string.Empty);

            object jsonObj = GetHttpWebResponseAsJSONData(request, typeof(DeviceList[]));

            DeviceList[] deviceList = jsonObj as DeviceList[];

            return deviceList;
        }

        public DeviceInfo GetCoreInfo()
        {
            if (CoreID == string.Empty || CoreID.Length == 0)
            {
                throw new ApplicationException("Cannot call this method without specifying a Core ID in the constructor.");
            }

            return GetCoreInfo(this.CoreID);
        }

        public DeviceInfo GetCoreInfo(string CoreId)
        {
            HttpWebRequest request = GetHttpWebRequest(webMethod.GET, string.Format(addrBaseAPIGetDeviceInfo, HttpUtility.UrlEncode(this.CoreID), HttpUtility.UrlEncode(this.AccessToken)), string.Empty);

            object jsonObj = GetHttpWebResponseAsJSONData(request, typeof(DeviceInfo));

            DeviceInfo deviceInfo = jsonObj as DeviceInfo;

            return deviceInfo;

        }
        #endregion

        #region Static Helper Functions
        public static byte[] StringToUTF8Bytes(string payload)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return Encoding.GetEncoding("iso-8859-1").GetBytes(payload);
        }

        public static string StreamToString(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        #endregion

        #region internal methods

        public static bool AcceptAllCertifications(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate cert,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors errors)
        {
            return true;
        }

        #endregion

    }
}
