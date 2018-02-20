using System;
using System.Diagnostics;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	public class NodeEventArgs : EventArgs
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
		protected internal NodeEventArgs(Node oNode)
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
