using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace ObjectGraphML.Designer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // aus StringKing-Projekt
        //    RenderToolStrip(menuStripMain, @"Definitions\MenuStripMain.xml", commands);
        //    RenderToolStrip(toolStripMain, @"Definitions\ToolStripMain.xml", commands);

        //private static void RenderToolStrip(ToolStrip toolStrip, string xmlDefinitionFile, Dictionary<string, ICommand> commandDictionary)
        //{
        //    toolStrip.Items.Clear();
        //    var renderEngine = new Engine(commandDictionary);
        //    renderEngine.RenderControl(toolStrip, xmlDefinitionFile, toolStrip.Parent);
        //}

        private void RenderML()
        {
            string xml = richTextBoxML.Text;

            Control targetContainer = splitContainerRight.Panel1;
            targetContainer.Controls.Clear();

            //-
            Environment.CurrentDirectory = @"C:\Work\PersonalThings\ima\Tools\StringKing\StringKingUI\bin\Debug";

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            try
            {
                Engine engine = new Engine();
                object rootObject = engine.GetInstanceOfRootObject(xml);
                Control rootControl = rootObject as Control;

                engine.RenderControlFromXml(rootControl, xml, this);
                targetContainer.Controls.Add(rootControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}:\n{1}", ex.GetType().Name, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //-
            if (args.Name == "StringKing")
            {
                return Assembly.LoadFile(@"C:\Work\PersonalThings\ima\Tools\StringKing\StringKingUI\bin\Debug\StringKing.exe");
            }
            
            throw new NotImplementedException();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderML();
        }
    }
}
