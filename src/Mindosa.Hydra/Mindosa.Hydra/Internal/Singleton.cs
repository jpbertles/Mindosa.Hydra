using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindosa.Hydra.Internal
{
    static class Singleton<T> where T : new()
    {
        public static T Instance = new T();
    }
}
