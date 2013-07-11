using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExportLogic
{
    public partial class ExportMailsWindow : Form
    {
        public Settings settings;
        public ExportMailsWindow()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnableEverything(false);
        }
        volatile string previousValue = null;

        private void tbProjectNumber_KeyPress(object sender, KeyPressEventArgs e)
        {            
            //ThreadPool.QueueUserWorkItem(o =>
            //{
            //    Thread.Sleep(350);
            //    string value = (string)o;
            //    this.BeginInvoke((Action)(()=>
            //    {
            //        if(!string.Equals(value, tbProjectNumber.Text, StringComparison.OrdinalIgnoreCase))
            //            return;
            //    }));
            //    ApplicationController.GuiThreadMarshaller.MarshalToGuiThreadAsync(
            //        s => TeCaseNameChangedAfterTimeout((string)s), o);
            //}, tbProjectNumber.Text);
        }

        private void tbProjectNumber_TextChanged(object sender, EventArgs e)
        {
            var results  = new ExportTargetAccessor(settings).Find(tbProjectNumber.Text);
            SetUpResults(results);
        }

        private void SetUpResults(FindResults results)
        {
            EnableEverything(results != null);
            if (!results.IsAtLeastOneValid())
            {
                var projNameDialog = new ProjectName();
                var dialogResult = projNameDialog.ShowDialog();
                if()
            }
        }

        private void EnableEverything(bool enabled)
        {
            gbAvailableLocations.Enabled =  bExport.Enabled = 
            rbMarketing.Enabled = rbProject.Enabled = rbProposal.Enabled = 
            bProjectExport.Enabled = bProjectView.Enabled = bProposalExport.Enabled = bProposalExport.Enabled = bMarketingExport.Enabled = bMarketingView.Enabled = enabled;
        }
    }
}
