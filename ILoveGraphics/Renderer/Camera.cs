using ILoveGraphics.Object;
using ILoveGraphics.Renderer.ScreenDrawer;
using MatrixCore;
using ILoveGraphics.Light;

namespace ILoveGraphics.Renderer
{
    internal class Camera
    {
        /// <summary>
        /// 相机顶部
        /// </summary>
        public Vector4 Top { get; set; } = new Vector4(0, 1, 0, 0);
        /// <summary>
        /// 相机朝向
        /// </summary>
        public Vector4 Gaze { get; set; } = new Vector4(0, 0, 1, 0);
        /// <summary>
        /// 相机位置
        /// </summary>
        public Vector4 Positon { get; set; } = new Vector4(0, 0, -1, 0);
        /// <summary>
        /// 相机宽高比
        /// </summary>
        public float AspectRatio => 1.0f * Screen.Width / Screen.Height;
        /// <summary>
        /// 视角范围
        /// </summary>
        public float FieldOfView { get; set; } = 90;
        /// <summary>
        /// 近平面
        /// </summary>
        public float Near { get; set; } = -1;
        /// <summary>
        /// 远平面
        /// </summary>
        public float Far { get; set; } = -3000;

        /// <summary>
        /// 渲染屏幕
        /// </summary>
        public Screen Screen { get; set; }
        /// <summary>
        /// 光照
        /// </summary>
        public DirectionalLight Light { get; set; }

        /// <summary>
        /// 视图矩阵
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                var gXt = Vector4.Cross(Gaze, Top);
                return new Matrix(new float[,]
                {
                    {gXt.X, gXt.Y, gXt.Z, 0},
                    {Top.X, Top.Y, Top.Z, 0},
                    {-Gaze.X, -Gaze.Y, -Gaze.Z, 0},
                    {0, 0, 0, 1}
                }) * Matrix.TranslationMatrix(-Positon);
            }
        }
        /// <summary>
        /// 透视投影矩阵
        /// </summary>
        public Matrix PerspectProjectionMatrix
        {
            get
            {
                return new float[,]
                {
                    {Near, 0, 0, 0},
                    {0, Near, 0, 0},
                    {0, 0, Near + Far, -Near * Far},
                    {0, 0, 1, 0}
                };
            }
        }
        /// <summary>
        /// 正交矩阵
        /// </summary>
        public Matrix OrthogonalizedProjectionMatrix
        {
            get
            {
                float top = MathF.Abs(Near) * MathF.Tan(MathF.PI * FieldOfView / 360);
                float left = top * AspectRatio;
                return new float[,]
                {
                    { 1 / left, 0, 0, 0},
                    { 0, 1 / top, 0, 0},
                    { 0, 0, 2 / (Near - Far), 0},
                    { 0, 0, 0, 1}
                };
            }
        }
        /// <summary>
        /// 视角变换
        /// </summary>
        public Matrix ViewingMatrix => OrthogonalizedProjectionMatrix * PerspectProjectionMatrix * ViewMatrix;

        public Camera(Screen screen, DirectionalLight? light = null)
        {
            Screen = screen;
            Light = light ?? new DirectionalLight();
        }


        /// <summary>
        /// 对物体进行渲染
        /// </summary>
        /// <param name="renderedObjects"></param>
        public void Render(IEnumerable<RenderedObject> renderedObjects, string message = "")
        {
            Screen.Clear();

            var viewingMatrix = ViewingMatrix;
            foreach (var renderedObject in renderedObjects)
            {
                var transformMatrix = renderedObject.Transform.TransformMatrix;

                // m矩阵变换
                var vertexes = (from vertex in renderedObject.Mesh.Vertexes
                                select transformMatrix * vertex).ToArray();

                // 计算光照
                Vector4[] colors = new Vector4[renderedObject.Mesh.Triangles.Length / 3];
                for (int i = 0; i < colors.Length; i++)
                {
                    var ab = vertexes[renderedObject.Mesh.Triangles[i * 3 + 1]] - vertexes[renderedObject.Mesh.Triangles[i * 3]];
                    var ac = vertexes[renderedObject.Mesh.Triangles[i * 3 + 2]] - vertexes[renderedObject.Mesh.Triangles[i * 3]];
                    var normal = Vector4.Cross(ab, ac).Normalized;
                    colors[i] = Light.Color * MathF.Max(0, normal * Light.Direction);
                }

                //message = string.Join(' ', colors.Select(c => c.Alpha));


                // 视图+投影变换
                vertexes = (from vertex in vertexes
                            let vertex1 = Screen.ViewportMatrix * viewingMatrix * vertex
                            select vertex1 / vertex1.W).ToArray();

                // 光栅化
                for (int i = 0; i < renderedObject.Mesh.Triangles.Length; i += 3)
                {
                    Screen.Rasterize(
                        vertexes[renderedObject.Mesh.Triangles[i]],
                        vertexes[renderedObject.Mesh.Triangles[i + 1]],
                        vertexes[renderedObject.Mesh.Triangles[i + 2]],
                        colors[i / 3]
                    );
                }
            }

            Screen.Draw(message);
        }
    }
}
