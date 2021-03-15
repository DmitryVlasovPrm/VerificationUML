using System;

namespace ActivityDiagramVer.entities
{
    class Swimlane : BaseNode
    {
        private String name;
        private int childCount = 0;
        public Swimlane(String id, String name) : base(id)
        {
            this.name = name;
        }

        public int ChildCount { get => childCount; set => childCount = value; }

        public String getName()
        {
            return name;
        }
    }
}
