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
using SparkIO.WebServices.Exceptions;

namespace SparkIO.WebServices
{
    public class BaseAPI
    {
        #region Variables and Constants
        protected const string sparkAddress = "https://api.spark.io";
        protected const string sparkVersion = "v1";
        protected const string sparkURL = "/" + sparkVersion;
        protected const string sparkClientId = "SparkIODotNet";
        protected const string contentType = "application/x-www-form-urlencoded";
        protected const int webrequestTimeout = 60000;

        protected const string addrBaseAPIListDevices = sparkURL + "/devices?access_token={0}"; // {0} = access token
        protected const string addrBaseAPIGetDeviceInfo = sparkURL + "/devices/{0}?access_token={1}"; // {1} = device id {0} = access token
        private const string payloadBaseAPIAccessToken = "access_token={0}"; // {0} = Access Token 
        private const string addrBaseAPIListTokens = sparkURL + "/access_tokens";
        private const string payloadBaseAPICreateToken = "grant_type=password&username={0}&password={1}"; // {0} = User Name  {1} = Passsword
        private const string addrBaseAPICreateToken = "/oauth/token";
        private const string addrBaseAPIDeleteToken = sparkURL + "/access_tokens/{0}";

        protected enum webMethod
        {
            GET,
            POST,
            DELETE
        }

        protected String CoreID = "";
        protected String AccessToken = "";
        protected WebProxy localProxy = null;
        protected bool AcceptAllCertificates = false;
        protected string URLSparkCloud = sparkAddress;
        #endregion

        #region Constructors
        public BaseAPI(String accessToken, String coreID = null, WebProxy webProxy = null, 
                       bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress)
        {
            shadowBaseAPI(coreID, accessToken, webProxy, acceptAllCertificates, urlSparkCloud);
        }

        public BaseAPI(String userName, String password, String coreID = null, WebProxy webProxy = null, 
                       bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress)
        {
            shadowBaseAPI(coreID, this.GetAccessToken(userName, password), webProxy, acceptAllCertificates, urlSparkCloud);
        }

        private void shadowBaseAPI(String coreID, String accessToken, WebProxy webProxy = null, bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress)
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
        protected HttpWebRequest GetHttpWebRequest(webMethod method, string addr, string payload = null, string basicUserName = null, string basicPassword = null )
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLSparkCloud + addr);

            request.Timeout = webrequestTimeout;

            if (localProxy != null)
            {
                request.Proxy = localProxy;
            }

            if (basicUserName != null & basicPassword != null)
            {
                byte[] authBytes = Encoding.UTF8.GetBytes((basicUserName + ":" + basicPassword).ToCharArray());
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(authBytes));
            }

            request.Method = method.ToString();
            if (method != webMethod.DELETE)
            {
                request.ContentType = contentType;
                request.ContentLength = 0;
            }

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

        protected object GetHttpWebResponseAsJSONData(HttpWebRequest request, Type type = null)
        {
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    String responseValue = String.Empty;


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
                using (WebResponse response = we.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;

                    switch (httpResponse.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            throw new InvalidVariableOrFunctionException();
                        case HttpStatusCode.Unauthorized:
                            throw new UsernameOrPasswordIncorrectException();
                        case HttpStatusCode.Forbidden:
                            throw new NotAuthorizedForThisCoreException();
                        case HttpStatusCode.NotFound:
                            throw new CoreNotConnectedToCloudException();
                        case HttpStatusCode.RequestTimeout:
                            throw new SparkCloudConnectionTimeoutException();
                        case HttpStatusCode.InternalServerError:
                            throw new SparkCloudNotAvailableException();
                        default:
                            throw new UnknownNetworkConnectionErrorException();
                    }
                }
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

        public TokenInfo[] GetAccessTokens(String userName, String password)
        {
            HttpWebRequest request = GetHttpWebRequest(webMethod.GET, addrBaseAPIListTokens, string.Empty, userName, password);

            object jsonObj = GetHttpWebResponseAsJSONData(request, typeof(TokenInfo[]));

            return jsonObj as TokenInfo[];
        }

        public CreateTokenInfo CreateToken(String userName, String password)
        {
            HttpWebRequest request = GetHttpWebRequest(webMethod.POST, addrBaseAPICreateToken, String.Format(payloadBaseAPICreateToken, userName, password), sparkClientId, sparkClientId);

            object jsonObj = GetHttpWebResponseAsJSONData(request, typeof(CreateTokenInfo));

            return jsonObj as CreateTokenInfo;
        }

        public bool DeleteToken(String userName, String password, String token)
        {
            HttpWebRequest request = GetHttpWebRequest(webMethod.DELETE, String.Format(addrBaseAPIDeleteToken, token), null, userName, password);

            object jsonObj = GetHttpWebResponseAsJSONData(request, typeof(DeleteTokenResult));

            return (jsonObj as DeleteTokenResult).ok;
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

        private static bool AcceptAllCertifications(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate cert,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors errors)
        {
            return true;
        }
        #endregion

        #region internal methods
        private String GetAccessToken(String userName, String password)
        {
            String curToken = string.Empty;

            TokenInfo[] deviceList = GetAccessTokens(userName, password);

            // check if we have a token for our client ID that is over an hour away from expiring
            foreach (TokenInfo token in deviceList)
            {
                if ((token.Client == sparkClientId) && (DateTime.Parse(token.ExpiresAt) - DateTime.Now).TotalMinutes > 60)
                {
                    curToken = token.Token;
                    break;
                }
            }
            // if we didn't find a token, make one!
            if (curToken == String.Empty)
            {
                CreateTokenInfo newToken = CreateToken(userName, password);

                curToken = newToken.AccessToken;
            }

            return curToken;
        }

        private void AddAuthHeaderToRequest(HttpWebRequest request, String userName, String password)
        {
            byte[] authBytes = Encoding.UTF8.GetBytes((userName + ":" + password).ToCharArray());
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(authBytes));
        }
        #endregion

    }
}
