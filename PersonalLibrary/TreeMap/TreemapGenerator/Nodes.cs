using Microsoft.Research.CommunityTechnologies.TreemapNoDoc;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	public class Nodes : IEnumerable
	{
		protected TreemapGenerator m_oTreemapGenerator;
		protected Node m_oParentNode;
		protected ArrayList m_oNodes;
		protected EmptySpace m_oEmptySpace;
		public int Count
		{
			get
			{
				this.AssertValid();
				return this.m_oNodes.Count;
			}
		}
		public int RecursiveCount
		{
			get
			{
				int num = 0;
				IEnumerator enumerator = this.m_oNodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Node node = (Node)enumerator.Current;
						num += 1 + node.Nodes.RecursiveCount;
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				return num;
			}
		}
		public Node this[int zeroBasedIndex]
		{
			get
			{
				this.AssertValid();
				int count = this.m_oNodes.Count;
				if (count == 0)
				{
					throw new InvalidOperationException("Nodes[]: There are no nodes in the collection.");
				}
				if (zeroBasedIndex < 0 || zeroBasedIndex >= count)
				{
					throw new ArgumentOutOfRangeException("zeroBasedIndex", zeroBasedIndex, "Nodes[]: zeroBasedIndex must be between 0 and Nodes.Count-1.");
				}
				return (Node)this.m_oNodes[zeroBasedIndex];
			}
		}
		public EmptySpace EmptySpace
		{
			get
			{
				this.AssertValid();
				return this.m_oEmptySpace;
			}
		}
		protected internal TreemapGenerator TreemapGenerator
		{
			set
			{
				this.m_oTreemapGenerator = value;
				IEnumerator enumerator = this.m_oNodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Node node = (Node)enumerator.Current;
						node.TreemapGenerator = value;
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				this.m_oEmptySpace.TreemapGenerator = value;
				this.AssertValid();
			}
		}
		protected internal Nodes(Node oParentNode)
		{
			this.Initialize(oParentNode);
		}
		public Node Add(string text, float sizeMetric, float colorMetric)
		{
			Node.ValidateSizeMetric(sizeMetric, "Nodes.Add()");
			Node.ValidateColorMetric(colorMetric, "Nodes.Add()");
			return this.Add(new Node(text, sizeMetric, colorMetric));
		}
		public Node Add(string text, float sizeMetric, Color absoluteColor)
		{
			Node.ValidateSizeMetric(sizeMetric, "Nodes.Add()");
			return this.Add(new Node(text, sizeMetric, absoluteColor));
		}
		public Node Add(string text, float sizeMetric, float colorMetric, object tag)
		{
			Node node = this.Add(text, sizeMetric, colorMetric);
			node.Tag = tag;
			this.AssertValid();
			return node;
		}
		public Node Add(string text, float sizeMetric, float colorMetric, object tag, string toolTip)
		{
			Node node = this.Add(text, sizeMetric, colorMetric);
			node.Tag = tag;
			node.ToolTip = toolTip;
			this.AssertValid();
			return node;
		}
		public Node Add(Node node)
		{
			this.m_oNodes.Add(node);
			node.SetParent(this.m_oParentNode);
			if (this.m_oTreemapGenerator != null)
			{
				node.TreemapGenerator = this.m_oTreemapGenerator;
			}
			this.FireRedrawRequired();
			this.AssertValid();
			return node;
		}
		public Node[] ToArray()
		{
			Node[] array = new Node[this.m_oNodes.Count];
			this.m_oNodes.CopyTo(array);
			return array;
		}
		public NodesEnumerator GetEnumerator()
		{
			this.AssertValid();
			return new NodesEnumerator(this);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			this.AssertValid();
			return new NodesEnumerator(this);
		}
		protected void Initialize(Node oParentNode)
		{
			this.m_oTreemapGenerator = null;
			this.m_oParentNode = oParentNode;
			this.m_oNodes = new ArrayList();
			this.m_oEmptySpace = new EmptySpace();
		}
		protected internal void Clear()
		{
			this.AssertValid();
			this.m_oNodes.Clear();
			this.m_oEmptySpace = new EmptySpace();
		}
		protected internal bool GetNodeFromPoint(PointF oPointF, out Node oNode)
		{
			IEnumerator enumerator = this.m_oNodes.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					Node node = (Node)enumerator.Current;
					if (node.GetNodeFromPoint(oPointF, out oNode))
					{
						result = true;
						return result;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			oNode = null;
			result = false;
			return result;
		}
		protected internal Node[] ToArraySortedBySizeMetric()
		{
			Node[] array = new Node[this.m_oNodes.Count];
			this.m_oNodes.CopyTo(array);
			Array.Sort(array);
			return array;
		}
		protected void FireRedrawRequired()
		{
			if (this.m_oTreemapGenerator != null)
			{
				this.m_oTreemapGenerator.FireRedrawRequired();
			}
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_oNodes != null);
			Debug.Assert(this.m_oEmptySpace != null);
			this.m_oEmptySpace.AssertValid();
		}
	}
}
