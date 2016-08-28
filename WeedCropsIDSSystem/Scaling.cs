using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WeedCropsIDSSystem
{
    public partial class Scaling : Form
    {
        public Scaling()
        {
            InitializeComponent();
        }

        private void Scaling_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = (trackBar1.Value / 100.0).ToString();
        }
        private void Scaling_MouseLeave(object sender, EventArgs e)
        {

        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            FrmMainMenu.suoxiao = textBox1.Text;
            this.Close();
        }

        private void Scaling_Leave(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
