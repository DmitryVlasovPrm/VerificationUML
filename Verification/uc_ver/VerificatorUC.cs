using System.Collections.Generic;

namespace Verification.uc_ver
{
    class VerificatorUC
    {
        private Dictionary<string, Element> elements;
        private Reader reader;
        private Checker checker;
        private Diagram diagram;
        public VerificatorUC(Diagram diagram)
        {
            elements = new Dictionary<string, Element>();
            reader = new Reader(elements, diagram);
            checker = new Checker(elements, diagram.Mistakes);
            this.diagram = diagram;
        }

        public void Verificate()
        {
            reader.ReadData(diagram.XmlInfo);
            checker.Check();
        }
    }
}
