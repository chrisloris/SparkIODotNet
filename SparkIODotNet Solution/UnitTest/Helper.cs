using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparkIO.WebServices;

namespace SparkIO.UnitTest
{
    class Helper
    {
        public static CoreAPI GetCoreAPI()
        {
                CoreAPI core;

                AppSettingsReader reader = new AppSettingsReader();

                core = new CoreAPI(reader.GetValue("AccessToken", typeof(string)).ToString(),
                    reader.GetValue("CoreID", typeof(string)).ToString());

                return core;
        }

        public static string GetCoreID()
        { 
            AppSettingsReader reader = new AppSettingsReader();
            return reader.GetValue("CoreID", typeof(string)).ToString(); 
        }

        public static string GetCoreName()
        { 
            AppSettingsReader reader = new AppSettingsReader();
            return reader.GetValue("CoreName", typeof(string)).ToString(); 
        }

        public static string GetAccessToken()
        { 
            AppSettingsReader reader = new AppSettingsReader();
            return reader.GetValue("AccessToken", typeof(string)).ToString(); 
        }

        public static string GetUserName()
        { 
            AppSettingsReader reader = new AppSettingsReader();
            return reader.GetValue("UserName", typeof(string)).ToString(); 
        }

        public static string GetPassword()
        { 
            AppSettingsReader reader = new AppSettingsReader();
            return reader.GetValue("Password", typeof(string)).ToString(); 
        }
    }
}
