using Verification.package_ver;

namespace ActivityDiagramVer.entities
{
    internal class Swimlane : BaseNode, IActor
    {
        private readonly string name;
        private int childCount = 0;
        public Swimlane(string id, string name) : base(id)
        {
            this.name = name;
        }

        public int ChildCount { get => childCount; set => childCount = value; }

        public string Name => name;

        public string getName()
        {
            return name;
        }
    }
}
