using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
    class BatchExtractPicDataFromTreeView
    {
        public static Image pictureBox1Image = null;
        public static Image pictureBox2Image = null;
        public static Image pictureBox3Image = null;
        DBConnect dbConnect = new DBConnect();

        //灰度化
        public void Button_2GRBGray_Click()
        {
            if (pictureBox1Image == null && pictureBox2Image == null && pictureBox3Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1Image = pictureBox3Image;

                Bitmap getPotImg = new Bitmap(pictureBox3Image);
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
                pictureBox2Image = getPotImg;

                pictureBox3Image = pictureBox2Image;
            }
        }
        //二值化
        public void Button_PicBinarization_Click()
        {
            if (pictureBox1Image == null && pictureBox2Image == null && pictureBox3Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1Image = pictureBox3Image;
                Bitmap getPotImg = new Bitmap(pictureBox3Image);

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

                if (FrmMainMenu.cis != 0)
                {
                    FrmMainMenu.cis = 0;
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
                            FrmMainMenu.cis++;    //土壤像素
                        }
                    }
                }
              
                pictureBox2Image = getPotImg;

                pictureBox3Image = pictureBox2Image;
            }
        }
        //作物行标记
        public void CropLineMarker_Click()
        {
            if (pictureBox1Image == null && pictureBox2Image == null && pictureBox3Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1Image = pictureBox3Image;

                //寻找作物行的中心线
                bezierCenterLineToolStripMenuItem_Click(null, null);

                pictureBox1Image = pictureBox2Image;
                Bitmap getPotImg = new Bitmap(pictureBox1Image);
                int length = 46;  //滤除作物行的宽度
                int m, k;
                Bitmap newBmp = new Bitmap(pictureBox1Image);

                if (FrmMainMenu.cropsCount != 0)
                {
                    FrmMainMenu.cropsCount = 0;
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
                                    FrmMainMenu.cropsCount++;
                                }
                            }
                        }
                    }
                }
                FrmMainMenu.cic = FrmMainMenu.cropsCount;  //作物覆盖面积

                pictureBox2Image = newBmp;

                pictureBox3Image = pictureBox2Image;
            }
        }
        //滤除作物行
        public void FilterCropRows_Click()
        {
            if (pictureBox1Image == null && pictureBox2Image == null && pictureBox3Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                pictureBox1Image = pictureBox3Image;
                Bitmap getPotImg = new Bitmap(pictureBox3Image);

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

                pictureBox2Image = getPotImg;
                pictureBox3Image = pictureBox2Image;

                pictureBox1Image = pictureBox3Image;
                //膨胀操作
                Image<Bgr, Byte> imgtem = new Image<Bgr, Byte>(new Bitmap(pictureBox3Image));
                pictureBox2Image = imgtem.Dilate(1).ToBitmap();

                pictureBox3Image = pictureBox2Image;
            }
        }
        //杂草和作物的株数和像素密度的提取统计
        public void ExtractWCsData_Click()
        {
            if (pictureBox1Image == null && pictureBox2Image == null && pictureBox3Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                Bitmap tempBmp = new Bitmap(pictureBox3Image);
                Image<Gray, Byte> imageSrc = new Image<Gray, Byte>(tempBmp);

                Contour<Point> contour = imageSrc.FindContours();

                for (int i = 0; i < tempBmp.Width; i++)
                {
                    for (int j = 0; j < tempBmp.Height; j++)
                    {
                        if (tempBmp.GetPixel(i, j).R == 255 && tempBmp.GetPixel(i, j).G == 255 && tempBmp.GetPixel(i, j).B == 255)
                        {
                            FrmMainMenu.weedCount++;
                        }
                    }
                }
                FrmMainMenu.ciw = FrmMainMenu.weedCount;  //杂草覆盖面积

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
                FrmMainMenu.weedNumber = CvInvoke.cvFindContours(imageSrc, Dynstorage, ref Dyncontour, Marshal.SizeOf(con), Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_LINK_RUNS, new Point(0, 0));

                Seq<Point> DyncontourTemp1 = new Seq<Point>(Dyncontour, null);//方便对IntPtr类型进行操作  
                Seq<Point> DyncontourTemp = DyncontourTemp1;

                for (; DyncontourTemp1 != null && DyncontourTemp1.Ptr.ToInt32() != 0; DyncontourTemp1 = DyncontourTemp1.HNext)
                {
                    CvInvoke.cvDrawContours(dstImage, DyncontourTemp1, new MCvScalar(255, 255, 255), new MCvScalar(255, 255, 255), 0, 1, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, new Point(0, 0));
                }

                pictureBox2Image = dstImage.ToBitmap();  //显示出来

                pictureBox3Image = pictureBox2Image;

                //数据计算并保存数据到数据中
                FrmMainMenu.weedDensity = FrmMainMenu.weedCount / (dstImage.Height * dstImage.Width);
                FrmMainMenu.cropDensity = FrmMainMenu.cropsCount / (dstImage.Height * dstImage.Width);

                FrmMainMenu.cisDensity = FrmMainMenu.cis / (dstImage.Height * dstImage.Width);
                FrmMainMenu.aic = dstImage.Height * dstImage.Width;

                //计算威胁率 tRate = (ciw/cic)(1-cis/aic)
                if (FrmMainMenu.cis != 0 && FrmMainMenu.aic != 0)
                {
                    FrmMainMenu.tRate = (FrmMainMenu.ciw / FrmMainMenu.cic) * (1 - FrmMainMenu.cis / FrmMainMenu.aic);
                }          


                if (pictureBox2Image != null)
                {
                    dbConnect.Insert(FrmMainMenu.imageName, FrmMainMenu.weedNumber, FrmMainMenu.weedDensity, FrmMainMenu.cropDensity, FrmMainMenu.cisDensity, FrmMainMenu.ciw, FrmMainMenu.cic, FrmMainMenu.cis, FrmMainMenu.aic,FrmMainMenu.tRate);
                    //保存图像的GPS坐标与杂草威胁度
                    dbConnect.InsertLatLongRate(FrmMainMenu.imageName, FrmMainMenu.latitude, FrmMainMenu.longitude, FrmMainMenu.tRate);

                    FrmMainMenu.weedNumber = 0;
                    FrmMainMenu.weedCount = 0;
                    FrmMainMenu.cropsCount = 0;
                    FrmMainMenu.ciw = 0;   //单元格内的杂草覆盖面积--weedCount   这里单元格指图像大小
                    FrmMainMenu.aic = 0;   //单元格的面积
                    FrmMainMenu.cic = 0;   //单元格内的作物覆盖面积--cropsCount
                    FrmMainMenu.cis = 0;   //单元格内的土壤覆盖面积
                    FrmMainMenu.tRate = 0;   //单元格内的杂草维修率

                }
            }
        }

        //寻找作物行中心线
        private void bezierCenterLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1Image == null && pictureBox2Image == null && pictureBox3Image == null)
            {
                MessageBox.Show("错误，没有导入图片！");
                return;
            }
            else
            {
                if (pictureBox1Image != null)
                {
                    Bitmap pic = new Bitmap(pictureBox1Image);
                    Bitmap picoriginal = new Bitmap(pictureBox1Image);
                    Graphics paint = Graphics.FromImage(pictureBox2Image);
                    paint.DrawImage(pic, 0, 0, pic.Width, pic.Height);
                    paint.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    Pen pen = new Pen(Color.Red, 3);
                    //Thread thread = new Thread(new ThreadStart(process));
                    //thread.Start();
                    //this.Refresh();
                    if (pictureBox2Image != null)
                    {
                        pictureBox3Image = pictureBox2Image;
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
                        for (i = 0; i < pictureBox2Image.Height - 1; i++)
                        {
                            j = 0;
                            while (j < pictureBox2Image.Width - 19)
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
                        for (i = 1; i < pictureBox2Image.Height - 1; i++)
                        {
                            j = 0;
                            while (j < pictureBox2Image.Width)
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
                        while (originalx < pictureBox2Image.Width)
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
                                while (county < pictureBox2Image.Height)//判断下一行中点,限定高度
                                {
                                    if (countx - 20 >= 0 && countx + 20 < pictureBox2Image.Width)//在指定的横行范围内进行探测（不超过边缘防止泄露）
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
                                    bezier[i].Y = pictureBox2Image.Height - 1;
                                    i++;
                                }
                                else
                                    if (i % 3 == 1)
                                    {
                                        bezier[i].X = bezier[i - 1].X;
                                        bezier[i].Y = pictureBox2Image.Height - 8;
                                        i++;
                                        bezier[i].X = bezier[i - 1].X;
                                        bezier[i].Y = pictureBox2Image.Height - 5;
                                        i++;
                                        bezier[i].X = bezier[i - 1].X;
                                        bezier[i].Y = pictureBox2Image.Height - 1;
                                        i++;
                                    }
                                    else
                                        if (i % 3 == 2)
                                        {
                                            bezier[i].X = bezier[i - 1].X;
                                            bezier[i].Y = pictureBox2Image.Height - 5;
                                            i++;
                                            bezier[i].X = bezier[i - 1].X;
                                            bezier[i].Y = pictureBox2Image.Height - 1;
                                            i++;
                                        }
                                if (i < 61)
                                {
                                    for (j = i; j < 61; j++)
                                    {
                                        bezier[j].X = bezier[i - 1].X;
                                        bezier[j].Y = pictureBox2Image.Height - 1;
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
                        Bitmap picagain = new Bitmap(pictureBox2Image);
                        pictureBox1Image = picoriginal;
                        pictureBox2Image = picagain;
                        //thread.Abort();
                    }
                }
            }
        }
    }
}
