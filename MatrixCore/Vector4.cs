namespace MatrixCore;

public struct Vector4
{
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

    public static Vector4 One => new Vector4(1, 1, 1, 1);

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }
    public float Magnitude => MathF.Sqrt(X * X + Y * Y + Z * Z);
    public Vector4 Normalize => new(X / Magnitude, Y / Magnitude, Z / Magnitude);

    public Vector4()
    {
        W = 1;
        X = Y = Z = 0;
    }
    public Vector4(float x, float y, float z, float w = 1)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

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
}
