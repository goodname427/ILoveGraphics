using MatrixCore;

namespace ILoveGraphics.Object
{
    public class TriangleIndex
    {
        public required int[] Vertexs { get; set; }
        public int[] Normals { get; set; }
        public int[] TextureCoords { get; set; }

        public TriangleIndex()
        {
            Normals = Array.Empty<int>();
            TextureCoords = Array.Empty<int>();
        }

        public Triangle Triangle(Vector4[] vertexs, Vector4[] worldPositions, Vector4[] normals, Vector4[] textureCoords)
        {
            Vector4[] triangleNormals;
            if (Normals.Length == 3)
            {
                triangleNormals = new Vector4[]
                {
                    normals[Normals[0]],
                    normals[Normals[1]],
                    normals[Normals[2]],
                };
            }
            else
            {
                var normal = Vector4.Cross(worldPositions[Vertexs[1]] - worldPositions[Vertexs[0]], worldPositions[Vertexs[2]] - worldPositions[Vertexs[0]]).Normalized;

                triangleNormals = new Vector4[]
                {
                    normal,
                    normal,
                    normal,
                };
            }

            Vector4[] triangleTextureCoords;
            if (TextureCoords.Length == 3)
            {
                triangleTextureCoords = new Vector4[]
                {
                    textureCoords[TextureCoords[0]],
                    textureCoords[TextureCoords[1]],
                    textureCoords[TextureCoords[2]],
                };
            }
            else
            {
                triangleTextureCoords = new Vector4[]
                {
                    Vector4.Zero,
                    Vector4.One,
                    Vector4.Zero,
                };
            }

            return new Triangle()
            {
                Normals = triangleNormals,
                TextureCoords = triangleTextureCoords,
                Vertexs = new Vector4[]
                {
                    vertexs[Vertexs[0]],
                    vertexs[Vertexs[1]],
                    vertexs[Vertexs[2]],
                },
                WorldPosition = new Vector4[]
                {
                    worldPositions[Vertexs[0]],
                    worldPositions[Vertexs[1]],
                    worldPositions[Vertexs[2]],
                }
            };

        }
    }
}
