using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.entities
{
    /**
 * Элемент, являющийся родительским для других узлов AD
 */
    public abstract class BaseNode
    {
        protected String id;
        protected ElementType type;
        public int x;
        public int y;
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            BaseNode baseNode = (BaseNode)obj;
            return id.Equals(baseNode.id);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
        public BaseNode(String id)
        {
            this.id = id;
        }

        //region Getter-Setter
        public String getId()
        {
            return id;
        }

        public void setId(String id)
        {
            this.id = id;
        }

        public ElementType getType()
        {
            return type;
        }

        public void setType(ElementType type)
        {
            this.type = type;
        }
        //endregion
    }

}
