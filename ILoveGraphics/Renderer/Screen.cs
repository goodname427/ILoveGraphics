using MatrixCore;

namespace ILoveGraphics.Renderer
{
    public class Screen
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; init; } = 100;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; init; } = 100;
        /// <summary>
        /// 视口变换
        /// </summary>
        public Matrix ViewportMatrix { get; }

        public Screen(int width, int height)
        {
            Width = width;
            Height = height;

            ViewportMatrix = new float[,]
            {
                {Width / 2, 0, 0, Width / 2},
                {0, Height / 2, 0, Height / 2 },
                { 0, 0, 1, 0},
                { 0, 0, 0, 1},
            };
        }        
    }
}
