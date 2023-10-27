using System.Diagnostics.CodeAnalysis;
using System.Text;
using static System.MathF;

namespace MatrixCore;

public struct Matrix
{
    #region 运算符重载
    /// <summary>
    /// 向量转矩阵（列向量）
    /// </summary>
    /// <param name="vector"></param>
    public static explicit operator Matrix(Vector4 vector)
    {
        return new Matrix(new float[4, 1] { { vector.X }, { vector.Y }, { vector.Z }, { vector.W } });
    }
    /// <summary>
    /// 矩阵转向量（行向量或者列向量）
    /// </summary>
    /// <param name="matrix"></param>
    public static explicit operator Vector4(Matrix matrix)
    {
        if (matrix.M == 1 && matrix.N == 4)
        {
            return new Vector4(matrix[1, 1], matrix[1, 2], matrix[1, 3], matrix[1, 4]);
        }
        else if (matrix.N == 1 && matrix.M == 4)
        {
            return new Vector4(matrix[1, 1], matrix[2, 1], matrix[3, 1], matrix[4, 1]);
        }
        throw new Exception();
    }
    /// <summary>
    /// 二维数组转矩阵
    /// </summary>
    /// <param name="elements"></param>
    public static implicit operator Matrix(float[,] elements)
    {
        return new Matrix(elements);
    }
    /// <summary>
    /// 矩阵转实数，当矩阵为1X1时
    /// </summary>
    /// <param name="matrix"></param>
    public static explicit operator float(Matrix matrix)
    {
        if (matrix.M == 1 && matrix.N == 1)
        {
            return matrix[1, 1];
        }
        throw new Exception();
    }
    /// <summary>
    /// 矩阵相乘
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Matrix operator *(Matrix left, Matrix right)
    {
        if (left.N != right.M)
            throw new ArgumentException($"{left.M}X{left.N}的矩阵无法与{right.M}X{right.N}的矩阵进行乘法");
        var matrix = new Matrix(left.M, right.N);
        foreach (var (i, j) in matrix.All)
        {
            for (int k = 1; k <= left.M; k++)
            {
                matrix[i, j] += left[i, k] * right[k, j];
            }
        }
        return matrix;
    }
    /// <summary>
    /// 矩阵与实数相乘
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static Matrix operator *(Matrix matrix, float k)
    {
        foreach (var (i, j) in matrix.All)
        {
            matrix[i, j] *= k;
        }
        return matrix;
    }
    /// <summary>
    /// 矩阵和向量相乘
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Vector4 operator *(Matrix matrix, Vector4 vector)
    {
        if (matrix.M != 4 && matrix.N != 4)
            throw new ArgumentException("矩阵并非标准变换矩阵(4X4)");
        return new()
        {
            X = matrix[1, 1] * vector.X + matrix[1, 2] * vector.Y + matrix[1, 3] * vector.Z + matrix[1, 4] * vector.W,
            Y = matrix[2, 1] * vector.X + matrix[2, 2] * vector.Y + matrix[2, 3] * vector.Z + matrix[2, 4] * vector.W,
            Z = matrix[3, 1] * vector.X + matrix[3, 2] * vector.Y + matrix[3, 3] * vector.Z + matrix[3, 4] * vector.W,
            W = matrix[4, 1] * vector.X + matrix[4, 2] * vector.Y + matrix[4, 3] * vector.Z + matrix[4, 4] * vector.W
        };
    }
    /// <summary>
    /// 矩阵与实数相乘
    /// </summary>
    /// <param name="k"></param>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static Matrix operator *(float k, Matrix matrix)
    {
        return matrix * k;
    }
    /// <summary>
    /// 矩阵除以实数
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static Matrix operator /(Matrix matrix, float k)
    {
        return (1 / k) * matrix;
    }
    /// <summary>
    /// 矩阵相加
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Matrix operator +(Matrix left, Matrix right)
    {
        if (left.M != right.M || left.N != right.N)
            throw new ArgumentException("矩阵尺寸不匹配");

        foreach (var (i, j) in left.All)
        {
            left[i, j] += right[i, j];
        }
        return left;
    }
    /// <summary>
    /// 矩阵取负
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static Matrix operator -(Matrix matrix)
    {
        return matrix * -1;
    }
    /// <summary>
    /// 矩阵相减
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Matrix operator -(Matrix left, Matrix right)
    {
        return left + (-right);
    }
    /// <summary>
    /// 矩阵相等
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Matrix left, Matrix right)
    {
        return left.Equals(right);
    }
    /// <summary>
    /// 矩阵不相等
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Matrix left, Matrix right)
    {
        return !left.Equals(right);
    }
    #endregion
    
    #region 预设矩阵
    /// <summary>
    /// n阶单位矩阵
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static Matrix IdentityMatrix(int n)
    {
        var matrix = new Matrix(n, n);
        for (int i = 1; i <= n; i++)
        {
            matrix[i, i] = 1;
        }
        return matrix;
    }
    /// <summary>
    /// 绕x轴旋转的3d旋转矩阵
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Matrix RotationMatrixByX(float a)
    {
        return new float[,] 
        {
            { 1,      0,       0, 0 },
            { 0, Cos(a), -Sin(a), 0 },
            { 0, Sin(a),  Cos(a), 0 },
            { 0,      0,       0, 1 }
        };
    }
    /// <summary>
    /// 绕y轴旋转的3d旋转矩阵
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Matrix RotationMatrixByY(float a)
    {
        return new float[,] 
        {
            { Cos(a), 0, Sin(a), 0 },
            {      0, 1,      0, 0 },
            {-Sin(a), 0, Cos(a), 0 },
            {      0, 0,      0, 1 }
        };
    }
    /// <summary>
    /// 绕z轴旋转的3d旋转矩阵
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Matrix RotationMatrixByZ(float a)
    {
        return new float[,] 
        {
            { Cos(a), -Sin(a), 0, 0 },
            { Sin(a),  Cos(a), 0, 0 },
            {      0,       0, 1, 0 },
            {      0,       0, 0, 1 }
        };
    }
    /// <summary>
    /// 旋转矩阵
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Matrix RotationMatrix(float x, float y, float z)
    {
        return RotationMatrixByX(x) * RotationMatrixByY(y) * RotationMatrixByZ(z);
    }
    /// <summary>
    /// 旋转矩阵
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Matrix RotationMatrix(Vector4 vector)
    {
        return RotationMatrix(vector.X, vector.Y, vector.Z);
    }
    /// <summary>
    /// 平移矩阵
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="dz"></param>
    /// <returns></returns>
    public static Matrix TranslationMatrix(float dx, float dy, float dz)
    {
        return new float[,] 
        {
            { 1, 0, 0, dx },
            { 0, 1, 0, dy },
            { 0, 0, 1, dz },
            { 0, 0, 0, 1  }
        };
    }
    /// <summary>
    /// 平移矩阵
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="dz"></param>
    /// <returns></returns>
    public static Matrix TranslationMatrix(Vector4 vector)
    {
        return TranslationMatrix(vector.X, vector.Y, vector.Z);
    }
    /// <summary>
    /// 缩放矩阵
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Matrix ScaleMatrix(float x, float y, float z)
    {
        return new float[,] 
        {
            { x, 0, 0, 0 },
            { 0, y, 0, 0 },
            { 0, 0, z, 0 },
            { 0, 0, 0, 1 }
        };
    }
    /// <summary>
    /// 缩放矩阵
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Matrix ScaleMatrix(Vector4 vector)
    {
        return ScaleMatrix(vector.X, vector.Y, vector.Z);
    }
    #endregion

    /// <summary>
    /// 矩阵元素,下标从1开始
    /// </summary>
    private float[,] _elements;

    #region 基本属性
    /// <summary>
    /// 行数
    /// </summary>
    public int M { get; init; }
    /// <summary>
    /// 列数
    /// </summary>
    public int N { get; init; }
    #endregion

    #region 相关计算
    /// <summary>
    /// 转置矩阵
    /// </summary>
    public Matrix TransposedMatrix
    {
        get
        {
            var matrix = new Matrix(N, M);
            foreach (var (i, j) in All)
            {
                matrix[j, i] = this[i, j];
            }
            return matrix;
        }
    }
    /// <summary>
    /// 行列式
    /// </summary>
    public float Determinant
    {
        get
        {
            if (M != N)
                throw new Exception();
            if (M == 1 && N == 1)
                return this[1, 1];
            float sum = 0;
            for (int i = 1; i <= M; i++)
            {
                sum += this[i, 1] * AlgebraicCofactor(i, 1);
            }
            return sum;
        }
    }
    /// <summary>
    /// 伴随矩阵
    /// </summary>
    public Matrix AdjointMatrix
    {
        get
        {
            if (M != N)
                throw new Exception();
            var matrix = new Matrix(M, N);
            foreach (var (i, j) in All)
            {
                matrix[i, j] = AlgebraicCofactor(j, i);
            }
            return matrix;
        }
    }
    /// <summary>
    /// 逆矩阵
    /// </summary>
    public Matrix InverseMatrix
    {
        get
        {
            if (!Invertible)
                throw new Exception();
            return AdjointMatrix / Determinant;
        }
    }
    /// <summary>
    /// 矩阵是否可逆
    /// </summary>
    public bool Invertible
    {
        get
        {
            return IsSquareMatrix && Determinant != 0;
        }
    }
    /// <summary>
    /// 是否为方阵
    /// </summary>
    public bool IsSquareMatrix
    {
        get
        {
            return M == N;
        }
    }
    #endregion

    #region 索引
    /// <summary>
    /// 返回全部索引
    /// </summary>
    public IEnumerable<(int, int)> All
    {
        get
        {
            for (int i = 1; i <= M; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    yield return (i, j);
                }
            }
        }
    }
    /// <summary>
    /// 矩阵元素
    /// </summary>
    /// <param name="m"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public float this[int m, int n]
    {
        get => _elements[m - 1, n - 1];
        set => _elements[m - 1, n - 1] = value;
    }
    #endregion

    #region 构造函数
    /// <summary>
    /// 指定大小的矩阵
    /// </summary>
    /// <param name="m"></param>
    /// <param name="n"></param>
    public Matrix(int m, int n, float initValue = 0)
    {
        M = m;
        N = n;
        _elements = new float[M, N];
        foreach (var (i, j) in All)
        {
            this[i, j] = initValue;
        }
    }
    /// <summary>
    /// 指定元素的矩阵
    /// </summary>
    /// <param name="elements"></param>
    public Matrix(float[,] elements)
    {
        M = elements.GetLength(0);
        N = elements.GetLength(1);
        _elements = new float[M, N];
        foreach (var (i, j) in All)
        {
            this[i, j] = elements[i - 1, j - 1];
        }
    }
    #endregion

    #region 函数重载
    public override string ToString()
    {
        StringBuilder sb = new();
        for (int i = 1; i <= M; i++)
        {
            for (int j = 1; j <= N; j++)
            {
                sb.Append(this[i, j]);
                sb.Append(' ');
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }
    public override bool Equals([NotNullWhen(true)] object? other)
    {
        if (other is not Matrix matrix)
            return false;

        if (M != matrix.M || N != matrix.N)
            return false;
        foreach (var (i, j) in All)
        {
            if (Abs(this[i, j] - matrix[i, j]) < 1e-6)
                return false;
        }
        return true;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion

    #region 基本函数
    /// <summary>
    /// 获取第m行
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    public float[] GetRow(int m)
    {
        var row = new float[N];
        for (int j = 1; j <= N; j++)
        {
            row[j] = this[m, j];
        }
        return row;
    }
    /// <summary>
    /// 获取第n列
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public float[] GetColumn(int n)
    {
        var column = new float[M];
        for (int i = 1; i <= M; i++)
        {
            column[i] = this[i, n];
        }
        return column;
    }
    /// <summary>
    /// 转置
    /// </summary>
    public void Transpose()
    {
        this = TransposedMatrix;
    }
    /// <summary>
    /// 余子式
    /// </summary>
    /// <param name="m"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public float Cofactor(int m, int n)
    {
        if (!IsSquareMatrix)
            throw new Exception();
        var matrix = new Matrix(M - 1, N - 1);
        //获取余子式对应矩阵
        for (int i = 1; i <= M; i++)
        {
            if (i == m)
                continue;
            for (int j = 1; j <= N; j++)
            {
                if (j == n)
                    continue;
                int _i = i < m ? i : i - 1;
                int _j = j < n ? j : j - 1;
                matrix[_i, _j] = this[i, j];
            }
        }
        return matrix.Determinant;
    }
    /// <summary>
    /// 代数余子式
    /// </summary>
    /// <param name="m"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public float AlgebraicCofactor(int m, int n)
    {
        return (((m + n) % 2 == 0) ? 1 : -1) * Cofactor(m, n);
    }
    /// <summary>
    /// 求矩阵的幂
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public Matrix Pow(int n)
    {
        if (!IsSquareMatrix)
            throw new Exception();
        var matrix = IdentityMatrix(M);
        for (int i = 0; i < n; i++)
        {
            matrix *= this;
        }
        return matrix;
    }
    #endregion
}

