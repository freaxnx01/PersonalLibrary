using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.Research.CommunityTechnologies.TreemapNoDoc;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	public class TreemapGenerator : ITreemapComponent
	{
		public delegate void TreemapDrawItemEventHandler(object sender, TreemapDrawItemEventArgs treemapDrawItemEventArgs);
		public const int MinPaddingPx = 1;
		public const int MaxPaddingPx = 100;
		public const int MinPaddingDecrementPerLevelPx = 0;
		public const int MaxPaddingDecrementPerLevelPx = 99;
		public const int MinPenWidthPx = 1;
		public const int MaxPenWidthPx = 100;
		public const int MinPenWidthDecrementPerLevelPx = 0;
		public const int MaxPenWidthDecrementPerLevelPx = 99;
		public const int MinDiscreteColors = 2;
		public const int MaxDiscreteColors = 50;
		protected const float MinRectangleWidthOrHeightPx = 1f;
		protected Nodes m_oNodes;
		protected int m_iPaddingPx;
		protected int m_iPaddingDecrementPerLevelPx;
		protected int m_iPenWidthPx;
		protected int m_iPenWidthDecrementPerLevelPx;
		protected Color m_oBackColor;
		protected Color m_oBorderColor;
		protected NodeColorAlgorithm m_eNodeColorAlgorithm;
		protected Color m_oMinColor;
		protected Color m_oMaxColor;
		protected float m_fMinColorMetric;
		protected float m_fMaxColorMetric;
		protected int m_iDiscretePositiveColors;
		protected int m_iDiscreteNegativeColors;
		protected string m_sFontFamily;
		protected float m_fFontMinSizePt;
		protected float m_fFontMaxSizePt;
		protected float m_fFontIncrementPt;
		protected Color m_oFontSolidColor;
		protected int m_iFontMinAlpha;
		protected int m_iFontMaxAlpha;
		protected int m_iFontAlphaIncrementPerLevel;
		protected Color m_oSelectedFontColor;
		protected Color m_oSelectedBackColor;
		protected NodeLevelsWithText m_iNodeLevelsWithText;
		protected int m_iMinNodeLevelWithText;
		protected int m_iMaxNodeLevelWithText;
		protected TextLocation m_eTextLocation;
		protected EmptySpaceLocation m_eEmptySpaceLocation;
		protected Node m_oSelectedNode;
		protected Bitmap m_oSavedSelectedNodeBitmap;
		protected bool m_bInBeginUpdate;
		protected LayoutAlgorithm m_eLayoutAlgorithm;
        public TreemapDrawItemEventHandler DrawItem;
        //public event TreemapGenerator.TreemapDrawItemEventHandler DrawItem
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.DrawItem = (TreemapGenerator.TreemapDrawItemEventHandler)Delegate.Combine(this.DrawItem, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.DrawItem = (TreemapGenerator.TreemapDrawItemEventHandler)Delegate.Remove(this.DrawItem, value);
        //    }
        //}
        public EventHandler RedrawRequired;
        //public event EventHandler RedrawRequired
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.RedrawRequired = (EventHandler)Delegate.Combine(this.RedrawRequired, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.RedrawRequired = (EventHandler)Delegate.Remove(this.RedrawRequired, value);
        //    }
        //}
		[Browsable(false)]
		public Nodes Nodes
		{
			get
			{
				this.AssertValid();
				return this.m_oNodes;
			}
		}
		[Browsable(false)]
		public string NodesXml
		{
			get
			{
				this.AssertValid();
				NodesXmlSerializer nodesXmlSerializer = new NodesXmlSerializer();
				return nodesXmlSerializer.SerializeToString(this.m_oNodes, this);
			}
			set
			{
				this.CancelSelectedNode();
				NodesXmlSerializer nodesXmlSerializer = new NodesXmlSerializer();
				this.m_oNodes = nodesXmlSerializer.DeserializeFromString(value, this);
				this.m_oNodes.TreemapGenerator = this;
				this.FireRedrawRequired();
				this.AssertValid();
			}
		}
		public LayoutAlgorithm LayoutAlgorithm
		{
			get
			{
				this.AssertValid();
				return this.m_eLayoutAlgorithm;
			}
			set
			{
				if (this.m_eLayoutAlgorithm != value)
				{
					this.m_eLayoutAlgorithm = value;
					this.FireRedrawRequired();
				}
			}
		}
		public int PaddingPx
		{
			get
			{
				this.AssertValid();
				return this.m_iPaddingPx;
			}
			set
			{
				if (value < 1 || value > 100)
				{
					throw new ArgumentOutOfRangeException("PaddingPx", value, string.Concat(new object[]
					{
						"TreemapGenerator.PaddingPx: Must be between ", 
						1, 
						" and ", 
						100, 
						"."
					}));
				}
				if (this.m_iPaddingPx != value)
				{
					this.m_iPaddingPx = value;
					this.FireRedrawRequired();
				}
			}
		}
		public int PaddingDecrementPerLevelPx
		{
			get
			{
				this.AssertValid();
				return this.m_iPaddingDecrementPerLevelPx;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("PaddingDecrementPerLevelPx", value, string.Concat(new object[]
					{
						"TreemapGenerator.PaddingDecrementPerLevelPx: Must be between ", 
						0, 
						" and ", 
						99, 
						"."
					}));
				}
				if (this.m_iPaddingDecrementPerLevelPx != value)
				{
					this.m_iPaddingDecrementPerLevelPx = value;
					this.FireRedrawRequired();
				}
			}
		}
		public int PenWidthPx
		{
			get
			{
				this.AssertValid();
				return this.m_iPenWidthPx;
			}
			set
			{
				if (value < 1 || value > 100)
				{
					throw new ArgumentOutOfRangeException("PenWidthPx", value, string.Concat(new object[]
					{
						"TreemapGenerator.PenWidthPx: Must be between ", 
						1, 
						" and ", 
						100, 
						"."
					}));
				}
				if (this.m_iPenWidthPx != value)
				{
					this.m_iPenWidthPx = value;
					this.FireRedrawRequired();
				}
			}
		}
		public int PenWidthDecrementPerLevelPx
		{
			get
			{
				this.AssertValid();
				return this.m_iPenWidthDecrementPerLevelPx;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("PenWidthDecrementPerLevelPx", value, string.Concat(new object[]
					{
						"TreemapGenerator.PenWidthDecrementPerLevelPx: Must be between ", 
						0, 
						" and ", 
						99, 
						"."
					}));
				}
				if (this.m_iPenWidthDecrementPerLevelPx != value)
				{
					this.m_iPenWidthDecrementPerLevelPx = value;
					this.FireRedrawRequired();
				}
			}
		}
		public Color BackColor
		{
			get
			{
				this.AssertValid();
				return this.m_oBackColor;
			}
			set
			{
				if (this.m_oBackColor != value)
				{
					this.m_oBackColor = value;
					this.FireRedrawRequired();
				}
			}
		}
		public Color BorderColor
		{
			get
			{
				this.AssertValid();
				return this.m_oBorderColor;
			}
			set
			{
				if (this.m_oBorderColor != value)
				{
					this.m_oBorderColor = value;
					this.FireRedrawRequired();
				}
			}
		}
		public NodeColorAlgorithm NodeColorAlgorithm
		{
			get
			{
				this.AssertValid();
				return this.m_eNodeColorAlgorithm;
			}
			set
			{
				if (this.m_eNodeColorAlgorithm != value)
				{
					this.m_eNodeColorAlgorithm = value;
					this.FireRedrawRequired();
				}
			}
		}
		public Color MinColor
		{
			get
			{
				this.AssertValid();
				return this.m_oMinColor;
			}
			set
			{
				if (this.m_oMinColor != value)
				{
					this.m_oMinColor = value;
					this.FireRedrawRequired();
				}
			}
		}
		public Color MaxColor
		{
			get
			{
				this.AssertValid();
				return this.m_oMaxColor;
			}
			set
			{
				if (this.m_oMaxColor != value)
				{
					this.m_oMaxColor = value;
					this.FireRedrawRequired();
				}
			}
		}
		public float MinColorMetric
		{
			get
			{
				this.AssertValid();
				return this.m_fMinColorMetric;
			}
			set
			{
				if (value >= 0f)
				{
					throw new ArgumentOutOfRangeException("MinColorMetric", value, "TreemapGenerator.MinColorMetric: Must be negative.");
				}
				if (this.m_fMinColorMetric != value)
				{
					this.m_fMinColorMetric = value;
					this.FireRedrawRequired();
				}
			}
		}
		public float MaxColorMetric
		{
			get
			{
				this.AssertValid();
				return this.m_fMaxColorMetric;
			}
			set
			{
				if (value <= 0f)
				{
					throw new ArgumentOutOfRangeException("MaxColorMetric", value, "TreemapGenerator.MaxColorMetric: Must be positive.");
				}
				if (this.m_fMaxColorMetric != value)
				{
					this.m_fMaxColorMetric = value;
					this.FireRedrawRequired();
				}
			}
		}
		public int DiscreteNegativeColors
		{
			get
			{
				this.AssertValid();
				return this.m_iDiscreteNegativeColors;
			}
			set
			{
				if (value < 2 || value > 50)
				{
					throw new ArgumentOutOfRangeException("DiscreteNegativeColors", value, string.Concat(new object[]
					{
						"TreemapGenerator.DiscreteNegativeColors: Must be between ", 
						2, 
						" and ", 
						50, 
						"."
					}));
				}
				if (this.m_iDiscreteNegativeColors != value)
				{
					this.m_iDiscreteNegativeColors = value;
					this.FireRedrawRequired();
				}
			}
		}
		public int DiscretePositiveColors
		{
			get
			{
				this.AssertValid();
				return this.m_iDiscretePositiveColors;
			}
			set
			{
				if (value < 2 || value > 50)
				{
					throw new ArgumentOutOfRangeException("DiscretePositiveColors", value, string.Concat(new object[]
					{
						"TreemapGenerator.DiscretePositiveColors: Must be between ", 
						2, 
						" and ", 
						50, 
						"."
					}));
				}
				if (this.m_iDiscretePositiveColors != value)
				{
					this.m_iDiscretePositiveColors = value;
					this.FireRedrawRequired();
				}
			}
		}
		public string FontFamily
		{
			get
			{
				this.AssertValid();
				return this.m_sFontFamily;
			}
			set
			{
				Font font = new Font(value, 8f);
				string name = font.FontFamily.Name;
				font.Dispose();
				if (name.ToLower() != value.ToLower())
				{
					throw new ArgumentOutOfRangeException("FontFamily", value, "TreemapGenerator.FontFamily: No such font.");
				}
				if (this.m_sFontFamily != value)
				{
					this.m_sFontFamily = value;
					this.FireRedrawRequired();
				}
			}
		}
		public Color FontSolidColor
		{
			get
			{
				this.AssertValid();
				return this.m_oFontSolidColor;
			}
			set
			{
				if (value.A != 255)
				{
					throw new ArgumentOutOfRangeException("FontSolidColor", value, "TreemapGenerator.FontSolidColor: Must not be transparent.");
				}
				if (this.m_oFontSolidColor != value)
				{
					this.m_oFontSolidColor = value;
					this.FireRedrawRequired();
				}
			}
		}
		public Color SelectedFontColor
		{
			get
			{
				this.AssertValid();
				return this.m_oSelectedFontColor;
			}
			set
			{
				if (this.m_oSelectedFontColor != value)
				{
					this.m_oSelectedFontColor = value;
					this.FireRedrawRequired();
				}
			}
		}
		public Color SelectedBackColor
		{
			get
			{
				this.AssertValid();
				return this.m_oSelectedBackColor;
			}
			set
			{
				if (this.m_oSelectedBackColor != value)
				{
					this.m_oSelectedBackColor = value;
					this.FireRedrawRequired();
				}
			}
		}
		public NodeLevelsWithText NodeLevelsWithText
		{
			get
			{
				this.AssertValid();
				return this.m_iNodeLevelsWithText;
			}
			set
			{
				if (this.m_iNodeLevelsWithText != value)
				{
					this.m_iNodeLevelsWithText = value;
					this.FireRedrawRequired();
				}
			}
		}
		public TextLocation TextLocation
		{
			get
			{
				this.AssertValid();
				return this.m_eTextLocation;
			}
			set
			{
				if (this.m_eTextLocation != value)
				{
					this.m_eTextLocation = value;
					this.FireRedrawRequired();
				}
			}
		}
		public EmptySpaceLocation EmptySpaceLocation
		{
			get
			{
				this.AssertValid();
				return this.m_eEmptySpaceLocation;
			}
			set
			{
				if (this.m_eEmptySpaceLocation != value)
				{
					this.m_eEmptySpaceLocation = value;
					this.FireRedrawRequired();
				}
			}
		}
		[Browsable(false)]
		public Node SelectedNode
		{
			get
			{
				this.AssertValid();
				return this.m_oSelectedNode;
			}
		}
		public TreemapGenerator()
		{
			this.m_oNodes = new Nodes(null);
			this.m_oNodes.TreemapGenerator = this;
			this.m_iPaddingPx = 5;
			this.m_iPaddingDecrementPerLevelPx = 1;
			this.m_iPenWidthPx = 3;
			this.m_iPenWidthDecrementPerLevelPx = 1;
			this.m_oBackColor = SystemColors.Window;
			this.m_oBorderColor = SystemColors.WindowFrame;
			this.m_eNodeColorAlgorithm = NodeColorAlgorithm.UseColorMetric;
			this.m_oMinColor = Color.Red;
			this.m_oMaxColor = Color.Green;
			this.m_fMinColorMetric = -100f;
			this.m_fMaxColorMetric = 100f;
			this.m_iDiscretePositiveColors = 20;
			this.m_iDiscreteNegativeColors = 20;
			this.m_sFontFamily = "Arial";
			this.m_fFontMinSizePt = 8f;
			this.m_fFontMaxSizePt = 100f;
			this.m_fFontIncrementPt = 2f;
			this.m_oFontSolidColor = SystemColors.WindowText;
			this.m_iFontMinAlpha = 105;
			this.m_iFontMaxAlpha = 255;
			this.m_iFontAlphaIncrementPerLevel = 50;
			this.m_oSelectedFontColor = SystemColors.HighlightText;
			this.m_oSelectedBackColor = SystemColors.Highlight;
			this.m_iNodeLevelsWithText = NodeLevelsWithText.All;
			this.m_iMinNodeLevelWithText = 0;
			this.m_iMaxNodeLevelWithText = 999;
			this.m_eTextLocation = TextLocation.Top;
			this.m_eEmptySpaceLocation = EmptySpaceLocation.DeterminedByLayoutAlgorithm;
			this.m_oSelectedNode = null;
			this.m_oSavedSelectedNodeBitmap = null;
			this.m_bInBeginUpdate = false;
			this.m_eLayoutAlgorithm = LayoutAlgorithm.BottomWeightedSquarified;
		}
		public void GetNodeLevelsWithTextRange(out int minLevel, out int maxLevel)
		{
			this.AssertValid();
			minLevel = this.m_iMinNodeLevelWithText;
			maxLevel = this.m_iMaxNodeLevelWithText;
		}
		public void SetNodeLevelsWithTextRange(int minLevel, int maxLevel)
		{
			if (minLevel < 0)
			{
				throw new ArgumentOutOfRangeException("minLevel", minLevel, "TreemapGenerator.SetNodeLevelsWithTextRange: Must be >= 0.");
			}
			if (maxLevel < 0)
			{
				throw new ArgumentOutOfRangeException("maxLevel", maxLevel, "TreemapGenerator.SetNodeLevelsWithTextRange: Must be >= 0.");
			}
			if (maxLevel < minLevel)
			{
				throw new ArgumentOutOfRangeException("maxLevel", maxLevel, "TreemapGenerator.SetNodeLevelsWithTextRange: Must be >= minLevel.");
			}
			this.m_iMinNodeLevelWithText = minLevel;
			this.m_iMaxNodeLevelWithText = maxLevel;
			this.FireRedrawRequired();
			this.AssertValid();
		}
		public void GetFontSizeRange(out float minSizePt, out float maxSizePt, out float incrementPt)
		{
			this.AssertValid();
			minSizePt = this.m_fFontMinSizePt;
			maxSizePt = this.m_fFontMaxSizePt;
			incrementPt = this.m_fFontIncrementPt;
		}
		public void SetFontSizeRange(float minSizePt, float maxSizePt, float incrementPt)
		{
			MaximizingFontMapper.ValidateSizeRange(minSizePt, maxSizePt, incrementPt, "TreemapGenerator.SetFontSizeRange()");
			this.m_fFontMinSizePt = minSizePt;
			this.m_fFontMaxSizePt = maxSizePt;
			this.m_fFontIncrementPt = incrementPt;
			this.FireRedrawRequired();
			this.AssertValid();
		}
		public void GetFontAlphaRange(out int minAlpha, out int maxAlpha, out int incrementPerLevel)
		{
			this.AssertValid();
			minAlpha = this.m_iFontMinAlpha;
			maxAlpha = this.m_iFontMaxAlpha;
			incrementPerLevel = this.m_iFontAlphaIncrementPerLevel;
		}
		public void SetFontAlphaRange(int minAlpha, int maxAlpha, int incrementPerLevel)
		{
			TransparentBrushMapper.ValidateAlphaRange(minAlpha, maxAlpha, incrementPerLevel, "TreemapGenerator.SetFontAlphaRange");
			this.m_iFontMinAlpha = minAlpha;
			this.m_iFontMaxAlpha = maxAlpha;
			this.m_iFontAlphaIncrementPerLevel = incrementPerLevel;
			this.FireRedrawRequired();
			this.AssertValid();
		}
		public void BeginUpdate()
		{
			this.AssertValid();
			this.m_bInBeginUpdate = true;
		}
		public void EndUpdate()
		{
			this.AssertValid();
			this.m_bInBeginUpdate = false;
			this.FireRedrawRequired();
		}
		public void Clear()
		{
			this.m_oNodes.Clear();
			this.CancelSelectedNode();
			this.FireRedrawRequired();
			this.AssertValid();
		}
		public void Draw(Bitmap bitmap, bool drawSelection)
		{
			Debug.Assert(bitmap != null);
			this.AssertValid();
			Rectangle treemapRectangle = Rectangle.FromLTRB(0, 0, bitmap.Width, bitmap.Height);
			this.Draw(bitmap, drawSelection, treemapRectangle);
		}
		public void Draw(Bitmap bitmap, bool drawSelection, Rectangle treemapRectangle)
		{
			Debug.Assert(bitmap != null);
			this.AssertValid();
			if (!Rectangle.FromLTRB(0, 0, bitmap.Width, bitmap.Height).Contains(treemapRectangle))
			{
				throw new ArgumentException("TreemapGenerator.Draw(): treemapRectangle is not contained within the bitmap.");
			}
			Node oSelectedNode = this.m_oSelectedNode;
			this.CancelSelectedNode();
			Graphics graphics = Graphics.FromImage(bitmap);
			this.Draw(graphics, treemapRectangle);
			graphics.Dispose();
			if (drawSelection && oSelectedNode != null)
			{
				this.SelectNode(oSelectedNode, bitmap);
			}
		}
		public void Draw(Graphics graphics, Rectangle treemapRectangle)
		{
			Debug.Assert(graphics != null);
			Debug.Assert(!treemapRectangle.IsEmpty);
			this.AssertValid();
			this.CalculateAndDrawRectangles(graphics, treemapRectangle, this.m_oNodes, null);
			if (this.m_iNodeLevelsWithText != NodeLevelsWithText.None)
			{
				this.DrawText(graphics, treemapRectangle, this.m_oNodes);
			}
		}
		public void Draw(Rectangle treemapRectangle)
		{
			this.AssertValid();
			if (this.DrawItem == null)
			{
				throw new InvalidOperationException("TreemapGenerator.Draw: The Draw(Rectangle) method initiates owner draw, which requires that the DrawItem event be handled.  The DrawItem event is not being handled.");
			}
			ILayoutEngine oLayoutEngine = this.CreateLayoutEngine();
			this.CalculateRectangles(this.m_oNodes, treemapRectangle, null, this.m_iPaddingPx, this.m_iPaddingPx, this.m_iPenWidthPx, oLayoutEngine);
			this.DrawNodesByOwnerDraw(this.m_oNodes);
		}
		public bool GetNodeFromPoint(PointF pointF, out Node node)
		{
			return this.m_oNodes.GetNodeFromPoint(pointF, out node);
		}
		public bool GetNodeFromPoint(int x, int y, out Node node)
		{
			return this.GetNodeFromPoint(new PointF((float)x, (float)y), out node);
		}
		public void SelectNode(Node node, Bitmap bitmap)
		{
			if (node != null)
			{
				node.AssertValid();
				if (node == this.m_oSelectedNode)
				{
					return;
				}
			}
			if (bitmap != null)
			{
				Graphics graphics = Graphics.FromImage(bitmap);
				if (this.m_oSelectedNode != null && this.m_oSavedSelectedNodeBitmap != null)
				{
					this.m_oSelectedNode.AssertValid();
					Debug.Assert(!this.m_oSelectedNode.Rectangle.IsEmpty);
					Debug.Assert(this.m_oSavedSelectedNodeBitmap != null);
					int penWidthPx = this.SetNodePenWidthForSelection(this.m_oSelectedNode);
					Rectangle rectangleToDraw = this.m_oSelectedNode.RectangleToDraw;
					graphics.DrawImage(this.m_oSavedSelectedNodeBitmap, rectangleToDraw.X, rectangleToDraw.Y);
					this.m_oSelectedNode.PenWidthPx = penWidthPx;
					this.CancelSelectedNode();
				}
				if (node != null && node.HasBeenDrawn)
				{
					Rectangle rectangleToDraw2 = node.RectangleToDraw;
					this.m_oSavedSelectedNodeBitmap = bitmap.Clone(Rectangle.FromLTRB(rectangleToDraw2.Left, rectangleToDraw2.Top, Math.Min(rectangleToDraw2.Right + 1, bitmap.Width), Math.Min(rectangleToDraw2.Bottom + 1, bitmap.Height)), bitmap.PixelFormat);
					this.DrawNodeAsSelected(node, graphics, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
				}
				graphics.Dispose();
			}
			this.m_oSelectedNode = node;
		}
		protected void CalculateAndDrawRectangles(Graphics oGraphics, RectangleF oTreemapRectangle, Nodes oNodes, Node oParentNode)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oNodes != null);
			this.AssertValid();
			Brush brush = new SolidBrush(this.m_oBackColor);
			oGraphics.FillRectangle(brush, oTreemapRectangle);
			brush.Dispose();
			ILayoutEngine oLayoutEngine = this.CreateLayoutEngine();
			this.CalculateRectangles(oNodes, oTreemapRectangle, oParentNode, this.GetTopLevelTopPaddingPx(oGraphics), this.m_iPaddingPx, this.m_iPenWidthPx, oLayoutEngine);
			ColorGradientMapper colorGradientMapper = null;
			ColorGradientMapper colorGradientMapper2 = null;
			if (this.m_eNodeColorAlgorithm == NodeColorAlgorithm.UseColorMetric)
			{
				colorGradientMapper = new ColorGradientMapper();
				Debug.Assert(this.m_fMaxColorMetric > 0f);
				colorGradientMapper.Initialize(oGraphics, 0f, this.m_fMaxColorMetric, Color.White, this.m_oMaxColor, this.m_iDiscretePositiveColors, true);
				colorGradientMapper2 = new ColorGradientMapper();
				Debug.Assert(this.m_fMinColorMetric < 0f);
				colorGradientMapper2.Initialize(oGraphics, 0f, -this.m_fMinColorMetric, Color.White, this.m_oMinColor, this.m_iDiscreteNegativeColors, true);
			}
			PenCache penCache = new PenCache();
			penCache.Initialize(this.m_oBorderColor);
			this.DrawRectangles(oNodes, 0, oGraphics, colorGradientMapper2, colorGradientMapper, penCache);
			if (colorGradientMapper2 != null)
			{
				colorGradientMapper2.Dispose();
			}
			if (colorGradientMapper != null)
			{
				colorGradientMapper.Dispose();
			}
			penCache.Dispose();
		}
		protected void CalculateRectangles(Nodes oNodes, RectangleF oParentRectangle, Node oParentNode, int iTopPaddingPx, int iLeftRightBottomPaddingPx, int iPenWidthPx, ILayoutEngine oLayoutEngine)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(iTopPaddingPx > 0);
			Debug.Assert(iLeftRightBottomPaddingPx > 0);
			Debug.Assert(iPenWidthPx > 0);
			Debug.Assert(oLayoutEngine != null);
			this.AssertValid();
			int num = iTopPaddingPx;
			if (oParentNode == null)
			{
				iTopPaddingPx = iLeftRightBottomPaddingPx;
			}
			if (!this.AddPaddingToParentRectangle(ref oParentRectangle, ref iTopPaddingPx, ref iLeftRightBottomPaddingPx))
			{
				oLayoutEngine.SetNodeRectanglesToEmpty(oNodes, true);
			}
			else
			{
				if (oParentNode == null)
				{
					iTopPaddingPx = num;
				}
				oLayoutEngine.CalculateNodeRectangles(oNodes, oParentRectangle, oParentNode, this.m_eEmptySpaceLocation);
				int num2 = this.DecrementPadding(iLeftRightBottomPaddingPx);
				int iPenWidthPx2 = this.DecrementPenWidth(iPenWidthPx);
				int iTopPaddingPx2 = 0;
				switch (this.m_eTextLocation)
				{
					case TextLocation.CenterCenter:
					{
						iTopPaddingPx2 = num2;
						break;
					}
					case TextLocation.Top:
					{
						iTopPaddingPx2 = iTopPaddingPx;
						break;
					}
					default:
					{
						Debug.Assert(false);
						break;
					}
				}
				foreach (Node current in oNodes)
				{
					if (!current.Rectangle.IsEmpty)
					{
						RectangleF rectangle = current.Rectangle;
						if (!this.AddPaddingToChildRectangle(ref rectangle, oParentRectangle, iLeftRightBottomPaddingPx))
						{
							current.Rectangle = this.FixSmallRectangle(current.Rectangle);
							current.PenWidthPx = 1;
							oLayoutEngine.SetNodeRectanglesToEmpty(current.Nodes, true);
						}
						else
						{
							current.Rectangle = rectangle;
							current.PenWidthPx = iPenWidthPx;
							RectangleF oParentRectangle2 = RectangleF.Inflate(rectangle, (float)(-(float)iPenWidthPx), (float)(-(float)iPenWidthPx));
							this.CalculateRectangles(current.Nodes, oParentRectangle2, current, iTopPaddingPx2, num2, iPenWidthPx2, oLayoutEngine);
						}
					}
				}
			}
		}
		protected void DrawRectangles(Nodes oNodes, int iNodeLevel, Graphics oGraphics, ColorGradientMapper oNegativeColorGradientMapper, ColorGradientMapper oPositiveColorGradientMapper, PenCache oPenCache)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(iNodeLevel >= 0);
			Debug.Assert(oGraphics != null);
			Debug.Assert(this.m_eNodeColorAlgorithm != NodeColorAlgorithm.UseColorMetric || oNegativeColorGradientMapper != null);
			Debug.Assert(this.m_eNodeColorAlgorithm != NodeColorAlgorithm.UseColorMetric || oPositiveColorGradientMapper != null);
			Debug.Assert(oPenCache != null);
			foreach (Node current in oNodes)
			{
				if (!current.Rectangle.IsEmpty)
				{
					Pen pen = oPenCache.GetPen(current.PenWidthPx);
					Brush brush = null;
					bool flag = false;
					switch (this.m_eNodeColorAlgorithm)
					{
						case NodeColorAlgorithm.UseColorMetric:
						{
							Debug.Assert(oNegativeColorGradientMapper != null);
							Debug.Assert(oPositiveColorGradientMapper != null);
							float colorMetric = current.ColorMetric;
							if (colorMetric <= 0f)
							{
								brush = oNegativeColorGradientMapper.ColorMetricToBrush(-colorMetric);
							}
							else
							{
								brush = oPositiveColorGradientMapper.ColorMetricToBrush(colorMetric);
							}
							break;
						}
						case NodeColorAlgorithm.UseAbsoluteColor:
						{
							brush = new SolidBrush(current.AbsoluteColor);
							flag = true;
							break;
						}
						default:
						{
							Debug.Assert(false);
							break;
						}
					}
					Debug.Assert(brush != null);
					Rectangle rectangleToDraw = current.RectangleToDraw;
					oGraphics.FillRectangle(brush, rectangleToDraw);
					oGraphics.DrawRectangle(pen, rectangleToDraw);
					if (flag)
					{
						brush.Dispose();
						brush = null;
					}
					this.DrawRectangles(current.Nodes, iNodeLevel + 1, oGraphics, oNegativeColorGradientMapper, oPositiveColorGradientMapper, oPenCache);
				}
			}
		}
		protected void DrawText(Graphics oGraphics, Rectangle oTreemapRectangle, Nodes oNodes)
		{
			this.AssertValid();
			ITextDrawer textDrawer = this.CreateTextDrawer();
			textDrawer.DrawTextForAllNodes(oGraphics, oTreemapRectangle, oNodes);
		}
		protected void DrawNodesByOwnerDraw(Nodes oNodes)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(this.DrawItem != null);
			this.AssertValid();
			foreach (Node current in oNodes)
			{
				if (!current.Rectangle.IsEmpty)
				{
					TreemapDrawItemEventArgs treemapDrawItemEventArgs = new TreemapDrawItemEventArgs(current);
					this.DrawItem(this, treemapDrawItemEventArgs);
					this.DrawNodesByOwnerDraw(current.Nodes);
				}
			}
		}
		protected void DrawNodeAsSelected(Node oNode, Graphics oGraphics, Rectangle oTreemapRectangle)
		{
			Debug.Assert(oNode != null);
			oNode.AssertValid();
			Debug.Assert(oGraphics != null);
			int penWidthPx = this.SetNodePenWidthForSelection(oNode);
			Brush brush = new SolidBrush(this.m_oSelectedBackColor);
			Pen pen = new Pen(brush, (float)oNode.PenWidthPx);
			pen.Alignment = PenAlignment.Inset;
			oGraphics.DrawRectangle(pen, oNode.RectangleToDraw);
			pen.Dispose();
			brush.Dispose();
			oNode.PenWidthPx = penWidthPx;
			ITextDrawer textDrawer = this.CreateTextDrawer();
			textDrawer.DrawTextForSelectedNode(oGraphics, oNode);
		}
		protected internal void FireRedrawRequired()
		{
			if (this.RedrawRequired != null && !this.m_bInBeginUpdate)
			{
				this.RedrawRequired.Invoke(this, EventArgs.Empty);
			}
		}
		protected void CancelSelectedNode()
		{
			this.m_oSelectedNode = null;
			if (this.m_oSavedSelectedNodeBitmap != null)
			{
				this.m_oSavedSelectedNodeBitmap.Dispose();
				this.m_oSavedSelectedNodeBitmap = null;
			}
		}
		protected ILayoutEngine CreateLayoutEngine()
		{
			ILayoutEngine result;
			switch (this.m_eLayoutAlgorithm)
			{
				case LayoutAlgorithm.BottomWeightedSquarified:
				{
					result = new BottomWeightedSquarifiedLayoutEngine();
					break;
				}
				case LayoutAlgorithm.TopWeightedSquarified:
				{
					result = new TopWeightedSquarifiedLayoutEngine();
					break;
				}
				default:
				{
					Debug.Assert(false);
					result = null;
					break;
				}
			}
			return result;
		}
		protected ITextDrawer CreateTextDrawer()
		{
			this.AssertValid();
			ITextDrawer result;
			switch (this.m_eTextLocation)
			{
				case TextLocation.CenterCenter:
				{
					result = new CenterCenterTextDrawer(this.m_iNodeLevelsWithText, this.m_iMinNodeLevelWithText, this.m_iMaxNodeLevelWithText, this.m_sFontFamily, this.m_fFontMinSizePt, this.m_fFontMaxSizePt, this.m_fFontIncrementPt, this.m_oFontSolidColor, this.m_iFontMinAlpha, this.m_iFontMaxAlpha, this.m_iFontAlphaIncrementPerLevel, this.m_oSelectedFontColor);
					break;
				}
				case TextLocation.Top:
				{
					result = new TopTextDrawer(this.m_iNodeLevelsWithText, this.m_iMinNodeLevelWithText, this.m_iMaxNodeLevelWithText, this.m_sFontFamily, this.m_fFontMinSizePt, this.GetTopMinimumTextHeight(), this.m_oFontSolidColor, this.m_oSelectedFontColor, this.m_oSelectedBackColor);
					break;
				}
				default:
				{
					Debug.Assert(false);
					result = null;
					break;
				}
			}
			return result;
		}
		protected bool AddPaddingToParentRectangle(ref RectangleF oParentRectangle, ref int iTopPaddingPx, ref int iLeftRightBottomPaddingPx)
		{
			Debug.Assert(iTopPaddingPx >= 0);
			Debug.Assert(iLeftRightBottomPaddingPx >= 0);
			int num = iTopPaddingPx;
			int num2 = iLeftRightBottomPaddingPx;
			RectangleF rectangleF = this.AddPaddingToRectangle(oParentRectangle, num, num2);
			bool result;
			if (this.RectangleIsSmallerThanMin(rectangleF))
			{
				result = false;
			}
			else
			{
				oParentRectangle = rectangleF;
				iTopPaddingPx = num;
				iLeftRightBottomPaddingPx = num2;
				result = true;
			}
			return result;
		}
		protected RectangleF AddPaddingToRectangle(RectangleF oRectangle, int iTopPaddingPx, int iLeftRightBottomPaddingPx)
		{
			Debug.Assert(iTopPaddingPx >= 0);
			Debug.Assert(iLeftRightBottomPaddingPx >= 0);
			return RectangleF.FromLTRB(oRectangle.Left + (float)iLeftRightBottomPaddingPx, oRectangle.Top + (float)iTopPaddingPx, oRectangle.Right - (float)iLeftRightBottomPaddingPx, oRectangle.Bottom - (float)iLeftRightBottomPaddingPx);
		}
		protected bool AddPaddingToChildRectangle(ref RectangleF oChildRectangle, RectangleF oParentRectangle, int iPaddingPx)
		{
			RectangleF rectangleF = this.AddPaddingToChildRectangle(oChildRectangle, oParentRectangle, iPaddingPx);
			bool result;
			if (this.RectangleIsSmallerThanMin(rectangleF))
			{
				if (iPaddingPx > 1)
				{
					rectangleF = this.AddPaddingToChildRectangle(oChildRectangle, oParentRectangle, 1);
				}
				if (this.RectangleIsSmallerThanMin(rectangleF))
				{
					result = false;
					return result;
				}
			}
			oChildRectangle = rectangleF;
			result = true;
			return result;
		}
		protected RectangleF AddPaddingToChildRectangle(RectangleF oChildRectangle, RectangleF oParentRectangle, int iPaddingPx)
		{
			float num = oChildRectangle.Left;
			float num2 = oChildRectangle.Top;
			float num3 = oChildRectangle.Right;
			float num4 = oChildRectangle.Bottom;
			float num5 = (float)(iPaddingPx + 1) / 2f;
			if (Math.Round((double)oChildRectangle.Left) != Math.Round((double)oParentRectangle.Left))
			{
				num += num5;
			}
			if (Math.Round((double)oChildRectangle.Top) != Math.Round((double)oParentRectangle.Top))
			{
				num2 += num5;
			}
			if (Math.Round((double)oChildRectangle.Right) != Math.Round((double)oParentRectangle.Right))
			{
				num3 -= num5;
			}
			if (Math.Round((double)oChildRectangle.Bottom) != Math.Round((double)oParentRectangle.Bottom))
			{
				num4 -= num5;
			}
			return RectangleF.FromLTRB(num, num2, num3, num4);
		}
		protected int GetTopLevelTopPaddingPx(Graphics oGraphics)
		{
			this.AssertValid();
			Debug.Assert(oGraphics != null);
			int result;
			switch (this.m_eTextLocation)
			{
				case TextLocation.CenterCenter:
				{
					result = this.m_iPaddingPx;
					break;
				}
				case TextLocation.Top:
				{
					result = TopTextDrawer.GetTextHeight(oGraphics, this.m_sFontFamily, this.m_fFontMinSizePt, this.GetTopMinimumTextHeight());
					break;
				}
				default:
				{
					Debug.Assert(false);
					result = -1;
					break;
				}
			}
			return result;
		}
		protected int GetTopMinimumTextHeight()
		{
			this.AssertValid();
			return this.DecrementPadding(this.m_iPaddingPx);
		}
		protected int DecrementPadding(int iPaddingPx)
		{
			return Math.Max(iPaddingPx - this.m_iPaddingDecrementPerLevelPx, 1);
		}
		protected int DecrementPenWidth(int iPenWidthPx)
		{
			return Math.Max(iPenWidthPx - this.m_iPenWidthDecrementPerLevelPx, 1);
		}
		protected bool RectangleIsSmallerThanMin(RectangleF oRectangle)
		{
			return oRectangle.Width < 1f || oRectangle.Height < 1f;
		}
		protected RectangleF FixSmallRectangle(RectangleF oUnpaddedNodeRectangle)
		{
			float num = oUnpaddedNodeRectangle.Left;
			float num2 = oUnpaddedNodeRectangle.Top;
			float num3 = oUnpaddedNodeRectangle.Right;
			float num4 = oUnpaddedNodeRectangle.Bottom;
			float width = oUnpaddedNodeRectangle.Width;
			float height = oUnpaddedNodeRectangle.Height;
			float num5 = 0.5f;
			if (height < 1f)
			{
				num2 = (float)((double)(num2 + num4) / 2.0 - (double)num5);
				num4 = num2 + 1f;
			}
			if (width < 1f)
			{
				num = (num + num3) / 2f - num5;
				num3 = num + 1f;
			}
			RectangleF result = RectangleF.FromLTRB(num, num2, num3, num4);
			if (height < 1f)
			{
				Debug.Assert(Math.Round((double)result.Height) == 1.0);
			}
			if (width < 1f)
			{
				Debug.Assert(Math.Round((double)result.Width) == 1.0);
			}
			return result;
		}
		protected int SetNodePenWidthForSelection(Node oNode)
		{
			Debug.Assert(oNode != null);
			this.AssertValid();
			int penWidthPx = oNode.PenWidthPx;
			int num = this.m_iPaddingPx;
			int level = oNode.Level;
			for (int i = 0; i < level + 1; i++)
			{
				num = this.DecrementPadding(num);
			}
			int num2 = penWidthPx + num;
			switch (this.m_eTextLocation)
			{
				case TextLocation.CenterCenter:
				{
					num2 = Math.Max(num2, 4);
					break;
				}
				case TextLocation.Top:
				{
					break;
				}
				default:
				{
					Debug.Assert(false);
					break;
				}
			}
			oNode.PenWidthPx = num2;
			return penWidthPx;
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_oNodes != null);
			this.m_oNodes.AssertValid();
			Debug.Assert(this.m_iPaddingPx >= 1);
			Debug.Assert(this.m_iPaddingPx <= 100);
			Debug.Assert(this.m_iPaddingDecrementPerLevelPx >= 0);
			Debug.Assert(this.m_iPaddingDecrementPerLevelPx <= 99);
			Debug.Assert(this.m_iPenWidthPx >= 1);
			Debug.Assert(this.m_iPenWidthPx <= 100);
			Debug.Assert(this.m_iPenWidthDecrementPerLevelPx >= 0);
			Debug.Assert(this.m_iPenWidthDecrementPerLevelPx <= 99);
			Debug.Assert(Enum.IsDefined(typeof(NodeColorAlgorithm), this.m_eNodeColorAlgorithm));
			Debug.Assert(this.m_fMinColorMetric < 0f);
			Debug.Assert(this.m_fMaxColorMetric > 0f);
			Debug.Assert(this.m_iDiscretePositiveColors >= 2);
			Debug.Assert(this.m_iDiscretePositiveColors <= 50);
			Debug.Assert(this.m_iDiscreteNegativeColors >= 2);
			Debug.Assert(this.m_iDiscreteNegativeColors <= 50);
			StringUtil.AssertNotEmpty(this.m_sFontFamily);
			Debug.Assert(this.m_fFontMinSizePt > 0f);
			Debug.Assert(this.m_fFontMaxSizePt > 0f);
			Debug.Assert(this.m_fFontMaxSizePt >= this.m_fFontMinSizePt);
			Debug.Assert(this.m_fFontIncrementPt > 0f);
			Debug.Assert(this.m_oFontSolidColor.A == 255);
			Debug.Assert(this.m_iFontMinAlpha >= 0 && this.m_iFontMinAlpha <= 255);
			Debug.Assert(this.m_iFontMaxAlpha >= 0 && this.m_iFontMaxAlpha <= 255);
			Debug.Assert(this.m_iFontMaxAlpha >= this.m_iFontMinAlpha);
			Debug.Assert(this.m_iFontAlphaIncrementPerLevel > 0);
			Debug.Assert(Enum.IsDefined(typeof(NodeLevelsWithText), this.m_iNodeLevelsWithText));
			Debug.Assert(this.m_iMinNodeLevelWithText >= 0);
			Debug.Assert(this.m_iMaxNodeLevelWithText >= 0);
			Debug.Assert(this.m_iMaxNodeLevelWithText >= this.m_iMinNodeLevelWithText);
			Debug.Assert(Enum.IsDefined(typeof(TextLocation), this.m_eTextLocation));
			Debug.Assert(Enum.IsDefined(typeof(EmptySpaceLocation), this.m_eEmptySpaceLocation));
			if (this.m_oSelectedNode != null)
			{
				this.m_oSelectedNode.AssertValid();
			}
			Debug.Assert(Enum.IsDefined(typeof(LayoutAlgorithm), this.m_eLayoutAlgorithm));
		}
	}
}
