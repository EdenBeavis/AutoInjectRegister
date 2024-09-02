using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInject
{
    public enum Lifetime
    {
        Transient,
        Scoped,
        Singleton
    }
}