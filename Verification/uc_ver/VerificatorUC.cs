using System.Collections.Generic;

namespace Verification.uc_ver
{
    internal class VerificatorUC
    {
        private readonly Dictionary<string, Element> elements;
        private readonly Reader reader;
        private readonly Checker checker;
        private readonly Diagram diagram;
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
