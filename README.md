# 使用说明
1.下载整个项目后直接导入vs  
2.启动项目选择DrawForm（或者ILoveGraphics，会将画面通过控制台渲染）  
```csharp
RenderedScene.RenderedObjects.AddRange(new RenderedObject[]
{	
	RenderedObject.Cube1,
	RenderedObject.Cube2,
});
RenderedScene.SetConsoleRenderArgs();
while (true)
{
	RenderedScene.Update(null, RenderedScene.RotateAroundY);
}
```

# 演示视频
[cow.mp4](cow.mp4)

# 项目说明
1.项目完全使用C#编写  
2.项目仅使用少量官方库（主要用于图像输出到屏幕）  

# 功能    
1.obj文件以及texture导入  
2.自定义shader编写  
3.基本的渲染管线，顶点变换->光栅化->着色->输出屏幕 