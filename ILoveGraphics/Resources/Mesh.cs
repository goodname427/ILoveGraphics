using ILoveGraphics.Shader;
using MatrixCore;
using System.Xml.Schema;

namespace ILoveGraphics.Resources
{
    public class Mesh
    {
        /// <summary>
        /// 模型路径
        /// </summary>
        public static string Path { get; } = "..\\..\\..\\..\\ILoveGraphics\\Models\\";

        #region 预制体
        /// <summary>
        /// 加载obj文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Mesh Load(string path)
        {
            if (!File.Exists(path))
            {
                path = Path + path;
            }

            var texts = File.ReadAllLines(path);

            var positions = new List<Vector4>();
            var normals = new List<Vector4>();
            var textureCoords = new List<Vector4>();

            var indexes = new List<int>();
            var vertexes = new List<VertexVS>();
            var vertexIndex = new Dictionary<string, int>();

            foreach (var text in texts)
            {
                if (string.IsNullOrWhiteSpace(text))
                    continue;

                var args = text.Split(' ').Where(arg => !string.IsNullOrWhiteSpace(arg)).ToArray();
                var tag = args[0];
                args = args[1..^0];

                if (args is null)
                    continue;

                switch (tag)
                {
                    case "v":
                        positions.Add(new Vector4(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2])));
                        break;
                    case "vn":
                        normals.Add(new Vector4(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2])));
                        break;
                    case "vt":
                        textureCoords.Add(new Vector4(float.Parse(args[0]), float.Parse(args[1])));
                        break;
                    case "f":
                        var faceIndexes = args.Select(
                                arg => arg.Split("/")
                                    .Where(i => !string.IsNullOrEmpty(i))
                                    .Select(i => int.Parse(i) - 1)
                                    .ToArray()
                            ).Where(index => index.Length >= 3)
                            .ToArray();

                        if (faceIndexes.Length < 3)
                            break;

                        // 将各子索引组合成顶点，并获取顶点索引
                        var faceVertexIndexes = new int[faceIndexes.Length];
                        for (int i = 0; i < faceIndexes.Length; i++)
                        {
                            if (!vertexIndex.TryGetValue(args[i], out faceVertexIndexes[i]))
                            {
                                vertexIndex.Add(args[i], faceVertexIndexes[i] = vertexes.Count);
                                vertexes.Add(new VertexVS
                                {
                                    Position = positions[faceIndexes[i][0]],
                                    TextureCoord = textureCoords[faceIndexes[i][1]],
                                    Normal = normals[faceIndexes[i][2]],
                                });
                            }
                        }

                        indexes.Add(faceVertexIndexes[0]);
                        indexes.Add(faceVertexIndexes[1]);
                        indexes.Add(faceVertexIndexes[2]);

                        if (faceVertexIndexes.Length > 3)
                        {
                            indexes.Add(faceVertexIndexes[0]);
                            indexes.Add(faceVertexIndexes[2]);
                            indexes.Add(faceVertexIndexes[3]);
                        }
                        break;
                    default:
                        break;
                }
            }

            return new Mesh(vertexes.ToArray(), indexes.ToArray());
        }
        /// <summary>
        /// cube mesh
        /// </summary>
        /// <returns></returns>
        public static Mesh Cube()
        {
            return new Mesh(
                new VertexVS[]
                {
                    new() { Position = new Vector4(-0.5f, 0.5f, 0.5f) },
                    new() { Position = new Vector4(0.5f, 0.5f, 0.5f) },
                    new() { Position = new Vector4(0.5f, 0.5f, -0.5f) },
                    new() { Position = new Vector4(-0.5f, 0.5f, -0.5f) },
                    new() { Position = new Vector4(-0.5f, -0.5f, 0.5f) },
                    new() { Position = new Vector4(0.5f, -0.5f, 0.5f) },
                    new() { Position = new Vector4(0.5f, -0.5f, -0.5f) },
                    new() { Position = new Vector4(-0.5f, -0.5f, -0.5f) },
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
                    0, 7, 4,

                }
            );
        }
        /// <summary>
        /// 爱心平面
        /// </summary>
        /// <returns></returns>
        public static Mesh HeartPlane()
        {
            return new Mesh(
                 new VertexVS[]
                 {
                    new() { Position = new Vector4(0, -1, 0.3f) },
                    new() { Position = new Vector4(3, 1) },
                    new() { Position = new Vector4(2, 2, 0.3f) },
                    new() { Position = new Vector4(1, 2) },
                    new() { Position = new Vector4(0, 1) },
                    new() { Position = new Vector4(-1, 2) },
                    new() { Position = new Vector4(-2, 2, 0.3f) },
                    new() { Position = new Vector4(-3, 1) },
                 },
                 new int[]
                 {
                    0, 1, 4,
                    1, 2, 3,
                    1, 3, 4,
                    5, 6, 7,
                    4, 5, 7,
                 }
             );
        }
        #endregion

        /// <summary>
        /// 顶点buff
        /// </summary>
        public VertexVS[] Vertexes { get; init; }
        /// <summary>
        /// 索引buff
        /// </summary>
        public int[] Indexes { get; init; }

        public Mesh(VertexVS[] vertexes, int[] indexes)
        {
            Vertexes = vertexes;
            Indexes = indexes;
        }
    }
}
