using ILoveGraphics.Object;
using MatrixCore;

namespace ILoveGraphics.Renderer
{
    public class Camera : Object.Object
    {
        /// <summary>
        /// 相机宽高比
        /// </summary>
        public float AspectRatio { get; set; } = 1f;
        /// <summary>
        /// 视角范围
        /// </summary>
        public float FieldOfView { get; set; } = 90;
        /// <summary>
        /// 近平面
        /// </summary>
        public float Near { get; set; } = 1;
        /// <summary>
        /// 远平面
        /// </summary>
        public float Far { get; set; } = 3000;

        /// <summary>
        /// 视图矩阵
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                var gaze = Transform.Forward;
                var top = Transform.Up;
                var left = Transform.Left;
                Matrix matrix;
                matrix = new Matrix(new float[,]
                {
                    {left.X, left.Y, left.Z, 0},
                    {top.X, top.Y, top.Z, 0},
                    {gaze.X, gaze.Y, gaze.Z, 0},
                    {0, 0, 0, 1}
                }) * Matrix.TranslationMatrix(-Transform.Position);
                return matrix;
            }
        }
        /// <summary>
        /// 透视投影矩阵
        /// </summary>
        public Matrix PerspectProjectionMatrix
        {
            get
            {
                return OrthogonalizedProjectionMatrix * new float[,]
                {
                    {Near, 0, 0, 0},
                    {0, Near, 0, 0},
                    {0, 0, Near + Far, -Near * Far},
                    {0, 0, 1, 0}
                };
            }
        }
        /// <summary>
        /// 正交矩阵
        /// </summary>
        public Matrix OrthogonalizedProjectionMatrix
        {
            get
            {
                float top = MathF.Abs(Near) * MathF.Tan(MathF.PI * FieldOfView / 360);
                float left = top * AspectRatio;

                //Matrix translate = new float[,]
                //{
                //    { 1, 0, 0, 0},
                //    { 0, 1, 0, 0},
                //    { 0, 0, 1, - ((Near + Far) / 2)},
                //    { 0, 0, 0, 1}
                //};
                //Matrix scale = new float[,]
                //{
                //    { 1 / left, 0, 0, 0},
                //    { 0, 1 / top, 0, 0},
                //    { 0, 0, 2 / (Near - Far), 0},
                //    { 0, 0, 0, 1}
                //};

                //return scale * translate;
                return new float[,]
                {
                    { 1 / left, 0, 0, 0},
                    { 0, 1 / top, 0, 0},
                    { 0, 0, 1 / (Far - Near), -Near / (Far - Near)},
                    { 0, 0, 0, 1}
                };
            }
        }
        /// <summary>
        /// 视角变换
        /// </summary>
        public Matrix ViewingMatrix => PerspectProjectionMatrix * ViewMatrix;

        public Camera()
        {
            Transform = new Transform();
        }

        public Camera(Screen screen) : this()
        {
            AspectRatio = 1.0f * screen.Width / screen.Height;
        }
    }
}
