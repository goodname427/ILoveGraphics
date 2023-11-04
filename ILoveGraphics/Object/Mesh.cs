using MatrixCore;

namespace ILoveGraphics.Object
{
    public class Mesh
    {
        #region 
        public static Mesh Load(string filename)
        {
            var texts = File.ReadAllLines(filename);
            var vertexs = new List<Vector4>();
            var normals = new List<Vector4>();
            var textureCoords = new List<Vector4>();
            var triangles = new List<TriangleIndex>();
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
                        vertexs.Add(new Vector4(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2])));
                        break;
                    case "vn":
                        normals.Add(new Vector4(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2])));
                        break;
                    case "vt":
                        textureCoords.Add(new Vector4(float.Parse(args[0]), float.Parse(args[1])));
                        break;
                    case "f":
                        var indexes = args.Select(arg => arg.Split("/").Select(i => int.Parse(i) - 1).ToArray()).ToArray();
                        triangles.Add(new TriangleIndex
                        {
                            Vertexs = new int[]
                            {
                                indexes[0][0],
                                indexes[1][0],
                                indexes[2][0]
                            },
                            TextureCoords = new int[]
                            {
                                indexes[0][1],
                                indexes[1][1],
                                indexes[2][1],
                            },
                            Normals = new int[]
                            {
                                indexes[0][2],
                                indexes[1][2],
                                indexes[2][2]
                            },
                        });
                        if (indexes.Length > 3)
                        {
                            triangles.Add(new TriangleIndex
                            {
                                Vertexs = new int[]
                            {
                                indexes[0][0],
                                indexes[2][0],
                                indexes[3][0]
                            },
                                TextureCoords = new int[]
                            {
                                indexes[0][1],
                                indexes[2][1],
                                indexes[3][1],
                            },
                                Normals = new int[]
                            {
                                indexes[0][2],
                                indexes[2][2],
                                indexes[3][2]
                            },
                            });
                        }
                        break;
                    default:
                        break;
                }
            }

            return new Mesh(vertexs.ToArray(), triangles.ToArray())
            {
                Normals = normals.ToArray(),
                TextureCoords = textureCoords.ToArray()
            };
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
                new TriangleIndex[]
                {
                    new()
                    {
                        Vertexs = new int[]{ 0, 1, 2 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 0, 2, 3 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 4, 7, 5 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 5, 7, 6 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 3, 2, 6 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 3, 6, 7 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 0, 4, 1 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 1, 4, 5 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 1, 5, 2 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 2, 5, 6 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 0, 3, 7 }
                    },
                    new()
                    {
                        Vertexs = new int[]{ 0, 7, 4 }
                    },
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
                 new TriangleIndex[]
                 {
                    //0, 1, 4,
                    //1, 2, 3,
                    //1, 3, 4,
                    //0, 4, 7,
                    //5, 6, 7,
                    //4, 5, 7,
                 }
             );
        }
        #endregion

        /// <summary>
        /// 顶点集
        /// </summary>
        public Vector4[] Vertexes { get; init; }
        /// <summary>
        /// 顶点的uv坐标
        /// </summary>
        public Vector4[] TextureCoords { get; init; }
        /// <summary>
        /// 法线
        /// </summary>
        public Vector4[] Normals { get; init; }
        /// <summary>
        /// 三角面
        /// </summary>
        public TriangleIndex[] Triangles { get; init; }

        public Mesh(Vector4[] vertexes, TriangleIndex[] triangles)
        {
            Vertexes = vertexes;
            Triangles = triangles;
            TextureCoords = Array.Empty<Vector4>();
            Normals = Array.Empty<Vector4>();
        }
    }
}
