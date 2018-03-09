using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Library.Gui
{
    public partial class InputBoxDialog : Form
    {
        public string Title { get; set; }
        public string Prompt { get; set; }
        public string UserInput { get; private set; }
        public string DefaultValue { get; set; }

        private const string DEFAULT_PROMPT = "Please enter a value:";

        public InputBoxDialog()
        {
            InitializeComponent();
        }

        private void InputBoxDialog_Load(object sender, EventArgs e)
        {
            textBoxInput.Text = DefaultValue;
            SetTitle();
            SetPrompt();
        }

        private void SetTitle()
        {
            if (string.IsNullOrEmpty(Title))
            {
                Text = Application.ProductName;
            }
            else
            {
                Text = Title;
            }
        }

        private void SetPrompt()
        {
            if (string.IsNullOrEmpty(Prompt))
            {
                labelPrompt.Text = DEFAULT_PROMPT;
            }
            else
            {
                labelPrompt.Text = Prompt;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            UserInput = textBoxInput.Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBoxInput_Enter(object sender, EventArgs e)
        {
            textBoxInput.SelectionStart = 0;
            textBoxInput.SelectionLength = textBoxInput.Text.Length;
        }

        public static string InputBox(string prompt)
        {
            return InputBox(prompt, string.Empty, string.Empty);
        }

        public static string InputBox(string prompt, string defaultValue)
        {
            return InputBox(prompt, string.Empty, defaultValue);
        }

        public static string InputBox(string prompt, string title, string defaultValue)
        {
            string userInput = string.Empty;
            
            using (InputBoxDialog dialog = new InputBoxDialog())
            {
                dialog.Prompt = prompt;
                dialog.SetPrompt();
                
                dialog.Title = title;
                dialog.SetTitle();

                dialog.DefaultValue = defaultValue;
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    userInput = dialog.UserInput;
                }
            }
            
            return userInput;
        }
    }
}
