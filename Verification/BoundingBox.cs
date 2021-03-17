namespace Verification
{
	public class BoundingBox
	{
		private int X { get; set; }
		private int Y { get; set; }
		private int W { get; set; }
		private int H { get; set; }

		public BoundingBox(int x, int y, int w, int h)
		{
			X = x;
			Y = y;
			W = w;
			H = h;
		}
	}
}
