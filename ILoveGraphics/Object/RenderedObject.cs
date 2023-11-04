using ILoveGraphics.Shader;
using MatrixCore;

namespace ILoveGraphics.Object
{
    public class RenderedObject
    {
        #region 预制体
        /// <summary>
        /// 方块1
        /// </summary>
        public static RenderedObject Cube1 => new(Mesh.Cube())
        {
            Transform = new()
            {
                Position = new(1, 0, 0),
                Scale = new(1, 1, 1)
            }
        };
        /// <summary>
        /// 方块2
        /// </summary>
        public static RenderedObject Cube2 => new(Mesh.Load("Cube.obj"))
        {
            Transform = new()
            {
                Position = new(-1, 0, 0),
                Scale = Vector4.One * 0.5f
            },
        };
        /// <summary>
        /// 爱心
        /// </summary>
        public static RenderedObject Heart => new(Mesh.Load("Heart.obj"))
        {
            Shader = new Shader.StandardShader
            {
                BaseColor = new Vector4(1)
            }
        };
        /// <summary>
        /// pose
        /// </summary>
        public static RenderedObject Pose => new(Mesh.Load("Pose1.obj"));
        /// <summary>
        /// 牛
        /// </summary>
        public static RenderedObject Spot => new(Mesh.Load("Spot\\spot_triangulated_good.obj"))
        {
            Transform = new()
            {
                Scale = Vector4.One * 2
            },
            Shader = new StandardShader
            {
                SpecularColor = Vector4.One * 0.85f,
            }
        };
        #endregion

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
