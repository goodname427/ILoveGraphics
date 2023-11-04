using MatrixCore;

namespace ILoveGraphics.Renderer.ScreenDrawer
{
    public struct PixelColor
    {
        public static PixelColor operator *(PixelColor left, float right)
        {
            return new PixelColor(left.Gray * right, left.ConsoleColor);
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

        /// <summary>
        /// 最大灰度值
        /// </summary>
        public const float MaxGray = 70;

        /// <summary>
        /// 将灰度值转为字符
        /// </summary>
        /// <param name="gray"></param>
        /// <returns></returns>
        private static char GetSameGrayValueChar(float gray, int left, int right)
        {
            if (left >= right)
                return s_grayValueChar[left];

            var mid = left + (right - left >> 1);

            if (MathTool.Appropriate(s_grayValue[mid], gray))
                return s_grayValueChar[mid];

            if (s_grayValue[mid] < gray)
                return GetSameGrayValueChar(gray, mid + 1, right);

            return GetSameGrayValueChar(gray, left, mid);
        }
        private static ConsoleColor GetApproximateConsoleColor(Vector4 color)
        {
            // 定义16种ConsoleColor的颜色映射
            //if (color.X < 0.5f && color.Y < 0.5f && color.Z < 0.5f)
            //    return ConsoleColor.Black;
            if (color.X < 0.5f && color.Y < 0.5f && color.Z >= 0.5f)
                return ConsoleColor.DarkBlue;
            if (color.X < 0.5f && color.Y >= 0.5f && color.Z < 0.5f)
                return ConsoleColor.DarkGreen;
            if (color.X < 0.5f && color.Y >= 0.5f && color.Z >= 0.5f)
                return ConsoleColor.DarkCyan;
            if (color.X >= 0.5f && color.Y < 0.5f && color.Z < 0.5f)
                return ConsoleColor.DarkRed;
            if (color.X >= 0.5f && color.Y < 0.5f && color.Z >= 0.5f)
                return ConsoleColor.DarkMagenta;
            if (color.X >= 0.5f && color.Y >= 0.5f && color.Z < 0.5f)
                return ConsoleColor.DarkYellow;
            if (color.X >= 0.5f && color.Y >= 0.5f && color.Z >= 0.5f)
                return ConsoleColor.Gray;
            if (color.X < 0.5f)
                return ConsoleColor.DarkGray;
            if (color.Y < 0.5f)
                return ConsoleColor.Blue;
            if (color.Z < 0.5f)
                return ConsoleColor.Green;
            if (color.X >= 0.5f && color.Y < 0.5f)
                return ConsoleColor.Red;
            if (color.X < 0.5f && color.Z >= 0.5f)
                return ConsoleColor.Cyan;
            if (color.Y >= 0.5f && color.Z < 0.5f)
                return ConsoleColor.Yellow;
            if (color.X >= 0.5f && color.Z >= 0.5f)
                return ConsoleColor.White;
            return ConsoleColor.White; // 默认为白色
        }


        /// <summary>
        /// 将rgb颜色转为控制台字符
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static PixelColor Parse(Vector4 color)
        {
            var gray = (color.X * 0.299f + color.Y * 0.587f + color.Z * 0.114f) * MaxGray;
            var consoleColor = ConsoleColor.White; //GetApproximateConsoleColor(color);
            return new PixelColor(gray, consoleColor);
        }

        private float _gray = 0;

        /// <summary>
        /// 控制台颜色
        /// </summary>
        public ConsoleColor ConsoleColor { get; set; }
        /// <summary>
        /// 控制台字符
        /// </summary>
        public char ConsoleChar { get; private set; } = ' ';
        /// <summary>
        /// 灰度值,直接影响灰度字符
        /// </summary>
        public float Gray
        {
            get => _gray;
            set
            {
                _gray = value switch
                {
                    < 0 => 0,
                    > MaxGray => MaxGray,
                    _ => value
                };

                ConsoleChar = _gray switch
                {
                    0 => s_grayValueChar[0],
                    MaxGray => s_grayValueChar[^1],
                    _ => GetSameGrayValueChar(_gray, 0, s_grayValue.Length - 1)
                };
            }
        }

        public PixelColor(float gray = 0, ConsoleColor consoleColor = ConsoleColor.White)
        {
            ConsoleColor = consoleColor;
            Gray = gray;
        }
    }
}
