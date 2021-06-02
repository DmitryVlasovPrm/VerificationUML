using System;
using Verification.rating_system;

namespace Verification
{
    public class Mistake
    {
        public Guid Id;
        public int Seriousness;
        public string Text;
        public BoundingBox Bbox;
        public ALL_MISTAKES type;

        public Mistake(int seriousness, string text, BoundingBox bbox, ALL_MISTAKES type)
        {
            Id = Guid.NewGuid();
            Seriousness = seriousness;
            Text = text;
            Bbox = bbox;
            this.type = type;
        }
    }
}
