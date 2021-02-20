using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.entities
{
    class Swimlane: BaseNode
    {
        private String name;

        public Swimlane(String id, String name):base(id)
        {
            this.name = name;
        }

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }
    }
}
