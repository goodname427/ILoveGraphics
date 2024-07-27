using ILoveGraphics.Object;
using ILoveGraphics.Test;

//Mesh.Load(@"E:\CGL\Programs\CSharp\ILoveGraphics\ILoveGraphics\models\untitled.obj");
RenderedScene.RenderedObjects.AddRange(new RenderedObject[]
{
    //RenderedObject.Cube1,
    //RenderedObject.Cube2,
    RenderedObject.Spot
});
RenderedScene.SetConsoleRenderArgs();
while (true)
{
    RenderedScene.Update(null, RenderedScene.RotateAroundY);
}

//Mesh.Load(Test.Path + "Prince\\Scene.obj");