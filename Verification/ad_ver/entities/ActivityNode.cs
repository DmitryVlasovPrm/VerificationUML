namespace ActivityDiagramVer.entities
{
    public class ActivityNode : DiagramElement
    {
        private readonly string name;    // содержит отображаемый на элементе текст

        public ActivityNode(string id, string inPartition, string name) : base(id, inPartition, name)
        {
            this.name = name;
        }
        /**
         * name содержит отображаемый на элементе текст
         */
        public string getName()
        {
            return name;
        }
    }
}
