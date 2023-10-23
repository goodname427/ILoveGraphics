using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
