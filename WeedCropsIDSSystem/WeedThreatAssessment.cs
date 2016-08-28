using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

namespace WeedCropsIDSSystem
{
    public partial class WeedThreatAssessment : Form
    {

        private DBConnect dbConnect; //数据库


        public WeedThreatAssessment()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
            dbConnect = new DBConnect();
            WeedsRateGPS();
            averageTRate.Text = (dbConnect.AverageRate() + "").ToString();
        }

        //提取数据库中图像的杂草威胁率GPS数据
        private void WeedsRateGPS()
        {
            List<string>[] listData;
            listData = dbConnect.SelectImageNameRate();

            dataWeedsRateGridView.Rows.Clear();

            for (int i = 0; i < listData[0].Count; i++)
            {
                int j = i + 1;
                int number = dataWeedsRateGridView.Rows.Add();
                dataWeedsRateGridView.Rows[number].HeaderCell.Value = j.ToString(); //显示添加的序号
                dataWeedsRateGridView.Rows[number].Cells[0].Value = listData[0][i];
                dataWeedsRateGridView.Rows[number].Cells[1].Value = listData[1][i];

            }
        }
    }
}
