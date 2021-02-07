using System;

namespace Verification
{
    public class Mistake
    {
        public Guid Id;
        public int Type;
        public int Seriousness;
        public string Text;
        public int X;
        public int Y;
        public int W;
        public int H;

        public Mistake(int type, int seriousness, string text, int x, int y, int w, int h)
        {
            Id = new Guid();
            Type = type;
            Seriousness = seriousness;
            Text = text;
            X = x;
            Y = y;
            W = w;
            H = h;
        }
    }
}
