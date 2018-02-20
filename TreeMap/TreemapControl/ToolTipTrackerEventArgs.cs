using System;
using System.Diagnostics;
namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	internal class ToolTipTrackerEventArgs : EventArgs
	{
		private object m_oObject;
		public object Object
		{
			get
			{
				this.AssertValid();
				return this.m_oObject;
			}
		}
		public ToolTipTrackerEventArgs(object oObject)
		{
			this.m_oObject = oObject;
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_oObject != null);
		}
	}
}
