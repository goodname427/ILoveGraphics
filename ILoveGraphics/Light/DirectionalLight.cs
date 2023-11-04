using ILoveGraphics.Object;
using MatrixCore;

namespace ILoveGraphics.Light
{
    public class DirectionalLight : BaseLight
    {
        private Vector4 _direction;

        public required Vector4 Direction { get => _direction; set => _direction = value.Normalized; }

        public override float GetIntensity(Vector4 position)
        {
            return Intensity;
        }

        public override Vector4 GetLightDirection(Vector4 position)
        {
            return Direction;
        }
    }
}
