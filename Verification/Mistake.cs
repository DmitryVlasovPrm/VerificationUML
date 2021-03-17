using System;

namespace Verification
{
    public class Mistake
    {
        private Guid Id { get; set; }
        public int Seriousness { get; set; }
        public string Text { get; set; }
        private BoundingBox Bbox { get; set; }

        public Mistake(int seriousness, string text, BoundingBox bbox)
        {
            Id = new Guid();
            Seriousness = seriousness;
            Text = $"{MistakesTypes.Strings[seriousness]}: {text}";
            Bbox = bbox;
        }
    }
}
