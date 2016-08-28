using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;   

namespace WeedCropsIDSSystem
{
    public partial class ListWeedsCropsData : Form
    {
        private DBConnect dbConnect; //数据库

          string saveFileName = string.Empty;
          SaveFileDialog saveDialog;

        public ListWeedsCropsData()
        {
            InitializeComponent();
            dbConnect = new DBConnect();
        }

        //提取数据库中图像的原始数据
        private void ExtractDataOriginalPicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string>[] listData;
            listData = dbConnect.Select();

            dataGridView1.Rows.Clear();

            for (int i = 0; i < listData[0].Count; i++)
            {
                int j = i + 1;
                int number = dataGridView1.Rows.Add();
                dataGridView1.Rows[number].HeaderCell.Value = j.ToString(); //显示添加的序号
                dataGridView1.Rows[number].Cells[0].Value = listData[0][i];
                dataGridView1.Rows[number].Cells[1].Value = listData[1][i];
                dataGridView1.Rows[number].Cells[2].Value = listData[2][i];
                dataGridView1.Rows[number].Cells[3].Value = listData[3][i];
                dataGridView1.Rows[number].Cells[4].Value = listData[4][i];
                dataGridView1.Rows[number].Cells[5].Value = listData[5][i];
                dataGridView1.Rows[number].Cells[6].Value = listData[6][i];
                dataGridView1.Rows[number].Cells[7].Value = listData[7][i];
                dataGridView1.Rows[number].Cells[8].Value = listData[8][i];
                dataGridView1.Rows[number].Cells[9].Value = listData[9][i];
            }
        }



        //导出原始数据在Excel里
        /// add com "Microsoft Excel Object Library"   
        /// using Excel=Microsoft.Office.Interop.Excel;  
        private void ExportOriginalDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件|*.xls";
            saveDialog.FileName = "杂草和作物数据";
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;

            ProcessOperator process = new ProcessOperator();
            //处理时间调用
            process.MessageInfo = "正在导出数据中，请稍等片刻...";
            process.BackgroundWork = this.Do;
            process.BackgroundWorkerCompleted += new EventHandler<BackgroundWorkerEventArgs>(process_BackgroundWorkerCompleted);
            process.Start();
        }

        //----- //显示进度条---进度条测试--------------------------------------------------------------------------------------
        private void process_BackgroundWorkerCompleted(object sender, BackgroundWorkerEventArgs e)
        {
            if (e.BackGroundException == null)
            {
                MessageBox.Show("文件： " + saveFileName + ".xls 保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("导出完成");
            }
            else
            {
                MessageBox.Show("异常:" + e.BackGroundException.Message);
            }
        }
        private void Do()
        {
            //-------处理方法----------------------------------------------
          
          
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能未安装Excel!");
                return;
            }

            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1

            //写入标题
            for (int i = 0; i < dataGridView1.ColumnCount; i++)  
            {
                worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }
            //写入数值
            for (int r = 0; r < dataGridView1.Rows.Count; r++)
            {
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dataGridView1.Rows[r].Cells[i].Value;
                }
                System.Windows.Forms.Application.DoEvents();
            }

            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }
            }
            xlApp.Quit();
            GC.Collect();//强行销毁
           // MessageBox.Show("文件： " + saveFileName + ".xls 保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(20);
            }
        }
        //--------------------------------------------------------------------------------------------
    }
}
