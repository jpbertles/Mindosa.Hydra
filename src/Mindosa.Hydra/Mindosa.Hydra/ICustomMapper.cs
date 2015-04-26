using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindosa.Hydra
{
    public interface ICustomMapper
    {
        Func<object, object> GetMapper();
    }
}
