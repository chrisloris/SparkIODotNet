using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices.JSON
{
    [DataContract]
    public class ResponseFunc
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
        public string ReturnValue { get; set; }
    }
}
