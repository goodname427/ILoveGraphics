using ILoveGraphics.Object;
using ILoveGraphics.Renderer.ScreenDrawer;
using ILoveGraphics.Shader;
using MatrixCore;
using System.ComponentModel;

namespace ILoveGraphics.Renderer
{
    public class Screen
    {
        /// <summary>
        /// 判断一个点是否在三角形里面
        /// </summary>
        /// <param name="v"></param>
        /// <param name="sides"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool IsInside(Vector4[] v, Vector4[] sides, Vector4 p)
        {
            var ap = p - v[0];
            var bp = p - v[1];
            var cp = p - v[2];
            var dir1 = MathF.Sign(Vector4.Cross(sides[0], ap).Z);
            var dir2 = MathF.Sign(Vector4.Cross(sides[1], bp).Z);
            var dir3 = MathF.Sign(Vector4.Cross(sides[2], cp).Z);

            return dir1 == dir2 && dir2 == dir3;
        }
        /// <summary>
        /// 求重心坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private static Vector4 GetBarycentric2D(Vector4 p, Vector4[] v)
        {
            var x = p.X;
            var y = p.Y;

            float c1 = (x * (v[1].Y - v[2].Y) + (v[2].X - v[1].X) * y + v[1].X * v[2].Y - v[2].X * v[1].Y) / (v[0].X * (v[1].Y - v[2].Y) + (v[2].X - v[1].X) * v[0].Y + v[1].X * v[2].Y - v[2].X * v[1].Y);
            float c2 = (x * (v[2].Y - v[0].Y) + (v[0].X - v[2].X) * y + v[2].X * v[0].Y - v[0].X * v[2].Y) / (v[1].X * (v[2].Y - v[0].Y) + (v[0].X - v[2].X) * v[1].Y + v[2].X * v[0].Y - v[0].X * v[2].Y);
            float c3 = (x * (v[0].Y - v[1].Y) + (v[1].X - v[0].X) * y + v[0].X * v[1].Y - v[1].X * v[0].Y) / (v[2].X * (v[0].Y - v[1].Y) + (v[1].X - v[0].X) * v[2].Y + v[0].X * v[1].Y - v[1].X * v[0].Y);

            if (float.IsNaN(c1) || float.IsNaN(c2) || float.IsNaN(c3))
                return new(1);

            return new(c1, c2, c3);
        }
        /// <summary>
        /// 插值
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="gamma"></param>
        /// <param name="vert1"></param>
        /// <param name="vert2"></param>
        /// <param name="vert3"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        private static Vector4 Interpolate(Vector4 barycentric, Vector4[] v, float weight)
        {
            return (barycentric.X * v[0] + barycentric.Y * v[1] + barycentric.Z * v[2]) / weight;
        }
        

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
        /// 视口变换
        /// </summary>
        public Matrix ViewportMatrix { get; }

        /// <summary>
        /// 使用多屏绘制器
        /// </summary>
        public bool UseMutilScreenDrawer { get; set; } = false;
        /// <summary>
        /// 屏幕绘制器
        /// </summary>
        public IScreenDrawer ScreenDrawer => ScreenDrawers[0];
        /// <summary>
        /// 屏幕绘制器
        /// </summary>
        public List<IScreenDrawer> ScreenDrawers { get; } = new List<IScreenDrawer>();

        public Screen() : this(new ConsoleScreenDrawer(), Console.WindowWidth / 2, Console.WindowHeight)
        {

        }
        public Screen(IScreenDrawer screenDrawer) : this(screenDrawer, Console.WindowWidth / 2, Console.WindowHeight)
        {

        }
        public Screen(IScreenDrawer screenDrawer, int width, int height)
        {
            ScreenDrawers.Add(screenDrawer);
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
        public void Rasterize(Triangle triangle, IShader shader, Vector4 eyePosition)
        {
            var v = triangle.Vertexs;

            // 获取bounding box
            var left = MathF.Max(MathTool.Min(v[0].X, v[1].X, v[2].X), 0);
            var right = MathF.Min(MathTool.Max(v[0].X, v[1].X, v[2].X), Width - 1);
            var bottom = MathF.Max(MathTool.Min(v[0].Y, v[1].Y, v[2].Y), 0);
            var top = MathF.Min(MathTool.Max(v[0].Y, v[1].Y, v[2].Y), Height - 1);

            // 三角形三边
            var sides = new Vector4[]{
                v[1] - v[0],
                v[2] - v[1],
                v[0] - v[2]
            };

            for (int x = (int)left; x <= right; x++)
            {
                for (int y = (int)bottom; y <= top; y++)
                {
                    // inside
                    var p = new Vector4(x + 0.5f, y + 0.5f);

                    if (!IsInside(v, sides, p))
                        continue;

                    var barycentric = GetBarycentric2D(p, v);

                    // z buffer
                    var z = barycentric.X * v[0].Z + barycentric.Y * v[1].Z + barycentric.Z * v[2].Z;

                    // 采样
                    if (z < _zBuffer[x, y])
                        continue;
                    _zBuffer[x, y] = z;


                    _frameBuffer[x, y] = shader.GetColor(new()
                    {
                        ShaderPosition = Interpolate(barycentric, triangle.WorldPosition, 1),
                        Normal = Interpolate(barycentric, triangle.Normals, 1).Normalized,
                        TextureCoord = Interpolate(barycentric, triangle.TextureCoords, 1),
                        EyePosition = eyePosition
                    });
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
            if (!UseMutilScreenDrawer)
            {
                ScreenDrawer.Draw(_frameBuffer, message);
            }
            else
            {
                foreach (var screenDrawer in ScreenDrawers)
                {
                    screenDrawer.Draw(_frameBuffer, message);
                }
            }
        }
    }
}
