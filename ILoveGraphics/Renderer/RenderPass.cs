using ILoveGraphics.Object;
using ILoveGraphics.Shader;
using MatrixCore;
using static MatrixCore.MathTool;

namespace ILoveGraphics.Renderer
{
    public class RenderPass
    {
        private readonly Vector4[,] _frameBuffer;
        private readonly float[,] _zBuffer;

        /// <summary>
        /// Pass所使用的屏幕
        /// </summary>
        public Screen Screen { get; init; }
        /// <summary>
        /// Pass所使用的相机
        /// </summary>
        public Camera Camera { get; init; }
        
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

        public RenderPass(Screen screen, Camera camera, IScreenDrawer screenDrawer)
        {
            Screen = screen;
            Camera = camera;
            ScreenDrawers.Add(screenDrawer);

            _frameBuffer = new Vector4[Screen.Width, Screen.Height];
            _zBuffer = new float[Screen.Width, Screen.Height];
            Clear();
        }

        /// <summary>
        /// 对物体进行渲染
        /// </summary>
        /// <param name="renderedObjects"></param>
        public void Draw(IEnumerable<RenderedObject> renderedObjects, string message = "")
        {
            Clear();

            var vvMatrix = Screen.ViewportMatrix * Camera.ViewingMatrix;
            var viewingMatrix = Camera.ViewingMatrix;
            StandardPixelShader.EyePosition = Camera.Transform.Position;

            foreach (var renderedObject in renderedObjects)
            {
                var transformMatrix = renderedObject.Transform.TransformMatrix;
                var transformInverseTransposeMatrix = transformMatrix.InverseMatrix.TransposedMatrix;

                VertexPS VS(VertexVS vertexVS)
                {
                    var output = new VertexPS();

                    // m矩阵变换
                    output.PositionW = transformMatrix * vertexVS.Position;
                    // 视图+投影变化
                    output.PositionH = viewingMatrix * output.PositionW;
                    // 法线
                    output.Normal = transformInverseTransposeMatrix * vertexVS.Normal;
                    // 纹理
                    output.TextureCoord = vertexVS.TextureCoord;

                    return output;
                };

                // 顶点变化
                var vertexes = renderedObject.Mesh.Vertexes.Select(VS).ToArray();

                // 齐次空间裁切
                var trianglesToRender = new List<int[]>();
                for (var i = 0; i < renderedObject.Mesh.Indexes.Length; i += 3)
                {
                    var triangle = new int[3];
                    for (int j = 0; j < 3; j++)
                        triangle[j] = renderedObject.Mesh.Indexes[i + j];

                    int k = 0;
                    for (; k < 3; k++)
                    {
                        var v = vertexes[triangle[k]].PositionH;

                        if (!(v.W < 0 || v.Z < 0 || v.X < -v.W || v.X > v.W || v.Y < -v.W || v.Y >= v.W))
                            break;
                    }
                    // 如果三个顶点都被裁切则不渲染该三角面
                    if (k == 3)
                        continue;

                    trianglesToRender.Add(triangle);
                }

                // 视口变化 + 归一化
                for (int i = 0; i < vertexes.Length; i++)
                {
                    vertexes[i].PositionH = Screen.ViewportMatrix * (vertexes[i].PositionH / vertexes[i].PositionH.W);
                }

                // 光栅化 + PS
                foreach (var triangle in trianglesToRender)
                {
                    Rasterize(vertexes[triangle[0]], vertexes[triangle[1]], vertexes[triangle[2]], renderedObject.Shader);
                }
            }

            Draw(message);
        }

        /// <summary>
        /// 光栅化以及像素着色器
        /// </summary>
        /// <param name="vertexes"></param>
        /// <param name="triangles"></param>
        private void Rasterize(VertexPS v0, VertexPS v1, VertexPS v2, IPixelShader shader)
        {
            // 三角形三点
            var v = new Vector4[] { v0.PositionH, v1.PositionH, v2.PositionH };

            // 背面剔除
            // 三角形顶点顺序为逆时针时剔除
            if (Vector4.Cross(v[1] - v[0], v[2] - v[0]).Z > 0)
            {
                return;
            }

            // 三角形三边
            var sides = new Vector4[]{
                v[1] - v[0],
                v[2] - v[1],
                v[0] - v[2]
            };

            // 根据顶点计算的法线
            var faceNormal = Vector4.Cross(v[2] - v[0], v[1] - v[0]);
            var useFaceNormal = v0.Normal.X == 0 && v0.Normal.Y == 0 && v0.Normal.Z == 0;

            // 获取bounding box
            var left = MathF.Max(Min(v[0].X, v[1].X, v[2].X), 0);
            var right = MathF.Min(Max(v[0].X, v[1].X, v[2].X), Screen.Width - 1);
            var bottom = MathF.Max(Min(v[0].Y, v[1].Y, v[2].Y), 0);
            var top = MathF.Min(Max(v[0].Y, v[1].Y, v[2].Y), Screen.Height - 1);

            for (int x = (int)left; x <= right; x++)
            {
                for (int y = (int)bottom; y <= top; y++)
                {
                    var p = new Vector4(x + 0.5f, y + 0.5f);

                    // inside
                    if (!IsInside(v, sides, p))
                        continue;

                    var barycentric = GetBarycentric2D(p, v);

                    // 计算像素深度
                    var z = barycentric.X * v[0].Z + barycentric.Y * v[1].Z + barycentric.Z * v[2].Z;

                    // 深度测试
                    if (z > _zBuffer[x, y])
                        continue;
                    _zBuffer[x, y] = z;

                    // 像素着色器
                    _frameBuffer[x, y] = shader.Pass(new()
                    {
                        PositionH = p,
                        PositionW = Interpolate(barycentric, v0.PositionW, v1.PositionW, v2.PositionW, 1),
                        Normal = useFaceNormal ? faceNormal : Interpolate(barycentric, v0.Normal, v1.Normal, v2.Normal, 1).Normalized,
                        TextureCoord = Interpolate(barycentric, v0.TextureCoord, v1.TextureCoord, v2.TextureCoord, 1),
                    });
                }
            }
        }
        /// <summary>
        /// 清除
        /// </summary>
        private void Clear()
        {
            for (int x = 0; x < Screen.Width; x++)
            {
                for (int y = 0; y < Screen.Height; y++)
                {
                    _frameBuffer[x, y] = Vector4.Zero;
                    _zBuffer[x, y] = float.MaxValue;
                }
            }


        }
        /// <summary>
        /// 将frame buffer输出
        /// </summary>
        private void Draw(string message = "")
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
