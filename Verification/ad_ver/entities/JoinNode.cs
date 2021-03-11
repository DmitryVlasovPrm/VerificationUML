using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.entities {
    class JoinNode : DiagramElement {
        public JoinNode(String id, String inPartition) : base(id, inPartition, "") {
        }
    }
}
