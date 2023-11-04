using MatrixCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILoveGraphics.Renderer.ScreenDrawer
{
    public interface IScreenDrawer
    {
        /// <summary>
        /// 将缓冲区内容输出
        /// </summary>
        /// <param name="frameBuffer"></param>
        /// <param name="message"></param>
        public void Draw(Vector4[,] frameBuffer, string message);
    }
}
