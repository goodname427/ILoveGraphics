using MatrixCore;

namespace ILoveGraphics
{
    internal class Transform
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector4 Position { get; init; }
        /// <summary>
        /// 旋转(欧拉角)
        /// </summary>
        public Vector4 EulerAngle { get; init; }
        /// <summary>
        /// 缩放
        /// </summary>
        public Vector4 Scale { get; init; }

        /// <summary>
        /// 模型矩阵
        /// </summary>
        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.TranslationMatrix(Position) *
                    Matrix.RotationMatrix(EulerAngle) *
                    Matrix.ScaleMatrix(Scale);
            }
        }

        public Transform()
        {
            Position = new Vector4();
            EulerAngle = new Vector4();
            Scale = Vector4.One;
        }
    }
}
