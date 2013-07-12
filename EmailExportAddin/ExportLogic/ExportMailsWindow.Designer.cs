namespace ExportLogic
{
    partial class ExportMailsWindow
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
            this.lProjectNumber = new System.Windows.Forms.Label();
            this.tbProjectNumber = new System.Windows.Forms.TextBox();
            this.bRefreshLocations = new System.Windows.Forms.Button();
            this.gbAvailableLocations = new System.Windows.Forms.GroupBox();
            this.bProjectView = new System.Windows.Forms.Button();
            this.bProjectExport = new System.Windows.Forms.Button();
            this.bMarketingView = new System.Windows.Forms.Button();
            this.bMarketingExport = new System.Windows.Forms.Button();
            this.bProposalView = new System.Windows.Forms.Button();
            this.bProposalExport = new System.Windows.Forms.Button();
            this.lProjectStatus = new System.Windows.Forms.Label();
            this.lMarketingStatus = new System.Windows.Forms.Label();
            this.rbProject = new System.Windows.Forms.RadioButton();
            this.rbMarketing = new System.Windows.Forms.RadioButton();
            this.rbProposal = new System.Windows.Forms.RadioButton();
            this.lProposalStatus = new System.Windows.Forms.Label();
            this.cbRemoveAfterExport = new System.Windows.Forms.CheckBox();
            this.gbExportHistory = new System.Windows.Forms.GroupBox();
            this.lbExportHistory = new System.Windows.Forms.ListBox();
            this.bExport = new System.Windows.Forms.Button();
            this.gbAvailableLocations.SuspendLayout();
            this.gbExportHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // lProjectNumber
            // 
            this.lProjectNumber.Location = new System.Drawing.Point(12, 9);
            this.lProjectNumber.Name = "lProjectNumber";
            this.lProjectNumber.Size = new System.Drawing.Size(100, 23);
            this.lProjectNumber.TabIndex = 0;
            this.lProjectNumber.Text = "Project number:";
            this.lProjectNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbProjectNumber
            // 
            this.tbProjectNumber.Location = new System.Drawing.Point(118, 11);
            this.tbProjectNumber.Name = "tbProjectNumber";
            this.tbProjectNumber.Size = new System.Drawing.Size(119, 20);
            this.tbProjectNumber.TabIndex = 1;
            this.tbProjectNumber.TextChanged += new System.EventHandler(this.tbProjectNumber_TextChanged);
            this.tbProjectNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbProjectNumber_KeyPress);
            this.tbProjectNumber.Validated += new System.EventHandler(this.tbProjectNumber_Validated);
            // 
            // bRefreshLocations
            // 
            this.bRefreshLocations.Location = new System.Drawing.Point(339, 8);
            this.bRefreshLocations.Name = "bRefreshLocations";
            this.bRefreshLocations.Size = new System.Drawing.Size(75, 23);
            this.bRefreshLocations.TabIndex = 2;
            this.bRefreshLocations.Text = "Refresh";
            this.bRefreshLocations.UseVisualStyleBackColor = true;
            this.bRefreshLocations.Click += new System.EventHandler(this.bRefreshLocations_Click);
            // 
            // gbAvailableLocations
            // 
            this.gbAvailableLocations.Controls.Add(this.bProjectView);
            this.gbAvailableLocations.Controls.Add(this.bProjectExport);
            this.gbAvailableLocations.Controls.Add(this.bMarketingView);
            this.gbAvailableLocations.Controls.Add(this.bMarketingExport);
            this.gbAvailableLocations.Controls.Add(this.bProposalView);
            this.gbAvailableLocations.Controls.Add(this.bProposalExport);
            this.gbAvailableLocations.Controls.Add(this.lProjectStatus);
            this.gbAvailableLocations.Controls.Add(this.lMarketingStatus);
            this.gbAvailableLocations.Controls.Add(this.rbProject);
            this.gbAvailableLocations.Controls.Add(this.rbMarketing);
            this.gbAvailableLocations.Controls.Add(this.rbProposal);
            this.gbAvailableLocations.Controls.Add(this.lProposalStatus);
            this.gbAvailableLocations.Location = new System.Drawing.Point(12, 37);
            this.gbAvailableLocations.Name = "gbAvailableLocations";
            this.gbAvailableLocations.Size = new System.Drawing.Size(402, 100);
            this.gbAvailableLocations.TabIndex = 3;
            this.gbAvailableLocations.TabStop = false;
            this.gbAvailableLocations.Text = "Available locations:";
            // 
            // bProjectView
            // 
            this.bProjectView.Location = new System.Drawing.Point(321, 65);
            this.bProjectView.Name = "bProjectView";
            this.bProjectView.Size = new System.Drawing.Size(75, 23);
            this.bProjectView.TabIndex = 15;
            this.bProjectView.Text = "View";
            this.bProjectView.UseVisualStyleBackColor = true;
            // 
            // bProjectExport
            // 
            this.bProjectExport.Location = new System.Drawing.Point(240, 65);
            this.bProjectExport.Name = "bProjectExport";
            this.bProjectExport.Size = new System.Drawing.Size(75, 23);
            this.bProjectExport.TabIndex = 14;
            this.bProjectExport.Text = "Export";
            this.bProjectExport.UseVisualStyleBackColor = true;
            // 
            // bMarketingView
            // 
            this.bMarketingView.Location = new System.Drawing.Point(321, 42);
            this.bMarketingView.Name = "bMarketingView";
            this.bMarketingView.Size = new System.Drawing.Size(75, 23);
            this.bMarketingView.TabIndex = 13;
            this.bMarketingView.Text = "View";
            this.bMarketingView.UseVisualStyleBackColor = true;
            // 
            // bMarketingExport
            // 
            this.bMarketingExport.Location = new System.Drawing.Point(240, 42);
            this.bMarketingExport.Name = "bMarketingExport";
            this.bMarketingExport.Size = new System.Drawing.Size(75, 23);
            this.bMarketingExport.TabIndex = 12;
            this.bMarketingExport.Text = "Export";
            this.bMarketingExport.UseVisualStyleBackColor = true;
            // 
            // bProposalView
            // 
            this.bProposalView.Location = new System.Drawing.Point(321, 19);
            this.bProposalView.Name = "bProposalView";
            this.bProposalView.Size = new System.Drawing.Size(75, 23);
            this.bProposalView.TabIndex = 11;
            this.bProposalView.Text = "View";
            this.bProposalView.UseVisualStyleBackColor = true;
            // 
            // bProposalExport
            // 
            this.bProposalExport.Location = new System.Drawing.Point(240, 19);
            this.bProposalExport.Name = "bProposalExport";
            this.bProposalExport.Size = new System.Drawing.Size(75, 23);
            this.bProposalExport.TabIndex = 10;
            this.bProposalExport.Text = "Export";
            this.bProposalExport.UseVisualStyleBackColor = true;
            // 
            // lProjectStatus
            // 
            this.lProjectStatus.Location = new System.Drawing.Point(110, 65);
            this.lProjectStatus.Name = "lProjectStatus";
            this.lProjectStatus.Size = new System.Drawing.Size(124, 23);
            this.lProjectStatus.TabIndex = 9;
            this.lProjectStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lMarketingStatus
            // 
            this.lMarketingStatus.Location = new System.Drawing.Point(110, 42);
            this.lMarketingStatus.Name = "lMarketingStatus";
            this.lMarketingStatus.Size = new System.Drawing.Size(124, 23);
            this.lMarketingStatus.TabIndex = 8;
            this.lMarketingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbProject
            // 
            this.rbProject.AutoSize = true;
            this.rbProject.Location = new System.Drawing.Point(6, 65);
            this.rbProject.Name = "rbProject";
            this.rbProject.Size = new System.Drawing.Size(90, 17);
            this.rbProject.TabIndex = 7;
            this.rbProject.TabStop = true;
            this.rbProject.Text = "Project Folder";
            this.rbProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbProject.UseVisualStyleBackColor = true;
            // 
            // rbMarketing
            // 
            this.rbMarketing.AutoSize = true;
            this.rbMarketing.Location = new System.Drawing.Point(6, 42);
            this.rbMarketing.Name = "rbMarketing";
            this.rbMarketing.Size = new System.Drawing.Size(104, 17);
            this.rbMarketing.TabIndex = 6;
            this.rbMarketing.TabStop = true;
            this.rbMarketing.Text = "Marketing Folder";
            this.rbMarketing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbMarketing.UseVisualStyleBackColor = true;
            // 
            // rbProposal
            // 
            this.rbProposal.AutoSize = true;
            this.rbProposal.Location = new System.Drawing.Point(6, 19);
            this.rbProposal.Name = "rbProposal";
            this.rbProposal.Size = new System.Drawing.Size(98, 17);
            this.rbProposal.TabIndex = 5;
            this.rbProposal.TabStop = true;
            this.rbProposal.Text = "Proposal Folder";
            this.rbProposal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbProposal.UseVisualStyleBackColor = true;
            // 
            // lProposalStatus
            // 
            this.lProposalStatus.Location = new System.Drawing.Point(110, 19);
            this.lProposalStatus.Name = "lProposalStatus";
            this.lProposalStatus.Size = new System.Drawing.Size(124, 23);
            this.lProposalStatus.TabIndex = 1;
            this.lProposalStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbRemoveAfterExport
            // 
            this.cbRemoveAfterExport.AutoSize = true;
            this.cbRemoveAfterExport.Location = new System.Drawing.Point(12, 396);
            this.cbRemoveAfterExport.Name = "cbRemoveAfterExport";
            this.cbRemoveAfterExport.Size = new System.Drawing.Size(122, 17);
            this.cbRemoveAfterExport.TabIndex = 4;
            this.cbRemoveAfterExport.Text = "Remove after export";
            this.cbRemoveAfterExport.UseVisualStyleBackColor = true;
            // 
            // gbExportHistory
            // 
            this.gbExportHistory.Controls.Add(this.lbExportHistory);
            this.gbExportHistory.Location = new System.Drawing.Point(12, 143);
            this.gbExportHistory.Name = "gbExportHistory";
            this.gbExportHistory.Size = new System.Drawing.Size(402, 241);
            this.gbExportHistory.TabIndex = 5;
            this.gbExportHistory.TabStop = false;
            this.gbExportHistory.Text = "Export history";
            // 
            // lbExportHistory
            // 
            this.lbExportHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbExportHistory.FormattingEnabled = true;
            this.lbExportHistory.Location = new System.Drawing.Point(3, 16);
            this.lbExportHistory.Name = "lbExportHistory";
            this.lbExportHistory.Size = new System.Drawing.Size(396, 222);
            this.lbExportHistory.TabIndex = 0;
            this.lbExportHistory.SelectedIndexChanged += new System.EventHandler(this.lbExportHistory_SelectedIndexChanged);
            // 
            // bExport
            // 
            this.bExport.Location = new System.Drawing.Point(337, 390);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(75, 23);
            this.bExport.TabIndex = 6;
            this.bExport.Text = "Export";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // ExportMailsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 425);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.gbExportHistory);
            this.Controls.Add(this.cbRemoveAfterExport);
            this.Controls.Add(this.gbAvailableLocations);
            this.Controls.Add(this.bRefreshLocations);
            this.Controls.Add(this.tbProjectNumber);
            this.Controls.Add(this.lProjectNumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExportMailsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExportMailsWindow";
            this.gbAvailableLocations.ResumeLayout(false);
            this.gbAvailableLocations.PerformLayout();
            this.gbExportHistory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lProjectNumber;
        private System.Windows.Forms.TextBox tbProjectNumber;
        private System.Windows.Forms.Button bRefreshLocations;
        private System.Windows.Forms.GroupBox gbAvailableLocations;
        private System.Windows.Forms.Label lProposalStatus;
        private System.Windows.Forms.Label lProjectStatus;
        private System.Windows.Forms.Label lMarketingStatus;
        private System.Windows.Forms.RadioButton rbProject;
        private System.Windows.Forms.RadioButton rbMarketing;
        private System.Windows.Forms.RadioButton rbProposal;
        private System.Windows.Forms.CheckBox cbRemoveAfterExport;
        private System.Windows.Forms.Button bProjectView;
        private System.Windows.Forms.Button bProjectExport;
        private System.Windows.Forms.Button bMarketingView;
        private System.Windows.Forms.Button bMarketingExport;
        private System.Windows.Forms.Button bProposalView;
        private System.Windows.Forms.Button bProposalExport;
        private System.Windows.Forms.GroupBox gbExportHistory;
        private System.Windows.Forms.ListBox lbExportHistory;
        private System.Windows.Forms.Button bExport;
    }
}