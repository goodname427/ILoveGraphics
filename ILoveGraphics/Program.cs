﻿using ILoveGraphics.Object;
using ILoveGraphics.Test;
using MatrixCore;
using static System.Formats.Asn1.AsnWriter;

//Mesh.Load(@"E:\CGL\Programs\CSharp\ILoveGraphics\ILoveGraphics\models\untitled.obj");
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
RenderedScene.SetConsoleRenderArgs();
while (true)
{
    RenderedScene.Update(null
        , RenderedScene.RotateAroundY
        );
}

//Mesh.Load(Test.Path + "Prince\\Scene.obj");