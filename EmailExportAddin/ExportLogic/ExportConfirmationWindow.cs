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
            lMessage.Text = string.Format("{0} message(s) will be exported to {1} {2} - {3}. \r\nDo you want to continue?", MailCount, Target.ProjectNumber, Target.ProjectName, Target.Type);
            lProjectName.Text = Target.ProjectName;
            LoadDirectories();
        }

        private void LoadDirectories()
        {
            DirectoryInfo dir;
            if (!string.IsNullOrEmpty(Target.EmailFolderPath))
                dir = new DirectoryInfo(Target.EmailFolderPath).Parent;
            else
                dir = new DirectoryInfo(Target.ProjectPath);
            var dirs = dir.GetDirectories();
            lbDirectories.DisplayMember = "Name";
            lbDirectories.DataSource = dirs;    
            var dirName = new DirectoryInfo(Target.EmailFolderPath).Name;
            lbDirectories.SelectedItem = dirs.Where(d => string.Equals(d.Name, dirName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

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
    }
}
