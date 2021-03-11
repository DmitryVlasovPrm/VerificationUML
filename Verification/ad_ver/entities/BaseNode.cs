using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.entities {
    /**
 * Элемент, являющийся родительским для других узлов AD
 */
    public abstract class BaseNode {
        protected String id;
        protected ElementType type;
        private int x = -1;
        private int y = -1;
        private int width = -1;
        private int height = -1;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public override bool Equals(object obj) {
            if (this == obj) return true;
            if (obj == null) return false;
            BaseNode baseNode = (BaseNode)obj;
            return id.Equals(baseNode.id);
        }

        public override int GetHashCode() {
            return id.GetHashCode();
        }
        public BaseNode(String id) {
            this.id = id;
        }

        //region Getter-Setter
        public String getId() {
            return id;
        }
        public ElementType getType() {
            return type;
        }

        public void setType(ElementType type) {
            this.type = type;
        }
        //endregion
    }

}
