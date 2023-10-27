using ILoveGraphics.Light;
using ILoveGraphics.Object;
using ILoveGraphics.Renderer;
using ILoveGraphics.Renderer.ScreenDrawer;
using MatrixCore;

namespace ILoveGraphics.Test
{
    internal class Test
    {
        public static void TestRender()
        {
            // 方便调整屏幕大小
            Console.WriteLine("Press Enter To Start!");
            Console.ReadLine();
            Console.Clear();

            // 屏幕
            var screen = new Screen(
                new ConsoleScreenDrawer(), Console.WindowWidth / 2, Console.WindowHeight
            );

            // 相机
            var camera = new Camera(screen)
            {
                FieldOfView = 90,
                Transform = new()
                {
                    Position = new(0, 0, -2.5f),
                },
                Light = new DirectionalLight
                {
                    Transform = new()
                    {
                        EulerAngle = new(45, 0, 0)
                    }
                }
            };

            // 需要渲染的所有物体
            var renderedObjects = new RenderedObject[]
            {
                new(Mesh.Load("E:\\CGL\\Programs\\CSharp\\ILoveGraphics\\ILoveGraphics\\Models\\Cube.obj"))
                {
                    Transform = new()
                    {
                        Position = new (-1, 0, 0),
                        Scale = Vector4.One * 0.5f
                    },
                },
                // new(Mesh.Load("E:\\CGL\\Programs\\CSharp\\ILoveGraphics\\ILoveGraphics\\Models\\Heart.obj")),
                // new(Mesh.Load("E:\\CGL\\Programs\\CSharp\\ILoveGraphics\\ILoveGraphics\\Models\\Pose1.obj")),
                new(Mesh.Cube())
                {
                    Transform = new()
                    {
                        Position = new(1, 0, 0),
                        //Scale = new(2, 2, 2)
                    }
                }
            };

            // 刷新间隔
            var interval = (milliseconds: 100, seconds: 0f);
            interval.seconds = 0.001f * interval.milliseconds;

            // 运行信息
            var runInfo = (frameCount: 0, startTime: DateTime.Now);

            // 变化参数
            var cycle = (value: 0f, step: interval.seconds, min: 0, max: 2 * MathF.PI);
            var pingPong = (value: 0f, step: interval.seconds, min: -1, max: 1);

            // 旋转速度
            var rotateSpeed = 1f;
            while (true)
            {
                // 刷新间隔
                Task.Delay(interval.milliseconds).Wait();

                // 统计帧数
                runInfo.frameCount++;

                // 变化参数
                MathTool.PingPong(pingPong.min, pingPong.max, ref pingPong.step, ref pingPong.value);
                MathTool.Cycle(cycle.min, cycle.max, cycle.step, ref cycle.value);

                foreach (var renderedObject in renderedObjects)
                {
                    // 模型变化
                    // renderedObjects[0].Transform.EulerAngle = new Vector4((scale - 1.5f) * 2 * 15, 0, 0);
                    renderedObject.Transform.EulerAngle = new Vector4(rotateSpeed * cycle.value, rotateSpeed * cycle.value, rotateSpeed * cycle.value);
                    // renderedObjects[0].Transform.EulerAngle = new Vector4(45, rotateScale * time, 0);
                    // renderedObject.Transform.EulerAngle = new Vector4(0, rotateSpeed * cycle.value, 0);
                    // renderedObjects[0].Transform.Scale = new Vector4(scale, 2.2f - scale);
                }

                // 渲染
                camera.Render(renderedObjects, $"""
                resolution: ({screen.Width}, {screen.Height}, {screen.Width * screen.Width})
                fps: {runInfo.frameCount / (DateTime.Now - runInfo.startTime).TotalSeconds:F2}
                """);
            }
        }
    }
}
