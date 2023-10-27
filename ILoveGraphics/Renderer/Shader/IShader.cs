using ILoveGraphics.Light;
using MatrixCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILoveGraphics.Renderer.Shader
{
    interface IShader
    {
        Vector4 GetColor(ShaderArgs args);

        public class ShaderArgs
        {
            public Vector4 A { get; init; }
            public Vector4 B { get; init; }
            public Vector4 C { get; init; }
            public required Light.Light Light { get; init; }
            public Vector4 EyePosition { get; init; }
        }
    }
}
