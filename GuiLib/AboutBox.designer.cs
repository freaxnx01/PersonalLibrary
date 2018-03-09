namespace Library.Gui
{
    partial class AboutBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.labelApplication = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelBuilt = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.listViewInfo = new System.Windows.Forms.ListView();
            this.radioButtonViewInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonViewLicenseInfo = new System.Windows.Forms.RadioButton();
            this.richTextBoxLicenceInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // labelApplication
            // 
            this.labelApplication.AutoSize = true;
            this.labelApplication.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApplication.Location = new System.Drawing.Point(25, 25);
            this.labelApplication.Name = "labelApplication";
            this.labelApplication.Size = new System.Drawing.Size(111, 16);
            this.labelApplication.TabIndex = 1;
            this.labelApplication.Text = "<Application>";
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Location = new System.Drawing.Point(25, 66);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(81, 13);
            this.labelCopyright.TabIndex = 2;
            this.labelCopyright.Text = "<Copyright>";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(25, 108);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(67, 13);
            this.labelVersion.TabIndex = 3;
            this.labelVersion.Text = "<Version>";
            // 
            // labelBuilt
            // 
            this.labelBuilt.AutoSize = true;
            this.labelBuilt.Location = new System.Drawing.Point(25, 130);
            this.labelBuilt.Name = "labelBuilt";
            this.labelBuilt.Size = new System.Drawing.Size(50, 13);
            this.labelBuilt.TabIndex = 4;
            this.labelBuilt.Text = "<Built>";
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.SystemColors.Control;
            this.buttonClose.Location = new System.Drawing.Point(423, 384);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(89, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "&OK";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // listViewInfo
            // 
            this.listViewInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewInfo.Location = new System.Drawing.Point(28, 221);
            this.listViewInfo.Name = "listViewInfo";
            this.listViewInfo.Size = new System.Drawing.Size(484, 144);
            this.listViewInfo.TabIndex = 8;
            this.listViewInfo.UseCompatibleStateImageBehavior = false;
            // 
            // radioButtonViewInfo
            // 
            this.radioButtonViewInfo.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonViewInfo.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.radioButtonViewInfo.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.radioButtonViewInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonViewInfo.Location = new System.Drawing.Point(28, 190);
            this.radioButtonViewInfo.Name = "radioButtonViewInfo";
            this.radioButtonViewInfo.Size = new System.Drawing.Size(138, 25);
            this.radioButtonViewInfo.TabIndex = 9;
            this.radioButtonViewInfo.TabStop = true;
            this.radioButtonViewInfo.Text = "Info";
            this.radioButtonViewInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonViewInfo.UseVisualStyleBackColor = false;
            this.radioButtonViewInfo.CheckedChanged += new System.EventHandler(this.radioButtonViewInfo_CheckedChanged);
            // 
            // radioButtonViewLicenseInfo
            // 
            this.radioButtonViewLicenseInfo.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonViewLicenseInfo.AutoSize = true;
            this.radioButtonViewLicenseInfo.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.radioButtonViewLicenseInfo.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.radioButtonViewLicenseInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonViewLicenseInfo.Location = new System.Drawing.Point(172, 190);
            this.radioButtonViewLicenseInfo.Name = "radioButtonViewLicenseInfo";
            this.radioButtonViewLicenseInfo.Size = new System.Drawing.Size(138, 25);
            this.radioButtonViewLicenseInfo.TabIndex = 10;
            this.radioButtonViewLicenseInfo.TabStop = true;
            this.radioButtonViewLicenseInfo.Text = "Lizenzbestimmungen";
            this.radioButtonViewLicenseInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonViewLicenseInfo.UseVisualStyleBackColor = false;
            this.radioButtonViewLicenseInfo.CheckedChanged += new System.EventHandler(this.radioButtonViewLicenseInfo_CheckedChanged);
            // 
            // richTextBoxLicenceInfo
            // 
            this.richTextBoxLicenceInfo.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxLicenceInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxLicenceInfo.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLicenceInfo.Location = new System.Drawing.Point(28, 221);
            this.richTextBoxLicenceInfo.Name = "richTextBoxLicenceInfo";
            this.richTextBoxLicenceInfo.ReadOnly = true;
            this.richTextBoxLicenceInfo.Size = new System.Drawing.Size(484, 144);
            this.richTextBoxLicenceInfo.TabIndex = 11;
            this.richTextBoxLicenceInfo.Text = "";
            this.richTextBoxLicenceInfo.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxLicenceInfo_LinkClicked);
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(540, 430);
            this.Controls.Add(this.listViewInfo);
            this.Controls.Add(this.radioButtonViewLicenseInfo);
            this.Controls.Add(this.radioButtonViewInfo);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelBuilt);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.labelApplication);
            this.Controls.Add(this.richTextBoxLicenceInfo);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Info";
            this.Load += new System.EventHandler(this.AboutBox_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AboutBox_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelApplication;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelBuilt;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ListView listViewInfo;
        private System.Windows.Forms.RadioButton radioButtonViewInfo;
        private System.Windows.Forms.RadioButton radioButtonViewLicenseInfo;
        private System.Windows.Forms.RichTextBox richTextBoxLicenceInfo;
    }
}