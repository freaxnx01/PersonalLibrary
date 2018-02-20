using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class ZoomedFromInnerNodeAction : ZoomAction
	{
		protected Node m_oInnerNode;
		public ZoomedFromInnerNodeAction(ZoomActionHistoryList oZoomActionHistoryList, Node oZoomedNode, Node oInnerNode) : base(oZoomActionHistoryList, oZoomedNode)
		{
			this.m_oInnerNode = oInnerNode;
			this.AssertValid();
		}
		public override bool CanZoomOutFromZoomedNode()
		{
			this.AssertValid();
			return this.m_oParentOfZoomedNode != null || this.m_oZoomActionHistoryList.OriginalTopLevelNodes.Length != 1;
		}
		public override void Undo(TreemapGenerator oTreemapGenerator)
		{
			this.AssertValid();
			base.Undo(oTreemapGenerator);
			oTreemapGenerator.Clear();
			oTreemapGenerator.Nodes.Add(this.m_oInnerNode);
		}
		public override void AssertValid()
		{
			base.AssertValid();
			Debug.Assert(this.m_oInnerNode != null);
		}
	}
}
