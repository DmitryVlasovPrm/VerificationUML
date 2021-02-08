using System.Xml;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Verification
{
    public class Diagram
    {
        public string Name;
        public XmlElement XmlInfo;
        public Image<Bgra, byte> Image;
        public List<Mistake> Mistakes;

        public Diagram(string name, XmlElement xmlInfo, Image<Bgra, byte> image)
        {
            Name = name;
            XmlInfo = xmlInfo;
            Image = image;
            Mistakes = new List<Mistake>();
        }
    }
}
