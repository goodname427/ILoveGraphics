using MatrixCore;

namespace ILoveGraphics
{
    internal class Mesh
    {
        #region 
        public static Mesh Load(string filename)
        {
            var texts = File.ReadAllLines(filename);
            var vertexes = new List<Vector4>();
            var triangles = new List<int>();
            foreach (var text in texts)
            {
                if (string.IsNullOrWhiteSpace(text))
                    continue;

                var args = text.Split(' ');
                var tag = args[0];
                args = args[1..^0];

                switch (tag)
                {
                    case "v":
                        vertexes.Add(new Vector4(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2])));
                        break;
                    case "f":
                        var indexes = args.Select(arg => arg[0..arg.IndexOf('/')]).ToArray();
                        triangles.Add(int.Parse(indexes[0]) - 1);
                        triangles.Add(int.Parse(indexes[1]) - 1);
                        triangles.Add(int.Parse(indexes[2]) - 1);
                        if (indexes.Length > 3)
                        {
                            triangles.Add(int.Parse(indexes[0]) - 1);
                            triangles.Add(int.Parse(indexes[2]) - 1);
                            triangles.Add(int.Parse(indexes[3]) - 1);
                        }
                        break;
                    default:
                        break;
                }
            }

            return new Mesh(vertexes.ToArray(), triangles.ToArray());
        }
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
