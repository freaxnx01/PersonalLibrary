using System;
using System.Diagnostics;
using System.Drawing;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	public class TreemapDrawItemEventArgs : EventArgs
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
		public Rectangle Bounds
		{
			get
			{
				this.AssertValid();
				return this.m_oNode.RectangleToDraw;
			}
		}
		public int PenWidthPx
		{
			get
			{
				this.AssertValid();
				return this.m_oNode.PenWidthPx;
			}
		}
		protected internal TreemapDrawItemEventArgs(Node oNode)
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
