using ILoveGraphics.Object;
using ILoveGraphics.Test;
using MatrixCore;

namespace DrawForm
{
    public static class ObjectController
    {
        private static bool _first = false;
        private static Vector4 _mousePosition;

        public static Vector4 Direction;
        public static Vector4 MouseDirection;

        public static void KeyInput(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    Direction.Z = 1;
                    break;
                case 's':
                    Direction.Z = -1;
                    break;
                case 'a':
                    Direction.X = 1;
                    break;
                case 'd':
                    Direction.X = -1;
                    break;
            }
        }

        public static void MouseInput(object sender, MouseEventArgs e)
        {
            var mousePosition = new Vector4(e.Location.X, e.Location.Y);
            var delta = mousePosition - _mousePosition;
            _mousePosition = mousePosition;
            
            if (!_first)
            {
                _first = true;
                return;
            }
            delta = new Vector4(delta.Y, -delta.X) * 0.001f;
            MouseDirection = delta;
            RenderedScene.Message = _mousePosition.ToString();
        }

        public static void DoMove(ILoveGraphics.Object.Object? @object)
        {
            if (@object == null)
                return;

            @object.Transform.Position += Direction;
            @object.Transform.EulerAngle += MouseDirection;
        }
    }
}
