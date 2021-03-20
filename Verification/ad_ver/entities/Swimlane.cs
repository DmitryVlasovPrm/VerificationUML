using System;
using Verification.package_ver;

namespace ActivityDiagramVer.entities
{
    class Swimlane : BaseNode, IActor
    {
        private String name;
        private int childCount = 0;
        public Swimlane(String id, String name) : base(id)
        {
            this.name = name;
        }

        public int ChildCount { get => childCount; set => childCount = value; }

        public string Name => name;

        public String getName()
        {
            return name;
        }
    }
}
