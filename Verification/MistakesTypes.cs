using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification
{
    static class MistakesTypes
    {
        public const int
            WARNING = 1,
            ERROR = 2,
            FATAL = 3;


        public static Dictionary<int, string> Strings = new Dictionary<int, string>()
        {
            {WARNING, "[WARNING]" },
            {ERROR, "[ERROR]" },
            {FATAL, "[FATAL]" }
        };
    }
}
