namespace WeedCropsIDSSystem
{
    partial class ListWeedsCropsData
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
            this.ExtractDataOriginalPicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportOriginalDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.imageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ciw = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weedDensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cropDensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soilDensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExtractDataOriginalPicToolStripMenuItem,
            this.ExportOriginalDataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1045, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ExtractDataOriginalPicToolStripMenuItem
            // 
            this.ExtractDataOriginalPicToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ExtractDataOriginalPicToolStripMenuItem.Name = "ExtractDataOriginalPicToolStripMenuItem";
            this.ExtractDataOriginalPicToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.ExtractDataOriginalPicToolStripMenuItem.Text = "数据列表";
            this.ExtractDataOriginalPicToolStripMenuItem.Click += new System.EventHandler(this.ExtractDataOriginalPicToolStripMenuItem_Click);
            // 
            // ExportOriginalDataToolStripMenuItem
            // 
            this.ExportOriginalDataToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ExportOriginalDataToolStripMenuItem.Name = "ExportOriginalDataToolStripMenuItem";
            this.ExportOriginalDataToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.ExportOriginalDataToolStripMenuItem.Text = "导出原始数据";
            this.ExportOriginalDataToolStripMenuItem.Click += new System.EventHandler(this.ExportOriginalDataToolStripMenuItem_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1045, 475);
            this.splitContainer1.SplitterDistance = 446;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imageName,
            this.weedCount,
            this.ciw,
            this.cic,
            this.cis,
            this.aic,
            this.weedDensity,
            this.cropDensity,
            this.soilDensity,
            this.tRate});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1045, 446);
            this.dataGridView1.TabIndex = 0;
            // 
            // imageName
            // 
            this.imageName.HeaderText = "图像名称";
            this.imageName.Name = "imageName";
            // 
            // weedCount
            // 
            this.weedCount.HeaderText = "杂草株数";
            this.weedCount.Name = "weedCount";
            // 
            // ciw
            // 
            this.ciw.HeaderText = "杂草覆盖面积";
            this.ciw.Name = "ciw";
            // 
            // cic
            // 
            this.cic.HeaderText = "作物覆盖面积";
            this.cic.Name = "cic";
            // 
            // cis
            // 
            this.cis.HeaderText = "土壤覆盖面积";
            this.cis.Name = "cis";
            // 
            // aic
            // 
            this.aic.HeaderText = "单元格的面积";
            this.aic.Name = "aic";
            // 
            // weedDensity
            // 
            this.weedDensity.HeaderText = "杂草密度";
            this.weedDensity.Name = "weedDensity";
            // 
            // cropDensity
            // 
            this.cropDensity.HeaderText = "作物密度";
            this.cropDensity.Name = "cropDensity";
            // 
            // soilDensity
            // 
            this.soilDensity.HeaderText = "土壤比重";
            this.soilDensity.Name = "soilDensity";
            // 
            // tRate
            // 
            this.tRate.HeaderText = "杂草威胁率";
            this.tRate.Name = "tRate";
            // 
            // ListWeedsCropsData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 500);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ListWeedsCropsData";
            this.Text = "杂草和作物数据列表";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ExtractDataOriginalPicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportOriginalDataToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn weedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ciw;
        private System.Windows.Forms.DataGridViewTextBoxColumn cic;
        private System.Windows.Forms.DataGridViewTextBoxColumn cis;
        private System.Windows.Forms.DataGridViewTextBoxColumn aic;
        private System.Windows.Forms.DataGridViewTextBoxColumn weedDensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn cropDensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn soilDensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn tRate;
    }
}