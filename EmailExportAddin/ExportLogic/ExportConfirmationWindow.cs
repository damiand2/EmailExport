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

        }

        private void bOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            var dir = (DirectoryInfo)lbDirectories.SelectedItem;
            if(dir.Name.Equals("Email", StringComparison.OrdinalIgnoreCase) || dir.Name.Equals("Emails", StringComparison.OrdinalIgnoreCase))
                Target.EmailFolderPath = dir.FullName;
            else
                Target.ProjectPath = dir.FullName;
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

       
    }
}
