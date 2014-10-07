using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparkIO.WebServices;

namespace DrivEmCoreAPI
{
    public class Helper
    {
        private CoreAPI core;
        private const string coreID = "abcdefabcdefabcdefabcdef";
        private const string accessToken = "abcdefabcdefabcdefabcdefabcdefabcdefabcd";

        public enum DriveCommands
        {
            STOP,
            BACK,
            BACKLEFT,
            BACKRIGHT,
            FORWARD,
            FORWARDLEFT,
            FORWARDRIGHT,
            RIGHT,
            LEFT
        }

        public Helper()
        {
            core = new CoreAPI(coreID, accessToken); //, new System.Net.WebProxy("127.0.0.1", 8888));
        }

        public void GiveCommand(DriveCommands command)
        {
            int iresult = core.CallFunctionInt("rccar", "rc," + command.ToString());
        }
    }
}
