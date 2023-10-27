using MatrixCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILoveGraphics.Renderer.Shader
{
    internal class StandardShader : IShader
    {
        public Vector4 GetColor(IShader.ShaderArgs args)
        {
            var ab = args.B - args.A;
            var ac = args.C - args.A;
            var pos = (args.A + args.B + args.C) / 3;
            
            // 法线
            var normal = Vector4.Cross(ab, ac).Normalized;
            var lightDir = args.Light.GetLightDirection(pos);
            var eyeDir = args.EyePosition - pos;
            var intensity = args.Light.GetIntensity(pos);

            // 漫反射
            var color = args.Light.Color * intensity * MathF.Max(0, normal * lightDir);

            // 高光
            var h = (eyeDir + lightDir).Normalized;
            color += Vector4.One * intensity * MathF.Max(0, MathF.Pow(normal * h, 128));

            // 环境光
            color += Vector4.One * 0.1f;

            color.W = 1;
            return color;
        }
    }
}
