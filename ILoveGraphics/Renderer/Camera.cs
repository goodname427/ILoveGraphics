﻿using ILoveGraphics.Object;
using MatrixCore;

namespace ILoveGraphics.Renderer
{
    public class Camera : Object.Object
    {
        /// <summary>
        /// 相机宽高比
        /// </summary>
        public float AspectRatio => 1.0f * Screen.Width / Screen.Height;
        /// <summary>
        /// 视角范围
        /// </summary>
        public float FieldOfView { get; set; } = 90;
        /// <summary>
        /// 近平面
        /// </summary>
        public float Near { get; set; } = 1;
        /// <summary>
        /// 远平面
        /// </summary>
        public float Far { get; set; } = 3000;

        /// <summary>
        /// 渲染屏幕
        /// </summary>
        public Screen Screen { get; set; }

        /// <summary>
        /// 视图矩阵
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                var gaze = Transform.Forward;
                var top = Transform.Up;
                var left = Transform.Left;
                Matrix matrix;
                matrix = new Matrix(new float[,]
                {
                    {left.X, left.Y, left.Z, 0},
                    {top.X, top.Y, top.Z, 0},
                    {gaze.X, gaze.Y, gaze.Z, 0},
                    {0, 0, 0, 1}
                }) * Matrix.TranslationMatrix(-Transform.Position);
                return matrix;
            }
        }
        /// <summary>
        /// 透视投影矩阵
        /// </summary>
        public Matrix PerspectProjectionMatrix
        {
            get
            {
                return OrthogonalizedProjectionMatrix * new float[,]
                {
                    {Near, 0, 0, 0},
                    {0, Near, 0, 0},
                    {0, 0, Near + Far, -Near * Far},
                    {0, 0, 1, 0}
                };
            }
        }
        /// <summary>
        /// 正交矩阵
        /// </summary>
        public Matrix OrthogonalizedProjectionMatrix
        {
            get
            {
                float top = MathF.Abs(Near) * MathF.Tan(MathF.PI * FieldOfView / 360);
                float left = top * AspectRatio;

                //Matrix translate = new float[,]
                //{
                //    { 1, 0, 0, 0},
                //    { 0, 1, 0, 0},
                //    { 0, 0, 1, - ((Near + Far) / 2)},
                //    { 0, 0, 0, 1}
                //};
                //Matrix scale = new float[,]
                //{
                //    { 1 / left, 0, 0, 0},
                //    { 0, 1 / top, 0, 0},
                //    { 0, 0, 2 / (Near - Far), 0},
                //    { 0, 0, 0, 1}
                //};

                //return scale * translate;
                return new float[,]
                {
                    { 1 / left, 0, 0, 0},
                    { 0, 1 / top, 0, 0},
                    { 0, 0, 1 / (Far - Near), -Near / (Far - Near)},
                    { 0, 0, 0, 1}
                };
            }
        }
        /// <summary>
        /// 视角变换
        /// </summary>
        public Matrix ViewingMatrix =>  PerspectProjectionMatrix * ViewMatrix;

        public Camera(Screen screen)
        {
            Screen = screen;
            Transform = new Transform();
        }

        /// <summary>
        /// 对物体进行渲染
        /// </summary>
        /// <param name="renderedObjects"></param>
        public void Render(IEnumerable<RenderedObject> renderedObjects, string message = "")
        {
            Screen.Clear();

            var vvMatrix = Screen.ViewportMatrix * ViewingMatrix;
            foreach (var renderedObject in renderedObjects)
            {
                //
                // VS
                //
                var transformMatrix = renderedObject.Transform.TransformMatrix;
                var transformInverseTransposeMatrix = transformMatrix.InverseMatrix.TransposedMatrix;

                // m矩阵变换
                var worldVertexs = (from vertex in renderedObject.Mesh.Vertexes
                                    select transformMatrix * vertex).ToArray();

                // 法线
                var normals = (from vertex in renderedObject.Mesh.Normals
                               select transformInverseTransposeMatrix * vertex).ToArray();

                // 视图+投影+视口变换, W存储z轴的正负信息
                var screenVertexs = (from vertex in worldVertexs
                                     let transVertex = vvMatrix * vertex
                                     let w = transVertex.W
                                     select transVertex with { W = MathF.Abs(w) } / w
                                     ).ToArray();

                // 光栅化
                foreach (var triangleIndex in renderedObject.Mesh.Triangles)
                {
                    // 齐次空间裁切
                    // 本来应该在顶点变换阶段就裁切的，但是由于合并了视口变换和视图变换，所以将裁切转移到这里
                    int i = 0;
                    for (; i < 3; i++)
                    {
                        var v = screenVertexs[triangleIndex.Vertexs[i]];

                        if (!(v.W < 0 || v.X < 0 || v.X >= Screen.Width || v.Y < 0 || v.Y >= Screen.Height))
                            break;
                    }

                    if (i == 3)
                        continue;

                    // 三角面光栅化
                    // PS
                    Screen.Rasterize(
                        triangleIndex.Triangle(screenVertexs, worldVertexs, normals, renderedObject.Mesh.TextureCoords),
                        renderedObject.Shader,
                        Transform.Position
                    );
                }
            }

            Screen.Draw(message);
        }
    }
}
