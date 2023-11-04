using ILoveGraphics.Light;
using ILoveGraphics.Object;
using ILoveGraphics.Renderer;
using ILoveGraphics.Renderer.ScreenDrawer;
using MatrixCore;

namespace ILoveGraphics.Test
{
    public class Test
    {
        public static Camera? Camera { get; set; }
        public static List<RenderedObject> RenderedObjects { get; } = new List<RenderedObject>();
        public static (int milliseconds, float seconds) Interval;
        public static (int frameCount, DateTime startTime) RunInfo;
        public static (float value, float step, float min, float max) Cycle;
        public static (float value, float step, float min, float max) PingPong;
        public static float RotateSpeed;

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

        public static void SetDefaultRenderArgs(Screen screen)
        {
            // 光照
            BaseLight.Lights.AddRange(new BaseLight[]{
                new DirectionalLight
                {
                    Direction = new Vector4(0, 1, -1)
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

            // 需要渲染的所有物体
            RenderedObjects.AddRange(new RenderedObject[]
            {
                //new(Mesh.Cube())
                //{
                //    Transform = new()
                //    {
                //        Position = new(1, 0, 0),
                //        Scale = new(1, 1, 1)
                //    }
                //},
                //new(Mesh.Load("E:\\CGL\\Programs\\CSharp\\ILoveGraphics\\ILoveGraphics\\Models\\Cube.obj"))
                //{
                //    Transform = new()
                //    {
                //        Position = new (-1, 0, 0),
                //        Scale = Vector4.One * 0.5f
                //    },
                //},
                new(Mesh.Load("E:\\CGL\\Programs\\CSharp\\ILoveGraphics\\ILoveGraphics\\Models\\Heart.obj"))
                {
                    Shader = new Shader.StandardShader
                    {
                        BaseColor = new Vector4(1)
                    }
                },
                //new(Mesh.Load("E:\\CGL\\Programs\\CSharp\\ILoveGraphics\\ILoveGraphics\\Models\\Pose1.obj")),
            });

            // 刷新间隔
            Interval = (milliseconds: 100, seconds: 0f);
            Interval.seconds = 0.001f * Interval.milliseconds;

            // 运行信息
            RunInfo = (frameCount: 0, startTime: DateTime.Now);

            // 变化参数
            Cycle = (value: 0f, step: Interval.seconds, min: 0f, max: 2 * MathF.PI);
            PingPong = (value: 0f, step: Interval.seconds, min: -1f, max: 1f);

            // 旋转速度
            RotateSpeed = 1;
        }

        public static void Render(Action<RenderedObject>? update = null)
        {
            // 刷新间隔
            Task.Delay(Interval.milliseconds).Wait();

            // 统计帧数
            RunInfo.frameCount++;

            // 变化参数
            MathTool.PingPong(PingPong.min, PingPong.max, ref PingPong.step, ref PingPong.value);
            MathTool.Cycle(Cycle.min, Cycle.max, Cycle.step, ref Cycle.value);

            foreach (var renderedObject in RenderedObjects)
            {
                update?.Invoke(renderedObject);
                // 
                // renderedObject.Transform.EulerAngle = new Vector4((scale - 1.5f) * 2 * 15, 0, 0);
                
                // 360°旋转
                // renderedObject.Transform.EulerAngle = new Vector4(RotateSpeed * Cycle.value, RotateSpeed * Cycle.value, RotateSpeed * Cycle.value);
                
                // 绕y轴旋转
                renderedObject.Transform.EulerAngle = new Vector4(0, RotateSpeed * Cycle.value, 0);

                // x, y轴来回缩放
                // renderedObjects[0].Transform.Scale = new Vector4(scale, 2.2f - scale);
            }

            // 渲染
            Camera?.Render(RenderedObjects, $"""
                resolution: ({Camera.Screen.Width}, {Camera.Screen.Height}, {Camera.Screen.Width * Camera.Screen.Width})
                fps: {RunInfo.frameCount / (DateTime.Now - RunInfo.startTime).TotalSeconds:F2}
                """);
        }
    }
}
