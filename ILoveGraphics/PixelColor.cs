using MatrixCore;

namespace ILoveGraphics
{
    internal struct PixelColor
    {
        public static PixelColor operator *(PixelColor left, float right)
        {
            return new PixelColor(left.Alpha * right, left.ConsoleColor);
        }
        public static PixelColor operator *(float left, PixelColor right)
        {
            return right * left;
        }

        /// <summary>
        /// 灰度字符
        /// </summary>
        private static readonly char[] s_grayValueChar = { ' ', '`', '.', '^', ',', ':', '~', '"', '<', '!', 'c', 't', '+', '{', 'i', '7', '?', 'u', '3', '0', 'p', 'w', '4', 'A', '8', 'D', 'X', '%', '#', 'H', 'W', 'M' };
        /// <summary>
        /// 灰度字符对应的灰度值
        /// </summary>
        private static readonly int[] s_grayValue = { 0, 5, 7, 9, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49, 51, 53, 55, 59, 61, 63, 66, 68, 70 };

        public const float MaxAlpha = 70;

        /// <summary>
        /// 将灰度值转为字符
        /// </summary>
        /// <param name="gray"></param>
        /// <returns></returns>
        private static char GetSameGrayValueChar(float gray, int left, int right)
        {
            if (left >= right)
                return s_grayValueChar[left];

            var mid = left + ((right - left) >> 1);

            if (MathTool.Appropriate(s_grayValue[mid], gray))
                return s_grayValueChar[mid];

            if (s_grayValue[mid] < gray)
                return GetSameGrayValueChar(gray, mid + 1, right);

            return GetSameGrayValueChar(gray, left, mid);
        }


        private float _alpha = 0;

        /// <summary>
        /// 控制台颜色
        /// </summary>
        public ConsoleColor ConsoleColor { get; set; }
        /// <summary>
        /// 控制台字符
        /// </summary>
        public char ConsoleChar { get; private set; } = ' ';
        /// <summary>
        /// 透明度,直接影响灰度字符
        /// </summary>
        public float Alpha
        {
            get => _alpha;
            set
            {
                _alpha = value switch
                {
                    < 0 => 0,
                    > MaxAlpha => MaxAlpha,
                    _ => value
                };

                ConsoleChar = _alpha switch
                {
                    0 => s_grayValueChar[0],
                    MaxAlpha => s_grayValueChar[^1],
                    _ => GetSameGrayValueChar(_alpha, 0, s_grayValue.Length - 1)
                };
            }
        }

        public PixelColor(float alpha = 0, ConsoleColor consoleColor = ConsoleColor.White)
        {
            ConsoleColor = consoleColor;
            Alpha = alpha;
        }
    }
}
