using System;

namespace ActivityDiagramVer.entities
{
    class ControlFlow : BaseNode
    {
        private String src = "";
        private String targets = "";
        private String text;
        public ControlFlow(String id) : base(id) { }

        public ControlFlow(String id, String text) : base(id)
        {
            this.text = text;
        }

        public String getText()
        {
            return text;
        }

        public String getSrc()
        {
            return src;
        }

        public void setSrc(String src)
        {
            this.src = src;
        }

        public String getTarget()
        {
            return targets;
        }

        public void setTarget(String targets)
        {
            this.targets = targets;
        }
    }
}
