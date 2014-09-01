using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.IO;

namespace EmailExportAddin
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logging.config")));
            var log  = log4net.LogManager.GetLogger(typeof(ThisAddIn));
            log.Info("Starting adding. OS ver: " + Environment.OSVersion + " . Runtime ver: " + Environment.Version + " . Outlook Ver: " + Application.Version);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new Ribbon1();
        }

        
        #endregion
    }
}
