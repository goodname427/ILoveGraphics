namespace ILoveGraphics
{
    internal class RenderedObject
    {
        /// <summary>
        /// 物体的变换信息
        /// </summary>
        public Transform Transform { get; init; }
        /// <summary>
        /// 物体模型
        /// </summary>
        public Mesh Mesh { get; init; }

        public RenderedObject(Mesh mesh)
        {
            Transform = new Transform();
            Mesh = mesh;
        }
    }
}
