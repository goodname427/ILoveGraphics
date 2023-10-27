using ILoveGraphics.Object;
using ILoveGraphics.Renderer.ScreenDrawer;
using MatrixCore;

namespace ILoveGraphics.Light
{
    internal class DirectionalLight : Light
    {
        public override float GetIntensity(Vector4 position)
        {
            return Intensity;
        }

        public override Vector4 GetLightDirection(Vector4 position)
        {
            return -Transform.Forward;
        }
    }
}
