using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace SparkIO.WebServices
{
    public class TinkerAPI : BaseAPI
    {
        #region Variables & Constants
        private const string addrTinkerAPICallFunction = sparkURL + "{0}/{1}";          // {0} - Core ID {1} - function
        private const string payloadWrite = "access_token={0}&params={1},{2}";          // {0} - Access Token {1} - Pin, {2} - Value
        private const string payloadRead = "access_token={0}&params={1}";               // {0} - Access Token {1} - Pin

        private enum Functions
        {
            digitalwrite,
            analogwrite,
            digitalread,
            analogread
        }

        public enum Pins
        {
            A0, A1, A2, A3, A4, A5, A6, A7, D0, D1, D2, D3, D4, D5, D6, D7
        }

        public enum SetStates
        {
            HIGH,
            LOW
        }

        public enum GetStates
        {
            HIGH,
            LOW,
            FAIL
        }
        #endregion

        #region JSON Data Contracts
        [DataContract]
        protected class Response
        {
            [DataMember(Name = "id")]
            public string ID { get; set; }
            [DataMember(Name = "name")]
            public string Name { get; set; }
            [DataMember(Name = "last_app")]
            public string LastApp { get; set; }
            [DataMember(Name = "connected")]
            public bool Connected { get; set; }
            [DataMember(Name = "return_value")]
            public int ReturnValue { get; set; }
        }
        #endregion

        #region Delegates
        public delegate GetStates DigitalWriteDelegate(Pins pins, SetStates setStates);
        public delegate bool AnalogWriteDelegate(Pins pins, short val);
        #endregion

        #region Constructors
        public TinkerAPI(String accessToken, String coreID, WebProxy webProxy = null,
                       bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress) :
            base(accessToken, coreID, webProxy, acceptAllCertificates, urlSparkCloud)
        {}

        public TinkerAPI(String userName, String password, String coreID, WebProxy webProxy = null,
                       bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress) :
            base(userName, password, coreID, webProxy, acceptAllCertificates, urlSparkCloud)
        { }
        #endregion
        #region Main Functions
        public GetStates DigitalWrite(Pins pin, SetStates state)
        {
            Response result = Post(Functions.digitalwrite, 
                String.Format(payloadWrite,
                              HttpUtility.UrlEncode(AccessToken), 
                              HttpUtility.UrlEncode(pin.ToString()), HttpUtility.UrlEncode(state.ToString())));

            switch (result.ReturnValue)
            {
                case -1:
                    return GetStates.FAIL;
                default:
                    if (state == SetStates.HIGH) return GetStates.HIGH;
                    return GetStates.LOW;
            }
        }

        public GetStates DigitalRead(Pins pin)
        {
            Response result = Post(Functions.digitalread,
                String.Format(payloadRead, HttpUtility.UrlEncode(AccessToken), HttpUtility.UrlEncode(pin.ToString())));

            switch (result.ReturnValue)
            {
                case 0:
                    return GetStates.LOW;
                case 1:
                    return GetStates.HIGH;
                default:
                    return GetStates.FAIL;
            }
        }

        public bool AnalogWrite(Pins pin, Int16 val)
        {
            if (val > 255) val = 255;
            if (val < 0) val = 0;

            Response result = Post(Functions.analogwrite,
                String.Format(payloadWrite,
                              HttpUtility.UrlEncode(AccessToken),
                              HttpUtility.UrlEncode(pin.ToString()), HttpUtility.UrlEncode(val.ToString())));

            switch (result.ReturnValue)
            {
                case -1:
                    return false;
                default:
                    return true;
            }
        }

        public int AnalogRead(Pins pin)
        {
            Response result = Post(Functions.analogread,
                String.Format(payloadRead, HttpUtility.UrlEncode(AccessToken), HttpUtility.UrlEncode(pin.ToString())));

            return result.ReturnValue;
        }

        private Response Post(Functions function, String payload)
        {
            HttpWebRequest request = GetHttpWebRequest(webMethod.POST, String.Format(addrTinkerAPICallFunction, this.CoreID, function.ToString()), payload);

            object jsonObj = GetHttpWebResponseAsJSONData(request, typeof(Response));
            Response jsonResponse = jsonObj as Response;

            // check for not connected
            if (jsonResponse.Connected == false)
            {
                throw new ApplicationException("SparkCore not connected.");
            }

            return jsonResponse;
        }
        #endregion
    }
}
