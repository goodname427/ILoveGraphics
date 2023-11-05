using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILoveGraphics.Object
{
    public class Object
    {
        /// <summary>
        /// 物体的变换信息
        /// </summary>
        public Transform Transform { get; init; } = new Transform();
    }
}
