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

                core = new CoreAPI(reader.GetValue("coreID", typeof(string)).ToString(),
                    reader.GetValue("accessToken", typeof(string)).ToString());

                return core;
        }
    }
}
