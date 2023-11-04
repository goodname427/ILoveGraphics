using MatrixCore;

namespace ILoveGraphics.Object
{
    public class Transform
    {
        /// <summary>
        /// 发生变换时调用
        /// </summary>
        public event Action? OnTransforming;

        private Vector4 _positon;
        private Vector4 _eulerAngle;
        private Vector4 _scale;

        /// <summary>
        /// 位置
        /// </summary>
        public Vector4 Position
        {
            get => _positon;
            set
            {
                _positon = value;
                OnTransforming?.Invoke();
            }
        }
        /// <summary>
        /// 旋转(欧拉角)
        /// </summary>
        public Vector4 EulerAngle
        {
            get => _eulerAngle;
            set
            {
                _eulerAngle = value;
                OnTransforming?.Invoke();
            }
        }
        /// <summary>
        /// 缩放
        /// </summary>
        public Vector4 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnTransforming?.Invoke();
            }
        }

        public Vector4 Up => TransformMatrix * Vector4.Up;
        public Vector4 Forward => Matrix.RotationMatrix(EulerAngle) * Vector4.Forward;
        public Vector4 Left => TransformMatrix * Vector4.Left;

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
