namespace ActivityDiagramVer.entities
{
    internal class ControlFlow : BaseNode
    {
        private string src = "";
        private string targets = "";
        private readonly string text;
        public ControlFlow(string id) : base(id) { }

        public ControlFlow(string id, string text) : base(id)
        {
            this.text = text;
        }

        public string getText()
        {
            return text;
        }

        public string getSrc()
        {
            return src;
        }

        public void setSrc(string src)
        {
            this.src = src;
        }

        public string getTarget()
        {
            return targets;
        }

        public void setTarget(string targets)
        {
            this.targets = targets;
        }
    }
}
