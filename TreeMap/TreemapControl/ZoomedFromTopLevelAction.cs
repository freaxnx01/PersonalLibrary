using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class ZoomedFromTopLevelAction : ZoomAction
	{
		public ZoomedFromTopLevelAction(ZoomActionHistoryList oZoomActionHistoryList, Node oZoomedNode, Nodes oOriginalTopLevelNodes) : base(oZoomActionHistoryList, oZoomedNode)
		{
			Debug.Assert(oZoomedNode != null);
			Debug.Assert(oOriginalTopLevelNodes != null);
			oZoomActionHistoryList.SetOriginalTopLevelInfo(oOriginalTopLevelNodes.ToArray(), oOriginalTopLevelNodes.EmptySpace.SizeMetric);
			this.AssertValid();
		}
		public override bool CanZoomOutFromZoomedNode()
		{
			this.AssertValid();
			return true;
		}
		public override void Undo(TreemapGenerator oTreemapGenerator)
		{
			this.AssertValid();
			base.Undo(oTreemapGenerator);
			this.m_oZoomActionHistoryList.RedoOriginalTopLevel(oTreemapGenerator);
		}
		public override void AssertValid()
		{
			base.AssertValid();
			Node[] originalTopLevelNodes = this.m_oZoomActionHistoryList.OriginalTopLevelNodes;
			float originalTopLevelEmptySpaceSizeMetric = this.m_oZoomActionHistoryList.OriginalTopLevelEmptySpaceSizeMetric;
			Debug.Assert(originalTopLevelNodes != null);
			Debug.Assert(originalTopLevelNodes.Length > 0);
			Debug.Assert(originalTopLevelEmptySpaceSizeMetric != -3.40282347E+38f);
		}
	}
}
