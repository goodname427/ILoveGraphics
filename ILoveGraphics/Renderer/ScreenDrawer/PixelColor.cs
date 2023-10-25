using MatrixCore;
using System.ComponentModel.Design;

namespace ILoveGraphics.Renderer.ScreenDrawer
{
    internal struct PixelColor
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

        /// <summary>
        /// 将rgb颜色转为控制台字符
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static PixelColor Parse(Vector4 color)
        {
            var gray = (color.X * 0.299f + color.Y * 0.587f + color.Z * 0.114f) * MaxGray;
            var v = MathTool.Max(color.X, color.Y, color.Z);
            var s = v == 0 ? 0 : (v - MathTool.Min(color.X, color.Y, color.Z)) / v;
            float h;
            if (v == color.X)
            {
                h = 60 * (color.Y - color.Z) / (v - MathTool.Min(color.X, color.Y, color.Z));
            }
            else if (v == color.Y)
            {
                h = 120 + 60 * (color.Z - color.X) / (v - MathTool.Min(color.X, color.Y, color.Z));
            }
            else
            {
                h = 240 + 60 * (color.X - color.Y) / (v - MathTool.Min(color.X, color.Y, color.Z));
            }
            h = (int)Math.Round(h / 60, MidpointRounding.AwayFromZero);

            ConsoleColor consoleColor;
            if (s < 0.5)
            {
                // we have a grayish color
                consoleColor = (int)(v * 3.5) switch
                {
                    0 => ConsoleColor.Black,
                    1 => ConsoleColor.DarkGray,
                    2 => ConsoleColor.Gray,
                    _ => ConsoleColor.White,
                };
            }
            else if (s < 0.4)
            {
                // dark color
                consoleColor = h switch
                {
                    1 => ConsoleColor.DarkYellow,
                    2 => ConsoleColor.DarkGreen,
                    3 => ConsoleColor.DarkCyan,
                    4 => ConsoleColor.DarkBlue,
                    5 => ConsoleColor.DarkMagenta,
                    _ => ConsoleColor.DarkRed,
                };
            }
            else
            {
                // bright color
                consoleColor = h switch
                {
                    1 => ConsoleColor.Yellow,
                    2 => ConsoleColor.Green,
                    3 => ConsoleColor.Cyan,
                    4 => ConsoleColor.Blue,
                    5 => ConsoleColor.Magenta,
                    _ => ConsoleColor.Red,
                };

            }
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
