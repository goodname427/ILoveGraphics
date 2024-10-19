using System.Diagnostics.CodeAnalysis;

namespace MatrixCore;

public struct Vector4
{
    #region 运算符重载
    public static Vector4 operator +(Vector4 left, Vector4 right)
    {
        return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }
    public static Vector4 operator -(Vector4 left, Vector4 right)
    {
        return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }
    public static Vector4 operator -(Vector4 vector)
    {
        return vector * -1;
    }
    public static Vector4 operator *(Vector4 vector, float k)
    {
        return new Vector4(vector.X * k, vector.Y * k, vector.Z * k, vector.W * k);
    }
    public static Vector4 operator *(float k, Vector4 vector)
    {
        return vector * k;
    }
    public static Vector4 operator /(Vector4 vector, float k)
    {
        return new Vector4(vector.X / k, vector.Y / k, vector.Z / k, vector.W / k);
    }
    public static float operator *(Vector4 letf, Vector4 right)
    {
        return letf.X * right.X + letf.Y * right.Y + letf.Z * right.Z;
    }
    public static bool operator ==(Vector4 left, Vector4 right)
    {
        return left.Equals(right);
    }
    public static bool operator !=(Vector4 left, Vector4 right)
    {
        return !left.Equals(right);
    }
    public static Vector4 Cross(Vector4 left, Vector4 right)
    {
        return new Vector4(
                left.Y * right.Z - left.Z * right.Y,
                left.Z * right.X - left.X * right.Z,
                left.X * right.Y - left.Y * right.X,
                1
            );
    }
    public static Vector4 Scale(Vector4 left, Vector4 right)
    {
        return new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }
    public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
    {
        return (1 - t) * a + t * b;
    }
    #endregion

    #region 常用向量
    public static Vector4 One { get; } = new(1, 1, 1, 0);
    public static Vector4 Zero { get; } = new();
    public static Vector4 Up { get; } = new(0, 1, 0, 0);
    public static Vector4 Left { get; } = new(1, 0, 0, 0);
    public static Vector4 Forward { get; } = new(0, 0, 1, 0);
    #endregion

    #region 基本属性
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }
    #endregion

    #region 相关计算
    public float Magnitude => MathF.Sqrt(SqrtMagnitude);
    public float SqrtMagnitude => X * X + Y * Y + Z * Z + W * W;
    public Vector4 Normalized
    {
        get
        {
            var magnitude = Magnitude;
            return this / magnitude;
        }
    }
    #endregion

    #region 构造函数
    public Vector4(float x = 0, float y = 0, float z = 0, float w = 1)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
    #endregion

    #region 重载方法
    public override string ToString()
    {
        return $"({X:f},{Y:f},{Z:f},{W:f})";
    }
    public string ToString(bool showW)
    {
        if (!showW)
            return $"({X:f},{Y:f},{Z:f})";
        return ToString();
    }
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not Vector4 vector)
            return false;

        if (vector.W != W)
            vector /= (vector.W / W);

        return MathTool.Appropriate(X, vector.X)
            && MathTool.Appropriate(Y, vector.Y)
            && MathTool.Appropriate(Z, vector.Z);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion
}
