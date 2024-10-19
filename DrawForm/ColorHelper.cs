using ILoveGraphics.Resources;
using MatrixCore;

namespace DrawForm
{
    internal static class ColorHelper
    {
        public static Vector4 ToVector4(this Color color)
        {
            return new(color.R / 255f, color.G / 255f, color.B / 255f);
        }

        public static Color ToColor(this Vector4 color)
        {
            color.X = MathTool.Clamp(color.X, 0, 1);
            color.Y = MathTool.Clamp(color.Y, 0, 1);
            color.Z = MathTool.Clamp(color.Z, 0, 1);
            color *= 255;
            
            return Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z);
        }

        public static Texture GetTexture(string path)
        {
            var image = new Bitmap(Image.FromFile(path));
            var rawData = new Vector4[image.Width, image.Height];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    rawData[i, j] = image.GetPixel(i, j).ToVector4();
                }
            }

            return new Texture(rawData);
        }
    }
}
