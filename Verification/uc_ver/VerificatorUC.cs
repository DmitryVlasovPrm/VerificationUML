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
        private static Dictionary<string, Element> elements = new Dictionary<string, Element>();
        private Reader reader;
        private Diagram diagram;
        public VerificatorUC(Diagram diagram)
        {
            reader = new Reader(elements, diagram.Mistakes);
            this.diagram = diagram;
        }

        public void Verificate()
        {
            reader.ReadData(diagram.XmlInfo);
        }
    }
}
