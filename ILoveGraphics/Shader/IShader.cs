using MatrixCore;

namespace ILoveGraphics.Shader
{
    public interface IShader
    {
        Vector4 GetColor(ShaderArgs args);

        public class ShaderArgs
        {
            public required Vector4 ShaderPosition { get; init; }
            public required Vector4 TextureCoord { get; init; }
            public required Vector4 Normal { get; init; }
            public Vector4 EyePosition { get; init; }
        }
    }
}
