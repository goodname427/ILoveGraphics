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
        /// <summary>
        /// 如果一个数在两个数之间, 则返回这个数, 否则返回边界
        /// </summary>
        /// <param name="a"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Clamp(int a, int min, int max)
        {
            if (a < min)
                return min;
            if (a > max)
                return max;

            return a;
        }
        /// <summary>
        /// 判断一个点是否在三角形里面
        /// </summary>
        /// <param name="v"></param>
        /// <param name="sides"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsInside(Vector4[] v, Vector4[] sides, Vector4 p)
        {
            var ap = p - v[0];
            var bp = p - v[1];
            var cp = p - v[2];
            var dir1 = MathF.Sign(Vector4.Cross(sides[0], ap).Z);
            var dir2 = MathF.Sign(Vector4.Cross(sides[1], bp).Z);
            var dir3 = MathF.Sign(Vector4.Cross(sides[2], cp).Z);

            return dir1 == dir2 && dir2 == dir3;
        }
        /// <summary>
        /// 求重心坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector4 GetBarycentric2D(Vector4 p, Vector4[] v)
        {
            var x = p.X;
            var y = p.Y;

            float c1 = (x * (v[1].Y - v[2].Y) + (v[2].X - v[1].X) * y + v[1].X * v[2].Y - v[2].X * v[1].Y) / (v[0].X * (v[1].Y - v[2].Y) + (v[2].X - v[1].X) * v[0].Y + v[1].X * v[2].Y - v[2].X * v[1].Y);
            float c2 = (x * (v[2].Y - v[0].Y) + (v[0].X - v[2].X) * y + v[2].X * v[0].Y - v[0].X * v[2].Y) / (v[1].X * (v[2].Y - v[0].Y) + (v[0].X - v[2].X) * v[1].Y + v[2].X * v[0].Y - v[0].X * v[2].Y);
            float c3 = (x * (v[0].Y - v[1].Y) + (v[1].X - v[0].X) * y + v[0].X * v[1].Y - v[1].X * v[0].Y) / (v[2].X * (v[0].Y - v[1].Y) + (v[1].X - v[0].X) * v[2].Y + v[0].X * v[1].Y - v[1].X * v[0].Y);

            if (float.IsNaN(c1) || float.IsNaN(c2) || float.IsNaN(c3))
                return new(1);

            return new(c1, c2, c3);
        }
        /// <summary>
        /// 插值
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="gamma"></param>
        /// <param name="vert1"></param>
        /// <param name="vert2"></param>
        /// <param name="vert3"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static Vector4 Interpolate(Vector4 barycentric, Vector4[] v, float weight)
        {
            return Interpolate(barycentric, v[0], v[1], v[2], weight);
        }
        /// <summary>
        /// 插值
        /// </summary>
        /// <param name="barycentric"></param>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static Vector4 Interpolate(Vector4 barycentric, Vector4 v0, Vector4 v1, Vector4 v2, float weight)
        {
            return (barycentric.X * v0 + barycentric.Y * v1 + barycentric.Z * v2) / weight;
        }
    }
}
