//#define IMAGE_MODE

using ILoveGraphics.Light;
using ILoveGraphics.Object;
using ILoveGraphics.Renderer.ScreenDrawer;
using ILoveGraphics.Shader;
using ILoveGraphics.Test;
using MatrixCore;

namespace DrawForm
{
    public partial class FormScreenDrawer : Form, IScreenDrawer
    {
        #region 预制体
        /// <summary>
        /// 牛
        /// </summary>
        public static RenderedObject Spot => new(Mesh.Load("Spot\\spot_triangulated_good.obj"))
        {
            Transform = new()
            {
                Scale = Vector4.One * 4
            },
            Shader = new StandardShader
            {
                SpecularColor = Vector4.One * 0.85f,
                Texture = ColorHelper.GetTexture(Mesh.Path + "Spot\\spot_texture.png")
            }
        };
        /// <summary>
        /// 龙
        /// </summary>
        public static RenderedObject Prince => new(Mesh.Load("Prince\\Scene.obj"))
        {
            Transform = new()
            {
                Scale = Vector4.One * 2,
                EulerAngle = new Vector4(0, 90, 0)
            },
            Shader = new StandardShader
            {
                SpecularColor = Vector4.One * 0.85f,
                Texture = ColorHelper.GetTexture(Mesh.Path + "Prince\\beast_Base_Color.png")
            }
        };
        #endregion

        private Vector4[,]? _frameBuff;
        private Graphics? _graphics;
        private Bitmap? _image;
        private Graphics? _imageGraphics;
        private int _scale = 2;

        public FormScreenDrawer()
        {
            InitializeComponent();
        }

        private void Init()
        {
            _ = int.TryParse(TBox_Scale.Text, out _scale);
            _frameBuff = new Vector4[Width / _scale, Height / _scale];
            _imageGraphics = Graphics.FromImage(_image = new Bitmap(Width, Height));
            _imageGraphics.Clear(Color.Black);
            _graphics = CreateGraphics();
            _graphics.Clear(Color.Black);
        }

        public void Draw(Vector4[,] frameBuffer, string message)
        {
            if (_graphics is null || _imageGraphics is null || _frameBuff is null || _image is null)
                return;

            /*
            1、首先将画面绘制到image上，然后将image绘制到graphics上，避免了画面撕裂
            2、同时使用frameBuff缓存上一帧画面，在绘制时只绘制这一帧与上一帧不同的部分

            以上两步可大大节省绘制的耗时
             */

            for (int x = 0; x < frameBuffer.GetLength(0); x++)
            {
                for (int y = 0; y < frameBuffer.GetLength(1); y++)
                {
                    if ((_frameBuff[x, y] - frameBuffer[x, y]).SqrtMagnitude > 0.001f)
                    {
                        _imageGraphics.FillRectangle(new SolidBrush(frameBuffer[x, y].ToColor()), x * _scale, (frameBuffer.GetLength(1) - y) * _scale, _scale, _scale);
                        _frameBuff[x, y] = frameBuffer[x, y];
                    }
                }
            }

            TBox_Message.Text = message;

            Task.Run(() =>
                _graphics.DrawImage(_image, new Point())
            );
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            Init();

            var screenDrawer =
#if IMAGE_MODE
            new ImageScreenDrawer("C:\\Users\\Administrator\\Desktop\\2.png");
#else
                this;
#endif
            var screen = new ILoveGraphics.Renderer.Screen(screenDrawer, Width / _scale, Height / _scale);

            RenderedScene.RenderedObjects.AddRange(new RenderedObject[]
            {
                Spot
            });
            RenderedScene.SetDefaultRenderArgs(screen);

            Lbl_1.Visible = false;
            Btn_Start.Visible = false;
            TBox_Scale.Visible = false;
            TBox_Message.Visible = true;

#if IMAGE_MODE
            Test.Render();
            Application.Exit();
#else
            Tmr_Update.Enabled = true;
#endif
        }

        private void Tmr_Update_Tick(object sender, EventArgs e)
        {
            //Focus();
            RenderedScene.Update(
                null
                //() => ObjectController.DoMove(RenderedScene.Camera)
                //RenderedScene.RotateLight

                ,RenderedScene.RotateAroundY
            );
            //ObjectController.Reset();
        }

        private void FormScreenDrawer_MouseMove(object sender, MouseEventArgs e)
        {
            ObjectController.MouseInput(sender, e);
        }

        private void FormScreenDrawer_MouseClick(object sender, MouseEventArgs e)
        {
            if (RenderedScene.Camera is null)
                return;
            RenderedScene.Camera.Transform.Position = -RenderedScene.Camera.Transform.Position;
        }

        private void FormScreenDrawer_KeyDown(object sender, KeyEventArgs e)
        {
            ObjectController.KeyInput(sender, e);
        }
    }
}