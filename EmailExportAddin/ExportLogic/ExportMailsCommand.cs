using log4net;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExportLogic
{
    public class ExportMailsCommand
    {
        private ILog log = LogManager.GetLogger(typeof(ExportMailsCommand));
        public MailItem[] Mails { get; set; }

        public void Execute()
        {
            try
            {
                log.Debug("Starting export mail command with number of mails: " + Mails.Length);
                var settings = Settings.Initialize();
                if (settings == null)
                    return;

                ExportMailsWindow window = new ExportMailsWindow { settings = settings, Mails = Mails };
                window.ShowDialog();
                window.Dispose();
            }
            catch (System.Exception ex)
            {                
                log.Fatal("Ending export mail command with exception.", ex);
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                log.Debug("Ending export mail command with number of mails: " + Mails.Length);
            }
            
        }
    }
}
