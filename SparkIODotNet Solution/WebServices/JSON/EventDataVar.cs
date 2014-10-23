using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices.JSON
{
    [DataContract]
    public class EventDataVar
    {
        [DataMember(Name = "data")]
        public string Data { get; set; }
        [DataMember(Name = "ttl")]
        public int TTL { get; set; }
        [DataMember(Name = "published_at")]
        public string PublishedAt { get; set; }
        [DataMember(Name = "coreid")]
        public string CoreID { get; set; }
    }
}
