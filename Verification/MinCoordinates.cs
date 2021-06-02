using Emgu.CV;
using Emgu.CV.Structure;

namespace Verification
{
    public static class MinCoordinates
    {
        public static (int, int) Compute(Image<Bgra, byte> image)
        {
            var grayImage = image.Convert<Gray, byte>();
            var firstPixel = grayImage.Data[0, 0, 0];
            var threshold = firstPixel == 0 ? 0 : 255;
            var imageHeight = grayImage.Height;
            var imageWidth = grayImage.Width;

            var minX = imageWidth;
            for (var i = 0; i < imageHeight; i++)
                for (var j = 0; j < imageWidth / 4; j++)
                    if (grayImage.Data[i, j, 0] != threshold && j < minX)
                        minX = j;

            var minY = imageHeight;
            for (var i = 0; i < imageWidth; i++)
                for (var j = 0; j < imageHeight / 4; j++)
                    if (grayImage.Data[j, i, 0] != threshold && j < minY)
                        minY = j;

            return (minX, minY);
        }
    }
}
