using ActivityDiagramVer.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification.ad_ver.entities
{
    class UnknownNode : BaseNode
    {
        public UnknownNode(string id) : base(id) { }
        public UnknownNode(string id, int x, int y, int width, int height) : base(id)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

    }
}
