using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification.uc_ver
{
    class Arrow : Element
    {
        public string From { get; set; }
        public string To { get; set; }

        public Arrow(string id, string type, string name, string parent, string from, string to) : base(id, type, name, parent)
        {
            From = from;
            To = to;
        }
    }
}
