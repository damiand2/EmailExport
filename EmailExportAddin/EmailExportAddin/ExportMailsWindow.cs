using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EmailExportAddin
{
    public partial class ExportMailsWindow : Form
    {
        public Settings settings;
        FindResults results;
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
            results  = new ExportTargetAccessor(settings).Find(tbProjectNumber.Text);
            SetUpResults();
        }

        private void SetUpResults()
        {
            var canProceed = results != null && !results.Results.All(r=>r.FatalError);
            EnableEverything(canProceed);
            if(!canProceed == null)
                return;            
            if (!results.IsAtLeastOneValid())
            {
                SetNewProject(results);
            }
            SetExportOption(results.DefaultResult);
            BindResults();
        }

        private void BindResults()
        {
            foreach (var result in results.Results)
            {
                switch(result.Type)
                {
                    case TargetType.Marketing:
                        SetSingleExportControls(result, lMarketingStatus, bMarketingExport, bMarketingView);
                        break;
                    case TargetType.Proposal:
                        SetSingleExportControls(result, lProposalStatus, bProposalExport, bProposalView);
                        break;
                    case TargetType.Project:
                        SetSingleExportControls(result, lProjectStatus, bProjectExport, bProjectView);
                        break;
                }
            }
            if (results.IsAtLeastOneValid())
                bExport.Enabled = true;
        }

        private void SetSingleExportControls(SingleResult result, Label messagelabel, Button export, Button view)
        {
            messagelabel.Text = result.WarningMessage ?? "OK";
            export.Enabled = !result.FatalError && !result.NoAccess;
            if (!result.FatalError && !result.Exists)
                export.Text = "Create";
            else
                export.Text = "Export";
        }

        private void SetNewProject(FindResults results)
        {
            var projNameDialog = new ProjectName();
            var dialogResult = projNameDialog.ShowDialog();
            var projName = tbProjectNumber.Text.Trim();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                projName += " " + projNameDialog.tbProjectName.Text.Trim();
            results.Results.ForEach(r =>
            {
                if (r.FatalError)
                    return;
                r.ProjectName = projName;
                r.ProjectPath = r.ProjectPath.AppendSlash() + r.ProjectName.AppendSlash();
            });
        }

        private void SetExportOption(TargetType targetType)
        {
            RadioButton button = null;
            switch(targetType)
            {
                case TargetType.Marketing:
                    button = rbMarketing; break;
                case TargetType.Project:
                    button = rbProject; break;
                case TargetType.Proposal:
                    button = rbProposal; break;
            }
            button.Checked = true;
        }

        private void EnableEverything(bool enabled)
        {
            gbAvailableLocations.Enabled =  bExport.Enabled = 
            rbMarketing.Enabled = rbProject.Enabled = rbProposal.Enabled = 
            bProjectExport.Enabled = bProjectView.Enabled = bProposalExport.Enabled = bProposalExport.Enabled = bMarketingExport.Enabled = bMarketingView.Enabled = enabled;
        }
    }
}
