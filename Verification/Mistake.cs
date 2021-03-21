using System;

namespace Verification
{
    public class Mistake
    {
        public Guid Id;
        public int Seriousness;
        public string Text;
        public BoundingBox Bbox;

        public Mistake(int seriousness, string text, BoundingBox bbox)
        {
            Id = Guid.NewGuid();
            Seriousness = seriousness;
            Text = text;
            Bbox = bbox;
        }
    }
}
