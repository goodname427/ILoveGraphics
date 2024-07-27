using ILoveGraphics.Renderer;
using MatrixCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawForm
{
    internal class ImageScreenDrawer : IScreenDrawer
    {
        public string Path { get; set; }

        public ImageScreenDrawer(string path)
        {
            Path = path;
        }

        public void Draw(Vector4[,] frameBuffer, string message)
        {
            var image = new Bitmap(frameBuffer.GetLength(0), frameBuffer.GetLength(1));
            for (int x = 0; x < frameBuffer.GetLength(0); x++)
            {
                for (int y = 0; y < frameBuffer.GetLength(1); y++)
                {
                    image.SetPixel(x, frameBuffer.GetLength(1) - y - 1, frameBuffer[x, y].ToColor());
                }
            }

            image.Save(Path);
        }
    }
}
