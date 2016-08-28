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
    public partial class progressBarShow : Form
    {
        public progressBarShow()
        {
            InitializeComponent();
        }

        // 设置提示信息
        public string MessageInfo
        {
            set { this.labelInfor.Text = value; }
        }

        // 设置进度条显示值
        public int ProcessValue
        {
            set { this.progressBar1.Value = value; }
        }

        // 设置进度条样式
        public ProgressBarStyle ProcessStyle
        {
            set { this.progressBar1.Style = value; }
        }
    }
}
