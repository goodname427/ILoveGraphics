﻿using ILoveGraphics.Light;
using ILoveGraphics.Object;
using ILoveGraphics.Test;
using MatrixCore;

namespace ILoveGraphics.Shader
{
    public class StandardShader : IShader
    {
        public Vector4 BaseColor { get; set; } = new Vector4(1, 1, 1, 1);
        public Vector4 SpecularColor { get; set; } = new Vector4(1, 1, 1, 1);
        public Texture? Texture { get; set; }
        public float Roughness { get; set; } = 128;

        public Vector4 GetColor(IShader.ShaderArgs args)
        {
            // 视线
            var eyeDir = (args.EyePosition - args.ShaderPosition).Normalized;
            var color = Vector4.Zero;
            foreach (var light in RenderedScene.Lights)
            {
                // 光照方向, 从该点指向光源
                var lightDir = -light.GetLightDirection(args.ShaderPosition);

                // 光强
                var intensity = light.GetIntensity(args.ShaderPosition);

                // 漫反射
                color += (Texture?.GetColor(args.TextureCoord) ?? BaseColor) * intensity * MathF.Max(0, args.Normal * lightDir);

                // 高光
                var h = ((eyeDir + lightDir) / 2).Normalized;
                color += SpecularColor * intensity * MathF.Pow(MathF.Max(0, args.Normal * h), Roughness);

                // 环境光
                color += RenderedScene.Ambient;
            }

            color.W = 1;
            return color;
        }
    }
}
