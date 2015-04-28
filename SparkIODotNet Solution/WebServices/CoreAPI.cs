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
    public class CoreAPI : BaseAPI
    {
        #region Variables & Constants
        private const string addrCoreAPICallFunction = sparkURL + "/devices/{0}/{1}"; // {0} - Core ID {1} - function
        private const string addrCoreAPIGetVariable = sparkURL + "/devices/{0}/{1}?access_token={2}"; // {0} - Core ID {1} - variable name {2} - access token
        private const string payloadCoreAPICallFunction = "access_token={0}&args={1}"; // {0} = Access Token {1} - args
        #endregion

        #region Delegates
        public delegate ResponseFunc CallFunctionRawDelegate(string funcName, string args);
        public delegate int CallFunctionIntDelegate(string funcName, string args);
        #endregion

        #region Constructors
        public CoreAPI(String accessToken, String coreID, WebProxy webProxy = null,
                       bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress) :
            base(accessToken, coreID, webProxy, acceptAllCertificates, urlSparkCloud)
        {}

        public CoreAPI(String userName, String password, String coreID, WebProxy webProxy = null,
                       bool acceptAllCertificates = false, string urlSparkCloud = sparkAddress) :
            base(userName, password, coreID, webProxy, acceptAllCertificates, urlSparkCloud)
        { }
        #endregion

        #region Main Functions - Get Variable
        public ResponseVar GetVariableRaw(string varName)
        {
            HttpWebRequest request = GetHttpWebRequest(webMethod.GET, String.Format(addrCoreAPIGetVariable, this.CoreID, varName, AccessToken), string.Empty);

            object jsonObj = GetHttpWebResponseAsJSONData(request, typeof(ResponseVar));
            ResponseVar jsonResponse = jsonObj as ResponseVar;

            return jsonResponse;
        }

        public string GetVariableString(string varName)
        {
            return GetVariableRaw(varName).Result;
        }

        public int GetVariableInt(string varName)
        {
            int result;

            string resultString = GetVariableString(varName);

            if (int.TryParse(resultString, out result))
            {
                return result;
            }
            else
            {
                String message = String.Format("Request for variable {0} failed to convert value '{1}' to int.", varName, resultString);
                throw new ApplicationException(message);
            }
        }

        public double GetVariableDouble(string varName)
        {
            double result;

            string resultString = GetVariableString(varName);

            if (double.TryParse(resultString, out result))
            {
                return result;
            }
            else
            {
                String message = String.Format("Request for variable {0} failed to convert value '{1}' to double.", varName, resultString);
                throw new ApplicationException(message);
            }
        }

        public bool GetVariableBoolean(string varName)
        {
            bool result;

            string resultString = GetVariableString(varName);

            if (bool.TryParse(resultString, out result))
            {
                return result;
            }
            else
            {
                String message = String.Format("Request for variable {0} failed to convert value '{1}' to boolean.", varName, resultString);
                throw new ApplicationException(message);
            }
        }
        #endregion

        #region Main Functions - Call Function
        public ResponseFunc CallFunctionRaw(string funcName, string args)
        {
            String payload =  String.Format(payloadCoreAPICallFunction,
                              HttpUtility.UrlEncode(AccessToken),
                              HttpUtility.UrlEncode(args));

            HttpWebRequest request = GetHttpWebRequest(webMethod.POST, String.Format(addrCoreAPICallFunction, this.CoreID, funcName), payload);

            object jsonObj = GetHttpWebResponseAsJSONData(request, typeof(ResponseFunc));
            ResponseFunc jsonResponse = jsonObj as ResponseFunc;

            // check for not connected
            if (jsonResponse.Connected == false)
            {
                throw new ApplicationException("SparkCore not connected.");
            }

            return jsonResponse;
        }
        
        public int CallFunctionInt(string funcName, string args)
        {
            int result;

            string resultString = CallFunctionRaw(funcName, args).ReturnValue;

            if (int.TryParse(resultString, out result))
            {
                return result;
            }
            else
            {
                String message = String.Format("Request for function {0} failed to convert value '{1}' to int.", funcName, resultString);
                throw new ApplicationException(message);
            }
        }
        #endregion

    }
}
