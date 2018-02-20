using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class ZoomedFromOneTopLevelNodeAction : ZoomAction
	{
		protected Node m_oOriginalTopLevelNode;
		public ZoomedFromOneTopLevelNodeAction(ZoomActionHistoryList oZoomActionHistoryList, Node oZoomedNode, Node oOriginalTopLevelNode) : base(oZoomActionHistoryList, oZoomedNode)
		{
			this.m_oOriginalTopLevelNode = oOriginalTopLevelNode;
			this.AssertValid();
		}
		public override bool CanZoomOutFromZoomedNode()
		{
			this.AssertValid();
			return this.m_oParentOfZoomedNode != null;
		}
		public override void Undo(TreemapGenerator oTreemapGenerator)
		{
			this.AssertValid();
			base.Undo(oTreemapGenerator);
			oTreemapGenerator.Clear();
			oTreemapGenerator.Nodes.Add(this.m_oOriginalTopLevelNode);
		}
		public override void AssertValid()
		{
			base.AssertValid();
			Debug.Assert(this.m_oOriginalTopLevelNode != null);
		}
	}
}
