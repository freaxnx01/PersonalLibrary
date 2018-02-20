using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Research.CommunityTechnologies.Treemap;

namespace WindowsFormsApplication1
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            //InitializeTreemap();
        }

        //protected void SetTreemapProperties(TreemapControl oTreemapControl)
        //{
        //    // All TreemapControl properties have default values that yield
        //    // reasonable results in many cases.  We want to change the
        //    // range of colors for this example.

        //    // Make Node.ColorMetric values of -200 to 200 map to a color
        //    // range between blue and yellow.

        //    oTreemapControl.MinColorMetric = -200F;
        //    oTreemapControl.MaxColorMetric = 200F;

        //    oTreemapControl.MinColor = Color.Blue;
        //    oTreemapControl.MaxColor = Color.Yellow;

        //    // (Set other properties that determine border width, spacing
        //    // between boxes, fonts, etc., if desired.)
        //}

        //// InitializeTreemap() should be called from the form's constructor.
        //protected void InitializeTreemap()
        //{
        //    // A TreemapControl has been placed on this form using the Visual
        //    // Studio designer.

        //    // Improve performance by turning off updating while the control is
        //    // being populated.

        //    oTreemapControl.BeginUpdate();
        //    PopulateTreemap(oTreemapControl);
        //    oTreemapControl.EndUpdate();

        //    SetTreemapProperties(oTreemapControl);
        //}

        //protected void PopulateTreemap(TreemapControl oTreemapControl)
        //{
        //    Nodes oNodes;
        //    Node oNode;
        //    Nodes oChildNodes;
        //    Node oChildNode;

        //    // Get the collection of top-level nodes.

        //    oNodes = oTreemapControl.Nodes;

        //    // Add a top-level node to the collection.

        //    oNode = oNodes.Add("Business Objects", 25F, -50F);

        //    // Add child nodes to the top-level node.

        //    oChildNodes = oNode.Nodes;
        //    //oChildNode = oChildNodes.Add("Artikel", 750, 2.5F);
        //    //oChildNode = oChildNodes.Add("SalesDoc", 314, -34.5F);

        //    Random rnd = new Random();
        //    for (int i = 0; i < 20; i++)
        //    {
        //        int metric = rnd.Next(500, 750);
        //        oChildNode = oChildNodes.Add("BO" + "(" + metric.ToString() + ")", metric, 2.5F);
        //        oChildNode.ToolTip = i.ToString();
        //    }

        //    for (int i = 0; i < 70; i++)
        //    {
        //        int metric = rnd.Next(4, 150);
        //        oChildNode = oChildNodes.Add("BO" + "(" + metric.ToString() + ")", metric, 2.5F);
        //        oChildNode.ToolTip = i.ToString();
        //    }

        //    // Add another top-level node.

        //    //oNode = oNodes.Add("Top Level 2", 50F, -40.1F);

        //    // Add child nodes to the second top-level node.

        //    //oChildNodes = oNode.Nodes;
        //    //oChildNode = oChildNodes.Add("Child 2-1", 61F, 0F);
        //    //oChildNode = oChildNodes.Add("Child 2-2", 100F, 200F);
        //    //oChildNode = oChildNodes.Add("Child 2-3", 100F, 200F);

        //    // (As an alternative to making multiple calls to the Nodes.Add
        //    // method, the control can be populated via an XML string
        //    // passed to the TreemapControl.NodesXml property.)
        //}

        //private void oTreemapControl_NodeDoubleClick(object sender, NodeEventArgs nodeEventArgs)
        //{
        //    MessageBox.Show(nodeEventArgs.Node.ToolTip);
        //}
    }
}
