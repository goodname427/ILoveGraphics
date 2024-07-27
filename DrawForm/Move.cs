using ILoveGraphics.Test;
using MatrixCore;

namespace DrawForm
{
    public static class ObjectController
    {
        private static bool _first = false;
        private static Vector4 _mousePosition;

        public static Vector4 _direction;
        public static Vector4 _mouseDirection;
        public static float _moveSpeed = 0.4f;
        public static float _rotateSpeed = 0.01f;

        public static void Reset()
        {
            _direction = Vector4.Zero;
            _mouseDirection = Vector4.Zero;
        }

        public static void KeyInput(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    _direction.Z = 1;
                    break;
                case Keys.S:
                    _direction.Z = -1;
                    break;
                case Keys.A:
                    _direction.X = 1;
                    break;
                case Keys.D:
                    _direction.X = -1;
                    break;
            }

            _direction *= _moveSpeed;
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

            delta = new Vector4(delta.Y, -delta.X) * _rotateSpeed;
            _mouseDirection = delta;
            RenderedScene.Message = _mousePosition.ToString();
        }

        public static void DoMove(ILoveGraphics.Object.Object? @object)
        {
            if (@object == null)
                return;

            @object.Transform.Position += _direction;
            @object.Transform.EulerAngle += _mouseDirection;
        }
    }
}
