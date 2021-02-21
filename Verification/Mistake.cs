using System;
using Verification.type_definer;

namespace Verification
{
    public class Mistake
    {
        public Guid Id;

        [Obsolete("This field is obsolete. Use NewProperty instead.", false)]
        public int Type;
        public EDiagramTypes eType;     // Добавлен enum вместо 

        public int Seriousness;
        public string Text;
        public int X;
        public int Y;
        public int W;
        public int H;

        [Obsolete("This constructor is obsolete. Use another instead.", false)]
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
        public Mistake(EDiagramTypes eType, int seriousness, string text, int x, int y, int w, int h)
        {
            Id = new Guid();
            this.eType = eType;
            Seriousness = seriousness;
            Text = text;
            X = x;
            Y = y;
            W = w;
            H = h;
        }
    }
}
