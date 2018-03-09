using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Library.Gui
{
    public static class TextBoxHelper
    {
        #region SetSelectAllOnGotFocus
        public static void SetSelectAllOnGotFocus(Form form)
        {
            SetSelectAllOnGotFocus(form.Controls);
        }

        private static void SetSelectAllOnGotFocus(Control.ControlCollection controlCollection)
        {
            foreach (Control control in controlCollection)
            {
                if (control is TextBox)
                {
                    control.GotFocus += delegate(object sender, EventArgs e)
                    {
                        ((TextBox)sender).SelectAll();
                    };
                }

                // rekursiv Controls-Auflistung durchlaufen
                if (control.Controls.Count > 0)
                {
                    SetSelectAllOnGotFocus(control.Controls);
                }
            }
        }
        #endregion
    }
}
