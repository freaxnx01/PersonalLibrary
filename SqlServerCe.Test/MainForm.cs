using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SqlServerCe.Test.EntitySplitting;
using System.IO;

namespace SqlServerCe.Test
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            using (ModelEntitySplittingContext ctx = new ModelEntitySplittingContext())
            {
                Article article = ctx.Articles.FirstOrDefault();

                this.Text = article.ArticleNo;

                textBox1.Text = article.Text;
                richTextBox1.Rtf = article.TextFormatted;

                //MemoryStream ms = new MemoryStream(article.Photo);
                //pictureBox1.Image = Image.FromStream(ms);
                pictureBox1.Image = ImageHelper.FromByteArray(article.Photo);

                toolTip1.SetToolTip(pictureBox1, article.Photo.LongLength.ToString());
            }
        }

        private void MainForm_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.Rtf);
        }
    }
}
