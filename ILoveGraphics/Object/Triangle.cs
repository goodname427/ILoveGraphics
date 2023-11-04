using MatrixCore;

namespace ILoveGraphics.Object
{
    public class Triangle
    {
        /// <summary>
        /// 顶点位置
        /// </summary>
        public required Vector4[] Vertexs { get; set; }
        /// <summary>
        /// 世界坐标
        /// </summary>
        public required Vector4[] WorldPosition { get; set; }
        /// <summary>
        /// 顶点uv
        /// </summary>
        public required Vector4[] TextureCoords { get; set; }
        /// <summary>
        /// 法线
        /// </summary>
        public required Vector4[] Normals { get; set; }

        public Triangle() { }

        public Triangle(Vector4[] vertexs, Vector4[] worldPositions, Vector4[] normals, Vector4[] textureCoords)
        {
            Vertexs = vertexs;
            WorldPosition = worldPositions;
            TextureCoords = textureCoords;
            Normals = normals;
        }
    }
}
