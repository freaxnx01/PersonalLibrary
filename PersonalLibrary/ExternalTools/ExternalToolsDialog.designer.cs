namespace Library.Gui.ExternalTools
{
    partial class ExternalToolsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExternalToolsDialog));
            this.listBoxTool = new System.Windows.Forms.ListBox();
            this.labelMenu = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelCommand = new System.Windows.Forms.Label();
            this.labelArguments = new System.Windows.Forms.Label();
            this.labelInitDirectory = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.buttonBrowseCommand = new System.Windows.Forms.Button();
            this.textBoxArguments = new System.Windows.Forms.TextBox();
            this.buttonSelectionArguments = new System.Windows.Forms.Button();
            this.textBoxInitialDirectory = new System.Windows.Forms.TextBox();
            this.buttonSelectionInitDirectory = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxPreview = new System.Windows.Forms.TextBox();
            this.labelPreview = new System.Windows.Forms.Label();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxTool
            // 
            this.listBoxTool.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxTool.FormattingEnabled = true;
            this.listBoxTool.Location = new System.Drawing.Point(15, 34);
            this.listBoxTool.Name = "listBoxTool";
            this.listBoxTool.Size = new System.Drawing.Size(338, 173);
            this.listBoxTool.TabIndex = 1;
            this.listBoxTool.SelectedIndexChanged += new System.EventHandler(this.listBoxTool_SelectedIndexChanged);
            // 
            // labelMenu
            // 
            this.labelMenu.AutoSize = true;
            this.labelMenu.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMenu.Location = new System.Drawing.Point(12, 18);
            this.labelMenu.Name = "labelMenu";
            this.labelMenu.Size = new System.Drawing.Size(94, 13);
            this.labelMenu.TabIndex = 0;
            this.labelMenu.Text = "Me&nu contents:";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(365, 34);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(105, 23);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "&Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDelete.Location = new System.Drawing.Point(365, 64);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(105, 23);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "&Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMoveUp.Location = new System.Drawing.Point(365, 155);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(105, 23);
            this.buttonMoveUp.TabIndex = 4;
            this.buttonMoveUp.Text = "Move &Up";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMoveDown.Location = new System.Drawing.Point(365, 184);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(105, 23);
            this.buttonMoveDown.TabIndex = 5;
            this.buttonMoveDown.Text = "Move Do&wn";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(13, 227);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(36, 13);
            this.labelTitle.TabIndex = 6;
            this.labelTitle.Text = "&Title:";
            // 
            // labelCommand
            // 
            this.labelCommand.AutoSize = true;
            this.labelCommand.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCommand.Location = new System.Drawing.Point(13, 254);
            this.labelCommand.Name = "labelCommand";
            this.labelCommand.Size = new System.Drawing.Size(71, 13);
            this.labelCommand.TabIndex = 8;
            this.labelCommand.Text = "&Command:";
            // 
            // labelArguments
            // 
            this.labelArguments.AutoSize = true;
            this.labelArguments.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelArguments.Location = new System.Drawing.Point(13, 281);
            this.labelArguments.Name = "labelArguments";
            this.labelArguments.Size = new System.Drawing.Size(74, 13);
            this.labelArguments.TabIndex = 11;
            this.labelArguments.Text = "A&rguments:";
            // 
            // labelInitDirectory
            // 
            this.labelInitDirectory.AutoSize = true;
            this.labelInitDirectory.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInitDirectory.Location = new System.Drawing.Point(13, 311);
            this.labelInitDirectory.Name = "labelInitDirectory";
            this.labelInitDirectory.Size = new System.Drawing.Size(99, 13);
            this.labelInitDirectory.TabIndex = 14;
            this.labelInitDirectory.Text = "&Initial directory:";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTitle.Location = new System.Drawing.Point(117, 224);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(354, 21);
            this.textBoxTitle.TabIndex = 7;
            this.textBoxTitle.TextChanged += new System.EventHandler(this.textBoxTitle_TextChanged);
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCommand.Location = new System.Drawing.Point(117, 251);
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.Size = new System.Drawing.Size(318, 21);
            this.textBoxCommand.TabIndex = 9;
            this.textBoxCommand.TextChanged += new System.EventHandler(this.textBoxCommand_TextChanged);
            this.textBoxCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCommand_KeyDown);
            // 
            // buttonBrowseCommand
            // 
            this.buttonBrowseCommand.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBrowseCommand.Location = new System.Drawing.Point(441, 251);
            this.buttonBrowseCommand.Name = "buttonBrowseCommand";
            this.buttonBrowseCommand.Size = new System.Drawing.Size(29, 21);
            this.buttonBrowseCommand.TabIndex = 10;
            this.buttonBrowseCommand.Text = "...";
            this.buttonBrowseCommand.UseVisualStyleBackColor = true;
            this.buttonBrowseCommand.Click += new System.EventHandler(this.buttonBrowseCommand_Click);
            // 
            // textBoxArguments
            // 
            this.textBoxArguments.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxArguments.Location = new System.Drawing.Point(117, 279);
            this.textBoxArguments.Name = "textBoxArguments";
            this.textBoxArguments.Size = new System.Drawing.Size(318, 21);
            this.textBoxArguments.TabIndex = 12;
            this.textBoxArguments.TextChanged += new System.EventHandler(this.textBoxArguments_TextChanged);
            this.textBoxArguments.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxArguments_KeyDown);
            // 
            // buttonSelectionArguments
            // 
            this.buttonSelectionArguments.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSelectionArguments.Image = ((System.Drawing.Image)(resources.GetObject("buttonSelectionArguments.Image")));
            this.buttonSelectionArguments.Location = new System.Drawing.Point(441, 279);
            this.buttonSelectionArguments.Name = "buttonSelectionArguments";
            this.buttonSelectionArguments.Size = new System.Drawing.Size(29, 21);
            this.buttonSelectionArguments.TabIndex = 13;
            this.buttonSelectionArguments.UseVisualStyleBackColor = true;
            this.buttonSelectionArguments.Click += new System.EventHandler(this.buttonSelectionArguments_Click);
            // 
            // textBoxInitialDirectory
            // 
            this.textBoxInitialDirectory.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxInitialDirectory.Location = new System.Drawing.Point(117, 309);
            this.textBoxInitialDirectory.Name = "textBoxInitialDirectory";
            this.textBoxInitialDirectory.Size = new System.Drawing.Size(318, 21);
            this.textBoxInitialDirectory.TabIndex = 15;
            this.textBoxInitialDirectory.TextChanged += new System.EventHandler(this.textBoxInitialDirectory_TextChanged);
            this.textBoxInitialDirectory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxInitialDirectory_KeyDown);
            // 
            // buttonSelectionInitDirectory
            // 
            this.buttonSelectionInitDirectory.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSelectionInitDirectory.Image = ((System.Drawing.Image)(resources.GetObject("buttonSelectionInitDirectory.Image")));
            this.buttonSelectionInitDirectory.Location = new System.Drawing.Point(441, 309);
            this.buttonSelectionInitDirectory.Name = "buttonSelectionInitDirectory";
            this.buttonSelectionInitDirectory.Size = new System.Drawing.Size(29, 21);
            this.buttonSelectionInitDirectory.TabIndex = 16;
            this.buttonSelectionInitDirectory.UseVisualStyleBackColor = true;
            this.buttonSelectionInitDirectory.Click += new System.EventHandler(this.buttonSelectionInitDirectory_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(366, 480);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(105, 23);
            this.buttonClose.TabIndex = 20;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxPreview
            // 
            this.textBoxPreview.Location = new System.Drawing.Point(16, 361);
            this.textBoxPreview.Multiline = true;
            this.textBoxPreview.Name = "textBoxPreview";
            this.textBoxPreview.ReadOnly = true;
            this.textBoxPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPreview.Size = new System.Drawing.Size(454, 100);
            this.textBoxPreview.TabIndex = 18;
            this.textBoxPreview.TextChanged += new System.EventHandler(this.textBoxPreview_TextChanged);
            // 
            // labelPreview
            // 
            this.labelPreview.AutoSize = true;
            this.labelPreview.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPreview.Location = new System.Drawing.Point(13, 345);
            this.labelPreview.Name = "labelPreview";
            this.labelPreview.Size = new System.Drawing.Size(57, 13);
            this.labelPreview.TabIndex = 17;
            this.labelPreview.Text = "&Preview:";
            // 
            // buttonExecute
            // 
            this.buttonExecute.Enabled = false;
            this.buttonExecute.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExecute.Location = new System.Drawing.Point(16, 467);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(105, 23);
            this.buttonExecute.TabIndex = 19;
            this.buttonExecute.Text = "E&xecute";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // ExternalToolsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 519);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.labelPreview);
            this.Controls.Add(this.textBoxPreview);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSelectionInitDirectory);
            this.Controls.Add(this.textBoxInitialDirectory);
            this.Controls.Add(this.buttonSelectionArguments);
            this.Controls.Add(this.textBoxArguments);
            this.Controls.Add(this.buttonBrowseCommand);
            this.Controls.Add(this.textBoxCommand);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.labelInitDirectory);
            this.Controls.Add(this.labelArguments);
            this.Controls.Add(this.labelCommand);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.labelMenu);
            this.Controls.Add(this.listBoxTool);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExternalToolsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "External Tools";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExternalToolsDialog_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxTool;
        private System.Windows.Forms.Label labelMenu;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelCommand;
        private System.Windows.Forms.Label labelArguments;
        private System.Windows.Forms.Label labelInitDirectory;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxCommand;
        private System.Windows.Forms.Button buttonBrowseCommand;
        private System.Windows.Forms.TextBox textBoxArguments;
        private System.Windows.Forms.Button buttonSelectionArguments;
        private System.Windows.Forms.TextBox textBoxInitialDirectory;
        private System.Windows.Forms.Button buttonSelectionInitDirectory;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxPreview;
        private System.Windows.Forms.Label labelPreview;
        private System.Windows.Forms.Button buttonExecute;
    }
}