using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices.JSON
{
    [DataContract]
    public class TokenInfo
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
        [DataMember(Name = "expires_at")]
        public string ExpiresAt { get; set; }
        [DataMember(Name = "client")]
        public string Client { get; set; }
    }
}
