namespace Verification.uc_ver
{
    public static class UCMistakeFactory
    {
        public static Mistake Create(int seriousness, string text, Element element)
        {
            var bbox = new BoundingBox(element.X, element.Y, element.W, element.H);
            return new Mistake(seriousness, text, bbox);
        }

        public static Mistake Create(int seriousness, string text)
        {
            var bbox = new BoundingBox(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
            return new Mistake(seriousness, text, bbox);
        }
    }
}
