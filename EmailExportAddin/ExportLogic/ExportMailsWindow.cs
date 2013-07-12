using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExportLogic
{
	public partial class ExportMailsWindow : Form
	{
		public Settings settings;
		public MailItem[] Mails;
		FindResults results;
		private bool settingUserSettings = false;
		UserSettings userSettings;
		public ExportMailsWindow()
		{
			InitializeComponent();

			bMarketingExport.Click += ExportClick;
			bProjectExport.Click += ExportClick;
			bProposalExport.Click += ExportClick;
			bMarketingView.Click += ViewClick;
			bProjectView.Click += ViewClick;
			bProposalView.Click += ViewClick;
		}

		void ExportClick(object sender, EventArgs e)
		{
			var result = ((Button)sender).Tag as SingleResult;
			if (result == null || string.IsNullOrEmpty(result.ProjectPath) || result.FatalError)
				return;

			if (!result.Exists)//this is create workflow
			{
				new ExportTargetAccessor(settings).CreateProjectFolder(result);
                RefreshResults();
			}
			else//export
			{
                PerformExport(result);
            }			
		}

		void ViewClick(object sender, EventArgs e)
		{
			var result = ((Button)sender).Tag as SingleResult;
			if (result == null || string.IsNullOrEmpty(result.ProjectPath))
				return;
			var psi = new System.Diagnostics.ProcessStartInfo();
			psi.UseShellExecute = true;
			psi.FileName = result.ProjectPath;
			System.Diagnostics.Process.Start(psi);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			EnableEverything(false);
			RestoreUserSettings();
		}

		private void RestoreUserSettings()
		{
			settingUserSettings = true;
			userSettings = settings.GetUserSettings();
			cbRemoveAfterExport.Checked = userSettings.RemoveMailAfterExport;
			lbExportHistory.DataSource = userSettings.MruItems;
            lbExportHistory.SelectedIndex = -1;
			settingUserSettings = false;
		}
		
		private void tbProjectNumber_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((int)e.KeyChar == (int)System.Windows.Forms.Keys.Enter)
				RefreshResults();
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
			
		}

		private void SetUpResults()
		{
			var canProceed = results != null && !results.Results.All(r => r.FatalError);
			EnableEverything(canProceed);
			BindResults();
			if (!canProceed)
				return;
			if (!results.IsAtLeastOneValid())
			{
				SetNewProject(results);
			}
			SetExportOption(results.DefaultResult);
			
		}

		private void BindResults()
		{
			if (results == null)
				return;
			foreach (var result in results.Results)
			{
				switch (result.Type)
				{
					case TargetType.Marketing:
						SetSingleExportControls(result, lMarketingStatus, bMarketingExport, bMarketingView, rbMarketing);
						break;
					case TargetType.Proposal:
						SetSingleExportControls(result, lProposalStatus, bProposalExport, bProposalView, rbProposal);
						break;
					case TargetType.Project:
						SetSingleExportControls(result, lProjectStatus, bProjectExport, bProjectView, rbProject);
						break;
				}
			}
			if (results.IsAtLeastOneValid())
				bExport.Enabled = true;
		}

		private void SetSingleExportControls(SingleResult result, Label messagelabel, Button export, Button view, RadioButton radioButton)
		{
			export.Tag = view.Tag = radioButton.Tag = result;
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
			switch (targetType)
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
			gbAvailableLocations.Enabled = bExport.Enabled = cbRemoveAfterExport.Enabled =
			rbMarketing.Enabled = rbProject.Enabled = rbProposal.Enabled =
			bProjectExport.Enabled = bProjectView.Enabled = bProposalExport.Enabled = bProposalExport.Enabled = bMarketingExport.Enabled = bMarketingView.Enabled = enabled;
		}

		private void tbProjectNumber_Validated(object sender, EventArgs e)
		{
			//RefreshResults();
		}

		private void RefreshResults()
		{
			results = new ExportTargetAccessor(settings).Find(tbProjectNumber.Text);
			SetUpResults();
		}

		

		private void bRefreshLocations_Click(object sender, EventArgs e)
		{
			RefreshResults();
		}

		private void bExport_Click(object sender, EventArgs e)
		{ 
			try
			{
				var selectedButton = new[] { rbMarketing, rbProject, rbProposal }.First(r => r.Checked);
				var result = selectedButton.Tag as SingleResult;
				if (result == null || string.IsNullOrEmpty(result.ProjectPath) || result.FatalError)
					return;

				if (!result.Exists)//this is create workflow
				{
					new ExportTargetAccessor(settings).CreateProjectFolder(result);
				}
				PerformExport(result);
				Close();
			}
			catch (System.Exception ex)
			{
				log4net.LogManager.GetLogger(typeof(ExportMailsWindow)).Error("Error during export", ex);
				MessageBox.Show("Error during export: " + ex.Message);
			}           
		}

		private void PerformExport(SingleResult target)
		{
			WaitDialogWithWork dialog = new WaitDialogWithWork();
			dialog.ShowWithWork(() =>
			{
				for (int i = 0; i < Mails.Length; i++)
				{

					dialog.pbWork.Step = 100 / Mails.Length;
					dialog.ShowMessage("Exporting mail " + (i + 1) + " of " + Mails.Length + ". Please wait...");
					MailItem mail = Mails[i];
					string name = mail.Subject;
					if (mail.Sent)
						name = mail.SentOn.ToString("s") + "_" + mail.SenderName + "_" + mail.Subject;
					if (mail.HasAnyAttachment())
						name += ("_" + settings.AttachmentsSuffix);
					name +=".msg";
                    try
                    {
                        mail.SaveAs(target.ProjectPath.AppendSlash() + "Mail".AppendSlash() + FileHelper.ConvertToValidFileName(name), OlSaveAsType.olMSGUnicode);
                        if (cbRemoveAfterExport.Checked)
                        {
                            mail.Delete();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        log4net.LogManager.GetLogger(typeof(ExportMailsWindow)).Error("Error wile saving mail:" + name, ex);
                        MessageBox.Show("Error while saving mail " + mail.Subject + " to disk, operation will continue with other mails. Details:" + ex.Message);
                    }
					
					dialog.pbWork.PerformStep();
				}
                
				
			}, false);
            PersistSettings(target);
            
            Close();
		}

        private void PersistSettings(SingleResult target)
        {
            MruItem item = new MruItem { ProjectNumber = tbProjectNumber.Text.Trim(), ProjectType = target.Type };
            userSettings.MruItems.Remove(item);
            userSettings.MruItems.Insert(0, item);
            if (userSettings.MruItems.Count > 20)
                userSettings.MruItems.RemoveRange(19, userSettings.MruItems.Count - 20);
            userSettings.RemoveMailAfterExport = cbRemoveAfterExport.Checked;
            settings.SetUserSettings(userSettings);
        }

		private void lbExportHistory_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (settingUserSettings)
                return;
            var item = (MruItem)lbExportHistory.SelectedItem;
            tbProjectNumber.Text = item.ProjectNumber;
            RefreshResults();
            switch(item.ProjectType)
            {
                case TargetType.Marketing:
                    rbMarketing.Checked = true; break;
                case TargetType.Project:
                    rbProject.Checked = true; break;
                case TargetType.Proposal:
                    rbProposal.Checked = true; break;
            }
		}
	}
}
