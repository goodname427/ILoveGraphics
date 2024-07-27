using ILoveGraphics.Object;
using ILoveGraphics.Renderer.ScreenDrawer;
using ILoveGraphics.Renderer;
using ILoveGraphics.Test;

// 方便调整屏幕大小
Console.WriteLine("Press Enter To Start!");
Console.ReadLine();
Console.Clear();

// 屏幕
var screen = new Screen(
    new ConsoleScreenDrawer(), Console.WindowWidth / 2, Console.WindowHeight
);

RenderedScene.RenderedObjects.AddRange(new RenderedObject[]
{
    //RenderedObject.Cube1,
    //RenderedObject.Cube2,
    RenderedObject.Spot
    //RenderedObject.Heart
    //new(Mesh.HeartPlane())
    //{
    //    Transform = new()
    //    {
    //        Scale = Vector4.One * 1
    //    }
    //}
});

RenderedScene.SetDefaultRenderArgs(screen);

while (true)
{
    RenderedScene.Update(null
        , RenderedScene.RotateAroundY
    );
}