using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace UILibrary
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripCheckBox : ToolStripControlHost
    {
        public event EventHandler CheckedChanged;

        public ToolStripCheckBox() : base(new CheckBox())
        {
            ((CheckBox)Control).TabStop = false;
        }

        public CheckBox CheckBoxControl
        {
            get { return Control as CheckBox; }
        }

        public bool Checked
        {
            get { return CheckBoxControl.Checked; }
            set { CheckBoxControl.Checked = value; }
        }

        public void OnCheckedChanged(object sender, EventArgs e)
        {
            if (CheckedChanged != null)
            {
                CheckedChanged(sender, e);
            }
        }

        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            (control as CheckBox).CheckedChanged += OnCheckedChanged;
        }

        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            (control as CheckBox).CheckedChanged -= OnCheckedChanged;
        }
    }
}
