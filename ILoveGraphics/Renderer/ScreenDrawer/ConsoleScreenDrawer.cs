using MatrixCore;
using System.Text;

namespace ILoveGraphics.Renderer.ScreenDrawer
{
    public class ConsoleScreenDrawer : IScreenDrawer
    {
        public ConsoleScreenDrawer()
        {
            Console.CursorVisible = false;
        }

        public void Draw(Vector4[,] frameBuffer, string message)
        {
            var width = frameBuffer.GetLength(0);
            var height = frameBuffer.GetLength(1);
            Console.SetCursorPosition(0, 0);
            
            var output = new StringBuilder();
            var color = ConsoleColor.White;

            // 将颜色相同（或者透明）的字符一次性输出
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixelColor = PixelColor.Parse(frameBuffer[x, y]);
                    if (pixelColor.Gray == 0 || pixelColor.ConsoleColor == color)
                    {
                        output.Append(pixelColor.ConsoleChar);
                        output.Append(' ');
                    }
                    else
                    {
                        Console.ForegroundColor = color;
                        Console.Write(output.ToString());
                        color = pixelColor.ConsoleColor;
                        output.Clear();
                    }
                }
                if (y != 0)
                    output.Append('\n');
            }

            Console.ForegroundColor = color;
            Console.Write(output.ToString());

            Console.SetCursorPosition(0, 0);
            Console.Write(message);
        }
    }
}
