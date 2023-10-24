using MatrixCore;

namespace ILoveGraphics
{
    internal class Mesh
    {
        #region 
        public static Mesh Cube()
        {
            return new Mesh(
                new Vector4[]
                {
                    new Vector4(-0.5f, 0.5f, 0.5f),
                    new Vector4(0.5f, 0.5f, 0.5f),
                    new Vector4(0.5f, 0.5f, -0.5f),
                    new Vector4(-0.5f, 0.5f, -0.5f),
                    new Vector4(-0.5f, -0.5f, 0.5f),
                    new Vector4(0.5f, -0.5f, 0.5f),
                    new Vector4(0.5f, -0.5f, -0.5f),
                    new Vector4(-0.5f, -0.5f, -0.5f),
                },
                new int[]
                {
                    0, 1, 2,
                    0, 2, 3,
                    4, 7, 5,
                    5, 7, 6,
                    3, 2, 6,
                    3, 6, 7,
                    0, 4, 1,
                    1, 4, 5,
                    1, 5, 2,
                    2, 5, 6,
                    0, 3, 7,
                    0, 7, 4
                }
            );
        }

        public static Mesh HeartPlane()
        {
            return new Mesh(
                 new Vector4[]
                 {
                    new Vector4(0, -1, 0.3f),
                    new Vector4(3, 1),
                    new Vector4(2, 2, 0.3f),
                    new Vector4(1, 2),
                    new Vector4(0, 1),
                    new Vector4(-1, 2),
                    new Vector4(-2, 2, 0.3f),
                    new Vector4(-3, 1),
                 },
                 new int[]
                 {
                    0, 1, 4,
                    1, 2, 3,
                    1, 3, 4,
                    0, 4, 7,
                    5, 6, 7,
                    4, 5, 7,
                 }
             );
        }
        #endregion

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
            if (triangles.Length % 3 != 0)
                throw new ArgumentException("三角形数组长度不为三的整数");

            Vertexes = new Vector4[vertexes.Length];
            Triangles = new int[triangles.Length];
            Array.Copy(vertexes, Vertexes, vertexes.Length);
            Array.Copy(triangles, Triangles, triangles.Length);
        }
    }
}
