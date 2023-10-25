using ILoveGraphics.Renderer.ScreenDrawer;
using MatrixCore;

namespace ILoveGraphics.Light
{
    internal class DirectionalLight
    {
        private Vector4 _direction;

        /// <summary>
        /// 光照方向
        /// </summary>
        public Vector4 Direction
        {
            get => _direction;
            set => _direction = value.Normalized;
        }
        /// <summary>
        /// 光照颜色
        /// </summary>
        public Vector4 Color { get; set; }

        public DirectionalLight() : this(new Vector4(0, 1, 1, 0), Vector4.One)
        {

        }

        public DirectionalLight(Vector4 direction) : this(direction, Vector4.One)
        {

        }

        public DirectionalLight(Vector4 direction, Vector4 color)
        {
            Direction = direction;
            Color = color;
        }
    }
}
