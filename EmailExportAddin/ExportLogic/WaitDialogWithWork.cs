using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExportLogic
{
    public partial class WaitDialogWithWork : Form
    {
        private ILog log = LogManager.GetLogger(typeof(WaitDialogWithWork));
        private Action work;
        private bool workInBackground;
        public WaitDialogWithWork()
        {
            InitializeComponent();
        }

        public void ShowWithWork(Action work, bool workInBackground)
        {            
            this.work = work;
            this.workInBackground = workInBackground;
            ShowDialog();
        }

        public static WaitDialogWithWork Current { get; private set; }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (workInBackground)
                BackgroundWork();
            else
                MainThreadWork();
        }

        private void MainThreadWork()
        {
            var currentDialog = Current;
            Current = this;
            try
            {
                pbWork.Style = ProgressBarStyle.Blocks;
                work();
            }
            catch (Exception ex)
            {
                log.Error("error in work", ex);
                MessageBox.Show(ex.Message, "Error in Email Export");
            }
            finally
            {
                Done();
                Current = currentDialog;
            }

        }

        private void BackgroundWork()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                var currentDialog = Current;
                Current = this;
                try
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                    work();
                }
                catch (Exception ex)
                {
                    log.Error("error in background work", ex);
                    this.BeginInvoke((Action)(() => MessageBox.Show(ex.Message, "Error in Email Export")));
                }
                finally
                {
                    Done();
                    Current = currentDialog;
                }
            }
                );
        }

        public void ShowMessage(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)delegate { ShowMessage(message); });
                return;
            }
            lMessage.Text = message;        
            if(!workInBackground)
                System.Windows.Forms.Application.DoEvents();
        }

        public void Done()
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)delegate { Done(); });
                return;
            }
            Close();
        }
    }
}
