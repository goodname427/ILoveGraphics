using ILoveGraphics.Light;
using MatrixCore;

namespace ILoveGraphics.Shader
{
    public class StandardShader : IShader
    {
        public Vector4 BaseColor { get; set; } = Vector4.One;

        public Vector4 GetColor(IShader.ShaderArgs args)
        {
            // 视线
            var eyeDir = args.EyePosition - args.ShaderPosition;
            var color = Vector4.Zero;
            foreach (var light in BaseLight.Lights)
            {
                // 光照方向
                var lightDir = light.GetLightDirection(args.ShaderPosition);
                // 光强
                var intensity = light.GetIntensity(args.ShaderPosition);

                // 漫反射
                color += BaseColor * intensity * MathF.Max(0, args.Normal * lightDir);

                // 高光
                var h = ((eyeDir + lightDir) / 2).Normalized;
                color += Vector4.One * intensity * MathF.Max(0, MathF.Pow(args.Normal * h, 128));

                // 环境光
                color += BaseLight.Ambient;

            }
            
            color.W = 1;
            return color;
        }
    }
}
