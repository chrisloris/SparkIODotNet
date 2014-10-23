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

        #region Constructors
        public CoreAPI(String coreID, String accessToken) :
            base(coreID, accessToken)
        {}

        public CoreAPI(String coreID, String accessToken, WebProxy webProxy) :
            base(coreID, accessToken, webProxy)
        {}
        #endregion

        #region Main Functions - Get Variable
        public string GetVariableString(string varName)
        {
            HttpWebRequest request = GetHttpWebRequest(webMethod.GET, String.Format(addrCoreAPIGetVariable, this.CoreID, varName, AccessToken), string.Empty);

            object jsonObj = GetHttpWebResponseAsJSONData(request, typeof(ResponseVar));
            ResponseVar jsonResponse = jsonObj as ResponseVar;

            return jsonResponse.Result;
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
        public string CallFunctionString(string funcName, string args)
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

            return jsonResponse.ReturnValue;
        }

        public int CallFunctionInt(string funcName, string args)
        {
            int result;

            string resultString = CallFunctionString(funcName, args);

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

        public double CallFunctionDouble(string funcName, string args)
        {
            double result;

            string resultString = CallFunctionString(funcName, args);

            if (double.TryParse(resultString, out result))
            {
                return result;
            }
            else
            {
                String message = String.Format("Request for function {0} failed to convert value '{1}' to double.", funcName, resultString);
                throw new ApplicationException(message);
            }
        }

        public bool CallFunctionBoolean(string funcName, string args)
        {
            bool result;

            string resultString = CallFunctionString(funcName, args);

            if (bool.TryParse(resultString, out result))
            {
                return result;
            }
            else
            {
                String message = String.Format("Request for function {0} failed to convert value '{1}' to bool.", funcName, resultString);
                throw new ApplicationException(message);
            }
        }
        #endregion

    }
}
