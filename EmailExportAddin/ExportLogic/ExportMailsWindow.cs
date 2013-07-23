using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
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
				new ExportTargetAccessor(settings).CreateProjectFolder(result, results, tbProjectNumber.Text.Trim());
				RefreshResults();
			}
			else//export
			{
				PerformExport(result);
			}			
		}

		void ViewClick(object sender, EventArgs e)
		{
			var button = (Button)sender;
			string path = null;
			var result = button.Tag as SingleResult;
			if (result == null || string.IsNullOrEmpty(result.ProjectPath))
			{
				if (button == bProjectView)
					path = settings.ProjectPath;
				if (button == bProposalView)
					path = settings.ProposalPath;
				if (button == bMarketingView)
					path = settings.MarketingPath;
				
			}
			else
			{
				path = result.ProjectPath;
			}

			ShowFolderInExplorer(path);
		}

		private static void ShowFolderInExplorer(string path)
		{
			var dirInfo = new DirectoryInfo(path);
			while (!dirInfo.Exists)
				dirInfo = dirInfo.Parent;
			var psi = new System.Diagnostics.ProcessStartInfo();
			psi.UseShellExecute = true;
			psi.FileName = dirInfo.FullName;
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
			lbExportHistory.DataSource = userSettings.MruItems;
			lbExportHistory.SelectedIndex = -1;
			settingUserSettings = false;
		}

		private void tbProjectNumber_TextChanged(object sender, EventArgs e)
		{
			if (tbProjectNumber.Text == null || tbProjectNumber.Text.Length < 5)
			{
				results = null;
				SetUpResults();
				return;
			}
			if(tbProjectNumber.Text.Length == 5 || tbProjectNumber.Text.Length == 7)
				RefreshResults();
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



		private void SetUpResults()
		{

			lProjectName.Text = "Please enter Project Number";
			if (tbProjectNumber.Text != null && tbProjectNumber.Text.Length > 4)
				lProjectName.Text = "Project Number not Found";
			var canProceed = results != null && !results.Results.All(r => r.FatalError);
			EnableEverything(canProceed);
			BindResults();
			if (!canProceed)
				return;

			SetExportOption(results.DefaultResult);
			var result = results.Results.Find(r => !string.IsNullOrEmpty(r.ProjectName));
			if (result == null)
				return;

			lProjectName.Text = result.ProjectName;
			//if (tbProjectNumber.Text !=result.ProjectNumber)
			//    tbProjectNumber.Text = result.ProjectNumber;
		}

		private void BindResults()
		{
			lMarketingStatus.Text = lProjectStatus.Text = lProposalStatus.Text = string.Empty;
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
			bExport.Enabled = 
			rbMarketing.Enabled = rbProject.Enabled = rbProposal.Enabled =
			bProjectExport.Enabled =  bProposalExport.Enabled = bMarketingExport.Enabled =  enabled;
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

				new ExportTargetAccessor(settings).CreateProjectFolder(result, results, tbProjectNumber.Text.Trim());				
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
			try
			{
				//if (string.IsNullOrEmpty(target.EmailFolderPath))
				//    target.EmailFolderPath = target.ProjectPath.AppendSlash() + "Emails".AppendSlash();
				var confirmationWindow = new ExportConfirmationWindow { MailCount = Mails.Length, Target = target };
				confirmationWindow.cbRemoveAfterExport.Checked = userSettings.RemoveMailAfterExport;
				confirmationWindow.cbOpenFolderAfterExport.Checked = userSettings.OpenFolderAfterExport;
				var dialogResult = confirmationWindow.ShowDialog();
				userSettings.RemoveMailAfterExport = confirmationWindow.cbRemoveAfterExport.Checked;
				userSettings.OpenFolderAfterExport = confirmationWindow.cbOpenFolderAfterExport.Checked;
				confirmationWindow.Dispose();
				if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
					return;
				//Emails subfolder might not exist yet
				if (!new ExportTargetAccessor(settings).CreateProjectFolder(target, results, tbProjectNumber.Text.Trim()))
					return;
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
							name = mail.SentOn.ToString("yyyy-MM-dd hh-mm-ss") + " " + mail.SenderName + " " + mail.Subject;
						if (mail.HasAnyAttachment())
							name += (" " + settings.AttachmentsSuffix);
						name += ".msg";
						try
						{
							mail.SaveAs(target.EmailFolderPath.AppendSlash() + FileHelper.ConvertToValidFileName(name), OlSaveAsType.olMSGUnicode);
							if (userSettings.RemoveMailAfterExport)
							{
								mail.Delete();
							}
						}
						catch (System.Exception ex)
						{
							log4net.LogManager.GetLogger(typeof(ExportMailsWindow)).Error("Error while saving mail:" + name, ex);
							MessageBox.Show("Error while saving mail " + mail.Subject + " to disk, operation will continue with other mails. Details:" + ex.Message);
						}

						dialog.pbWork.PerformStep();
					}


				}, false);
				PersistSettings(target);
				if (userSettings.OpenFolderAfterExport)
					ShowFolderInExplorer(target.EmailFolderPath);
				Close();
			}
			catch (System.Exception ex)
			{
				log4net.LogManager.GetLogger(typeof(ExportMailsWindow)).Error("Error while performing export operation.", ex);
				MessageBox.Show("Error while performing export operation. All info will be saved to log file for future bugfixing. Details:" + ex.Message);
			}
			
		}

		

		private void PersistSettings(SingleResult target)
		{
			MruItem item = new MruItem { ProjectNumber = target.ProjectNumber, ProjectType = target.Type, ProjectName = target.ProjectName };
			userSettings.MruItems.Remove(item);
			userSettings.MruItems.Insert(0, item);
			if (userSettings.MruItems.Count > 20)
				userSettings.MruItems.RemoveRange(19, userSettings.MruItems.Count - 20);			
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
			settingUserSettings = true;
			lbExportHistory.SelectedIndex = -1;
			settingUserSettings = false;
		}
	}
}
