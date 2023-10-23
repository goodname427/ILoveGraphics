namespace ILoveGraphics
{
    internal class RenderedObject
    {
        /// <summary>
        /// 物体的变换信息
        /// </summary>
        public Transform Transform { get; } = new Transform();
        /// <summary>
        /// 物体模型
        /// </summary>
        public Mesh Mesh { get; set; }

        public RenderedObject(Mesh mesh)
        {
            Transform = new Transform();
            Mesh = mesh;
        }
    }
}
