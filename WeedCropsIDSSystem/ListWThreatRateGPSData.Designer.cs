namespace WeedCropsIDSSystem
{
    partial class ListWThreatRateGPSData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ExtractDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportWtGPSDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataRateGridView = new System.Windows.Forms.DataGridView();
            this.imageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.latitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.longitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataRateGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExtractDataToolStripMenuItem,
            this.ExportWtGPSDataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(459, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ExtractDataToolStripMenuItem
            // 
            this.ExtractDataToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ExtractDataToolStripMenuItem.Name = "ExtractDataToolStripMenuItem";
            this.ExtractDataToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.ExtractDataToolStripMenuItem.Text = "数据列表";
            this.ExtractDataToolStripMenuItem.Click += new System.EventHandler(this.ExtractDataToolStripMenuItem_Click);
            // 
            // ExportWtGPSDataToolStripMenuItem
            // 
            this.ExportWtGPSDataToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ExportWtGPSDataToolStripMenuItem.Name = "ExportWtGPSDataToolStripMenuItem";
            this.ExportWtGPSDataToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.ExportWtGPSDataToolStripMenuItem.Text = "导出数据";
            this.ExportWtGPSDataToolStripMenuItem.Click += new System.EventHandler(this.ExportWtGPSDataToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataRateGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Size = new System.Drawing.Size(459, 475);
            this.splitContainer1.SplitterDistance = 445;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataRateGridView
            // 
            this.dataRateGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataRateGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataRateGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imageName,
            this.latitude,
            this.longitude,
            this.tRate});
            this.dataRateGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataRateGridView.Location = new System.Drawing.Point(0, 0);
            this.dataRateGridView.Name = "dataRateGridView";
            this.dataRateGridView.RowTemplate.Height = 23;
            this.dataRateGridView.Size = new System.Drawing.Size(459, 445);
            this.dataRateGridView.TabIndex = 0;
            // 
            // imageName
            // 
            this.imageName.HeaderText = "图像名称";
            this.imageName.Name = "imageName";
            // 
            // latitude
            // 
            this.latitude.HeaderText = "纬度";
            this.latitude.Name = "latitude";
            // 
            // longitude
            // 
            this.longitude.HeaderText = "经度";
            this.longitude.Name = "longitude";
            // 
            // tRate
            // 
            this.tRate.HeaderText = "杂草威胁率";
            this.tRate.Name = "tRate";
            // 
            // ListWThreatRateGPSData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 500);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ListWThreatRateGPSData";
            this.Text = "杂草和作物数据列表";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataRateGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ExtractDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportWtGPSDataToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataRateGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn latitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn longitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn tRate;
    }
}