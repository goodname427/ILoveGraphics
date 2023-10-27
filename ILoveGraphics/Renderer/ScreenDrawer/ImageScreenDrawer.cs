using MatrixCore;
using Aspose.Imaging;
using Aspose.Imaging.FileFormats.Wmf;
using Aspose.Imaging.FileFormats.Wmf.Graphics;

namespace ILoveGraphics.Renderer.ScreenDrawer
{
    internal class ImageScreenDrawer : IScreenDrawer
    {
        public ImageScreenDrawer()
        {

            using (Image image = Image.Load("path/to/your/image.jpg"))
            {
                // Your code for displaying the image goes here
            }


        }

        public void Draw(Vector4[,] frameBuffer, string message)
        {
            var windth = frameBuffer.GetLength(0);
            var height = frameBuffer.GetLength(1);
            for (var x = 0; x < windth; x++)
            {

            }
        }
    }
}
