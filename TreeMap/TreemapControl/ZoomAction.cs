using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public abstract class ZoomAction
	{
		protected ZoomActionHistoryList m_oZoomActionHistoryList;
		protected Node m_oZoomedNode;
		protected Node m_oParentOfZoomedNode;
		public Node ParentOfZoomedNode
		{
			get
			{
				this.AssertValid();
				return this.m_oParentOfZoomedNode;
			}
		}
		protected ZoomAction(ZoomActionHistoryList oZoomActionHistoryList, Node oZoomedNode)
		{
			this.m_oZoomActionHistoryList = oZoomActionHistoryList;
			this.m_oZoomedNode = oZoomedNode;
			if (this.m_oZoomedNode != null)
			{
				this.m_oParentOfZoomedNode = oZoomedNode.Parent;
			}
			else
			{
				this.m_oParentOfZoomedNode = null;
			}
		}
		public abstract bool CanZoomOutFromZoomedNode();
		public virtual void Undo(TreemapGenerator oTreemapGenerator)
		{
			this.AssertValid();
			if (this.m_oParentOfZoomedNode != null)
			{
				Debug.Assert(oTreemapGenerator.Nodes.Count == 1);
				oTreemapGenerator.Nodes[0].PrivateSetParent(this.m_oParentOfZoomedNode);
			}
		}
		public void Redo(TreemapGenerator oTreemapGenerator)
		{
			this.AssertValid();
			Nodes nodes = oTreemapGenerator.Nodes;
			if (this.m_oZoomedNode == null)
			{
				this.m_oZoomActionHistoryList.RedoOriginalTopLevel(oTreemapGenerator);
			}
			else
			{
				oTreemapGenerator.Clear();
				nodes.Add(this.m_oZoomedNode);
			}
		}
		[Conditional("DEBUG")]
		public virtual void AssertValid()
		{
			Debug.Assert(this.m_oZoomActionHistoryList != null);
		}
	}
}
