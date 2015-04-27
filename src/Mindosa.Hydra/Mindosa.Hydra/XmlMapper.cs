using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mindosa.Hydra
{
    public class XmlMapper<T> : ICustomMapper
    {
        public Func<object, object> GetMapper()
        {
            return x =>
            {
                using (var reader = new StringReader(x.ToString()))
                    return (T)(new XmlSerializer(typeof(T)).Deserialize(reader));
            };
        }
    }
}
