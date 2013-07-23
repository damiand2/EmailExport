namespace ExportLogic
{
    partial class ExportConfirmationWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lMessage = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cbOpenFolderAfterExport = new System.Windows.Forms.CheckBox();
            this.cbRemoveAfterExport = new System.Windows.Forms.CheckBox();
            this.lProjectName = new System.Windows.Forms.Label();
            this.gbDirectories = new System.Windows.Forms.GroupBox();
            this.lbDirectories = new System.Windows.Forms.ListBox();
            this.gbDirectories.SuspendLayout();
            this.SuspendLayout();
            // 
            // lMessage
            // 
            this.lMessage.Location = new System.Drawing.Point(13, 13);
            this.lMessage.Name = "lMessage";
            this.lMessage.Size = new System.Drawing.Size(425, 39);
            this.lMessage.TabIndex = 0;
            this.lMessage.Text = "Do you want to export messages?";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(276, 244);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 1;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(363, 244);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // cbOpenFolderAfterExport
            // 
            this.cbOpenFolderAfterExport.AutoSize = true;
            this.cbOpenFolderAfterExport.Location = new System.Drawing.Point(12, 246);
            this.cbOpenFolderAfterExport.Name = "cbOpenFolderAfterExport";
            this.cbOpenFolderAfterExport.Size = new System.Drawing.Size(141, 17);
            this.cbOpenFolderAfterExport.TabIndex = 10;
            this.cbOpenFolderAfterExport.Text = "Open Folder after Export";
            this.cbOpenFolderAfterExport.UseVisualStyleBackColor = true;
            // 
            // cbRemoveAfterExport
            // 
            this.cbRemoveAfterExport.AutoSize = true;
            this.cbRemoveAfterExport.Location = new System.Drawing.Point(12, 223);
            this.cbRemoveAfterExport.Name = "cbRemoveAfterExport";
            this.cbRemoveAfterExport.Size = new System.Drawing.Size(122, 17);
            this.cbRemoveAfterExport.TabIndex = 9;
            this.cbRemoveAfterExport.Text = "Remove after export";
            this.cbRemoveAfterExport.UseVisualStyleBackColor = true;
            // 
            // lProjectName
            // 
            this.lProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lProjectName.Location = new System.Drawing.Point(13, 52);
            this.lProjectName.Name = "lProjectName";
            this.lProjectName.Size = new System.Drawing.Size(425, 21);
            this.lProjectName.TabIndex = 11;
            // 
            // gbDirectories
            // 
            this.gbDirectories.Controls.Add(this.lbDirectories);
            this.gbDirectories.Location = new System.Drawing.Point(12, 76);
            this.gbDirectories.Name = "gbDirectories";
            this.gbDirectories.Size = new System.Drawing.Size(426, 141);
            this.gbDirectories.TabIndex = 12;
            this.gbDirectories.TabStop = false;
            this.gbDirectories.Text = "Click below for subfolder export";
            // 
            // lbDirectories
            // 
            this.lbDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDirectories.FormattingEnabled = true;
            this.lbDirectories.Location = new System.Drawing.Point(3, 16);
            this.lbDirectories.Name = "lbDirectories";
            this.lbDirectories.Size = new System.Drawing.Size(420, 122);
            this.lbDirectories.Sorted = true;
            this.lbDirectories.TabIndex = 0;
            // 
            // ExportConfirmationWindow
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(450, 273);
            this.Controls.Add(this.gbDirectories);
            this.Controls.Add(this.lProjectName);
            this.Controls.Add(this.cbOpenFolderAfterExport);
            this.Controls.Add(this.cbRemoveAfterExport);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.lMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExportConfirmationWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export mails?";
            this.gbDirectories.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lMessage;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        public System.Windows.Forms.CheckBox cbOpenFolderAfterExport;
        public System.Windows.Forms.CheckBox cbRemoveAfterExport;
        private System.Windows.Forms.Label lProjectName;
        private System.Windows.Forms.GroupBox gbDirectories;
        private System.Windows.Forms.ListBox lbDirectories;
    }
}