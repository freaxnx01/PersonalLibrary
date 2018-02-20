using System;
using System.Diagnostics;
using System.Windows.Forms;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	public class NodeMouseEventArgs : MouseEventArgs
	{
		private Node m_oNode;
		public Node Node
		{
			get
			{
				this.AssertValid();
				return this.m_oNode;
			}
		}
		protected internal NodeMouseEventArgs(MouseEventArgs oMouseEventArgs, Node oNode) : base(oMouseEventArgs.Button, oMouseEventArgs.Clicks, oMouseEventArgs.X, oMouseEventArgs.Y, oMouseEventArgs.Delta)
		{
			this.m_oNode = oNode;
			this.AssertValid();
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_oNode != null);
			this.m_oNode.AssertValid();
		}
	}
}
