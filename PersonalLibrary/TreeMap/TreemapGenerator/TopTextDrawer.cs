using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class TopTextDrawer : TextDrawerBase
	{
		protected const float TextHeightMultiplier = 1.1f;
		protected string m_sFontFamily;
		protected float m_fFontSizePt;
		protected int m_iMinimumTextHeight;
		protected Color m_oTextColor;
		protected Color m_oSelectedFontColor;
		protected Color m_oSelectedBackColor;
		public TopTextDrawer(NodeLevelsWithText eNodeLevelsWithText, int iMinNodeLevelWithText, int iMaxNodeLevelWithText, string sFontFamily, float fFontSizePt, int iMinimumTextHeight, Color oTextColor, Color oSelectedFontColor, Color oSelectedBackColor) : base(eNodeLevelsWithText, iMinNodeLevelWithText, iMaxNodeLevelWithText)
		{
			this.m_sFontFamily = sFontFamily;
			this.m_fFontSizePt = fFontSizePt;
			this.m_iMinimumTextHeight = iMinimumTextHeight;
			this.m_oTextColor = oTextColor;
			this.m_oSelectedFontColor = oSelectedFontColor;
			this.m_oSelectedBackColor = oSelectedBackColor;
			this.AssertValid();
		}
		public override void DrawTextForAllNodes(Graphics oGraphics, Rectangle oTreemapRectangle, Nodes oNodes)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oNodes != null);
			this.AssertValid();
			FontForRectangle fontForRectangle = null;
			SolidBrush solidBrush = null;
			TextRenderingHint textRenderingHint = TextRenderingHint.SystemDefault;
			try
			{
				fontForRectangle = new FontForRectangle(this.m_sFontFamily, this.m_fFontSizePt, oGraphics);
				textRenderingHint = base.SetTextRenderingHint(oGraphics, fontForRectangle.Font);
				solidBrush = new SolidBrush(this.m_oTextColor);
				StringFormat oNonLeafStringFormat = this.CreateStringFormat(false);
				StringFormat oLeafStringFormat = this.CreateStringFormat(true);
				int textHeight = TopTextDrawer.GetTextHeight(oGraphics, fontForRectangle.Font, this.m_iMinimumTextHeight);
				this.DrawTextForNodes(oNodes, oGraphics, fontForRectangle, textHeight, solidBrush, null, oNonLeafStringFormat, oLeafStringFormat, 0);
			}
			finally
			{
				if (solidBrush != null)
				{
					solidBrush.Dispose();
				}
				if (fontForRectangle != null)
				{
					fontForRectangle.Dispose();
				}
				oGraphics.TextRenderingHint = textRenderingHint;
			}
		}
		public override void DrawTextForSelectedNode(Graphics oGraphics, Node oSelectedNode)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oSelectedNode != null);
			this.AssertValid();
			FontForRectangle fontForRectangle = null;
			SolidBrush solidBrush = null;
			SolidBrush solidBrush2 = null;
			TextRenderingHint textRenderingHint = TextRenderingHint.SystemDefault;
			try
			{
				fontForRectangle = new FontForRectangle(this.m_sFontFamily, this.m_fFontSizePt, oGraphics);
				textRenderingHint = base.SetTextRenderingHint(oGraphics, fontForRectangle.Font);
				solidBrush = new SolidBrush(this.m_oSelectedFontColor);
				solidBrush2 = new SolidBrush(this.m_oSelectedBackColor);
				StringFormat oNonLeafStringFormat = this.CreateStringFormat(false);
				StringFormat oLeafStringFormat = this.CreateStringFormat(true);
				int textHeight = TopTextDrawer.GetTextHeight(oGraphics, fontForRectangle.Font, this.m_iMinimumTextHeight);
				this.DrawTextForNode(oGraphics, oSelectedNode, fontForRectangle, textHeight, solidBrush, solidBrush2, oNonLeafStringFormat, oLeafStringFormat);
			}
			finally
			{
				if (solidBrush != null)
				{
					solidBrush.Dispose();
				}
				if (solidBrush2 != null)
				{
					solidBrush2.Dispose();
				}
				if (fontForRectangle != null)
				{
					fontForRectangle.Dispose();
				}
				oGraphics.TextRenderingHint = textRenderingHint;
			}
		}
		public static int GetTextHeight(Graphics oGraphics, string sFontFamily, float fFontSizePt, int iMinimumTextHeight)
		{
			Debug.Assert(oGraphics != null);
			StringUtil.AssertNotEmpty(sFontFamily);
			Debug.Assert(fFontSizePt > 0f);
			Debug.Assert(iMinimumTextHeight >= 0);
			FontForRectangle fontForRectangle = null;
			int textHeight;
			try
			{
				fontForRectangle = new FontForRectangle(sFontFamily, fFontSizePt, oGraphics);
				textHeight = TopTextDrawer.GetTextHeight(oGraphics, fontForRectangle.Font, iMinimumTextHeight);
			}
			finally
			{
				if (fontForRectangle != null)
				{
					fontForRectangle.Dispose();
				}
			}
			return textHeight;
		}
		protected static int GetTextHeight(Graphics oGraphics, Font oFont, int iMinimumTextHeight)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oFont != null);
			Debug.Assert(iMinimumTextHeight >= 0);
			int num = (int)Math.Ceiling((double)(1.1f * oFont.GetHeight(oGraphics)));
			return Math.Max(num, iMinimumTextHeight);
		}
		protected void DrawTextForNodes(Nodes oNodes, Graphics oGraphics, FontForRectangle oFontForRectangle, int iTextHeight, Brush oTextBrush, Brush oBackgroundBrush, StringFormat oNonLeafStringFormat, StringFormat oLeafStringFormat, int iNodeLevel)
		{
			Debug.Assert(oNodes != null);
			Debug.Assert(oGraphics != null);
			Debug.Assert(oFontForRectangle != null);
			Debug.Assert(iTextHeight >= 0);
			Debug.Assert(oTextBrush != null);
			Debug.Assert(oNonLeafStringFormat != null);
			Debug.Assert(oLeafStringFormat != null);
			Debug.Assert(iNodeLevel >= 0);
			foreach (Node current in oNodes)
			{
				if (!current.Rectangle.IsEmpty)
				{
					if (base.TextShouldBeDrawnForNode(current, iNodeLevel))
					{
						this.DrawTextForNode(oGraphics, current, oFontForRectangle, iTextHeight, oTextBrush, oBackgroundBrush, oNonLeafStringFormat, oLeafStringFormat);
					}
					this.DrawTextForNodes(current.Nodes, oGraphics, oFontForRectangle, iTextHeight, oTextBrush, oBackgroundBrush, oNonLeafStringFormat, oLeafStringFormat, iNodeLevel + 1);
				}
			}
		}
		protected void DrawTextForNode(Graphics oGraphics, Node oNode, FontForRectangle oFontForRectangle, int iTextHeight, Brush oTextBrush, Brush oBackgroundBrush, StringFormat oNonLeafStringFormat, StringFormat oLeafStringFormat)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oNode != null);
			Debug.Assert(oFontForRectangle != null);
			Debug.Assert(iTextHeight >= 0);
			Debug.Assert(oTextBrush != null);
			Debug.Assert(oNonLeafStringFormat != null);
			Debug.Assert(oLeafStringFormat != null);
			this.AssertValid();
			bool flag = oNode.Nodes.Count == 0;
			Rectangle rectangleToDraw = oNode.RectangleToDraw;
			int penWidthPx = oNode.PenWidthPx;
			rectangleToDraw.Inflate(-penWidthPx, -penWidthPx);
			Rectangle rectangle;
			if (flag)
			{
				rectangle = rectangleToDraw;
			}
			else
			{
				rectangle = Rectangle.FromLTRB(rectangleToDraw.Left, rectangleToDraw.Top, rectangleToDraw.Right, rectangleToDraw.Top + iTextHeight);
			}
			int width = rectangle.Width;
			int height = rectangle.Height;
			if (width > 0 && height > 0 && height <= rectangleToDraw.Height)
			{
				if (oBackgroundBrush != null)
				{
					oGraphics.FillRectangle(oBackgroundBrush, rectangle);
				}
				oGraphics.DrawString(oNode.Text, oFontForRectangle.Font, oTextBrush, rectangle, flag ? oLeafStringFormat : oNonLeafStringFormat);
			}
		}
		protected StringFormat CreateStringFormat(bool bLeafNode)
		{
			return new StringFormat
			{
				LineAlignment = StringAlignment.Near, 
				Alignment = bLeafNode ? StringAlignment.Near : StringAlignment.Center, 
				Trimming = StringTrimming.EllipsisCharacter
			};
		}
		public override void AssertValid()
		{
			base.AssertValid();
			StringUtil.AssertNotEmpty(this.m_sFontFamily);
			Debug.Assert(this.m_fFontSizePt > 0f);
			Debug.Assert(this.m_iMinimumTextHeight >= 0);
		}
	}
}
