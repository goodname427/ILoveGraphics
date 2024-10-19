using MatrixCore;
using System.Drawing;

namespace ILoveGraphics.Resources
{
    public class Texture
    {
        private readonly Vector4[,] _rawData;

        public bool UseBilinear { get; set; } = false;
        public int Width => _rawData.GetLength(0);
        public int Height => _rawData.GetLength(1);

        public Texture(Vector4[,] rawData)
        {
            _rawData = rawData;
        }

        private Vector4 RawSample(float u, float v)
        {
            var uImg = MathTool.Clamp((int)u , 0, Width - 1);
            var vImg = MathTool.Clamp((int)v, 0, Height - 1);

            return _rawData[uImg, vImg];
        }

        /// <summary>
        /// 根据uv坐标获取颜色信息
        /// </summary>
        /// <param name="textureCoord"></param>
        /// <returns></returns>
        public Vector4 GetColor(Vector4 textureCoord)
        {
            return Sample(textureCoord.X, textureCoord.Y);
        }

        /// <summary>
        /// 根据uv坐标获取颜色信息
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector4 Sample(float u, float v)
        {
            u *= Width;
            v = (1 - v) * Height;

            if (UseBilinear)
            {
                var uFloor = MathF.Floor(u);
                var uCeil = MathF.Ceiling(u);
                var ut = u - uFloor;

                var vFloor = MathF.Floor(v);
                var vCeil = MathF.Ceiling(v);
                var vt = v - vFloor;

                var c1 = RawSample(uFloor, vFloor);
                var c2 = RawSample(uFloor, vCeil);
                var c3 = RawSample(uCeil, vFloor);
                var c4 = RawSample(uCeil, vCeil);

                return Vector4.Lerp(Vector4.Lerp(c1, c2, vt), Vector4.Lerp(c3, c4, vt), ut);
            }

            return RawSample(u, v);
        }

        
    }
}
