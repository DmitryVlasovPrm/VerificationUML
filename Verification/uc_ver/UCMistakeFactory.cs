using System;
using Verification.type_definer;

namespace Verification.uc_ver
{
    public static class UCMistakeFactory
    {
        public static Mistake Create(int seriousness, string text, Element element)
        {
            return new Mistake(EDiagramTypes.UCD, seriousness, text, element.X, element.Y, element.W, element.H);
        }

        public static Mistake Create(int seriousness, string text)
        {
            return new Mistake(EDiagramTypes.UCD, seriousness, text, -1, -1, -1, -1);
        }
    }
}
