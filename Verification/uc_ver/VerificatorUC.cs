using System;
using System.Collections.Generic;
using System.Linq;

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
            var minX = elements.Min(e => e.Value.X);
            var minY = elements.Min(e => e.Value.Y);
            var tuple = MinCoordinates.Compute(diagram.Image);
            var diffX = Math.Abs(minX - tuple.Item1)/2;
            var diffY = Math.Abs(minY - tuple.Item2)/2;

            foreach (var element in elements)
            {
                element.Value.X += diffX;
                element.Value.Y += diffY;
            }

            checker.Check();
        }
    }
}
