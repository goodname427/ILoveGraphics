using MatrixCore;

namespace ILoveGraphics
{
    internal class Test
    {
        public static void TestRender()
        {
            Console.WriteLine("Press Enter To Start!");
            Console.ReadLine();
            Console.Clear();

            var screen = new Screen();

            var camera = new Camera(screen)
            {
                FieldOfView = 90,
                Positon = new Vector4(0, 0, -3f),
                Light = new DirectionalLight(new Vector4(0, 1, -1), new PixelColor(PixelColor.MaxAlpha, ConsoleColor.Red))
            };

            var renderedObjects = new RenderedObject[]
            {
                // new(Mesh.Load("E:\\CGL\\Programs\\CSharp\\ILoveGraphics\\ILoveGraphics\\models\\untitled.obj")),
                new(Mesh.Load("E:\\CGL\\Programs\\CSharp\\ILoveGraphics\\ILoveGraphics\\models\\heart.obj")),
                // new(Mesh.Cube())
            };

            int interval = 50;
            float time = 0;
            float scale = 1;
            float step = 0.001f * interval;
            float rotateScale = 1;
            float frameCount = 0;
            var start = DateTime.Now;
            while (true)
            {
                Task.Delay(interval).Wait();
                frameCount++;
                
                
                MathTool.PingPong(1, 1.2f, ref step, ref scale);

                time += 0.001f * interval;
                if (time > 360)
                    time -= 360;


                // renderedObjects[0].Transform.EulerAngle = new Vector4((scale - 1.5f) * 2 * 15, 0, 0);
                // renderedObjects[0].Transform.EulerAngle = new Vector4(rotateScale * time, rotateScale * time, rotateScale * time);
                // renderedObjects[0].Transform.EulerAngle = new Vector4(45, rotateScale * time, 0);
                renderedObjects[0].Transform.EulerAngle = new Vector4(0, rotateScale * time, 0);
                // renderedObjects[0].Transform.Scale = new Vector4(scale, 2.2f - scale);


                camera.Render(renderedObjects, $"resolution: ({screen.Width}, {screen.Height})\npixel count: {screen.Width * screen.Width}\nfps:{frameCount / (DateTime.Now - start).TotalSeconds:F2}");
            }
        }
    }
}
