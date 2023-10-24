﻿using MatrixCore;

namespace ILoveGraphics
{
    internal class DirectLight
    {
        private Vector4 _direction;

        /// <summary>
        /// 光照方向
        /// </summary>
        public Vector4 Direction
        {
            get => _direction;
            set => _direction = value.Normalized;
        }
        /// <summary>
        /// 光照颜色
        /// </summary>
        public PixelColor Color { get; set; }

        public DirectLight() : this(new Vector4(0, 1, 1, 0), new(PixelColor.MaxAlpha, ConsoleColor.White))
        {

        }

        public DirectLight(Vector4 direction) : this(direction, new(PixelColor.MaxAlpha, ConsoleColor.White))
        {

        }

        public DirectLight(Vector4 direction, PixelColor color)
        {
            Direction = direction;
            Color = color;
        }
    }
}
