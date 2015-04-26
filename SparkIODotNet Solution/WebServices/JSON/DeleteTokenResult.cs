using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices.JSON
{
    [DataContract]
    class DeleteTokenResult
    {
        [DataMember(Name = "ok")]
        public bool ok { get; set; }
    }
}
