using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindosa.Hydra.Attributes
{
    public class CustomMappingAttribute : ColumnAttribute
    {
        public Type Mapper { get; private set; }

        public CustomMappingAttribute(string name, Type mapper )
            : base(name)
        {
            this.Mapper = mapper;
        }
    }
}
