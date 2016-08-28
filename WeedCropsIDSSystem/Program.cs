using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WeedCropsIDSSystem
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThreadAttribute]  //   STA：Thread 将创建并进入一个单线程单元。
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMainMenu());
        }
    }
}
