using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace WeedCropsIDSSystem
{
    public class ProcessOperator
    {
        private BackgroundWorker _backgroundWorker;//后台线程
        private progressBarShow _processForm;//进度条窗体
        private BackgroundWorkerEventArgs _eventArgs;//异常参数

        public ProcessOperator()
        {
            _backgroundWorker = new BackgroundWorker();
            _processForm = new progressBarShow();
            _eventArgs = new BackgroundWorkerEventArgs();
            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
        }

        //操作进行完毕后关闭进度条窗体
        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_processForm.Visible == true)
            {
                _processForm.Close();
            }
            if (this.BackgroundWorkerCompleted != null)
            {
                this.BackgroundWorkerCompleted(null, _eventArgs);
            }
        }

        //后台执行的操作
        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (BackgroundWork != null)
            {
                try
                {
                    BackgroundWork();
                }
                catch (Exception ex)
                {
                    _eventArgs.BackGroundException = ex;
                }
            }
        }

        #region 公共方法、属性、事件
      
        // 后台执行的操作
        public Action BackgroundWork { get; set; }

        // 设置进度条显示的提示信息
        public string MessageInfo
        {
            set { _processForm.MessageInfo = value; }
        }

        // 后台任务执行完毕后事件
        public event EventHandler<BackgroundWorkerEventArgs> BackgroundWorkerCompleted;

        // 开始执行
        public void Start()
        {
            _backgroundWorker.RunWorkerAsync();
            _processForm.ShowDialog();
        }

        #endregion
    }

    public class BackgroundWorkerEventArgs : EventArgs
    {
        // 后台程序运行时抛出的异常
        public Exception BackGroundException { get; set; }
    }

}
