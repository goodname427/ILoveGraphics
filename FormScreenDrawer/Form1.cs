
namespace FormScreenDrawer
{
    public partial class FormScreenDrawer : Form
    {
        private Graphics _graphics;

        public FormScreenDrawer()
        {
            InitializeComponent();
        }

        public void Draw(Image image)
        {

        }

        private void FormScreenDrawer_Paint(object sender, PaintEventArgs e)
        {
            _graphics = e.Graphics;
        }
    }
}