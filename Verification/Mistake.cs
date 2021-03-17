using System;
using Verification.type_definer;

namespace Verification
{
    public class Mistake
    {
        private Guid Id { get; set; }
        private EDiagramTypes EType { get; set; }
        public int Seriousness { get; set; }
        public string Text { get; set; }
        private BoundingBox Bbox { get; set; }

        public Mistake(EDiagramTypes eType, int seriousness, string text, BoundingBox bbox)
        {
            Id = new Guid();
            EType = eType;
            Seriousness = seriousness;
            Text = $"{MistakesTypes.Strings[seriousness]}: {text}";
            Bbox = bbox;
        }
    }
}
