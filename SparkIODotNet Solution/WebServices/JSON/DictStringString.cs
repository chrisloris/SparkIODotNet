using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices.JSON
{
    [Serializable]
    public class DictStringString : ISerializable
    {
        public Dictionary<string, string> dict;
        public DictStringString()
        {
            dict = new Dictionary<string, string>();
        }
        protected DictStringString(SerializationInfo info, StreamingContext context)
        {
            dict = new Dictionary<string, string>();
            foreach (var entry in info)
            {
                dict.Add(entry.Name, entry.Value.ToString());
            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (string key in dict.Keys)
            {
                info.AddValue(key, dict[key]);
            }
        }
    }  
}
