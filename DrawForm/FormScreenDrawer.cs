//#define IMAGE_MODE

using ILoveGraphics.Renderer.ScreenDrawer;
using ILoveGraphics.Shader;
using ILoveGraphics.Test;
using MatrixCore;

namespace DrawForm
{
    public partial class FormScreenDrawer : Form, IScreenDrawer
    {
        private Graphics? _graphics;
        private int _scale = 10;

        public FormScreenDrawer()
        {
            InitializeComponent();
        }

        public void Draw(Vector4[,] frameBuffer, string message)
        {
            if (_graphics == null)
                return;

            for (int x = 0; x < frameBuffer.GetLength(0); x++)
            {
                for (int y = 0; y < frameBuffer.GetLength(1); y++)
                {
                    _graphics.FillRectangle(new SolidBrush(frameBuffer[x, y].ToColor()), x * _scale, (frameBuffer.GetLength(1) - y) * _scale, _scale, _scale);
                }
            }

            _graphics.DrawString(message, Font, new SolidBrush(Color.Green), new PointF(10, 10));
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            _graphics = CreateGraphics();

            var screenDrawer =
#if IMAGE_MODE
            new ImageScreenDrawer("C:\\Users\\Administrator\\Desktop\\2.png");
#else
                this;
#endif

            var screen = new ILoveGraphics.Renderer.Screen(screenDrawer, Width / _scale, Height / _scale);


            Test.SetDefaultRenderArgs(screen);
            ((StandardShader)Test.RenderedObjects[0].Shader).Texture = ColorHelper.GetTexture(Test.Path + "Spot\\spot_texture.png");


            Btn_Start.Enabled = false;
            Btn_Start.Visible = false;

#if IMAGE_MODE
                Test.Render();
                Application.Exit();
#else
            Tmr_Update.Enabled = true;
#endif
        }

        private void Tmr_Update_Tick(object sender, EventArgs e)
        {
            Test.Render();
        }
    }
}