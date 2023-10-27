namespace MatrixCore
{
    public static class MathTool
    {
        /// <summary>
        /// 判断两个浮点数是否近似相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static bool Appropriate(float a, float b, float delta = 1e-6f)
        {
            return MathF.Abs(a - b) < delta;
        }
        /// <summary>
        /// 使得一个数在两个数之间来回变化
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <param name="res"></param>
        public static void PingPong(float a, float b, ref float t, ref float res)
        {
            if (res > b)
            {
                res = b;
                t = -t;
            }
            if (res < a)
            {
                res = a;
                t = -t;
            }

            res += t;
        }
        /// <summary>
        /// 使得一个数在两个数之间循环变化
        /// </summary>
        /// <param name="a"></param>
        /// <param name="t"></param>
        /// <param name="res"></param>
        public static void Cycle(float a, float b, float t, ref float res)
        {
            res += t;
            if (res > b)
                res -= b - a;            
        }
        /// <summary>
        /// 求最小值
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static float Min(params float[] nums)
        {
            return nums.Min();
        }
        /// <summary>
        /// 求最大值
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static float Max(params float[] nums)
        {
            return nums.Max();
        }
        /// <summary>
        /// 如果一个数在两个数之间, 则返回这个数, 否则返回边界
        /// </summary>
        /// <param name="a"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float a, float min, float max)
        {
            if (a < min)
                return min;
            if (a > max)
                return max;

            return a;
        }
    }
}
