using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using System;
using System.Diagnostics;
using System.Drawing;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	public class Node : IComparable
	{
		protected TreemapGenerator m_oTreemapGenerator;
		protected Node m_oParentNode;
		protected string m_sText;
		protected float m_fSizeMetric;
		private NodeColor m_oNodeColor;
		protected object m_oTag;
		protected string m_sToolTip;
		protected Nodes m_oNodes;
		protected RectangleF m_oRectangle;
		protected RectangleF m_oSavedRectangle;
		protected int m_iPenWidthPx;
		protected bool m_bRectangleSet;
		protected bool m_bRectangleSaved;
		public string Text
		{
			get
			{
				this.AssertValid();
				return this.m_sText;
			}
			set
			{
				if (this.m_sText != value)
				{
					this.m_sText = value;
					this.FireRedrawRequired();
				}
				this.AssertValid();
			}
		}
		public float SizeMetric
		{
			get
			{
				this.AssertValid();
				return this.m_fSizeMetric;
			}
			set
			{
				Node.ValidateSizeMetric(value, "Node.SizeMetric");
				if (this.m_fSizeMetric != value)
				{
					this.m_fSizeMetric = value;
					this.FireRedrawRequired();
				}
				this.AssertValid();
			}
		}
		public float ColorMetric
		{
			get
			{
				this.AssertValid();
				return this.m_oNodeColor.ColorMetric;
			}
			set
			{
				Node.ValidateColorMetric(value, "Node.ColorMetric");
				if (this.m_oNodeColor.ColorMetric != value)
				{
					this.m_oNodeColor.ColorMetric = value;
					this.FireRedrawRequired();
				}
				this.AssertValid();
			}
		}
		public Color AbsoluteColor
		{
			get
			{
				this.AssertValid();
				return this.m_oNodeColor.AbsoluteColor;
			}
			set
			{
				if (this.m_oNodeColor.AbsoluteColor != value)
				{
					this.m_oNodeColor.AbsoluteColor = value;
					this.FireRedrawRequired();
				}
				this.AssertValid();
			}
		}
		public object Tag
		{
			get
			{
				this.AssertValid();
				return this.m_oTag;
			}
			set
			{
				this.m_oTag = value;
				this.AssertValid();
			}
		}
		public string ToolTip
		{
			get
			{
				this.AssertValid();
				return this.m_sToolTip;
			}
			set
			{
				this.m_sToolTip = value;
				this.AssertValid();
			}
		}
		public Nodes Nodes
		{
			get
			{
				this.AssertValid();
				return this.m_oNodes;
			}
		}
		public Node Parent
		{
			get
			{
				this.AssertValid();
				return this.m_oParentNode;
			}
		}
		public int Level
		{
			get
			{
				this.AssertValid();
				Node parent = this.Parent;
				int num = 0;
				while (parent != null)
				{
					parent = parent.Parent;
					num++;
				}
				return num;
			}
		}
		protected internal bool HasBeenDrawn
		{
			get
			{
				this.AssertValid();
				bool result;
				if (!this.m_oRectangle.IsEmpty)
				{
					Debug.Assert(this.m_bRectangleSet);
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
		protected internal RectangleF Rectangle
		{
			get
			{
				this.AssertValid();
				Debug.Assert(this.m_bRectangleSet);
				return this.m_oRectangle;
			}
			set
			{
				this.m_oRectangle = value;
				this.m_bRectangleSet = true;
				this.AssertValid();
			}
		}
		protected internal Rectangle RectangleToDraw
		{
			get
			{
				this.AssertValid();
				return GraphicsUtil.RectangleFToRectangle(this.Rectangle, this.PenWidthPx);
			}
		}
		protected internal double AspectRatio
		{
			get
			{
				this.AssertValid();
				Debug.Assert(this.m_bRectangleSet);
				float width = this.m_oRectangle.Width;
				float height = this.m_oRectangle.Height;
				double result;
				if (width > height)
				{
					if (height == 0f)
					{
						result = 1.7976931348623157E+308;
					}
					else
					{
						result = (double)(width / height);
					}
				}
				else
				{
					if (width == 0f)
					{
						result = 1.7976931348623157E+308;
					}
					else
					{
						result = (double)(height / width);
					}
				}
				return result;
			}
		}
		protected internal int PenWidthPx
		{
			get
			{
				this.AssertValid();
				Debug.Assert(this.m_iPenWidthPx != -1);
				return this.m_iPenWidthPx;
			}
			set
			{
				this.m_iPenWidthPx = value;
				this.AssertValid();
			}
		}
		protected internal TreemapGenerator TreemapGenerator
		{
			set
			{
				this.m_oTreemapGenerator = value;
				this.m_oNodes.TreemapGenerator = value;
				this.AssertValid();
			}
		}
		public Node(string text, float sizeMetric, float colorMetric)
		{
			this.InitializeWithValidation(text, sizeMetric, colorMetric);
			this.AssertValid();
		}
		public Node(string text, float sizeMetric, Color absoluteColor)
		{
			this.InitializeWithValidation(text, sizeMetric, 0f);
			this.m_oNodeColor.AbsoluteColor = absoluteColor;
			this.AssertValid();
		}
		public Node(string text, float sizeMetric, float colorMetric, object tag)
		{
			this.InitializeWithValidation(text, sizeMetric, colorMetric);
			this.m_oTag = tag;
			this.AssertValid();
		}
		public Node(string text, float sizeMetric, float colorMetric, object tag, string toolTip)
		{
			this.InitializeWithValidation(text, sizeMetric, colorMetric);
			this.m_oTag = tag;
			this.m_sToolTip = toolTip;
			this.AssertValid();
		}
		public int CompareTo(object otherNode)
		{
			this.AssertValid();
			return -this.m_fSizeMetric.CompareTo(((Node)otherNode).m_fSizeMetric);
		}
		public override string ToString()
		{
			this.AssertValid();
			return string.Format("Node object: Text=\"{0}\",  SizeMetric={1}, Tag={2}, Rectangle={{L={3}, R={4}, T={5}, B={6},  W={7}, H={8}}}, Size={9}", new object[]
			{
				this.m_sText, 
				this.m_fSizeMetric, 
				(this.m_oTag == null) ? "null" : this.m_oTag.ToString(), 
				this.m_oRectangle.Left, 
				this.m_oRectangle.Right, 
				this.m_oRectangle.Top, 
				this.m_oRectangle.Bottom, 
				this.m_oRectangle.Width, 
				this.m_oRectangle.Height, 
				this.m_oRectangle.Width * this.m_oRectangle.Height
			});
		}
		public void PrivateSetParent(Node oParentNode)
		{
			this.SetParent(oParentNode);
			this.AssertValid();
		}
		protected void InitializeWithValidation(string sText, float fSizeMetric, float fColorMetric)
		{
			Node.ValidateSizeMetric(fSizeMetric, "Node");
			Node.ValidateColorMetric(fColorMetric, "Node");
			this.m_oTreemapGenerator = null;
			this.m_oParentNode = null;
			this.m_sText = sText;
			this.m_fSizeMetric = fSizeMetric;
			this.m_oNodeColor = new NodeColor(fColorMetric);
			this.m_oTag = null;
			this.m_sToolTip = null;
			this.m_oNodes = new Nodes(this);
			this.m_iPenWidthPx = -1;
			this.m_bRectangleSet = false;
			this.m_bRectangleSaved = false;
		}
		protected internal void SetParent(Node oParentNode)
		{
			Debug.Assert(oParentNode != this);
			this.m_oParentNode = oParentNode;
			this.AssertValid();
		}
		protected internal bool GetNodeFromPoint(PointF oPointF, out Node oNode)
		{
			this.AssertValid();
			bool result;
			if (this.m_oRectangle.Contains(oPointF))
			{
				if (!this.m_oNodes.GetNodeFromPoint(oPointF, out oNode))
				{
					oNode = this;
				}
				result = true;
			}
			else
			{
				oNode = null;
				result = false;
			}
			return result;
		}
		protected internal static void ValidateSizeMetric(float fValue, string sCaller)
		{
			if (fValue < 0f)
			{
				throw new ArgumentOutOfRangeException(sCaller, fValue, sCaller + ": SizeMetric must be >= 0.");
			}
		}
		protected internal static void ValidateColorMetric(float fValue, string sCaller)
		{
			if (float.IsNaN(fValue))
			{
				throw new ArgumentOutOfRangeException(sCaller, fValue, sCaller + ": ColorMetric can't be NaN.");
			}
		}
		protected internal void SaveRectangle()
		{
			this.m_oSavedRectangle = this.m_oRectangle;
			this.m_bRectangleSaved = true;
			this.AssertValid();
		}
		protected internal void RestoreRectangle()
		{
			Debug.Assert(this.m_bRectangleSaved);
			this.m_oRectangle = this.m_oSavedRectangle;
			this.m_bRectangleSaved = false;
			this.AssertValid();
		}
		protected void FireRedrawRequired()
		{
			this.AssertValid();
			if (this.m_oTreemapGenerator != null)
			{
				this.m_oTreemapGenerator.FireRedrawRequired();
			}
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_fSizeMetric >= 0f);
			this.m_oNodeColor.AssertValid();
			Debug.Assert(this.m_oNodes != null);
			this.m_oNodes.AssertValid();
			Debug.Assert(this.m_iPenWidthPx == -1 || this.m_iPenWidthPx >= 0);
		}
	}
}
