using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindosa.Hydra.Internal;

namespace Mindosa.Hydra.Tests.Infrastructure
{
    public class IdMapper : ICustomMapper
    {

        public Func<object, object> GetMapper()
        {
            return x => "Id: " + x;
        }
    }
}
