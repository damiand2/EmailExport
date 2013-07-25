using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExportLogic
{
    public partial class ExportConfirmationWindow : Form
    {
        public ExportConfirmationWindow()
        {
            InitializeComponent();
            ToggleSubfolderExport();
        }

        public int MailCount;
        public SingleResult Target;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            lMessage.Text = string.Format("{0} message(s) will be exported to {1} Folder. \r\nDo you want to continue?", MailCount, Target.Type);
            lProjectName.Text = Target.ProjectNumber + " " + Target.ProjectName;
            LoadDirectories();
            
        }       

        private void LoadDirectories()
        {
            DirectoryInfo dir;
            if (!string.IsNullOrEmpty(Target.EmailFolderPath))
                dir = new DirectoryInfo(Target.EmailFolderPath).Parent;
            else
                dir = new DirectoryInfo(Target.ProjectPath);
            var dirs = new List<DirectoryInfo>(dir.GetDirectories());
            for (int i = dirs.Count -1 ; i >= 0; i--)
            {
                if (dirs[i].Name.Equals("Email", StringComparison.OrdinalIgnoreCase)
                    || dirs[i].Name.Equals("Emails", StringComparison.OrdinalIgnoreCase))
                    dirs.RemoveAt(i);
            }
            lbDirectories.DisplayMember = "Name";
            lbDirectories.DataSource = dirs;               
            lbDirectories.SelectedIndex = -1;
            if (string.IsNullOrEmpty(Target.SubFolder))
                return;
            ceSubfolderExport.Checked = true;
            var selectedDir = dirs.FirstOrDefault(d => string.Equals(Target.SubFolder, d.Name, StringComparison.OrdinalIgnoreCase));
            if (selectedDir != null)
                lbDirectories.SelectedItem = selectedDir;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (ceSubfolderExport.Checked && lbDirectories.SelectedIndex == -1)
            {
                MessageBox.Show("Please select subfolder for export. If you want to export to main folder, uncheck 'Subfolder export' checkbox");
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
            if (ceSubfolderExport.Checked)
            {
                var dir = (DirectoryInfo)lbDirectories.SelectedItem;
                Target.ProjectPath = dir.FullName;
                Target.SubFolder = dir.Name;
            }
            Close();

        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void lbDirectories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbDirectories.SelectedIndex == -1)
                lProjectName.Text = Target.ProjectNumber + " " + Target.ProjectName;
            else
                lProjectName.Text = Target.ProjectNumber + " " + Target.ProjectName +" - " + ((DirectoryInfo)lbDirectories.SelectedItem).Name;
        }

        private void lbDirectories_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys.HasFlag(Keys.Control))
                lbDirectories.ClearSelected();
        }

        private void ceSubfolderExport_CheckedChanged(object sender, EventArgs e)
        {
            ToggleSubfolderExport();
        }

        private void ToggleSubfolderExport()
        {
            if (ceSubfolderExport.Checked)
            {
                gbDirectories.Visible = true;
                this.Height = this.Height + 141;
            }
            else
            {
                lbDirectories.ClearSelected();
                gbDirectories.Visible = false;
                this.Height = this.Height - 141;
            }
        }

       
    }
}
