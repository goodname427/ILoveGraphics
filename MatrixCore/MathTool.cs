namespace MatrixCore
{
    public static class MathTool
    {
        public static bool Appropriate(float a, float b, float delta = 1e-6f)
        {
            return MathF.Abs(a - b) < delta;
        }

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

        public static float Min(params float[] nums)
        {
            return nums.Min();
        }

        public static float Max(params float[] nums)
        {
            return nums.Max();
        }

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
