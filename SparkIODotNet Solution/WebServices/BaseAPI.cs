using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices
{
    public class BaseAPI
    {
        #region Variables and Constants
        protected const string sparkAddress = "https://api.spark.io";
        protected const string sparkVersion = "v1";
        protected const string sparkURL = sparkAddress + "/" + sparkVersion; // + "/devices/";
        protected const string contentType = "application/x-www-form-urlencoded";
        protected const int webrequestTimeout = 10000;

        protected enum webMethod
        {
            GET,
            POST
        }

        protected String CoreID = "";
        protected String AccessToken = "";
        protected WebProxy localProxy = null;
        #endregion

        #region Constructors
        public BaseAPI(String coreID, String accessToken)
        {
            CoreID = coreID;
            AccessToken = accessToken;
        }

        public BaseAPI(String coreID, String accessToken, WebProxy webProxy)
        {
            localProxy = webProxy;
            CoreID = coreID;
            AccessToken = accessToken;
        }
        #endregion

        #region Methods
        protected HttpWebRequest GetHttpWebRequest(webMethod method, string addr, string payload)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(addr);

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
                        objResponse = jsonSerializer.ReadObject(responseStream);
                    }

                    return objResponse;
                }
            }
            catch (WebException we)
            {
                throw new ApplicationException("A WebException has been thrown.  This usually indicates that the spark function name, core id or access toekn is incorrect, or the device is not online.", we);
            }
            catch
            {
                throw;
            }
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

    }
}
