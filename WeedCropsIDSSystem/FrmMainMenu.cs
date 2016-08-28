using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

//PS:调用的Emgu dll  
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using Emgu.CV.OpenCL;


namespace WeedCropsIDSSystem
{
    public partial class FrmMainMenu : Form
    {
        private string path = string.Empty; //加载图片/文件夹的路径
        List<string> listPicsFiles = new List<string>();  //存放加载进系统的所有图片的路径

        public static string suoxiao = "0";//图像缩放

        public static string imageName = string.Empty;
        public static float cropsCount = 0;  //作物像素数
        public static float weedCount = 0;  //杂草像素数
        public static int weedNumber = 0;  //杂草株数
        public static float weedDensity;  //杂草密度
        public static float cropDensity;  //作物密度
        public static float cisDensity;   //土壤密度

        //---------杂草威胁度评估计算指标------------
        public static float ciw;         //单元格内的杂草覆盖面积--weedCount   这里单元格指图像大小
        public static float aic;         //单元格的面积
        public static float cic;         //单元格内的作物覆盖面积--cropsCount
        public static float cis = 0;     //单元格内的土壤覆盖面积
        public static float tRate = 0;   //威胁率rate
        public static float averageTRate = 0;  //总体威胁率

        public static string latitude = string.Empty;   //纬度
        public static string longitude = string.Empty;   //经度


        private DBConnect dbConnect;  //链接数据库

        public FrmMainMenu()
        {
            InitializeComponent();
            //链接数据库
            dbConnect = new DBConnect();
        }

        //加载一张图像
        private void SingleOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfiledialog1 = new OpenFileDialog();  //打开文件对话框(OpenFileDialog)
            openfiledialog1.Filter = "图像文件(*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png|所有文件(*.*)|*.*";  //要在对话框中显示的文件筛选器
            openfiledialog1.RestoreDirectory = false;
            openfiledialog1.AddExtension = true;

            TreeNode node = null;

            if (openfiledialog1.ShowDialog() == DialogResult.OK)
            {
                if (openfiledialog1.FileName.Substring(openfiledialog1.FileName.LastIndexOf('.')).ToLower() == ".bmp" || openfiledialog1.FileName.Substring(openfiledialog1.FileName.LastIndexOf('.')).ToLower() == ".jpg" || openfiledialog1.FileName.Substring(openfiledialog1.FileName.LastIndexOf('.')).ToLower() == ".jpeg" || openfiledialog1.FileName.Substring(openfiledialog1.FileName.LastIndexOf('.')).ToLower() == ".gif" || openfiledialog1.FileName.Substring(openfiledialog1.FileName.LastIndexOf('.')).ToLower() == ".png")
                {
                    Bitmap image = new Bitmap(openfiledialog1.FileName);
                    path = openfiledialog1.FileName;
                    node = new TreeNode();
                    node.Text = System.IO.Path.GetFileNameWithoutExtension(path);
                    node.Name = path;
                    imageName = node.Text;
                    node.Tag = path;
                    treeView1.Nodes.Add(node);

                    this.pictureBox1.Image = Image.FromFile(node.Tag.ToString(), true);
                    this.pictureBox2.Image = Image.FromFile(node.Tag.ToString(), true);
                    this.pictureBox3.Image = Image.FromFile(node.Tag.ToString(), true);
                    this.pictureBoxReturn.Image = Image.FromFile(node.Tag.ToString(), true);
                    pictureBox2.Visible = false;
                    pictureBox3.Visible = false;
                }
                listPicsFiles.Add(path);
            }
        }

        //加载一张图像
        private void toolStripButton_SingleOpenPic_Click(object sender, EventArgs e)
        {
            SingleOpenToolStripMenuItem_Click(null, null);
        }

        //同时加载多张图像
        private void BatchAddPicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdlg = new FolderBrowserDialog();
            fbdlg.ShowNewFolderButton = false;
            fbdlg.Description = "选择图片文件夹";
            if (fbdlg.ShowDialog() == DialogResult.OK)
            {
                path = fbdlg.SelectedPath;
                AppendPicFiles(path);
            }
        }
        //追加多张图片
        private void AppendPicFiles(string imagePath)
        {
            TreeNode batchNode = null;
            string[] imageFiles = System.IO.Directory.GetFiles(imagePath);
            if (imageFiles.Length > 0)
            {
                foreach (string imagefile in imageFiles)
                {
                    batchNode = new TreeNode();
                    batchNode.Text = System.IO.Path.GetFileNameWithoutExtension(imagefile);
                    batchNode.Name = imagefile;
                    batchNode.Tag = imagefile;
                    treeView1.Nodes.Add(batchNode);
                    listPicsFiles.Add(imagefile);
                }
            }

            string[] dirctorys = System.IO.Directory.GetDirectories(imagePath);
            if (dirctorys.Length > 0)
            {
                //TreeNode childNode = null;
                foreach (string dirctory in dirctorys)
                {
                    batchNode = new TreeNode();
                    batchNode.Name = dirctory;
                    batchNode.Text = System.IO.Path.GetFileName(dirctory);
                    batchNode.Tag = dirctory;
                    treeView1.Nodes.Add(batchNode);

                    AppendPicFiles(dirctory);
                }
            }
        }
        //添加节点的单击事件
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                if (e.Node.Tag.ToString().Contains("."))
                {
                    this.pictureBox1.Image = Image.FromFile(e.Node.Tag.ToString(), true);
                    this.pictureBox2.Image = Image.FromFile(e.Node.Tag.ToString(), true);
                    this.pictureBox3.Image = Image.FromFile(e.Node.Tag.ToString(), true);
                    this.pictureBoxReturn.Image = Image.FromFile(e.Node.Tag.ToString(), true);
                    imageName = e.Node.Text;
                    pictureBox2.Visible = false;
                    pictureBox3.Visible = false;
                    pictureBoxReturn.Visible = false;


                    this.histogramBox1.Visible = false;
                    this.histogramBox2.Visible = false;

                    this.pictureBox1.Refresh();
                    this.pictureBox2.Refresh();
                    this.pictureBoxReturn.Refresh();
                }
            }
        }
        //退出系统
        private void ExitSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void FrmMainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确认要退出系统吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        //普通模式
        private void OrdinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
            // pictureBox3.Visible = false;
        }
        //对比模式
        private void ComparisonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // pictureBox1.Visible = false;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            //  pictureBox3.Visible = true;
        }
        //图像还原
        private void ReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBoxReturn.Image != null)
            {
                pictureBox1.Image = pictureBoxReturn.Image;
                pictureBox2.Image = pictureBoxReturn.Image;
                pictureBox3.Image = pictureBoxReturn.Image;

                this.histogramBox1.Visible = false;
                this.histogramBox2.Visible = false;
            }
            else
            {
                MessageBox.Show("错误，还原图像失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //彩色直方图均衡化
        private void ColorHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                //histogramBox1 = new HistogramBox();
                //histogramBox2 = new HistogramBox();

              
                this.histogramBox1.Visible = true;
                this.histogramBox2.Visible = true;

                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                pictureBox1.Image = pictureBox3.Image;

                Image<Bgr, Byte> imageSrc = new Image<Bgr, Byte>(new Bitmap(pictureBox3.Image));
                //计算直方图
                int rBins = 256;
                RangeF rRange = new RangeF(0f, 255f);

                Image<Gray, Byte> imageBlue = imageSrc.Split()[0];
                Image<Gray, Byte> imageGreen = imageSrc.Split()[1];
                Image<Gray, Byte> imageRed = imageSrc.Split()[2];

                DenseHistogram hist = new DenseHistogram(rBins, rRange);
                hist.Calculate(new IImage[] { imageBlue }, false, null);

                //显示图像的直方图
                histogramBox1.AddHistogram("蓝色色调直方图", Color.FromArgb(0, 0, 255), hist);
                histogramBox1.Refresh();

                hist.Calculate(new IImage[] { imageGreen }, false, null);
                histogramBox1.AddHistogram("绿色色调直方图", Color.FromArgb(0, 255, 0), hist);
                histogramBox1.Refresh();

                hist.Calculate(new IImage[] { imageRed }, false, null);
                histogramBox1.AddHistogram("红色色调直方图", Color.FromArgb(255, 0, 0), hist);
                histogramBox1.Refresh();


                Image<Bgr, Byte> imageDst = imageSrc.Clone();
                //均衡化后的图像
                imageDst._EqualizeHist();
                this.pictureBox2.Image = imageDst.ToBitmap();

                //计算直方图
                int rBinsDst = 256;
                RangeF rRangeDst = new RangeF(0f, 255f);

                Image<Gray, Byte> imageDstBlue = imageDst.Split()[0];
                Image<Gray, Byte> imageDstGreen = imageDst.Split()[1];
                Image<Gray, Byte> imageDstRed = imageDst.Split()[2];

                DenseHistogram histDst = new DenseHistogram(rBinsDst, rRangeDst);
                histDst.Calculate(new IImage[] { imageDstBlue }, false, null);

                //显示处理后的图像的直方图
                histogramBox2.AddHistogram("蓝色色调直方图", Color.FromArgb(0, 0, 255), histDst);
                histogramBox2.Refresh();

                hist.Calculate(new IImage[] { imageDstGreen }, false, null);
                histogramBox2.AddHistogram("绿色色调直方图", Color.FromArgb(0, 255, 0), histDst);
                histogramBox2.Refresh();

                hist.Calculate(new IImage[] { imageDstRed }, false, null);
                histogramBox2.AddHistogram("红色色调直方图", Color.FromArgb(255, 0, 0), histDst);
                histogramBox2.Refresh();


                //释放资源
                imageSrc.Dispose();
                imageBlue.Dispose();
                imageGreen.Dispose();
                imageRed.Dispose();
                hist.Dispose();

                imageDst.Dispose();
                imageDstBlue.Dispose();
                imageDstGreen.Dispose();
                imageDstRed.Dispose();
                histDst.Dispose();
            }
        }
        //灰度直方图均衡化
        private void GrayHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                //histogramBox1 = new HistogramBox();
                //histogramBox2 = new HistogramBox();

                this.histogramBox1.Visible = true;
                this.histogramBox2.Visible = true;

                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                pictureBox1.Image = pictureBox3.Image;

                Image<Bgr, Byte> imageSrc = new Image<Bgr, Byte>(new Bitmap(pictureBox3.Image));
                //灰度图显示
                Image<Gray, byte> grayImage = imageSrc.Convert<Gray, byte>();
                pictureBox1.Image = grayImage.ToBitmap();

                //计算直方图
                int rBins = 256;
                RangeF rRange = new RangeF(0f, 255f);

                //Image<Gray, byte> imageBlue = imageSrc.Split()[0];
                // Image<Gray, byte> imageGreen = imageSrc.Split()[1];
                // Image<Gray, byte> imageRed = imageSrc.Split()[2];

                DenseHistogram hist = new DenseHistogram(rBins, rRange);
                hist.Calculate(new IImage[] { grayImage }, false, null);
                // hist.Normalize(100);

                //显示处理后的图像的直方图
                histogramBox1.AddHistogram("色调直方图", Color.FromArgb(0, 255, 255), hist);
                histogramBox1.Refresh();


                //均衡化
                Image<Gray, Byte> imageDst = grayImage.Clone();
                //均衡化后的图像
                imageDst._EqualizeHist();
                pictureBox2.Image = imageDst.ToBitmap();

                //计算直方图
                int rBinsDst = 256;
                RangeF rRangeDst = new RangeF(0f, 255f);
                //Image<Gray, byte> imageDstBlue = imageDst.Split()[0];
                //Image<Gray, byte> imageDstGreen = imageDst.Split()[1];
                //Image<Gray, byte> imageDstRed = imageDst.Split()[2];


                DenseHistogram histDst = new DenseHistogram(rBinsDst, rRangeDst);
                histDst.Calculate(new IImage[] { imageDst }, false, null);
                // histDst.Normalize(100);

                //显示处理后的图像的直方图
                histogramBox2.AddHistogram("色调直方图", Color.FromArgb(170, 164, 164), histDst);  //131 139 139
                histogramBox2.Refresh();


                //释放资源
                imageSrc.Dispose();
                grayImage.Dispose();
                //imageRed.Dispose();
                hist.Dispose();

                imageDst.Dispose();
                // imageDstBlue.Dispose();
                histDst.Dispose();
            }
        }

        //生成自己的直方图图示
        private Image<Bgr, Byte> GenerateHistImage(DenseHistogram hist)
        {
            Image<Bgr, Byte> imageHist = null;
            float minValue, maxValue;
            int[] minLocations, maxLocations;
            hist.MinMax(out minValue, out maxValue, out minLocations, out maxLocations);
            if (hist.Dimension == 1)
            {
                int bins = hist.BinDimension[0].Size;
                int width = bins;
                int height = 300;
                imageHist = new Image<Bgr, Byte>(width, height, new Bgr(255d, 255d, 255d));
                double heightPerTick = 1d * height / maxValue;
                Bgr color = new Bgr(0d, 0d, 255d);
                //遍历每个bin对应的值，并画一条线
                for (int i = 0; i < bins; i++)
                {
                    LineSegment2D line = new LineSegment2D(new Point(i, height), new Point(i, (int)(height - heightPerTick * hist[i])));
                    imageHist.Draw(line, color, 1);
                }
            }
            else if (hist.Dimension == 2)
            {
                int scale = 2;
                int width = hist.BinDimension[0].Size * scale;
                int height = hist.BinDimension[1].Size * scale;
                imageHist = new Image<Bgr, Byte>(width, height, new Bgr(255d, 255d, 255d));
                //遍历每个bin对应的值，并画一个矩形
                for (int i = 0; i < width / scale; i++)
                {
                    for (int j = 0; j < height / scale; j++)
                    {
                        double binValue = hist[i, j];
                        double intensity = 1d * binValue * 255 / maxValue;
                        Rectangle rect = new Rectangle(i * scale, j * scale, 1, 1);
                        Bgr color = new Bgr(intensity, intensity, intensity);
                        imageHist.Draw(rect, color, 1);
                    }
                }
            }
            return imageHist;
        }

        //图像二值化(菜单栏上的图像二值化，调用子菜单栏里方法)
        private void PicBinarizationBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                toolStripButton_PicBinarization_Click(null, null);
            }
        }

        //图像腐蚀
        private void ErodeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;
                Image<Bgr, Byte> srcImg = new Image<Bgr, Byte>(new Bitmap(pictureBox3.Image));

                pictureBox2.Image = srcImg.Erode(1).ToBitmap();
                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();

                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);
                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);

                // HistogramViewer.Show(endImg, 32);　//显示所有信道


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 32);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
            }
        }

        //膨胀图像
        private void DilationImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;
                Image<Bgr, Byte> srcImg = new Image<Bgr, Byte>(new Bitmap(pictureBox3.Image));

                pictureBox2.Image = srcImg.Dilate(1).ToBitmap();
                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();


                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);

                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);

                // HistogramViewer.Show(endImg, 32);　//显示所有信道


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 32);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
            }
        }

        //开运算
        private void MorphologicalOpeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;
                Bitmap tempBmp = new Bitmap(pictureBox3.Image.Width, pictureBox3.Image.Height);

                Image<Bgr, Byte> srcImg = new Image<Bgr, Byte>(new Bitmap(pictureBox3.Image));

                tempBmp = srcImg.Erode(1).ToBitmap();
                Image<Bgr, Byte> dstImg = new Image<Bgr, Byte>(tempBmp);

                pictureBox2.Image = dstImg.Dilate(1).ToBitmap();
                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();


                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);

                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);

                // HistogramViewer.Show(endImg, 32);　//显示所有信道


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 32);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
            }
        }
        //闭运算
        private void ClosedOperationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;
                Bitmap tempBmp = new Bitmap(pictureBox3.Image.Width, pictureBox3.Image.Height);

                Image<Bgr, Byte> srcImg = new Image<Bgr, Byte>(new Bitmap(pictureBox3.Image));

                tempBmp = srcImg.Dilate(1).ToBitmap();
                Image<Bgr, Byte> dstImg = new Image<Bgr, Byte>(tempBmp);

                pictureBox2.Image = dstImg.Erode(1).ToBitmap();
                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();


                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);

                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);

                // HistogramViewer.Show(endImg, 32);　//显示所有信道


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 32);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
            }
        }

        //图像灰度化--RGB-- r=-1,g=2,b=-1
        private void toolStripButton_2GRBGray_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;

                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                BitmapData bmData = getPotImg.LockBits(new Rectangle(0, 0, getPotImg.Width, getPotImg.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int stride = bmData.Stride; //步幅，扫描宽度
                System.IntPtr Scan0 = bmData.Scan0;
                //System.Runtime.InteropServices.Marshal.Copy(Scan0,Rgb,0,length);
                unsafe
                {
                    byte* p = (byte*)(void*)Scan0;
                    int nOffset = stride - getPotImg.Width * 3;
                    byte red, green, blue;


                    for (int j = 0; j < getPotImg.Height; j++)
                    {
                        for (int i = 0; i < getPotImg.Width; i++)
                        {

                            ///* 灰度化
                            blue = p[0];
                            green = p[1];
                            red = p[2];
                            //注意：在GDI+中的图像存储的格式是BGR而非RGB，即其顺序为Blue、Green、Red.以下提供三组参数试验。
                            // p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue); //r=0.299,g=0.587,b=0.114             
                            // p[0] = p[1] = p[2] = (byte)(-0.7 * red + 0.588 * green + 0.136 * blue); // r=-0.7,g=0.588,b=0.136
                            p[0] = p[1] = p[2] = (byte)(-1 * red + 2 * green + -1 * blue); // r=-1,g=2,b=-1

                            p = p + 3;
                        }
                        p = p + nOffset;
                    }
                }
                getPotImg.UnlockBits(bmData);
                pictureBox2.Image = getPotImg;
                pictureBox2.Visible = true;
                pictureBox2.Refresh();
                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();



                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
               // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);
               
                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);
 
               // HistogramViewer.Show(endImg, 32);　//显示所有信道


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 32);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
                
            }
        }
        //图像二值化
        private void toolStripButton_PicBinarization_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;
                Bitmap getPotImg = new Bitmap(pictureBox3.Image);


                //------------灰度反转（把每个像素点的R、G、B三个分量的值0的设为255，255的设为0）------------------
                Color color, newColor;
                for (int i = 0; i < getPotImg.Width; i++)
                {
                    for (int j = 0; j < getPotImg.Height; j++)
                    {
                        color = getPotImg.GetPixel(i, j);
                        newColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                        getPotImg.SetPixel(i, j, newColor);
                    }
                }
                //---------------------------------------------------------------------------------------------------

                //------------------图像二值化------------------
                //BitmapData bmData = getPotImg.LockBits(new Rectangle(0, 0, getPotImg.Width, getPotImg.Height), ImageLockMode.ReadWrite, getPotImg.PixelFormat);
                BitmapData bmData = getPotImg.LockBits(new Rectangle(0, 0, getPotImg.Width, getPotImg.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int stride = bmData.Stride; //步幅，扫描宽度
                System.IntPtr Scan0 = bmData.Scan0;

                //int threshold = 195;  //阈值1 = 195
                int threshold = 132;  //阈值2 = 227

                unsafe
                {
                    byte* p = (byte*)(void*)Scan0;
                    int nOffset = stride - getPotImg.Width * 3;

                    for (int j = 0; j < getPotImg.Height; j++)
                    {
                        for (int i = 0; i < getPotImg.Width; i++)
                        {
                            if (p[1] > threshold)
                            {
                                p[0] = p[1] = p[2] = byte.MaxValue;  //255
                            }
                            if (p[1] <= threshold)
                            {
                                p[0] = p[1] = p[2] = byte.MinValue;  //0
                            }
                            p = p + 3;
                        }
                        p = p + nOffset;
                    }
                }

                if (cis != 0)
                {
                    cis = 0;
                }

                getPotImg.UnlockBits(bmData);
                Bitmap soilImage = new Bitmap(getPotImg);

                //计算土壤面积
                for (int j = 0; j < soilImage.Height; j++)
                {
                    for (int i = 0; i < soilImage.Width; i++)
                    {
                        if (soilImage.GetPixel(i, j).R == 0 && soilImage.GetPixel(i, j).G == 0 && soilImage.GetPixel(i, j).B == 0)
                        {
                            cis++;    //土壤像素
                        }
                    }
                }

                pictureBox2.Image = getPotImg;
                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();

                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);

                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);

               //HistogramViewer.Show(endImg, 32);　//显示所有信道

                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 32);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
            }
        }
        //作物行标记
        private void toolStripButton_CropLineMarker_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;

                //寻找作物行的中心线
                bezierCenterLineToolStripMenuItem_Click(null, null);

                pictureBox1.Image = pictureBox2.Image;
                Bitmap getPotImg = new Bitmap(pictureBox1.Image);
                int length = 46;  //滤除作物行的宽度
                int m, k;
                Bitmap newBmp = new Bitmap(pictureBox1.Image);

                if (cropsCount != 0)
                {
                    cropsCount = 0;
                }

                for (int j = 0; j < getPotImg.Height; j++)  //int i = 0; i < getPot.Width; i++
                {
                    for (int i = 0; i < getPotImg.Width; i++)   //int j = 0; j < getPot.Height; j++
                    {
                        m = 0;
                        k = 0;
                        if (getPotImg.GetPixel(i, j).R == 255 && getPotImg.GetPixel(i, j).G == 0 && getPotImg.GetPixel(i, j).B == 0)
                        {
                            if (i - length > 0 && i + length < getPotImg.Width - 1)  //j - length > 0 && j + length < getPot.Height - 1
                            {
                                for (m = 0, k = 0; m < length && k < length; m++, k++)
                                {
                                    newBmp.SetPixel(i - m + 1, j, Color.Yellow);
                                    newBmp.SetPixel(i + k, j, Color.Yellow);
                                    cropsCount++;
                                }
                            }
                        }
                    }
                }

                cic = cropsCount;  //作物覆盖面积

                pictureBox2.Image = newBmp;
                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();

                this.histogramBox1.Visible = false;
                this.histogramBox2.Visible = false;
            }
        }

        //寻找作物行中心线
        private void bezierCenterLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                if (pictureBox1.Visible == true)
                {
                    Bitmap pic = new Bitmap(pictureBox1.Image);
                    Bitmap picoriginal = new Bitmap(pictureBox1.Image);
                    Graphics paint = Graphics.FromImage(pictureBox2.Image);
                    paint.DrawImage(pic, 0, 0, pic.Width, pic.Height);
                    paint.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    Pen pen = new Pen(Color.Red, 3);
                    //Thread thread = new Thread(new ThreadStart(process));
                    //thread.Start();
                    this.Refresh();
                    if (pictureBox2.Image != null)
                    {
                        pictureBox3.Image = pictureBox2.Image;
                        int i = 0, j = 0, edgeleft = 0, flagright = 0, flagleft = 0, countxx = 0, countx = 0, county = 0;
                        int coordleft = 0, coordright = 0, middle = 0;
                        int nextx = 0, originalx = 0;
                        Color point = new Color();
                        Color point1 = new Color();
                        Color point2 = new Color();
                        Color point3 = new Color();
                        Color point4 = new Color();
                        Color point5 = new Color();
                        Color point6 = new Color();
                        Color point7 = new Color();
                        Color point8 = new Color();
                        Color point9 = new Color();
                        Color point10 = new Color();
                        Color point11 = new Color();
                        Color point12 = new Color();
                        Color point13 = new Color();
                        Color point14 = new Color();
                        Color point15 = new Color();
                        Color point16 = new Color();
                        Color point17 = new Color();
                        Color point18 = new Color();
                        for (i = 0; i < pictureBox2.Image.Height - 1; i++)
                        {
                            j = 0;
                            while (j < pictureBox2.Image.Width - 19)
                            {
                                if (flagright == 1 && flagleft == 1)
                                {
                                    j = edgeleft;
                                    flagright = 0;
                                    flagleft = 0;
                                }
                                point = pic.GetPixel(j, i);
                                point1 = pic.GetPixel(j + 1, i);
                                point2 = pic.GetPixel(j + 2, i);
                                point3 = pic.GetPixel(j + 3, i);
                                point4 = pic.GetPixel(j + 4, i);
                                point5 = pic.GetPixel(j + 5, i);
                                point6 = pic.GetPixel(j + 6, i);
                                point7 = pic.GetPixel(j + 7, i);
                                point8 = pic.GetPixel(j + 8, i);
                                point9 = pic.GetPixel(j + 9, i);
                                point10 = pic.GetPixel(j + 10, i);
                                point11 = pic.GetPixel(j + 11, i);
                                point12 = pic.GetPixel(j + 12, i);
                                point13 = pic.GetPixel(j + 13, i);
                                point14 = pic.GetPixel(j + 14, i);
                                point15 = pic.GetPixel(j + 15, i);
                                point16 = pic.GetPixel(j + 16, i);
                                point17 = pic.GetPixel(j + 17, i);
                                point18 = pic.GetPixel(j + 18, i);
                                if (point.R + point1.R + point2.R + point3.R + point4.R + point5.R + point6.R + point7.R + point8.R + point9.R + point10.R + point11.R + point12.R + point13.R + point14.R + point15.R + point16.R + point17.R + point18.R >= 3570 && flagleft == 0)//the edge of left
                                {
                                    pic.SetPixel(j, i, Color.AliceBlue);
                                    flagleft = 1;
                                }

                                if (point.R + point1.R + point2.R + point3.R + point4.R + point5.R + point6.R + point7.R + point8.R + point9.R + point10.R + point11.R + point12.R + point13.R + point14.R + point15.R + point16.R + point17.R + point18.R <= 1275 && flagleft == 1 && flagright == 0)//the edge of right
                                {
                                    pic.SetPixel(j, i, Color.AntiqueWhite);
                                    flagright = 1;
                                    edgeleft = j + 1;
                                }
                                j++;
                            }
                        }
                        for (i = 1; i < pictureBox2.Image.Height - 1; i++)
                        {
                            j = 0;
                            while (j < pictureBox2.Image.Width)
                            {
                                if (flagright == 1 && flagleft == 1)
                                {
                                    j = edgeleft;
                                    flagright = 0;
                                    flagleft = 0;
                                }
                                point = pic.GetPixel(j, i);
                                if (point.R == 240 && flagleft == 0)
                                {
                                    coordleft = j;
                                    flagleft = 1;
                                }
                                if (point.R == 250 && flagleft == 1 && flagright == 0)
                                {
                                    coordright = j;
                                    flagright = 1;
                                }
                                if (flagleft == 1 && flagright == 1)
                                {
                                    middle = (coordright + coordleft) / 2;
                                    pic.SetPixel(middle, i, Color.Red);
                                    flagright = 0;
                                    flagleft = 0;
                                    edgeleft = coordright + 1;
                                }
                                j++;
                            }
                        }
                        j = 0;
                        i = 0;
                        while (originalx < pictureBox2.Image.Width)
                        {
                            Point[] bezier = new Point[61];
                            point = pic.GetPixel(originalx, 10);
                            countx = originalx;
                            if (point.R == 255 && point.G == 0)//判断第十行第一个中点
                            {
                                bezier[i].X = originalx;
                                bezier[i].Y = 0;
                                // System.Diagnostics.Debug.Write(" x" + sum[i, 0] + "y" + sum[i, 1]);
                                i++;
                                bezier[i].X = originalx;
                                bezier[i].Y = 10;
                                i++;
                                county = 20;
                                while (county < pictureBox2.Image.Height)//判断下一行中点,限定高度
                                {
                                    if (countx - 20 >= 0 && countx + 20 < pictureBox2.Image.Width)//在指定的横行范围内进行探测（不超过边缘防止泄露）
                                    {
                                        for (countxx = countx - 20; countxx <= countx + 20; countxx++)//像素范围为十
                                        {
                                            point1 = pic.GetPixel(countxx, county);
                                            if (point1.R == 255 && point1.G == 0 && i <= 11)
                                            {
                                                bezier[i].X = countxx;
                                                bezier[i].Y = county;
                                                //System.Diagnostics.Debug.Write(" x" + sum[i, 0] + "y" + sum[i, 1]+"i"+i+"\n");
                                                i++;
                                            }
                                        }
                                    }
                                    county = county + 20;
                                }
                                nextx = 1;
                            }

                            if (nextx == 1)
                            {
                                if (i % 3 == 0)
                                {
                                    bezier[i].X = bezier[i - 1].X;
                                    bezier[i].Y = pictureBox2.Image.Height - 1;
                                    i++;
                                }
                                else
                                    if (i % 3 == 1)
                                    {
                                        bezier[i].X = bezier[i - 1].X;
                                        bezier[i].Y = pictureBox2.Image.Height - 8;
                                        i++;
                                        bezier[i].X = bezier[i - 1].X;
                                        bezier[i].Y = pictureBox2.Image.Height - 5;
                                        i++;
                                        bezier[i].X = bezier[i - 1].X;
                                        bezier[i].Y = pictureBox2.Image.Height - 1;
                                        i++;
                                    }
                                    else
                                        if (i % 3 == 2)
                                        {
                                            bezier[i].X = bezier[i - 1].X;
                                            bezier[i].Y = pictureBox2.Image.Height - 5;
                                            i++;
                                            bezier[i].X = bezier[i - 1].X;
                                            bezier[i].Y = pictureBox2.Image.Height - 1;
                                            i++;
                                        }
                                if (i < 61)
                                {
                                    for (j = i; j < 61; j++)
                                    {
                                        bezier[j].X = bezier[i - 1].X;
                                        bezier[j].Y = pictureBox2.Image.Height - 1;
                                    }
                                }
                                System.Diagnostics.Debug.Write("i" + i + "\n");
                                paint.DrawBeziers(pen, bezier);
                                i = 0;
                                nextx = 0;
                                bezier = null;
                            }
                            originalx++;
                        }
                        Bitmap picagain = new Bitmap(pictureBox2.Image);
                        pictureBox1.Image = picoriginal;
                        pictureBox2.Image = picagain;
                        //thread.Abort();
                    }
                }
            }
        }

        //滤除作物行
        private void toolStripButtonFilterCropRows_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;
                Bitmap getPotImg = new Bitmap(pictureBox3.Image);

                BitmapData bmData = getPotImg.LockBits(new Rectangle(0, 0, getPotImg.Width, getPotImg.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int stride = bmData.Stride; //步幅，扫描宽度
                System.IntPtr Scan0 = bmData.Scan0;

                unsafe
                {
                    byte* p = (byte*)(void*)Scan0;
                    int nOffset = stride - getPotImg.Width * 3;
                    byte red, green, blue;

                    for (int j = 0; j < getPotImg.Height; j++)
                    {
                        for (int i = 0; i < getPotImg.Width; i++)
                        {//注意：在GDI+中的图像存储的格式是BGR而非RGB，即其顺序为Blue、Green、Red.
                            blue = p[0];
                            green = p[1];
                            red = p[2];
                            if (red == 255 && green == 255 && blue == 0)  //滤除原来用黄色标记的作物行
                            {
                                p[0] = p[1] = p[2] = 0;
                            }
                            p = p + 3;
                        }
                        p = p + nOffset;
                    }
                }
                getPotImg.UnlockBits(bmData);

                //腐蚀操作
                Image<Bgr, Byte> img = new Image<Bgr, Byte>(getPotImg);
                getPotImg = img.Erode(2).ToBitmap();

                pictureBox2.Image = getPotImg;
                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();

                pictureBox1.Image = pictureBox3.Image;
                //膨胀操作
                Image<Bgr, Byte> imgtem = new Image<Bgr, Byte>(new Bitmap(pictureBox3.Image));
                pictureBox2.Image = imgtem.Dilate(1).ToBitmap();
                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();

            }
        }

        //单一图像的杂草和作物的株数和像素密度的提取统计
        private void toolStripButton_ExtractWCsData_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                if (weedCount != 0)
                {
                    weedCount = 0;
                }
                Bitmap tempBmp = new Bitmap(pictureBox3.Image);
                Image<Gray, Byte> imageSrc = new Image<Gray, Byte>(tempBmp);

                Contour<Point> contour = imageSrc.FindContours();

                for (int i = 0; i < tempBmp.Width; i++)
                {
                    for (int j = 0; j < tempBmp.Height; j++)
                    {
                        if (tempBmp.GetPixel(i, j).R == 255 && tempBmp.GetPixel(i, j).G == 255 && tempBmp.GetPixel(i, j).B == 255)
                        {
                            weedCount++;
                        }
                    }
                }

                ciw = weedCount;  //杂草覆盖面积

                //----------------------------------------------------------------------------
                Image<Gray, byte> dstImage = new Image<Gray, byte>(imageSrc.Size);//存储最终结果图像 
                // Image<Gray, Single> dstImage = new Image<Gray, Single>(imageSrc.Size);//存储最终结果图像 
                IntPtr Dyncontour = new IntPtr();//存放检测到的图像块的首地址  
                IntPtr Dynstorage = CvInvoke.cvCreateMemStorage(0);//开辟内存区域 

                /// int m = 88;//在不安全处理下获取的数据  
                //获得杂草株数
                ///  weedNumber = CvInvoke.cvFindContours(imageSrc, Dynstorage, ref Dyncontour, m, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, new Point(1, 1));

                MCvContour con = new MCvContour();
                //以Marshal.SizeOf(con)形式作为cvFindContours的第4个参数,，EMGU中没有CvContour这样的东东，只有MCvContour，但直接使用sizeof(MCvContour) 是不行的.
                //weedNumber = CvInvoke.cvFindContours(imageSrc, Dynstorage, ref Dyncontour, Marshal.SizeOf(con), Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, new Point(1, 1));
                weedNumber = CvInvoke.cvFindContours(imageSrc, Dynstorage, ref Dyncontour, Marshal.SizeOf(con), Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_LINK_RUNS, new Point(0, 0));

                Seq<Point> DyncontourTemp1 = new Seq<Point>(Dyncontour, null);//方便对IntPtr类型进行操作  
                Seq<Point> DyncontourTemp = DyncontourTemp1;

                for (; DyncontourTemp1 != null && DyncontourTemp1.Ptr.ToInt32() != 0; DyncontourTemp1 = DyncontourTemp1.HNext)
                {
                    CvInvoke.cvDrawContours(dstImage, DyncontourTemp1, new MCvScalar(255, 255, 255), new MCvScalar(255, 255, 255), 0, 1, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, new Point(0, 0));
                }

                pictureBox2.Image = dstImage.ToBitmap();  //显示出来
                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();


                weedDensity = weedCount / (dstImage.Height * dstImage.Width);
                cropDensity = cropsCount / (dstImage.Height * dstImage.Width);
                cisDensity = cis / (dstImage.Height * dstImage.Width);

                aic = dstImage.Height * dstImage.Width;

                //计算威胁率 tRate = (ciw/cic)(1-cis/aic)
                if(cis !=0 && aic !=0){
                    tRate = (ciw / cic) * (1 - cis / aic);
                }           

                if (pictureBox2.Image != null)
                {
                    DisplayWCDsData dwcd = new DisplayWCDsData();
                    this.Refresh();
                    dwcd.ShowDialog();
                }

            }
        }


        //显示杂草作物提取数据列表
        private void DisplayWeedsCropsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbConnect.Count() <= 0)
            {
                MessageBox.Show("数据库当前没有记录，请添加数据！");
                this.Refresh();
            }
            else
            {
                ListWeedsCropsData listWCsData = new ListWeedsCropsData();
                this.Refresh();
                listWCsData.ShowDialog();
            }
        }


        //显示图像GPS坐标和杂草威胁率列表
        private void WeedsTRateGPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbConnect.Count() <= 0)
            {
                MessageBox.Show("数据库当前没有记录，请添加数据！");
                this.Refresh();
            }
            else
            {
                ListWThreatRateGPSData listRateGPS = new ListWThreatRateGPSData();
                this.Refresh();
                listRateGPS.ShowDialog();
            }
        }



        //批量提取图像数据从TreeView的列表中
        private void BatchExtractImageDataFromTreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count <= 0)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                ProcessOperator process = new ProcessOperator();
                //处理时间调用
                process.MessageInfo = "图像正在处理中，请稍等片刻...";
                process.BackgroundWork = this.Do;
                process.BackgroundWorkerCompleted += new EventHandler<BackgroundWorkerEventArgs>(process_BackgroundWorkerCompleted);
                process.Start();
            }
        }

      //----- //显示进度条---进度条测试--------------------------------------------------------------------------------------
        private void process_BackgroundWorkerCompleted(object sender, BackgroundWorkerEventArgs e)
        {
            if (e.BackGroundException == null)
            {
                MessageBox.Show("提取图像杂草作物数据完成");
            }
            else
            {
                MessageBox.Show("异常:" + e.BackGroundException.Message);
            }
        }
        private void Do()
        {
            //-------处理方法----------------------------------------------
            BatchExtractPicDataFromTreeView bepdft = new BatchExtractPicDataFromTreeView();
            if (treeView1.Nodes.Count > 0)
            {
                //遍历TreeView列表
                foreach (TreeNode listNode in treeView1.Nodes)
                {
                    BatchExtractPicDataFromTreeView.pictureBox1Image = Image.FromFile(listNode.Name.ToString(), true);
                    BatchExtractPicDataFromTreeView.pictureBox2Image = Image.FromFile(listNode.Name.ToString(), true);
                    BatchExtractPicDataFromTreeView.pictureBox3Image = Image.FromFile(listNode.Name.ToString(), true);

                    imageName = listNode.Text;

                    //灰度化
                    // toolStripButton_2GRBGray_Click(null, null);
                    bepdft.Button_2GRBGray_Click();
                    //二值化
                    //toolStripButton_PicBinarization_Click(null, null);
                    bepdft.Button_PicBinarization_Click();
                    //作物行标记
                    //toolStripButton_CropLineMarker_Click(null, null);
                    bepdft.CropLineMarker_Click();
                    //滤除作物行
                    //toolStripButtonFilterCropRows_Click(null, null);
                    bepdft.FilterCropRows_Click();
                    //杂草和作物的株数和像素密度的提取统计
                    //toolStripButton_ExtractWCsData_Click(null, null);
                    bepdft.ExtractWCsData_Click();
                }
               // MessageBox.Show("提取图像杂草作物数据完成。");
                //thread.Abort();
            }
            else
            {
                MessageBox.Show("图像列表为空，请加载农田图像。");
            }

            //-----------------------------------------------------
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(10);
            }
        }

        //杂草空间分布图
        private void WeedDistributionMapPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WeedSpatialDistributeMap wsdm = new WeedSpatialDistributeMap();
            this.Refresh();
            wsdm.ShowDialog();
        }

        //杂草威胁度评估
        private void toolStripButton_WeedThreatAssessment_Click(object sender, EventArgs e)
        {
            WeedThreatAssessment wta = new WeedThreatAssessment();
            this.Refresh();
            wta.ShowDialog();
        }

        //杂草威胁度评估(主菜单)
        private void WeedThreatAssessmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_WeedThreatAssessment_Click(null,null);
        }

        //退出系统
        private void toolStripButton_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //---------------------菜单栏上功能-----------------------------------------------
        //灰度化
        private void 平均值法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;
                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                Image<Bgr, byte> img = new Image<Bgr, byte>(getPotImg);

                //原图显示
                IntPtr srcimg = img.Ptr;
                /*      IplImage* 转换为Bitmap      */
                MIplImage srcmi = (MIplImage)Marshal.PtrToStructure(srcimg, typeof(MIplImage));
                Image<Bgr, Byte> srcimage = new Image<Bgr, Byte>(srcmi.width, srcmi.height, srcmi.widthStep, srcmi.imageData);
                pictureBox1.Image = srcimage.ToBitmap();

                //灰度图显示
                IntPtr grayimg = CvInvoke.cvCreateImage(CvInvoke.cvGetSize(srcimg), Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8U, 1);
                CvInvoke.cvCvtColor(srcimg, grayimg, Emgu.CV.CvEnum.COLOR_CONVERSION.RGB2GRAY);
                MIplImage graymi = (MIplImage)Marshal.PtrToStructure(grayimg, typeof(MIplImage));
                //和彩色图像显示采用函数不一致，如果继续使用上面函数，会报内存错误
                Image<Gray, Byte> grayimage = new Image<Gray, Byte>(graymi.width, graymi.height, graymi.widthStep, graymi.imageData);
                pictureBox2.Image = grayimage.ToBitmap();

                CvInvoke.cvWaitKey(0);
                CvInvoke.cvReleaseImage(ref srcimg);
                CvInvoke.cvReleaseImage(ref grayimg);

                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();

                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);

                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);

                // HistogramViewer.Show(endImg, 32);　//显示所有信道


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 16);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();

            }
        }

        private void 过绿特征灰度化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                this.Refresh();//过绿特征灰度化

                    Color currentColor;
                    int r;
                    Bitmap currentBitmap = new Bitmap(pictureBox2.Image);
                    for (int w = 0; w < currentBitmap.Width; w++)
                    {
                        for (int h = 0; h < currentBitmap.Height; h++)
                        {
                            currentColor = currentBitmap.GetPixel(w, h);
                            r = 2 * currentColor.G - currentColor.R - currentColor.B;
                            if (r < 0) r = 0;
                            if (r > 255) r = 255;
                            currentBitmap.SetPixel(w, h, Color.FromArgb(r, r, r));
                        }
                        this.Refresh();
                        pictureBox2.Image = currentBitmap;
                    }

                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();

                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);

                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 16);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
             
            }
        }
     

        private void 简单平滑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;

                Bitmap scrImage = new Bitmap(pictureBox3.Image);
                Image<Bgr, byte> image = new Image<Bgr, byte>(scrImage);
                pictureBox2.Image = image.SmoothMedian(5).ToBitmap(); //使用5*5的卷积核

                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();


                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);

                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);

                // HistogramViewer.Show(endImg, 32);　//显示所有信道


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 32);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
            }
        }

        private void 高斯平滑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;

                Bitmap scrImage = new Bitmap(pictureBox3.Image);
                Image<Bgr, byte> image = new Image<Bgr, byte>(scrImage);
                pictureBox2.Image = image.SmoothGaussian(5).ToBitmap(); //SmoothGaussian方法进行高斯模糊

                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();


                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);

                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);

                // HistogramViewer.Show(endImg, 32);　//显示所有信道


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 32);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
            }
        }

        private void 中值滤波法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1.Image = pictureBox3.Image;

                Bitmap scrImage = new Bitmap(pictureBox3.Image);
                Image<Bgr, byte> image = new Image<Bgr, byte>(scrImage);
                //中值滤波是指以一个像素点为中心点，在这个中心点的正方形邻域内求所有像素的中间值，
                //然后用这个中间值替换邻域内所有点的像素值。中值滤波器可以有效的去除图像中的椒盐噪声和斑点噪声。
                pictureBox2.Image = image.SmoothMedian(5).ToBitmap(); //使用5*5的卷积核

                pictureBox2.Visible = true;
                pictureBox2.Refresh();

                pictureBox3.Image = pictureBox2.Image;
                pictureBox3.Refresh();


                //计算直方图

                //GrayHistogramToolStripMenuItem_Click(null, null);
                // Image<bgr, byte> startImg = new Image<bgr, byte>(pictureBox1.Image);

                Bitmap getPotImg = new Bitmap(pictureBox3.Image);
                Image<Bgr, Byte> startImg = new Image<Bgr, Byte>(new Bitmap(pictureBox1.Image));
                Image<Bgr, Byte> endImg = new Image<Bgr, Byte>(getPotImg);

                // HistogramViewer.Show(endImg, 32);　//显示所有信道


                //清空histogramBox1和histogramBox2
                histogramBox1.ClearHistogram();
                histogramBox2.ClearHistogram();

                histogramBox1.GenerateHistograms(startImg, 32);
                histogramBox1.Refresh(); //更新数据
                histogramBox1.Show();

                histogramBox2.GenerateHistograms(endImg, 32);
                histogramBox2.Refresh(); //更新数据
                histogramBox2.Show();
            }
        }

        private void 图像比例缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
