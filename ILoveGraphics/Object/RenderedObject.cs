using ILoveGraphics.Renderer.Shader;

namespace ILoveGraphics.Object
{
    internal class RenderedObject
    {
        /// <summary>
        /// 物体的变换信息
        /// </summary>
        public Transform Transform { get; init; } = new Transform();
        /// <summary>
        /// 物体模型
        /// </summary>
        public Mesh Mesh { get; set; }
        /// <summary>
        /// 着色器
        /// </summary>
        public IShader Shader { get; set; }

        public RenderedObject(Mesh mesh) : this(mesh, new StandardShader())
        {

        }
        public RenderedObject(Mesh mesh, IShader shader)
        {
            Transform = new Transform();
            Mesh = mesh;
            Shader = shader;
        }
    }
}
