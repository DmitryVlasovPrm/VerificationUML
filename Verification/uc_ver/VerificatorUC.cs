using System;
using System.Collections.Generic;
using System.Linq;

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

            if (diagram.Image != null)
                FixCoordinates();

            checker.Check();
        }

        private void FixCoordinates()
        {
            var minX = elements.Min(e => e.Value.X);
            var minY = elements.Min(e => e.Value.Y);
            var (realMinX, realMinY) = MinCoordinates.Compute(diagram.Image);

            var diffX = realMinX - minX;
            var diffY = realMinY - minY;

            foreach (var element in elements)
            {
                element.Value.X += diffX;
                element.Value.Y += diffY;
            }
        }
    }
}
