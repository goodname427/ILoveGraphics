using ILoveGraphics;
using MatrixCore;

Console.WriteLine("Press Enter To Start!");
Console.ReadLine();
Console.Clear();

var screen = new Screen();

var camera = new Camera(screen)
{
    FieldOfView = 90,
    Positon = new Vector4(0, 0, -6.4f)
};

camera.Reset();

var mesh = new Mesh(
    new Vector4[]
    {
        new Vector4(0, -1, 0.3f),
        new Vector4(3, 1),
        new Vector4(2, 2, 0.3f),
        new Vector4(1, 2),
        new Vector4(0, 1),
        new Vector4(-1, 2),
        new Vector4(-2, 2, 0.3f),
        new Vector4(-3, 1),
    },
    new int[]
    {
        0, 1, 4,
        1, 2, 3,
        1, 3, 4,
        0, 4, 7,
        5, 6, 7,
        4, 5, 7,
    }
);

var renderedObjects = new RenderedObject[]
{
    new(mesh)
};

int interval = 10;
float time = 0;
float scale = 1;
float step = 0.001f * interval;
while (true)
{
    Task.Delay(interval).Wait();
    MathTool.PingPong(1, 2, ref step, ref scale);

    time += 0.001f * interval;
    if (time > 360)
        time -= 360;

    // renderedObjects[0].Transform.EulerAngle = new Vector4((scale - 1.5f) * 2 * 15, 0, 0);
    renderedObjects[0].Transform.EulerAngle = new Vector4(0, 5 * time, 0);
    renderedObjects[0].Transform.Scale = new Vector4(scale, 3 - scale);
    camera.Renderer(renderedObjects);
}