namespace Library.Gui
{
    partial class InputBoxDialog
    {
        private System.Windows.Forms.Label labelPrompt;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxInput;

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
            this.labelPrompt = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelPrompt
            // 
            this.labelPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPrompt.BackColor = System.Drawing.SystemColors.Control;
            this.labelPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelPrompt.Location = new System.Drawing.Point(5, 9);
            this.labelPrompt.Name = "labelPrompt";
            this.labelPrompt.Size = new System.Drawing.Size(302, 82);
            this.labelPrompt.TabIndex = 3;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(323, 9);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(64, 24);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "&OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(323, 39);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(64, 24);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(8, 100);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(379, 20);
            this.textBoxInput.TabIndex = 0;
            this.textBoxInput.Enter += new System.EventHandler(this.textBoxInput_Enter);
            // 
            // InputBoxDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(398, 128);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelPrompt);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBoxDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InputBox";
            this.Load += new System.EventHandler(this.InputBoxDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}