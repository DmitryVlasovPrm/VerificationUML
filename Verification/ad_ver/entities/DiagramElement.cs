using System.Collections.Generic;

namespace ActivityDiagramVer.entities
{
    public class DiagramElement : BaseNode
    {
        protected string inPartition = "";
        protected List<string> idsOut = new List<string>();       // массив ид входящих переходов
        protected List<string> idsIn = new List<string>();        // массив ид выходящих переходов
        protected string description = "";

        public int petriId;


        public DiagramElement(string id, string inPartition, string description) : base(id)
        {
            this.inPartition = inPartition;
            this.description = description;
        }

        public string getInPartition()
        {
            return inPartition;
        }
        public string getDescription()
        {
            return description;
        }

        public void addIn(string allId)
        {
            string[] ids = allId.Split(' ');
            foreach (string id in ids)
            {
                if (!id.Equals("")) idsIn.Add(id);
            }
        }
        public void addOut(string allId)
        {
            string[] ids = allId.Split(' ');
            foreach (string id in ids)
            {
                if (!id.Equals("")) idsOut.Add(id);
            }
        }

        public string getInId(int index)
        {
            return idsIn[index];
        }

        public string getOutId(int index)
        {
            return idsOut[index];
        }

        public int inSize()
        {
            return idsIn.Count;
        }

        public int outSize()
        {
            return idsOut.Count;
        }
    }
}
