using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class ZoomActionHistoryList : HistoryList
	{
		protected Node[] m_aoOriginalTopLevelNodes;
		protected float m_fOriginalTopLevelEmptySpaceSizeMetric;
		public bool HasCurrentState
		{
			get
			{
				this.AssertValid();
				return this.m_iCurrentObjectIndex >= 0;
			}
		}
		public ZoomAction PeekCurrentState
		{
			get
			{
				this.AssertValid();
				if (!this.HasCurrentState)
				{
					throw new InvalidOperationException("ZoomActionHistoryList.PeekCurrentState: There is no current state.  Check HasCurrentState before calling this.");
				}
				Debug.Assert(this.m_oStateList[this.m_iCurrentObjectIndex] is ZoomAction);
				return (ZoomAction)this.m_oStateList[this.m_iCurrentObjectIndex];
			}
		}
		public ZoomAction CurrentState
		{
			get
			{
				this.AssertValid();
				if (!this.HasCurrentState)
				{
					throw new InvalidOperationException("ZoomActionHistoryList.CurrentState: There is no current state.  Check HasCurrentState before calling this.");
				}
				object obj = this.m_oStateList[this.m_iCurrentObjectIndex];
				this.m_iCurrentObjectIndex--;
				this.AssertValid();
				base.FireChangeEvent();
				Debug.Assert(obj is ZoomAction);
				return (ZoomAction)obj;
			}
		}
		public Node[] OriginalTopLevelNodes
		{
			get
			{
				this.AssertValid();
				Debug.Assert(this.m_aoOriginalTopLevelNodes != null);
				Debug.Assert(this.m_aoOriginalTopLevelNodes.Length > 0);
				Debug.Assert(this.m_fOriginalTopLevelEmptySpaceSizeMetric != -3.40282347E+38f);
				return this.m_aoOriginalTopLevelNodes;
			}
		}
		public float OriginalTopLevelEmptySpaceSizeMetric
		{
			get
			{
				this.AssertValid();
				Debug.Assert(this.m_aoOriginalTopLevelNodes != null);
				Debug.Assert(this.m_aoOriginalTopLevelNodes.Length > 0);
				Debug.Assert(this.m_fOriginalTopLevelEmptySpaceSizeMetric != -3.40282347E+38f);
				return this.m_fOriginalTopLevelEmptySpaceSizeMetric;
			}
		}
		public ZoomActionHistoryList()
		{
			this.m_aoOriginalTopLevelNodes = null;
			this.m_fOriginalTopLevelEmptySpaceSizeMetric = -3.40282347E+38f;
			this.AssertValid();
		}
		public void SetOriginalTopLevelInfo(Node[] aoOriginalTopLevelNodes, float fOriginalTopLevelEmptySpaceSizeMetric)
		{
			this.m_aoOriginalTopLevelNodes = aoOriginalTopLevelNodes;
			this.m_fOriginalTopLevelEmptySpaceSizeMetric = fOriginalTopLevelEmptySpaceSizeMetric;
			this.AssertValid();
		}
		public void RedoOriginalTopLevel(TreemapGenerator oTreemapGenerator)
		{
			Debug.Assert(oTreemapGenerator != null);
			this.AssertValid();
			oTreemapGenerator.Clear();
			Nodes nodes = oTreemapGenerator.Nodes;
			Debug.Assert(this.m_aoOriginalTopLevelNodes != null);
			Debug.Assert(this.m_aoOriginalTopLevelNodes.Length > 0);
			Debug.Assert(this.m_fOriginalTopLevelEmptySpaceSizeMetric != -3.40282347E+38f);
			oTreemapGenerator.BeginUpdate();
			Node[] aoOriginalTopLevelNodes = this.m_aoOriginalTopLevelNodes;
			for (int i = 0; i < aoOriginalTopLevelNodes.Length; i++)
			{
				Node node = aoOriginalTopLevelNodes[i];
				nodes.Add(node);
			}
			nodes.EmptySpace.SizeMetric = this.m_fOriginalTopLevelEmptySpaceSizeMetric;
			oTreemapGenerator.EndUpdate();
		}
		public void Reset()
		{
			base.Reset();
			this.m_aoOriginalTopLevelNodes = null;
			this.m_fOriginalTopLevelEmptySpaceSizeMetric = -3.40282347E+38f;
			this.AssertValid();
		}
		public void AssertValid()
		{
			base.AssertValid();
			if (this.m_aoOriginalTopLevelNodes != null)
			{
				Debug.Assert(this.m_aoOriginalTopLevelNodes.Length > 0);
				Debug.Assert(this.m_fOriginalTopLevelEmptySpaceSizeMetric != -3.40282347E+38f);
			}
		}
	}
}
