using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.entities {
    class ForkNode : DiagramElement {
        public ForkNode(String id, String inPartition) : base(id, inPartition, "") {
        }
    }
}
