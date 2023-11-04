using MatrixCore;
using System.Drawing;

namespace ILoveGraphics.Object
{
    public class Texture
    {
        private readonly Vector4[,] _rawData;

        public int Width => _rawData.GetLength(0);
        public int Height => _rawData.GetLength(1);

        public Texture(Vector4[,] rawData)
        {
            _rawData = rawData;
        }

        /// <summary>
        /// 根据uv坐标获取颜色信息
        /// </summary>
        /// <param name="textureCoord"></param>
        /// <returns></returns>
        public Vector4 GetColor(Vector4 textureCoord)
        {
            return GetColor(textureCoord.X, textureCoord.Y);
        }

        /// <summary>
        /// 根据uv坐标获取颜色信息
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector4 GetColor(float u, float v)
        {
            var uImg = MathTool.Clamp((int)(u * Width), 0, Width - 1);
            var vImg = MathTool.Clamp((int)((1 - v) * Height), 0, Height - 1);

            return _rawData[uImg, vImg];
        }
    }
}
