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
        Reader reader;
        XmlNode root;
        public VerificatorUC(XmlNode root)
        {
            reader = new Reader(elements);
            this.root = root.FirstChild;
        }

        public void Verificate()
        {
            reader.ReadData(root);
        }
    }
}
