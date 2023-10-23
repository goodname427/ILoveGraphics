using MatrixCore;

namespace ILoveGraphics
{
    internal class Mesh
    {
        /// <summary>
        /// 顶点集
        /// </summary>
        public Vector4[] Vertexes { get; init; }
        /// <summary>
        /// 三角面
        /// </summary>
        public int[] Triangles { get; init; }

        public Mesh(Vector4[] vertexes, int[] triangles)
        {
            if (triangles.Length % 3 !=  0)
                throw new ArgumentException("三角形数组长度不为三的整数");

            Vertexes = new Vector4[vertexes.Length];
            Triangles = new int[triangles.Length];
            Array.Copy(vertexes, Vertexes, vertexes.Length);
            Array.Copy(triangles, Triangles, triangles.Length);
        }
    }
}
