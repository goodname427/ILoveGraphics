using ILoveGraphics.Light;
using ILoveGraphics.Object;
using ILoveGraphics.Renderer;
using ILoveGraphics.Renderer.ScreenDrawer;
using MatrixCore;

namespace ILoveGraphics.Test
{
    public static class RenderedScene
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
        public static string Message { get; set; } = "";
        /// <summary>
        /// 场景中的运行信息
        /// </summary>
        public static RunInfo CurrentRunInfo { get; private set; } = new RunInfo();
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
                    Direction = new Vector4(1, -1, 1, 0),
                    Intensity = 1
                }
            });

            // 相机
            Camera = new Camera(screen)
            {
                FieldOfView = 90,
                Transform = new()
                {
                    Position = new(0, 0, -10f),
                },
            };

            // 运行信息
            CurrentRunInfo.FrameCount = 0;
            CurrentRunInfo.StartTime = DateTime.Now;
            CurrentRunInfo.LastTime = DateTime.Now;

            // 变化参数
            Cycle = (0f, 1, 0f, 2 * MathF.PI);
            PingPong = (0f, 1, -1f, 1f);
        }

        /// <summary>
        /// 更新场景
        /// </summary>
        /// <param name="objectUpdate"></param>
        public static void Update(Action? sceneUpdate = null, params Action<RenderedObject>[] objectUpdate)
        {
            // 统计运行信息
            var now = DateTime.Now;
            CurrentRunInfo.DeltaTime = (float)(now - CurrentRunInfo.LastTime).TotalSeconds;
            CurrentRunInfo.LastTime = now;

            CurrentRunInfo.FrameCount++;

            // 变化参数
            MathTool.PingPong(PingPong.min, PingPong.max, ref PingPong.step, ref PingPong.value);
            MathTool.Cycle(Cycle.min, Cycle.max, Cycle.step * CurrentRunInfo.DeltaTime, ref Cycle.value);

            sceneUpdate?.Invoke();
            foreach (var renderedObject in RenderedObjects)
            {
                foreach (var action in objectUpdate)
                    action?.Invoke(renderedObject);
            }

            // 渲染
            Camera?.Render(RenderedObjects, $"""
                resolution: ({Camera.Screen.Width}, {Camera.Screen.Height}, {Camera.Screen.Width * Camera.Screen.Width})
                fps: {CurrentRunInfo.FPS:F2}
                deltaTime: {CurrentRunInfo.DeltaTime * 1000:F2} ms
                message: {Message}
                """);
        }

        #region 预设好的操作
        /// <summary>
        /// 使灯光绕y轴旋转
        /// </summary>
        public static void RotateLight()
        {
            var dir = new Vector4(MathF.Cos(Cycle.value), -1 , MathF.Sin(Cycle.value));
            ((DirectionalLight)Lights[0]).Direction = dir;
        }
        /// <summary>
        /// 绕y轴旋转
        /// </summary>
        /// <param name="renderedObject"></param>
        public static void RotateAroundY(RenderedObject renderedObject)
        {
            renderedObject.Transform.EulerAngle = new Vector4(0, Cycle.value, 0);
        }
        /// <summary>
        /// 绕y轴旋转
        /// </summary>
        /// <param name="renderedObject"></param>
        public static void RotateAroundX(RenderedObject renderedObject)
        {
            renderedObject.Transform.EulerAngle = new Vector4(Cycle.value, 0, 0);
        }
        /// <summary>
        /// 绕y轴旋转
        /// </summary>
        /// <param name="renderedObject"></param>
        public static void RotateAroundZ(RenderedObject renderedObject)
        {
            renderedObject.Transform.EulerAngle = new Vector4(0, 0, Cycle.value);
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
    
        public class RunInfo
        {
            /// <summary>
            /// 总帧数
            /// </summary>
            public int FrameCount { get; set; }
            /// <summary>
            /// 上一次运行时间
            /// </summary>
            public DateTime LastTime { get; set;}
            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime StartTime { get; set;}
            /// <summary>
            /// 上一帧到这一帧的时间差
            /// </summary>
            public float DeltaTime { get; set; }
            /// <summary>
            /// 每秒帧数
            /// </summary>
            public float FPS => (float)(FrameCount / (LastTime - StartTime).TotalSeconds);
        }
    }
}
