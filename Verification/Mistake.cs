using System;
using Verification.type_definer;

namespace Verification
{
    public class Mistake
    {
        private Guid Id { get; set; }

        [Obsolete("This field is obsolete. Use NewProperty instead.", false)]
        private EDiagramTypes EType { get; set; }
        private int Seriousness { get; set; }
        private string Text { get; set; }
        private int X { get; set; }
        private int Y { get; set; }
        private int W { get; set; }
        private int H { get; set; }

        [Obsolete]
        public Mistake(EDiagramTypes eType, int seriousness, string text, int x, int y, int w, int h)
        {
            Id = new Guid();
            EType = eType;
            Seriousness = seriousness;
            Text = $"{MistakesTypes.Strings[seriousness]}: {text}";
            X = x;
            Y = y;
            W = w;
            H = h;
        }
    }
}
