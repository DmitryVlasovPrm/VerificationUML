using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.entities {
    public class DiagramElement : BaseNode {
        protected String inPartition = "";
        protected List<string> idsOut = new List<string>();       // массив ид входящих переходов
        protected List<string> idsIn = new List<string>();        // массив ид выходящих переходов
        protected String description = "";

        public int petriId;


        public DiagramElement(String id, String inPartition, String description) : base(id) {
            this.inPartition = inPartition;
            this.description = description;
        }

        public String getInPartition() {
            return inPartition;
        }
        public String getDescription() {
            return description;
        }

        public void addIn(String allId) {
            String[] ids = allId.Split(' ');
            foreach (String id in ids) {
                if (!id.Equals("")) idsIn.Add(id);
            }
        }
        public void addOut(String allId) {
            String[] ids = allId.Split(' ');
            foreach (String id in ids) {
                if (!id.Equals("")) idsOut.Add(id);
            }
        }

        public String getInId(int index) {
            return idsIn[index];
        }

        public String getOutId(int index) {
            return idsOut[index];
        }

        public int inSize() {
            return idsIn.Count;
        }

        public int outSize() {
            return idsOut.Count;
        }
    }
}
