using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Collections;
using System.Diagnostics;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class NodesEnumerator : IEnumerator
	{
		protected Nodes m_oNodes;
		protected int m_iZeroBasedIndex;
		object IEnumerator.Current
		{
			get
			{
				this.AssertValid();
				return this.m_oNodes[this.m_iZeroBasedIndex];
			}
		}
		public Node Current
		{
			get
			{
				this.AssertValid();
				return this.m_oNodes[this.m_iZeroBasedIndex];
			}
		}
		public NodesEnumerator(Nodes nodes)
		{
			nodes.AssertValid();
			this.m_iZeroBasedIndex = -1;
			this.m_oNodes = nodes;
		}
		public bool MoveNext()
		{
			this.AssertValid();
			bool result;
			if (this.m_iZeroBasedIndex < this.m_oNodes.Count - 1)
			{
				this.m_iZeroBasedIndex++;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
		public void Reset()
		{
			this.AssertValid();
			this.m_iZeroBasedIndex = -1;
		}
		[Conditional("DEBUG")]
		protected internal void AssertValid()
		{
			Debug.Assert(this.m_oNodes != null);
			this.m_oNodes.AssertValid();
			Debug.Assert(this.m_iZeroBasedIndex >= -1);
			Debug.Assert(this.m_iZeroBasedIndex <= this.m_oNodes.Count);
		}
	}
}
