using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UILibrary
{
    public class Dialog
    {
        public static void ShowTextInDialog(string text)
        {
            ShowTextInDialog(Application.ProductName, text);
        }

        public static void ShowTextInDialog(string dialogTitle, string text)
        {
            using (Form showTextForm = new Form())
            {
                TextBox textBoxShow = new TextBox();
                textBoxShow.Dock = DockStyle.Fill;
                textBoxShow.Text = text;
                textBoxShow.Multiline = true;
                textBoxShow.ScrollBars = ScrollBars.Both;

                showTextForm.Controls.Add(textBoxShow);
                showTextForm.Text = dialogTitle;
                showTextForm.StartPosition = FormStartPosition.CenterScreen;
                showTextForm.KeyPreview = true;
                showTextForm.KeyDown += new KeyEventHandler(delegate(object sender, KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        ((Form)sender).Close();
                    }
                });
                showTextForm.ShowDialog();
            }
        }
    }
}
