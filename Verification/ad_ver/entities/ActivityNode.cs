using System;

namespace ActivityDiagramVer.entities
{
    public class ActivityNode : DiagramElement
    {
        private String name;    // содержит отображаемый на элементе текст

        public ActivityNode(String id, String inPartition, String name) : base(id, inPartition, name)
        {
            this.name = name;
        }
        /**
         * name содержит отображаемый на элементе текст
         */
        public String getName()
        {
            return name;
        }
    }
}
