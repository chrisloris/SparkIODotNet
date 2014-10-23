using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices.JSON
{
    [DataContract]
    public class DeviceInfo
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "connected")]
        public string LastApp { get; set; }
        [DataMember(Name = "variables")]
        public DictStringString Variables { get; set; }
        [DataMember(Name = "functions")]
        public string[] Functions { get; set; }
        [DataMember(Name = "cc3000_patch_version")]
        public string CC3000PatchVersion { get; set; }
    }
}
