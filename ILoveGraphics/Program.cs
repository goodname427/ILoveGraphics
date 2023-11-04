using ILoveGraphics.Object;
using ILoveGraphics.Test;
using MatrixCore;

//Mesh.Load(@"E:\CGL\Programs\CSharp\ILoveGraphics\ILoveGraphics\models\untitled.obj");
RenderedScene.RenderedObjects.AddRange(new RenderedObject[]
{
    RenderedObject.Cube1,
    RenderedObject.Cube2,
});
RenderedScene.SetConsoleRenderArgs();
while (true)
{
    RenderedScene.Update(RenderedScene.RotateAroundY);
}

//Mesh.Load(Test.Path + "Prince\\Scene.obj");