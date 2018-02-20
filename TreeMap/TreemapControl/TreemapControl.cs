using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.ControlLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.Research.CommunityTechnologies.TreemapNoDoc;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	public class TreemapControl : Panel, ITreemapComponent
	{
		public delegate void NodeEventHandler(object sender, NodeEventArgs nodeEventArgs);
		public delegate void NodeMouseEventHandler(object sender, NodeMouseEventArgs nodeMouseEventArgs);
		protected TreemapGenerator m_oTreemapGenerator;
		protected Bitmap m_oBitmap;
		protected bool m_bShowToolTips;
		protected bool m_bAllowDrag;
		protected bool m_bIsZoomable;
		protected ZoomActionHistoryList m_oZoomActionHistoryList;
		private ToolTipTracker m_oToolTipTracker;
		protected Point m_oLastMouseMovePoint;
		protected Point m_oLastDraggableMouseDownPoint;
		private Container components = null;
		private PictureBox picPictureBox;
		private ToolTipPanel pnlToolTip;
        [Category("Mouse")]
        public event TreemapControl.NodeMouseEventHandler NodeMouseDown;
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.NodeMouseDown = (TreemapControl.NodeMouseEventHandler)Delegate.Combine(this.NodeMouseDown, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.NodeMouseDown = (TreemapControl.NodeMouseEventHandler)Delegate.Remove(this.NodeMouseDown, value);
        //    }
        //}
		[Category("Mouse")]
		public event TreemapControl.NodeMouseEventHandler NodeMouseUp;
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.NodeMouseUp = (TreemapControl.NodeMouseEventHandler)Delegate.Combine(this.NodeMouseUp, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.NodeMouseUp = (TreemapControl.NodeMouseEventHandler)Delegate.Remove(this.NodeMouseUp, value);
        //    }
        //}
		[Category("Mouse")]
		public event TreemapControl.NodeEventHandler NodeMouseHover;
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.NodeMouseHover = (TreemapControl.NodeEventHandler)Delegate.Combine(this.NodeMouseHover, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.NodeMouseHover = (TreemapControl.NodeEventHandler)Delegate.Remove(this.NodeMouseHover, value);
        //    }
        //}
		[Category("Action")]
		public event TreemapControl.NodeEventHandler NodeDoubleClick;
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.NodeDoubleClick = (TreemapControl.NodeEventHandler)Delegate.Combine(this.NodeDoubleClick, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.NodeDoubleClick = (TreemapControl.NodeEventHandler)Delegate.Remove(this.NodeDoubleClick, value);
        //    }
        //}
		[Category("Action")]
		public event EventHandler ZoomStateChanged;
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.ZoomStateChanged = (EventHandler)Delegate.Combine(this.ZoomStateChanged, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.ZoomStateChanged = (EventHandler)Delegate.Remove(this.ZoomStateChanged, value);
        //    }
        //}
		[Category("Action")]
		public event EventHandler SelectedNodeChanged;
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.SelectedNodeChanged = (EventHandler)Delegate.Combine(this.SelectedNodeChanged, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.SelectedNodeChanged = (EventHandler)Delegate.Remove(this.SelectedNodeChanged, value);
        //    }
        //}
		[ReadOnly(true), Browsable(false)]
		public Nodes Nodes
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.Nodes;
			}
		}
		[ReadOnly(true), Browsable(false)]
		public string NodesXml
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.NodesXml;
			}
			set
			{
				this.m_oTreemapGenerator.NodesXml = value;
				this.AssertValid();
			}
		}
		[Description("The algorithm used to lay out the treemap's rectangles."), Category("Layout")]
		public LayoutAlgorithm LayoutAlgorithm
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.LayoutAlgorithm;
			}
			set
			{
				this.m_oTreemapGenerator.LayoutAlgorithm = value;
				this.AssertValid();
			}
		}
		[Category("Node Borders"), Description("The padding that is added to the rectangles for top-level nodes.")]
		public int PaddingPx
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.PaddingPx;
			}
			set
			{
				this.m_oTreemapGenerator.PaddingPx = value;
				this.AssertValid();
			}
		}
		[Description("The number of pixels that is subtracted from the padding at each node level."), Category("Node Borders")]
		public int PaddingDecrementPerLevelPx
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.PaddingDecrementPerLevelPx;
			}
			set
			{
				this.m_oTreemapGenerator.PaddingDecrementPerLevelPx = value;
				this.AssertValid();
			}
		}
		[Category("Node Borders"), Description("The pen width that is used to draw the rectangles for the top-level nodes.")]
		public int PenWidthPx
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.PenWidthPx;
			}
			set
			{
				this.m_oTreemapGenerator.PenWidthPx = value;
				this.AssertValid();
			}
		}
		[Description("The number of pixels that is subtracted from the pen width at each node level."), Category("Node Borders")]
		public int PenWidthDecrementPerLevelPx
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.PenWidthDecrementPerLevelPx;
			}
			set
			{
				this.m_oTreemapGenerator.PenWidthDecrementPerLevelPx = value;
				this.AssertValid();
			}
		}
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				this.m_oTreemapGenerator.BackColor = value;
				base.BackColor = value;
				this.AssertValid();
			}
		}
		[Description("The color of rectangle borders."), Category("Node Borders")]
		public Color BorderColor
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.BorderColor;
			}
			set
			{
				this.m_oTreemapGenerator.BorderColor = value;
				this.AssertValid();
			}
		}
		[Category("Node Fill Colors"), Description("The algorithm used to color the treemap's rectangles.")]
		public NodeColorAlgorithm NodeColorAlgorithm
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.NodeColorAlgorithm;
			}
			set
			{
				this.m_oTreemapGenerator.NodeColorAlgorithm = value;
				this.AssertValid();
			}
		}
		[Category("Node Fill Colors"), Description("The maximum negative fill color.")]
		public Color MinColor
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.MinColor;
			}
			set
			{
				this.m_oTreemapGenerator.MinColor = value;
				this.AssertValid();
			}
		}
		[Description("The maximum positive fill color."), Category("Node Fill Colors")]
		public Color MaxColor
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.MaxColor;
			}
			set
			{
				this.m_oTreemapGenerator.MaxColor = value;
				this.AssertValid();
			}
		}
		[Category("Node Fill Colors"), Description("The Node.ColorMetric value to map to MinColor.")]
		public float MinColorMetric
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.MinColorMetric;
			}
			set
			{
				this.m_oTreemapGenerator.MinColorMetric = value;
				this.AssertValid();
			}
		}
		[Category("Node Fill Colors"), Description("The Node.ColorMetric value to map to MaxColor.")]
		public float MaxColorMetric
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.MaxColorMetric;
			}
			set
			{
				this.m_oTreemapGenerator.MaxColorMetric = value;
				this.AssertValid();
			}
		}
		[Category("Node Fill Colors"), Description("The number of discrete fill colors to use in the negative color range.")]
		public int DiscreteNegativeColors
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.DiscreteNegativeColors;
			}
			set
			{
				this.m_oTreemapGenerator.DiscreteNegativeColors = value;
				this.AssertValid();
			}
		}
		[Description("The number of discrete fill colors to use in the positive color range."), Category("Node Fill Colors")]
		public int DiscretePositiveColors
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.DiscretePositiveColors;
			}
			set
			{
				this.m_oTreemapGenerator.DiscretePositiveColors = value;
				this.AssertValid();
			}
		}
		[Description("The font family to use for drawing node text."), Category("Node Text")]
		public string FontFamily
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.FontFamily;
			}
			set
			{
				this.m_oTreemapGenerator.FontFamily = value;
				this.AssertValid();
			}
		}
		[Description("The solid color to use for unselected node text."), Category("Node Text")]
		public Color FontSolidColor
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.FontSolidColor;
			}
			set
			{
				this.m_oTreemapGenerator.FontSolidColor = value;
				this.AssertValid();
			}
		}
		[Category("Node Text"), Description("The color to use for selected node text.")]
		public Color SelectedFontColor
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.SelectedFontColor;
			}
			set
			{
				this.m_oTreemapGenerator.SelectedFontColor = value;
				this.AssertValid();
			}
		}
		[Description("The color to use to highlight the selected node."), Category("Node Text")]
		public Color SelectedBackColor
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.SelectedBackColor;
			}
			set
			{
				this.m_oTreemapGenerator.SelectedBackColor = value;
				this.AssertValid();
			}
		}
		[Description("The node levels to show text for."), Category("Node Text")]
		public NodeLevelsWithText NodeLevelsWithText
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.NodeLevelsWithText;
			}
			set
			{
				this.m_oTreemapGenerator.NodeLevelsWithText = value;
				this.AssertValid();
			}
		}
		[Category("Node Text"), Description("The location within a node's rectangle where the node's text is shown.")]
		public TextLocation TextLocation
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.TextLocation;
			}
			set
			{
				this.m_oTreemapGenerator.TextLocation = value;
				this.AssertValid();
			}
		}
		[Description("The location within a node's rectangle where the node's empty space is shown."), Category("Layout")]
		public EmptySpaceLocation EmptySpaceLocation
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.EmptySpaceLocation;
			}
			set
			{
				this.m_oTreemapGenerator.EmptySpaceLocation = value;
				this.AssertValid();
			}
		}
		[Browsable(false), ReadOnly(true)]
		public Node SelectedNode
		{
			get
			{
				this.AssertValid();
				return this.m_oTreemapGenerator.SelectedNode;
			}
		}
		[Category("Node Text"), Description("Indicates whether tooltips should be shown.")]
		public bool ShowToolTips
		{
			get
			{
				this.AssertValid();
				return this.m_bShowToolTips;
			}
			set
			{
				if (!value)
				{
					this.m_oToolTipTracker.Reset();
				}
				this.m_bShowToolTips = value;
				this.AssertValid();
			}
		}
		[Description("Indicates whether a node can be dragged out of the control."), Category("Behavior")]
		public bool AllowDrag
		{
			get
			{
				this.AssertValid();
				return this.m_bAllowDrag;
			}
			set
			{
				this.m_bAllowDrag = value;
				this.AssertValid();
			}
		}
		[Category("Zooming"), Description("Indicates whether the treemap can be zoomed.")]
		public bool IsZoomable
		{
			get
			{
				this.AssertValid();
				return this.m_bIsZoomable;
			}
			set
			{
				if (value != this.m_bIsZoomable)
				{
					this.m_bIsZoomable = value;
					if (this.m_bIsZoomable)
					{
						Debug.Assert(this.m_oZoomActionHistoryList == null);
						this.m_oZoomActionHistoryList = new ZoomActionHistoryList();
						this.m_oZoomActionHistoryList.Change += new HistoryList.ChangeEventHandler(this.ZoomActionHistoryList_Change);
					}
					else
					{
						Debug.Assert(this.m_oZoomActionHistoryList != null);
						this.m_oZoomActionHistoryList.Change -= new HistoryList.ChangeEventHandler(this.ZoomActionHistoryList_Change);
						this.m_oZoomActionHistoryList = null;
					}
				}
				this.AssertValid();
			}
		}
		[ReadOnly(true), Browsable(false)]
		public Bitmap Bitmap
		{
			get
			{
				this.AssertValid();
				return this.m_oBitmap;
			}
		}
		public TreemapControl()
		{
			this.InitializeComponent();
			base.Controls.Add(this.picPictureBox);
			base.Controls.Add(this.pnlToolTip);
			this.pnlToolTip.BringToFront();
			this.m_oTreemapGenerator = new TreemapGenerator();
			this.m_oTreemapGenerator.RedrawRequired += new EventHandler(this.TreemapGenerator_RedrawRequired);
			this.m_oBitmap = null;
			this.m_bShowToolTips = true;
			this.m_bAllowDrag = false;
			this.m_bIsZoomable = false;
			this.m_oZoomActionHistoryList = null;
			this.m_oToolTipTracker = new ToolTipTracker();
			this.m_oToolTipTracker.ShowToolTip += new ToolTipTracker.ToolTipTrackerEvent(this.oToolTipTracker_ShowToolTip);
			this.m_oToolTipTracker.HideToolTip += new ToolTipTracker.ToolTipTrackerEvent(this.oToolTipTracker_HideToolTip);
			this.m_oLastMouseMovePoint = new Point(-1, -1);
			this.m_oLastDraggableMouseDownPoint = new Point(-1, -1);
			base.ResizeRedraw = true;
		}
		public void GetNodeLevelsWithTextRange(out int minLevel, out int maxLevel)
		{
			this.AssertValid();
			this.m_oTreemapGenerator.GetNodeLevelsWithTextRange(out minLevel, out maxLevel);
		}
		public void SetNodeLevelsWithTextRange(int minLevel, int maxLevel)
		{
			this.m_oTreemapGenerator.SetNodeLevelsWithTextRange(minLevel, maxLevel);
			this.AssertValid();
		}
		public void GetFontSizeRange(out float minSizePt, out float maxSizePt, out float incrementPt)
		{
			this.AssertValid();
			this.m_oTreemapGenerator.GetFontSizeRange(out minSizePt, out maxSizePt, out incrementPt);
		}
		public void SetFontSizeRange(float minSizePt, float maxSizePt, float incrementPt)
		{
			this.m_oTreemapGenerator.SetFontSizeRange(minSizePt, maxSizePt, incrementPt);
		}
		public void GetFontAlphaRange(out int minAlpha, out int maxAlpha, out int incrementPerLevel)
		{
			this.AssertValid();
			this.m_oTreemapGenerator.GetFontAlphaRange(out minAlpha, out maxAlpha, out incrementPerLevel);
		}
		public void SetFontAlphaRange(int minAlpha, int maxAlpha, int incrementPerLevel)
		{
			this.m_oTreemapGenerator.SetFontAlphaRange(minAlpha, maxAlpha, incrementPerLevel);
			this.AssertValid();
		}
		public void BeginUpdate()
		{
			this.AssertValid();
			this.m_oTreemapGenerator.BeginUpdate();
		}
		public void EndUpdate()
		{
			this.AssertValid();
			this.m_oTreemapGenerator.EndUpdate();
		}
		public void Clear()
		{
			this.AssertValid();
			Node selectedNode = this.SelectedNode;
			this.m_oTreemapGenerator.Clear();
			if (this.m_bIsZoomable)
			{
				Debug.Assert(this.m_oZoomActionHistoryList != null);
				this.m_oZoomActionHistoryList.Reset();
			}
			if (selectedNode != null)
			{
				this.FireSelectedNodeChanged();
			}
		}
		public bool CanZoomIn(Node node)
		{
			this.AssertValid();
			this.VerifyIsZoomable("CanZoomIn");
			Nodes nodes = this.Nodes;
			int count = nodes.Count;
			return count != 0 && (count != 1 || node != nodes[0]);
		}
		public bool CanZoomOut()
		{
			this.AssertValid();
			this.VerifyIsZoomable("CanZoomOut");
			bool result;
			if (!this.m_oZoomActionHistoryList.HasCurrentState)
			{
				result = false;
			}
			else
			{
				ZoomAction peekCurrentState = this.m_oZoomActionHistoryList.PeekCurrentState;
				result = peekCurrentState.CanZoomOutFromZoomedNode();
			}
			return result;
		}
		public bool CanMoveBack()
		{
			this.AssertValid();
			this.VerifyIsZoomable("CanMoveBack");
			Debug.Assert(this.m_oZoomActionHistoryList != null);
			return this.m_oZoomActionHistoryList.HasCurrentState;
		}
		public bool CanMoveForward()
		{
			this.AssertValid();
			this.VerifyIsZoomable("CanMoveForward");
			Debug.Assert(this.m_oZoomActionHistoryList != null);
			return this.m_oZoomActionHistoryList.HasNextState;
		}
		public void ZoomIn(Node node)
		{
			Debug.Assert(node != null);
			this.AssertValid();
			this.VerifyIsZoomable("ZoomIn");
			if (!this.CanZoomIn(node))
			{
				throw new InvalidOperationException("TreemapControl.ZoomIn: Can't zoom in to node.  Check the CanZoomIn property first.");
			}
			Nodes nodes = this.Nodes;
			Debug.Assert(nodes.Count > 0);
			ZoomAction oState = null;
			if (nodes.Count > 1 || !this.m_oZoomActionHistoryList.HasCurrentState)
			{
				oState = new ZoomedFromTopLevelAction(this.m_oZoomActionHistoryList, node, nodes);
			}
			else
			{
				Debug.Assert(nodes.Count == 1);
				Debug.Assert(this.m_oZoomActionHistoryList.HasCurrentState);
				Node node2 = nodes[0];
				ZoomAction peekCurrentState = this.m_oZoomActionHistoryList.PeekCurrentState;
				if (peekCurrentState.ParentOfZoomedNode == null)
				{
					if (this.m_oZoomActionHistoryList.OriginalTopLevelNodes.Length > 1)
					{
						oState = new ZoomedFromOneTopLevelNodeAction(this.m_oZoomActionHistoryList, node, node2);
					}
					else
					{
						oState = new ZoomedFromTopLevelAction(this.m_oZoomActionHistoryList, node, nodes);
					}
				}
				else
				{
					Debug.Assert(peekCurrentState.ParentOfZoomedNode != null);
					node2.PrivateSetParent(peekCurrentState.ParentOfZoomedNode);
					oState = new ZoomedFromInnerNodeAction(this.m_oZoomActionHistoryList, node, node2);
				}
			}
			this.m_oZoomActionHistoryList.InsertState(oState);
			Node selectedNode = this.SelectedNode;
			this.m_oTreemapGenerator.Clear();
			nodes.Add(node);
			if (selectedNode != null)
			{
				this.FireSelectedNodeChanged();
			}
		}
		public void ZoomOut()
		{
			this.AssertValid();
			this.VerifyIsZoomable("ZoomOut");
			if (!this.CanZoomOut())
			{
				throw new InvalidOperationException("TreemapControl.ZoomOut: Can't zoom out.  Check the CanZoomOut property first.");
			}
			Nodes nodes = this.Nodes;
			Debug.Assert(nodes.Count == 1);
			Node node = nodes[0];
			Node[] array = null;
			float sizeMetric = -3.40282347E+38f;
			ZoomAction oState = null;
			Debug.Assert(this.m_oZoomActionHistoryList.HasCurrentState);
			ZoomAction peekCurrentState = this.m_oZoomActionHistoryList.PeekCurrentState;
			if (peekCurrentState.ParentOfZoomedNode == null)
			{
				array = this.m_oZoomActionHistoryList.OriginalTopLevelNodes;
				sizeMetric = this.m_oZoomActionHistoryList.OriginalTopLevelEmptySpaceSizeMetric;
				oState = new ZoomedFromOneTopLevelNodeAction(this.m_oZoomActionHistoryList, null, node);
			}
			else
			{
				Node parentOfZoomedNode = peekCurrentState.ParentOfZoomedNode;
				array = new Node[]
				{
					parentOfZoomedNode
				};
				sizeMetric = 0f;
				Debug.Assert(parentOfZoomedNode != null);
				node.PrivateSetParent(parentOfZoomedNode);
				oState = new ZoomedFromInnerNodeAction(this.m_oZoomActionHistoryList, parentOfZoomedNode, node);
			}
			this.m_oZoomActionHistoryList.InsertState(oState);
			Node selectedNode = this.SelectedNode;
			this.m_oTreemapGenerator.Clear();
			this.BeginUpdate();
			Node[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Node node2 = array2[i];
				nodes.Add(node2);
			}
			nodes.EmptySpace.SizeMetric = sizeMetric;
			this.EndUpdate();
			if (selectedNode != null)
			{
				this.FireSelectedNodeChanged();
			}
		}
		public void MoveBack()
		{
			this.AssertValid();
			this.VerifyIsZoomable("MoveBack");
			if (!this.CanMoveBack())
			{
				throw new InvalidOperationException("TreemapControl.MoveBack: Can't move back.  Check the CanMoveBack property first.");
			}
			Debug.Assert(this.m_oZoomActionHistoryList.HasCurrentState);
			ZoomAction currentState = this.m_oZoomActionHistoryList.CurrentState;
			Node selectedNode = this.SelectedNode;
			currentState.Undo(this.m_oTreemapGenerator);
			if (selectedNode != null)
			{
				this.FireSelectedNodeChanged();
			}
		}
		public void MoveForward()
		{
			this.AssertValid();
			this.VerifyIsZoomable("MoveForward");
			if (!this.CanMoveForward())
			{
				throw new InvalidOperationException("TreemapControl.MoveForward: Can't move forward.  Check the CanMoveForward property first.");
			}
			Debug.Assert(this.m_oZoomActionHistoryList.HasNextState);
			ZoomAction zoomAction = (ZoomAction)this.m_oZoomActionHistoryList.NextState;
			Node selectedNode = this.SelectedNode;
			zoomAction.Redo(this.m_oTreemapGenerator);
			if (selectedNode != null)
			{
				this.FireSelectedNodeChanged();
			}
		}
		public void Draw(Graphics graphics, Rectangle treemapRectangle)
		{
			Debug.Assert(graphics != null);
			Debug.Assert(!treemapRectangle.IsEmpty);
			this.AssertValid();
			this.m_oTreemapGenerator.Draw(graphics, treemapRectangle);
			base.Invalidate();
		}
		public void SelectNode(Node node)
		{
			this.AssertValid();
			if (node != this.SelectedNode)
			{
				this.m_oTreemapGenerator.SelectNode(node, this.m_oBitmap);
				if (this.m_oBitmap != null)
				{
					this.picPictureBox.Image = this.m_oBitmap;
				}
				this.FireSelectedNodeChanged();
			}
		}
		protected void Draw(bool bRetainSelection)
		{
			this.AssertValid();
			this.m_oToolTipTracker.Reset();
			this.m_oLastMouseMovePoint = new Point(-1, -1);
			Size bitmapSizeToDraw = this.GetBitmapSizeToDraw();
			if (bitmapSizeToDraw.Width != 0 && bitmapSizeToDraw.Height != 0)
			{
				if (this.m_oBitmap != null)
				{
					this.m_oBitmap.Dispose();
				}
				Graphics graphics = base.CreateGraphics();
				try
				{
					this.m_oBitmap = new Bitmap(bitmapSizeToDraw.Width, bitmapSizeToDraw.Height, graphics);
				}
				catch (ArgumentException ex)
				{
					this.m_oBitmap = null;
					throw new InvalidOperationException("The treemap image could not be created.  It may be too large.  (Its size is " + bitmapSizeToDraw + ".)", ex);
				}
				finally
				{
					graphics.Dispose();
				}
				this.m_oTreemapGenerator.Draw(this.m_oBitmap, bRetainSelection);
				this.picPictureBox.Size = bitmapSizeToDraw;
				this.picPictureBox.Location = new Point(0);
				this.picPictureBox.Image = this.m_oBitmap;
			}
		}
		protected PointF GetClientMousePosition()
		{
			return ControlUtil.GetClientMousePosition(this);
		}
		protected Size GetBitmapSizeToDraw()
		{
			bool autoScroll = this.AutoScroll;
			this.AutoScroll = false;
			Size clientSize = base.ClientSize;
			this.AutoScroll = autoScroll;
			Size result;
			if (!this.AutoScroll)
			{
				result = clientSize;
			}
			else
			{
				result = base.AutoScrollMinSize;
			}
			return result;
		}
		protected override void OnPaintBackground(PaintEventArgs oPaintEventArgs)
		{
		}
		protected override void OnPaint(PaintEventArgs oPaintEventArgs)
		{
			this.AssertValid();
			Debug.Assert(oPaintEventArgs != null);
			this.Draw(true);
		}
		protected void ShowToolTip(Node oNode)
		{
			Debug.Assert(this.m_bShowToolTips);
			Debug.Assert(oNode != null);
			string toolTip = oNode.ToolTip;
			if (toolTip != null && !(toolTip == ""))
			{
				this.pnlToolTip.ShowToolTip(toolTip, this);
			}
		}
		protected void HideToolTip()
		{
			this.pnlToolTip.HideToolTip();
			this.AssertValid();
		}
		protected void VerifyIsZoomable(string sMethodName)
		{
			Debug.Assert(sMethodName != null);
			Debug.Assert(sMethodName.Length > 0);
			if (!this.IsZoomable)
			{
				throw new InvalidOperationException(string.Format("TreemapControl.{0}: This can't be used if the IsZoomable property is false.", sMethodName));
			}
		}
		protected void FireSelectedNodeChanged()
		{
			this.AssertValid();
			if (this.SelectedNodeChanged != null)
			{
				this.SelectedNodeChanged.Invoke(this, EventArgs.Empty);
			}
		}
		protected override void Dispose(bool bDisposing)
		{
			if (bDisposing)
			{
				if (this.components != null)
				{
					this.components.Dispose();
				}
				if (this.m_oBitmap != null)
				{
					this.m_oBitmap.Dispose();
					this.m_oBitmap = null;
				}
				if (this.m_oToolTipTracker != null)
				{
					this.m_oToolTipTracker.Dispose();
					this.m_oToolTipTracker = null;
				}
			}
			base.Dispose(bDisposing);
		}
		protected void picPictureBox_MouseDown(object oSource, MouseEventArgs oMouseEventArgs)
		{
			Node node;
			if (this.m_oTreemapGenerator.GetNodeFromPoint(oMouseEventArgs.X, oMouseEventArgs.Y, out node))
			{
				this.SelectNode(node);
				if (this.NodeMouseDown != null)
				{
					this.NodeMouseDown(this, new NodeMouseEventArgs(oMouseEventArgs, node));
				}
				if (oMouseEventArgs.Clicks == 2)
				{
					if (this.NodeDoubleClick != null)
					{
						this.NodeDoubleClick(this, new NodeEventArgs(node));
					}
				}
				else
				{
					if (this.m_bAllowDrag && (oMouseEventArgs.Button & MouseButtons.Left) == MouseButtons.Left)
					{
						this.m_oLastDraggableMouseDownPoint = new Point(oMouseEventArgs.X, oMouseEventArgs.Y);
					}
				}
			}
		}
		protected void picPictureBox_MouseUp(object oSource, MouseEventArgs oMouseEventArgs)
		{
			Node oNode;
			if (this.m_oTreemapGenerator.GetNodeFromPoint(oMouseEventArgs.X, oMouseEventArgs.Y, out oNode) && this.NodeMouseUp != null)
			{
				NodeMouseEventArgs nodeMouseEventArgs = new NodeMouseEventArgs(oMouseEventArgs, oNode);
				this.NodeMouseUp(this, nodeMouseEventArgs);
			}
			this.m_oLastDraggableMouseDownPoint = new Point(-1, -1);
		}
		private void picPictureBox_MouseMove(object oSource, MouseEventArgs oMouseEventArgs)
		{
			if (oMouseEventArgs.X != this.m_oLastMouseMovePoint.X || oMouseEventArgs.Y != this.m_oLastMouseMovePoint.Y)
			{
				this.m_oLastMouseMovePoint = new Point(oMouseEventArgs.X, oMouseEventArgs.Y);
				if (!this.pnlToolTip.Visible || !new Rectangle(this.pnlToolTip.Location, this.pnlToolTip.Size).Contains(this.m_oLastMouseMovePoint))
				{
					Node node;
					this.m_oTreemapGenerator.GetNodeFromPoint(oMouseEventArgs.X, oMouseEventArgs.Y, out node);
					this.m_oToolTipTracker.OnMouseMoveOverObject(node);
					if (this.m_oLastDraggableMouseDownPoint != new Point(-1, -1))
					{
						int x = this.m_oLastDraggableMouseDownPoint.X;
						int y = this.m_oLastDraggableMouseDownPoint.Y;
						if (Math.Abs(oMouseEventArgs.X - x) >= SystemInformation.DragSize.Width / 2 || Math.Abs(oMouseEventArgs.Y - y) >= SystemInformation.DragSize.Height / 2)
						{
							bool nodeFromPoint = this.m_oTreemapGenerator.GetNodeFromPoint(x, y, out node);
							Debug.Assert(nodeFromPoint);
							Debug.Assert(node != null);
							object obj = node.Tag;
							if (obj == null)
							{
								obj = string.Empty;
							}
							base.DoDragDrop(obj, DragDropEffects.Copy);
						}
					}
				}
			}
		}
		private void picPictureBox_MouseLeave(object oSource, EventArgs oEventArgs)
		{
			this.m_oToolTipTracker.OnMouseMoveOverObject(null);
			this.m_oLastDraggableMouseDownPoint = new Point(-1, -1);
		}
		private void oToolTipTracker_ShowToolTip(object oSource, ToolTipTrackerEventArgs oToolTipTrackerEventArgs)
		{
			Node node = (Node)oToolTipTrackerEventArgs.Object;
			Debug.Assert(node != null);
			if (this.m_bShowToolTips)
			{
				this.ShowToolTip(node);
			}
			if (this.NodeMouseHover != null)
			{
				this.NodeMouseHover(this, new NodeEventArgs(node));
			}
		}
		private void oToolTipTracker_HideToolTip(object oSource, ToolTipTrackerEventArgs oToolTipTrackerEventArgs)
		{
			if (this.m_bShowToolTips)
			{
				this.HideToolTip();
			}
		}
		private void ZoomActionHistoryList_Change(object oSender, EventArgs oEventArgs)
		{
			if (this.ZoomStateChanged != null)
			{
				this.ZoomStateChanged.Invoke(this, EventArgs.Empty);
			}
		}
		protected void TreemapGenerator_RedrawRequired(object oSender, EventArgs oEventArgs)
		{
			base.Invalidate();
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_oTreemapGenerator != null);
			this.m_oTreemapGenerator.AssertValid();
			if (this.m_bIsZoomable)
			{
				Debug.Assert(this.m_oZoomActionHistoryList != null);
			}
			else
			{
				Debug.Assert(this.m_oZoomActionHistoryList == null);
			}
			Debug.Assert(this.m_oToolTipTracker != null);
		}
		private void InitializeComponent()
		{
			this.picPictureBox = new PictureBox();
			this.pnlToolTip = new ToolTipPanel();
			this.picPictureBox.Location = new Point(126, 17);
			this.picPictureBox.Name = "picPictureBox";
			this.picPictureBox.TabIndex = 0;
			this.picPictureBox.TabStop = false;
			this.picPictureBox.MouseUp += new MouseEventHandler(this.picPictureBox_MouseUp);
			this.picPictureBox.MouseMove += new MouseEventHandler(this.picPictureBox_MouseMove);
			this.picPictureBox.MouseLeave += new EventHandler(this.picPictureBox_MouseLeave);
			this.picPictureBox.MouseDown += new MouseEventHandler(this.picPictureBox_MouseDown);
			this.pnlToolTip.BackColor = SystemColors.Window;
			this.pnlToolTip.BorderStyle = BorderStyle.FixedSingle;
			this.pnlToolTip.Name = "pnlToolTip";
			this.pnlToolTip.TabIndex = 0;
		}
	}
}
