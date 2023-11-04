using ILoveGraphics.Light;
using ILoveGraphics.Object;
using ILoveGraphics.Renderer;
using ILoveGraphics.Renderer.ScreenDrawer;
using MatrixCore;

namespace ILoveGraphics.Test
{
    public class RenderedScene
    {
        #region 场景必要组件
        /// <summary>
        /// 场景中的相机
        /// </summary>
        public static Camera? Camera { get; set; }
        /// <summary>
        /// 场景中需要渲染的物体
        /// </summary>
        public static List<RenderedObject> RenderedObjects { get; } = new List<RenderedObject>();
        /// <summary>
        /// 场景中的灯光
        /// </summary>
        public static List<BaseLight> Lights { get; } = new List<BaseLight>();
        /// <summary>
        /// 场景中的环境光参数
        /// </summary>
        public static Vector4 Ambient { get; set; } = Vector4.One * 0.1f;
        #endregion

        #region 场景参数
        /// <summary>
        /// 场景中的运行信息
        /// </summary>
        public static (int frameCount, DateTime startTime, DateTime lastTime) RunInfo;
        /// <summary>
        /// 循环数
        /// </summary>
        public static (float value, float step, float min, float max) Cycle;
        /// <summary>
        /// pingpong数
        /// </summary>
        public static (float value, float step, float min, float max) PingPong;
        #endregion


        /// <summary>
        /// 设置窗口数据
        /// </summary>
        public static void SetConsoleRenderArgs()
        {
            // 方便调整屏幕大小
            Console.WriteLine("Press Enter To Start!");
            Console.ReadLine();
            Console.Clear();

            // 屏幕
            var screen = new Screen(
                new ConsoleScreenDrawer(), Console.WindowWidth / 2, Console.WindowHeight
            );

            SetDefaultRenderArgs(screen);
        }
        /// <summary>
        /// 设置默认数据
        /// </summary>
        /// <param name="screen"></param>
        public static void SetDefaultRenderArgs(Screen screen)
        {
            // 光照
            Lights.AddRange(new BaseLight[]{
                new DirectionalLight
                {
                    Direction = new Vector4(0, 1, -1),
                }
            });

            // 相机
            Camera = new Camera(screen)
            {
                FieldOfView = 45,
                Transform = new()
                {
                    Position = new(0, 0, -10f),
                },
            };

            // 运行信息
            RunInfo = (0, DateTime.Now, DateTime.Now);

            // 变化参数
            Cycle = (0f, 1, 0f, 2 * MathF.PI);
            PingPong = (0f, 1, -1f, 1f);
        }

        /// <summary>
        /// 更新场景
        /// </summary>
        /// <param name="update"></param>
        public static void Update(params Action<RenderedObject>[] update)
        {
            // 统计运行信息
            var now = DateTime.Now;
            var deltaTime = (float)(now - RunInfo.lastTime).TotalSeconds;
            RunInfo.lastTime = now;

            RunInfo.frameCount++;

            // 变化参数
            MathTool.PingPong(PingPong.min, PingPong.max, ref PingPong.step, ref PingPong.value);
            MathTool.Cycle(Cycle.min, Cycle.max, Cycle.step * deltaTime, ref Cycle.value);

            foreach (var renderedObject in RenderedObjects)
            {
                foreach (var action in update)
                    action?.Invoke(renderedObject);
            }

            // 渲染
            Camera?.Render(RenderedObjects, $"""
                resolution: ({Camera.Screen.Width}, {Camera.Screen.Height}, {Camera.Screen.Width * Camera.Screen.Width})
                fps: {RunInfo.frameCount / (now - RunInfo.startTime).TotalSeconds:F2}
                deltaTime: {deltaTime:F2}
                """);
        }

        #region 预设好的操作
        /// <summary>
        /// 绕y轴旋转
        /// </summary>
        /// <param name="renderedObject"></param>
        public static void RotateAroundY(RenderedObject renderedObject)
        {
            renderedObject.Transform.EulerAngle = new Vector4(0, Cycle.value, 0);
        }
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="renderedObject"></param>
        public static void Rotate(RenderedObject renderedObject)
        {
            renderedObject.Transform.EulerAngle = new Vector4(Cycle.value, Cycle.value, Cycle.value);
        }
        #endregion
    }
}
