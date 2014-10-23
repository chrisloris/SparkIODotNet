using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices.JSON
{
    [DataContract]
    public class ResponseVar
    {
        [DataMember(Name = "cmd")]
        public string Command { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "result")]
        public string Result { get; set; }
        [DataMember(Name = "coreInfo")]
        public ResponseVarChild CoreInfo { get; set; }
    }
}
