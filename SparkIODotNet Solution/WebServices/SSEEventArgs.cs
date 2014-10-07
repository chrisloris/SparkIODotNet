using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices
{
    public class SSEEventArgs : EventArgs
    {
        #region Fields
        public string EventName { get; set; }
        public string Data { get; set; }
        public int TTL { get; set; }
        public DateTime PublishedAt { get; set; }
        public string CoreID { get; set; }
        #endregion Fields

        #region Constructors
        public SSEEventArgs(string eventName, string data, int ttl, DateTime publishedAt, string coreID)
        {
            EventName = eventName;
            Data = data;
            TTL = ttl;
            PublishedAt = publishedAt;
            CoreID = coreID;
        }
        #endregion Constructors
    }
}
