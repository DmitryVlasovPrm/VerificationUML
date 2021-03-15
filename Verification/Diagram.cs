using Emgu.CV;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Xml;

namespace Verification
{
    public class Diagram
    {
        public string Name;
        public XmlElement XmlInfo;
        public Image<Bgra, byte> Image;
        public List<Mistake> Mistakes;
        public bool Verificated { get; set; }

        public Diagram(string name, XmlElement xmlInfo, Image<Bgra, byte> image)
        {
            Name = name;
            XmlInfo = xmlInfo;
            Image = image;
            Mistakes = new List<Mistake>();
            Verificated = false;
        }
    }
}
