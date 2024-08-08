# 项目说明

本项目为纯软件实现光栅化渲染管线，运算只基于CPU。  

## 演示视频

[Cow](Cow.mp4)
[Cow(cmd)](Cow_cmd.mp4)

## 项目依赖

1.项目完全使用C#编写（.Net 7.0）。  
2.项目仅使用少量WinForm库，主要用于图像绘制到Windows窗口。  

## 项目功能    

1.Obj文件以及Texture导入。  
2.支持自定义Pixel Shader。  
3.基本的渲染管线，顶点变换->光栅化->着色->输出屏幕。  

# 使用说明

## Quick Start

1.下载整个项目后直接导入vs。  
2.启动项目选择DrawForm（或者DrawConsole，会将画面通过控制台渲染）。

## 部分代码说明

以下会说明部分代码功能，以便快速验证项目功能。

### 启动

以DrawConsole为例，说明启动项目需要的必要步骤。

```csharp
// 创建屏幕
var screen = new Screen(
    new ConsoleScreenDrawer(), Console.WindowWidth / 2, Console.WindowHeight
);

// 添加需要渲染的物体（可以使用预设的物体或者自行创建）
RenderedScene.RenderedObjects.AddRange(new RenderedObject[]
{
    RenderedObject.Spot
});

// 初始化一些基本的渲染参数，例如绑定相机和屏幕，添加光源等等。
RenderedScene.SetDefaultRenderArgs(screen);

// 循环渲染
while (true)
{
    RenderedScene.Update(null
        , RenderedScene.RotateAroundY
    );
}
```

### 创建Mesh

支持两种方式创建Mesh。  

1.手动输入顶点数据和索引数据。
```csharp
// 创建一个立方体
var cube = new Mesh(
    // 顶点数据
    new Vector4[]
    {
        new Vector4(-0.5f, 0.5f, 0.5f),
        new Vector4(0.5f, 0.5f, 0.5f),
        new Vector4(0.5f, 0.5f, -0.5f),
        new Vector4(-0.5f, 0.5f, -0.5f),
        new Vector4(-0.5f, -0.5f, 0.5f),
        new Vector4(0.5f, -0.5f, 0.5f),
        new Vector4(0.5f, -0.5f, -0.5f),
        new Vector4(-0.5f, -0.5f, -0.5f),
    },
    // 索引数据
    new TriangleIndex[]
    {
        new()
        {
            Vertexs = new int[]{ 0, 1, 2 }
        },
        new()
        {
            Vertexs = new int[]{ 0, 2, 3 }
        },
        new()
        {
            Vertexs = new int[]{ 4, 7, 5 }
        },
        new()
        {
            Vertexs = new int[]{ 5, 7, 6 }
        },
        new()
        {
            Vertexs = new int[]{ 3, 2, 6 }
        },
        new()
        {
            Vertexs = new int[]{ 3, 6, 7 }
        },
        new()
        {
            Vertexs = new int[]{ 0, 4, 1 }
        },
        new()
        {
            Vertexs = new int[]{ 1, 4, 5 }
        },
        new()
        {
            Vertexs = new int[]{ 1, 5, 2 }
        },
        new()
        {
            Vertexs = new int[]{ 2, 5, 6 }
        },
        new()
        {
            Vertexs = new int[]{ 0, 3, 7 }
        },
        new()
        {
            Vertexs = new int[]{ 0, 7, 4 }
        },
    }
);
```

2.加载Obj文件。

```csharp
// 可以输入绝对路径，也可以输入局部路径（如果需要使用局部路径，请将模型放在'ILoveGraphics/Models'目录下）
var cube = Mesh.Load("Cube.obj");
```


### 创建Texture

仅在DrawForm项目中支持创建Texture。

```csharp
// 这里只支持绝对路径（。。因为还没写局部路径的功能）
var texture = ColorHelper.GetTexture(Mesh.Path + "Spot\\spot_texture.png")
```

### 创建RenderedObject

如果需要渲染自定义的物体，首先需要创建RenderedObject。

```csharp
// 参数为物体所使用的Mesh。
var renderedObject = new(Mesh.Load("Spot\\spot_triangulated_good.obj"))
{
    // 物体的transform信息，可以不填使用默认的
    Transform = new()
    {
        Scale = Vector4.One * 4
    },
    // 物体使用的Shader，在这里指定Texture以及一些渲染信息
    Shader = new StandardShader
    {
        SpecularColor = Vector4.One * 0.85f,
        Texture = ColorHelper.GetTexture(Mesh.Path + "Spot\\spot_texture.png")
    }
};
```

### 自定义Shader

目前只支持自定义PS。

```
// 实现以下接口即可
public interface IShader
{
    // args会传递当前像素的一些信息。
    Vector4 GetColor(ShaderArgs args);

    public class ShaderArgs
    {
        // 世界坐标
        public required Vector4 ShaderPosition { get; init; }
        // 纹理坐标
        public required Vector4 TextureCoord { get; init; }
        // 法线
        public required Vector4 Normal { get; init; }
        // 相机坐标
        public Vector4 EyePosition { get; init; }
    }
}

// 以下是项目中目前仅有的标准实现，Blinn-Phong光照模型
public class StandardShader : IShader
{
    // 基础颜色，当没有填充纹理的时候会使用这个颜色作为漫反射的基础颜色 
    public Vector4 BaseColor { get; set; } = new Vector4(1, 1, 1, 1);
    // 高光颜色，一般默认即可，默认为白色
    public Vector4 SpecularColor { get; set; } = new Vector4(1, 1, 1, 1);
    // 纹理
    public Texture? Texture { get; set; }
    // 粗糙度，影响高光
    public float Roughness { get; set; } = 128;

    public Vector4 GetColor(IShader.ShaderArgs args)
    {
        // 视线
        var eyeDir = (args.EyePosition - args.ShaderPosition).Normalized;
        var color = Vector4.Zero;
        foreach (var light in RenderedScene.Lights)
        {
            // 光照方向, 从该点指向光源
            var lightDir = -light.GetLightDirection(args.ShaderPosition);

            // 光强
            var intensity = light.GetIntensity(args.ShaderPosition);

            // 漫反射
            color += (Texture?.GetColor(args.TextureCoord) ?? BaseColor) * intensity * MathF.Max(0, args.Normal * lightDir);

            // 高光
            var h = ((eyeDir + lightDir) / 2).Normalized;
            color += SpecularColor * intensity * MathF.Pow(MathF.Max(0, args.Normal * h), Roughness);

            // 环境光
            color += RenderedScene.Ambient;
        }

        color.W = 1;
        return color;
    }
}
```




