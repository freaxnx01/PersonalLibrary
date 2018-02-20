using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.GraphicsLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class CenterCenterTextDrawer : TextDrawerBase
	{
		protected string m_sFontFamily;
		protected float m_fFontMinSizePt;
		protected float m_fFontMaxSizePt;
		protected float m_fFontIncrementPt;
		protected Color m_oFontSolidColor;
		protected int m_iFontMinAlpha;
		protected int m_iFontMaxAlpha;
		protected int m_iFontAlphaIncrementPerLevel;
		protected Color m_oSelectedFontColor;
		public CenterCenterTextDrawer(NodeLevelsWithText eNodeLevelsWithText, int iMinNodeLevelWithText, int iMaxNodeLevelWithText, string sFontFamily, float fFontMinSizePt, float fFontMaxSizePt, float fFontIncrementPt, Color oFontSolidColor, int iFontMinAlpha, int iFontMaxAlpha, int iFontAlphaIncrementPerLevel, Color oSelectedFontColor) : base(eNodeLevelsWithText, iMinNodeLevelWithText, iMaxNodeLevelWithText)
		{
			this.m_sFontFamily = sFontFamily;
			this.m_fFontMinSizePt = fFontMinSizePt;
			this.m_fFontMaxSizePt = fFontMaxSizePt;
			this.m_fFontIncrementPt = fFontIncrementPt;
			this.m_oFontSolidColor = oFontSolidColor;
			this.m_iFontMinAlpha = iFontMinAlpha;
			this.m_iFontMaxAlpha = iFontMaxAlpha;
			this.m_iFontAlphaIncrementPerLevel = iFontAlphaIncrementPerLevel;
			this.m_oSelectedFontColor = oSelectedFontColor;
			this.AssertValid();
		}
		public override void DrawTextForAllNodes(Graphics oGraphics, Rectangle oTreemapRectangle, Nodes oNodes)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oNodes != null);
			this.AssertValid();
			IFontMapper fontMapper = null;
			TransparentBrushMapper transparentBrushMapper = null;
			TextRenderingHint textRenderingHint = oGraphics.TextRenderingHint;
			try
			{
				fontMapper = this.CreateFontMapper(oGraphics);
				transparentBrushMapper = this.CreateTransparentBrushMapper();
				StringFormat oStringFormat = this.CreateStringFormat();
				this.DrawTextForNodes(oNodes, oGraphics, fontMapper, oStringFormat, transparentBrushMapper, 0);
			}
			finally
			{
				if (fontMapper != null)
				{
					fontMapper.Dispose();
				}
				if (transparentBrushMapper != null)
				{
					transparentBrushMapper.Dispose();
				}
				oGraphics.TextRenderingHint = textRenderingHint;
			}
		}
		public override void DrawTextForSelectedNode(Graphics oGraphics, Node oSelectedNode)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oSelectedNode != null);
			this.AssertValid();
			IFontMapper fontMapper = null;
			Brush brush = null;
			TextRenderingHint textRenderingHint = TextRenderingHint.SystemDefault;
			try
			{
				fontMapper = this.CreateFontMapper(oGraphics);
				brush = new SolidBrush(this.m_oSelectedFontColor);
				Font font;
				string s;
				if (fontMapper.NodeToFont(oSelectedNode, oSelectedNode.Level, oGraphics, out font, out s))
				{
					textRenderingHint = base.SetTextRenderingHint(oGraphics, font);
					StringFormat format = this.CreateStringFormat();
					oGraphics.DrawString(s, font, brush, oSelectedNode.Rectangle, format);
				}
			}
			finally
			{
				if (fontMapper != null)
				{
					fontMapper.Dispose();
				}
				if (brush != null)
				{
					brush.Dispose();
				}
				oGraphics.TextRenderingHint = textRenderingHint;
			}
		}
		protected void DrawTextForNodes(Nodes oNodes, Graphics oGraphics, IFontMapper oFontMapper, StringFormat oStringFormat, TransparentBrushMapper oTransparentBrushMapper, int iNodeLevel)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(oGraphics != null);
			Debug.Assert(oFontMapper != null);
			Debug.Assert(oStringFormat != null);
			Debug.Assert(oTransparentBrushMapper != null);
			Debug.Assert(iNodeLevel >= 0);
			foreach (Node current in oNodes)
			{
				RectangleF rectangle = current.Rectangle;
				if (!rectangle.IsEmpty)
				{
					Font font;
					string s;
					if (base.TextShouldBeDrawnForNode(current, iNodeLevel) && oFontMapper.NodeToFont(current, iNodeLevel, oGraphics, out font, out s))
					{
						base.SetTextRenderingHint(oGraphics, font);
						Brush brush = oTransparentBrushMapper.LevelToTransparentBrush(iNodeLevel);
						oGraphics.DrawString(s, font, brush, rectangle, oStringFormat);
					}
					this.DrawTextForNodes(current.Nodes, oGraphics, oFontMapper, oStringFormat, oTransparentBrushMapper, iNodeLevel + 1);
				}
			}
		}
		protected IFontMapper CreateFontMapper(Graphics oGraphics)
		{
			Debug.Assert(oGraphics != null);
			this.AssertValid();
			return new MaximizingFontMapper(this.m_sFontFamily, this.m_fFontMinSizePt, this.m_fFontMaxSizePt, this.m_fFontIncrementPt, oGraphics);
		}
		protected TransparentBrushMapper CreateTransparentBrushMapper()
		{
			TransparentBrushMapper transparentBrushMapper = new TransparentBrushMapper();
			transparentBrushMapper.Initialize(this.m_oFontSolidColor, this.m_iFontMinAlpha, this.m_iFontMaxAlpha, this.m_iFontAlphaIncrementPerLevel);
			return transparentBrushMapper;
		}
		protected StringFormat CreateStringFormat()
		{
			return new StringFormat
			{
				Alignment = StringAlignment.Center, 
				LineAlignment = StringAlignment.Center
			};
		}
		public override void AssertValid()
		{
			base.AssertValid();
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
		}
	}
}
