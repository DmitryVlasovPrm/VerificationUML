using System;
using System.Xml;
using System.Collections.Generic;

namespace Verification
{
    public class Diagram
    {
        public string Name;
        public string PathToXmi;
        public string PathToPng;
        public XmlElement XmlInfo;
        public List<Mistake> Mistakes;

        public Diagram(string name, string pathToXmi, string pathToPng, XmlElement xmlInfo)
        {
            Name = name;
            PathToXmi = pathToXmi;
            PathToPng = pathToPng;
            XmlInfo = xmlInfo;
            Mistakes = new List<Mistake>();
        }
    }
}
