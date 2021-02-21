using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification.uc_ver
{
    class ErrorTypes
    {
        public const string
            WARNING = "[WARNING]",
            ERROR = "[ERROR]",
            FATAL = "[FATAL]";


        public static string[] List =
            { WARNING, ERROR, FATAL };
    }
}
