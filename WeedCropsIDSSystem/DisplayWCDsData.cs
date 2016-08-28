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
    public partial class DisplayWCDsData : Form
    {
        private DBConnect dbConnect;  //数据库

        public DisplayWCDsData()
        {
            InitializeComponent();
            //链接数据
            dbConnect = new DBConnect();
        }

        private void DisplayWCDsData_Load(object sender, EventArgs e)
        {
            this.textBox_weeds.Enabled = false;
            this.textBox_weedsmidu.Enabled = false;
            this.textBox_cropsmidu.Enabled = false;
            this.textBox_soilmidu.Enabled = false;

            this.textBox_ciw.Enabled = false;   //杂草覆盖面积
            this.textBox_cic.Enabled = false;   //作物覆盖面积
            this.textBox_cis.Enabled = false;   //土壤覆盖面积
            this.textBox_aic.Enabled = false;   //单元格的面积 


            this.textBox_ciw.Text = FrmMainMenu.ciw.ToString();
            this.textBox_cic.Text = FrmMainMenu.cic.ToString();
            this.textBox_cis.Text = FrmMainMenu.cis.ToString();
            this.textBox_aic.Text = FrmMainMenu.aic.ToString();

            this.textBox_weeds.Text = FrmMainMenu.weedNumber.ToString();
            this.textBox_weedsmidu.Text = (FrmMainMenu.weedDensity * 100 + "%").ToString();
            this.textBox_cropsmidu.Text = (FrmMainMenu.cropDensity * 100 + "%").ToString();
            this.textBox_soilmidu.Text = (FrmMainMenu.cisDensity * 100 + "%").ToString();

            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           //Insert(string imageName,float weedCounts,float weedsDensity,float cropsDensity,float soilsDensity,float ciws,float cics,float ciss,float aics,float tRate)
            dbConnect.Insert(FrmMainMenu.imageName, FrmMainMenu.weedNumber, FrmMainMenu.weedDensity, FrmMainMenu.cropDensity,FrmMainMenu.cisDensity,FrmMainMenu.ciw,FrmMainMenu.cic,FrmMainMenu.cis,FrmMainMenu.aic,FrmMainMenu.tRate);
            //保存图像的GPS坐标与杂草威胁度
            dbConnect.InsertLatLongRate(FrmMainMenu.imageName, FrmMainMenu.latitude, FrmMainMenu.longitude, FrmMainMenu.tRate);
            this.Refresh();
            MessageBox.Show("保存成功！");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
