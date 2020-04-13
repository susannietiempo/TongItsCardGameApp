using System;
using System.Windows.Forms;

namespace Cards
{
    public partial class form2 : Form
    {
        public form2()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            f1.Show();
        }

    }
}
