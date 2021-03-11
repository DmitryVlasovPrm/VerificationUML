using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.entities {
    class DecisionNode : DiagramElement {
        private String question;
        private List<String> alternatives = new List<string>();     // хранит названия альтернатив

        public DecisionNode(String id, String inPartition, String question) : base(id, inPartition, question) {
            this.question = question;
        }

        public List<String> findEqualAlternatives() {
            List<String> equals = new List<string>();
            for (int i = 0; i < alternatives.Count - 1; i++) {
                for (int j = i + 1; j < alternatives.Count; j++) {
                    if (alternatives[i].Equals(alternatives[j]) && alternatives[i] != "")
                        equals.Add(alternatives[i]);
                }
            }
            return equals;
        }

        public bool findEmptyAlternative() {
            for (int i = 0; i < alternatives.Count; i++) {
                if (alternatives[i].Equals("")) return true;
            }
            return false;
        }

        public String getQuestion() {
            return question;
        }

        public void setQuestion(String question) {
            this.question = question;
        }

        public void addAlternative(String alternative) {
            alternatives.Add(alternative);
        }
        public String getAlternative(int index) {
            return alternatives[index];
        }
        public int alternativeSize() {
            return alternatives.Count;
        }
    }
}
