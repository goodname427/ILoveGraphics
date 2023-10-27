using ILoveGraphics.Renderer.ScreenDrawer;
using MatrixCore;
using System.Text;

namespace ILoveGraphics.Renderer
{
    internal class Screen
    {
        private readonly Vector4[,] _frameBuffer;
        private readonly float[,] _zBuffer;

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; init; } = 100;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; init; } = 100;
        /// <summary>
        /// 屏幕绘制器
        /// </summary>
        public IScreenDrawer ScreenDrawer { get; } 
        /// <summary>
        /// 视口变换
        /// </summary>
        public Matrix ViewportMatrix { get; }

        public Screen() : this(new ConsoleScreenDrawer(), Console.WindowWidth / 2, Console.WindowHeight)
        {

        }
        public Screen(IScreenDrawer screenDrawer) : this(screenDrawer, Console.WindowWidth / 2, Console.WindowHeight)
        {

        }
        public Screen(IScreenDrawer screenDrawer, int width, int height)
        {
            ScreenDrawer = screenDrawer;
            Width = width;
            Height = height;

            _frameBuffer = new Vector4[Width, Height];
            _zBuffer = new float[Width, Height];
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
        /// 光栅化
        /// </summary>
        /// <param name="vertexes"></param>
        /// <param name="triangles"></param>
        public void Rasterize(Vector4 a, Vector4 b, Vector4 c, Vector4 color)
        {
            // 获取bounding box
            var left = MathF.Max(MathTool.Min(a.X, b.X, c.X), 0);
            var right = MathF.Min(MathTool.Max(a.X, b.X, c.X), Width - 1);
            var bottom = MathF.Max(MathTool.Min(a.Y, b.Y, c.Y), 0);
            var top = MathF.Min(MathTool.Max(a.Y, b.Y, c.Y), Height - 1);

            // 三角形三边
            var ab = b - a;
            var bc = c - b;
            var ca = a - c;
            // 法线
            var n = Vector4.Cross(ab, bc);
            for (int x = (int)left; x <= right; x++)
            {
                for (int y = (int)bottom; y <= top; y++)
                {
                    // inside
                    var p = new Vector4(x + 0.5f, y + 0.5f, a.Z);
                    var ap = p - a;
                    var bp = p - b;
                    var cp = p - c;
                    var dir1 = MathF.Sign(Vector4.Cross(ab, ap).Z);
                    var dir2 = MathF.Sign(Vector4.Cross(bc, bp).Z);
                    var dir3 = MathF.Sign(Vector4.Cross(ca, cp).Z);

                    // z buffer
                    var z = a.Z - n * ap / n.Z;

                    // 采样
                    if (dir1 == dir2 && dir2 == dir3 && z > _zBuffer[x, y])
                    {
                        _frameBuffer[x, y] = color;
                        _zBuffer[x, y] = z;
                    }
                }
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _frameBuffer[x, y] = Vector4.Zero;
                    _zBuffer[x, y] = float.MinValue;
                }
            }


        }
        /// <summary>
        /// 将frame buffer输出
        /// </summary>
        public void Draw(string message = "")
        {
            ScreenDrawer.Draw(_frameBuffer, message);
        }
    }
}
