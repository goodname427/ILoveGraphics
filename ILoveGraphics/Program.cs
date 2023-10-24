using ILoveGraphics;
using MatrixCore;

Console.WriteLine("Press Enter To Start!");
Console.ReadLine();
Console.Clear();

var screen = new Screen();

var camera = new Camera(screen)
{
    FieldOfView = 90,
    Positon = new Vector4(0, 0, -2f),
    Light = new DirectLight(new Vector4(0, 1, -1), new PixelColor(PixelColor.MaxAlpha, ConsoleColor.Blue))
};

var renderedObjects = new RenderedObject[]
{
    new(Mesh.Cube())
    {
        Transform = new Transform
        {
            EulerAngle = new Vector4(90, 0, 0)
        }
    }
};

int interval = 50;
float time = 0;
float scale = 1;
float step = 0.001f * interval;
float rotateScale = 1;
while (true)
{
    Task.Delay(interval).Wait();
    MathTool.PingPong(1, 2, ref step, ref scale);

    time += 0.001f * interval;
    if (time > 360)
        time -= 360;


    // renderedObjects[0].Transform.EulerAngle = new Vector4((scale - 1.5f) * 2 * 15, 0, 0);
    // renderedObjects[0].Transform.EulerAngle = new Vector4(rotateScale * time, rotateScale * time, rotateScale * time);
    renderedObjects[0].Transform.EulerAngle = new Vector4(0, rotateScale * time, 0);
    // renderedObjects[0].Transform.Scale = new Vector4(scale, 3 - scale);


    camera.Renderer(renderedObjects);
}