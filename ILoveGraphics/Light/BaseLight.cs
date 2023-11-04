using ILoveGraphics.Object;
using MatrixCore;

namespace ILoveGraphics.Light
{
    public abstract class BaseLight
    {
        /// <summary>
        /// 场景中的所有光照
        /// </summary>
        public static List<BaseLight> Lights { get; } = new();
        /// <summary>
        /// 环境光
        /// </summary>
        public static Vector4 Ambient { get; set; } = Vector4.One * 0.1f;

        /// <summary>
        /// 光照信息
        /// </summary>
        public Transform Transform { get; init; }
        /// <summary>
        /// 光照颜色
        /// </summary>
        public Vector4 Color { get; set; }
        /// <summary>
        /// 光照强度
        /// </summary>
        public float Intensity { get; set; }

        public BaseLight()
        {
            Color = Vector4.One;
            Transform = new Transform();
            Intensity = 1;
        }

        /// <summary>
        /// 获取某一点光照方向
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public abstract Vector4 GetLightDirection(Vector4 position);
        /// <summary>
        /// 获取某一点光照强度
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public abstract float GetIntensity(Vector4 position);
    }
}
