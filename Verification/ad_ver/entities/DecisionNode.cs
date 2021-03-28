using System.Collections.Generic;

namespace ActivityDiagramVer.entities
{
    internal class DecisionNode : DiagramElement
    {
        private string question;
        private readonly List<string> alternatives = new List<string>();     // хранит названия альтернатив

        public DecisionNode(string id, string inPartition, string question) : base(id, inPartition, question)
        {
            this.question = question;
        }

        public List<string> findEqualAlternatives()
        {
            List<string> equals = new List<string>();
            for (int i = 0; i < alternatives.Count - 1; i++)
            {
                for (int j = i + 1; j < alternatives.Count; j++)
                {
                    if (alternatives[i].Equals(alternatives[j]) && alternatives[i] != "")
                        equals.Add(alternatives[i]);
                }
            }
            return equals;
        }

        public bool findEmptyAlternative()
        {
            for (int i = 0; i < alternatives.Count; i++)
            {
                if (alternatives[i].Equals("")) return true;
            }
            return false;
        }

        public string getQuestion()
        {
            return question;
        }

        public void setQuestion(string question)
        {
            this.question = question;
        }

        public void addAlternative(string alternative)
        {
            alternatives.Add(alternative);
        }
        public string getAlternative(int index)
        {
            return alternatives[index];
        }
        public int alternativeSize()
        {
            return alternatives.Count;
        }
    }
}
