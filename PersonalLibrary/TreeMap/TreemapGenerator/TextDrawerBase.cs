using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public abstract class TextDrawerBase : ITextDrawer
	{
		protected NodeLevelsWithText m_eNodeLevelsWithText;
		protected int m_iMinNodeLevelWithText;
		protected int m_iMaxNodeLevelWithText;
		public TextDrawerBase(NodeLevelsWithText eNodeLevelsWithText, int iMinNodeLevelWithText, int iMaxNodeLevelWithText)
		{
			this.m_eNodeLevelsWithText = eNodeLevelsWithText;
			this.m_iMinNodeLevelWithText = iMinNodeLevelWithText;
			this.m_iMaxNodeLevelWithText = iMaxNodeLevelWithText;
		}
		public abstract void DrawTextForAllNodes(Graphics oGraphics, Rectangle oTreemapRectangle, Nodes oNodes);
		public abstract void DrawTextForSelectedNode(Graphics oGraphics, Node oSelectedNode);
		protected TextRenderingHint SetTextRenderingHint(Graphics oGraphics, Font oFont)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(oFont != null);
			this.AssertValid();
			TextRenderingHint textRenderingHint = oGraphics.TextRenderingHint;
			oGraphics.TextRenderingHint = ((oFont.Size < 3.1f) ? TextRenderingHint.AntiAlias : TextRenderingHint.SystemDefault);
			return textRenderingHint;
		}
		protected bool TextShouldBeDrawnForNode(Node oNode, int iNodeLevel)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(iNodeLevel >= 0);
			this.AssertValid();
			bool result;
			switch (this.m_eNodeLevelsWithText)
			{
				case NodeLevelsWithText.All:
				{
					result = true;
					break;
				}
				case NodeLevelsWithText.None:
				{
					result = false;
					break;
				}
				case NodeLevelsWithText.Leaves:
				{
					result = (oNode.Nodes.Count == 0);
					break;
				}
				case NodeLevelsWithText.Range:
				{
					result = (iNodeLevel >= this.m_iMinNodeLevelWithText && iNodeLevel <= this.m_iMaxNodeLevelWithText);
					break;
				}
				default:
				{
					Debug.Assert(false);
					result = false;
					break;
				}
			}
			return result;
		}
		[Conditional("DEBUG")]
		public virtual void AssertValid()
		{
			Debug.Assert(Enum.IsDefined(typeof(NodeLevelsWithText), this.m_eNodeLevelsWithText));
			Debug.Assert(this.m_iMinNodeLevelWithText >= 0);
			Debug.Assert(this.m_iMaxNodeLevelWithText >= 0);
			Debug.Assert(this.m_iMaxNodeLevelWithText >= this.m_iMinNodeLevelWithText);
		}
	}
}
