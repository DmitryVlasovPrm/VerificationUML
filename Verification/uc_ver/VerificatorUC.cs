using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Verification.uc_ver
{
    class VerificatorUC
    {
        private Dictionary<string, Element> elements;
        private Reader reader;
        private Diagram diagram;
        public VerificatorUC(Diagram diagram)
        {
            elements = new Dictionary<string, Element>();
            reader = new Reader(elements, diagram.Mistakes);
            this.diagram = diagram;
        }

        public void Verificate()
        {
            reader.ReadData(diagram.XmlInfo);
        }
    }
}
