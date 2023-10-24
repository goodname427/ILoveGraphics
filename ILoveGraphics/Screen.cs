using MatrixCore;
using System.Text;

namespace ILoveGraphics
{
    internal class Screen
    {
        private readonly PixelColor[,] _buff;

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; init; } = 100;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; init; } = 100;

        /// <summary>
        /// 视口变换
        /// </summary>
        public Matrix ViewportMatrix { get; }

        public Screen() : this(Console.WindowWidth / 2, Console.WindowHeight)
        {

        }
        public Screen(int width = 100, int height = 100)
        {
            Console.CursorVisible = false;
            Width = width;
            Height = height;

            _buff = new PixelColor[Width, Height];
            Clear();

            ViewportMatrix = new float[,]
            {
                {Width / 2, 0, 0, Width / 2},
                {0, Height / 2, 0, Height / 2 },
                { 0, 0, 1, 0},
                { 0, 0, 0, 1},
            };
        }

        /// <summary>
        /// 视口变换
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        private Vector4 ViewportTranformation(Vector4 vertex)
        {
            return (ViewportMatrix * vertex) / vertex.W;
        }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        private void Draw(int x, int y, PixelColor color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return;

            _buff[x, y] = color;
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    _buff[x, y] = new PixelColor(0, ConsoleColor.White);
        }
        /// <summary>
        /// 输出
        /// </summary>
        public void Output()
        {
            Console.SetCursorPosition(0, 0);
            var output = new StringBuilder();
            var color = ConsoleColor.White;
            
            // 将颜色相同（或者透明）的字符一次性输出
            for (int y = Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_buff[x, y].ConsoleColor == color || _buff[x, y].Alpha == 0)
                    {
                        output.Append(_buff[x, y].ConsoleChar);
                        output.Append(' ');
                    }
                    else
                    {
                        Console.ForegroundColor = color;
                        Console.Write(output);
                        color = _buff[x, y].ConsoleColor;
                        output.Clear();
                    }
                }
                if(y != 0)
                    output.Append('\n');
            }

            Console.ForegroundColor = color;
            Console.Write(output);
        }
        /// <summary>
        /// 光栅化
        /// </summary>
        /// <param name="vertexes"></param>
        /// <param name="triangles"></param>
        public void Rasterize(Vector4 a, Vector4 b, Vector4 c, PixelColor color)
        {
            // 视口变换
            a = ViewportTranformation(a);
            b = ViewportTranformation(b);
            c = ViewportTranformation(c);

            // 获取bounding box
            var left = MathF.Min(a.X, MathF.Min(b.X, c.X));
            var right = MathF.Max(a.X, MathF.Max(b.X, c.X));
            var bottom = MathF.Min(a.Y, MathF.Min(b.Y, c.Y));
            var top = MathF.Max(a.Y, MathF.Max(b.Y, c.Y));

            // 判断点是否在三角面中
            var ab = b - a;
            var bc = c - b;
            var ca = a - c;
            for (int x = (int)left; x <= right; x++)
            {
                for (int y = (int)bottom; y <= top; y++)
                {
                    var p = new Vector4(x + 0.5f, y + 0.5f, 0);
                    var ap = p - a;
                    var bp = p - b;
                    var cp = p - c;

                    var dir1 = MathF.Sign(Vector4.Cross(ab, ap).Z);
                    var dir2 = MathF.Sign(Vector4.Cross(bc, bp).Z);
                    var dir3 = MathF.Sign(Vector4.Cross(ca, cp).Z);

                    // 在三角形里面
                    if (dir1 == dir2 && dir2 == dir3)
                    {
                        Draw(x, y, color);
                    }
                }
            }
        }
    }
}
