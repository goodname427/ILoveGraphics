using Accessibility;
using ILoveGraphics.Renderer.ScreenDrawer;
using ILoveGraphics.Test;
using MatrixCore;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace DrawForm
{
    public partial class DrawForm : Form, IScreenDrawer
    {
        #region
        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hDC, int x, int y);
        [DllImport("gdi32.dll")]
        private static extern int SetPixel(IntPtr hdc, int x, int y, int color);
        private static uint UintColor(Vector4 color)
        {
            // 返回由RGB构成的32位uint
            uint R = (uint)(color.X * 255);
            uint G = (uint)(color.Y * 255);
            uint B = (uint)(color.Z * 255);
            G <<= 8;
            B <<= 16;
            return ((uint)(R | G | B));
        }
        private static Color ColorArgb(Vector4 color)
        {
            color.X = MathTool.Clamp(color.X, 0, 1);
            color.Y = MathTool.Clamp(color.Y, 0, 1);
            color.Z = MathTool.Clamp(color.Z, 0, 1);
            color *= 255;
            return Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z);
        }
        #endregion

        private Graphics? _graphics;
        private nint _hdc;
        private int _scale = 10;

        public DrawForm()
        {
            InitializeComponent();
        }

        public void Draw(Vector4[,] frameBuffer, string message)
        {
            if (_graphics == null)
                return;

            //_hdc = _graphics.GetHdc();
            // _graphics.Clear(Color.White);
            for (int x = 0; x < frameBuffer.GetLength(0); x++)
            {
                for (int y = 0; y < frameBuffer.GetLength(1); y++)
                {
                    // _ = SetPixel(_hdc, x, frameBuffer.GetLength(1) - y, (int)UintColor(frameBuffer[x, y]));
                    _graphics.FillRectangle(new SolidBrush(ColorArgb(frameBuffer[x, y])), x * _scale, (frameBuffer.GetLength(1) - y) * _scale, _scale, _scale);
                }
            }
            //_graphics.ReleaseHdc();
            _graphics.DrawString(message, Font, new SolidBrush(Color.Green), new PointF(10, 10));


        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            _graphics = CreateGraphics();

            var screen = new ILoveGraphics.Renderer.Screen(this, Width / _scale, Height / _scale);
            // screen.ScreenDrawers.Add(new ConsoleScreenDrawer());

            Test.SetDefaultRenderArgs(screen);
            Btn_Start.Enabled = false;
            Btn_Start.Visible = false;
            Tmr_Update.Enabled = true;
        }

        private void Tmr_Update_Tick(object sender, EventArgs e)
        {
            Test.Render();
            // Task.Run(() =>);
        }
    }
}