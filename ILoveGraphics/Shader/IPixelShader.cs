using MatrixCore;

namespace ILoveGraphics.Shader
{
    public interface IPixelShader
    {
        Vector4 Pass(VertexPS args);
    }
}
