using System.Windows.Forms;

namespace UILibrary
{
    public class BindableToolStripMenuItem : System.Windows.Forms.ToolStripMenuItem, IBindableComponent
    {
        #region IBindableComponent Members

        private BindingContext bindingContext;
        private ControlBindingsCollection dataBindings;

        public BindingContext BindingContext
        {
            get
            {
                if (bindingContext == null)
                {
                    bindingContext = new BindingContext();
                }
                return bindingContext;
            }
            set
            {
                bindingContext = value;
            }
        }

        public ControlBindingsCollection DataBindings
        {
            get
            {
                if (dataBindings == null)
                {
                    dataBindings = new ControlBindingsCollection(this);
                }
                return dataBindings;
            }
        }

        #endregion
    }
}
