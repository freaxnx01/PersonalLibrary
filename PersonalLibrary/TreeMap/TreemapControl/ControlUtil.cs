using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace Microsoft.Research.CommunityTechnologies.ControlLib
{
	public class ControlUtil
	{
		private ControlUtil()
		{
		}
		public static Point GetClientMousePosition(Control oControl)
		{
			Debug.Assert(oControl != null);
			Point mousePosition = Control.MousePosition;
			return oControl.PointToClient(mousePosition);
		}
	}
}
