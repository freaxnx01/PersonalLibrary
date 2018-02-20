using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class PerLevelFontMapper : IFontMapper, IDisposable
	{
		private ArrayList m_oFontForRectangles;
		protected bool m_bDisposed;
		protected internal PerLevelFontMapper(string sFamily, Rectangle oTreemapRectangle, float fTreemapRectangleDivisor, float fPerLevelDivisor, float fMinimumFontSize, Graphics oGraphics)
		{
			StringUtil.AssertNotEmpty(sFamily);
			Debug.Assert(fTreemapRectangleDivisor > 0f);
			Debug.Assert(fPerLevelDivisor > 0f);
			Debug.Assert(fMinimumFontSize > 0f);
			Debug.Assert(oGraphics != null);
			float num = (float)oTreemapRectangle.Height / fTreemapRectangleDivisor;
			this.m_oFontForRectangles = new ArrayList();
			while (num > fMinimumFontSize)
			{
				FontForRectangle fontForRectangle = new FontForRectangle(sFamily, num, oGraphics);
				this.m_oFontForRectangles.Add(fontForRectangle);
				num /= fPerLevelDivisor;
			}
			this.m_bDisposed = false;
			this.AssertValid();
		}
		~PerLevelFontMapper()
		{
			this.Dispose(false);
		}
		public bool NodeToFont(Node oNode, int iNodeLevel, Graphics oGraphics, out Font oFont, out string sTextToDraw)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(iNodeLevel >= 0);
			Debug.Assert(oGraphics != null);
			this.AssertValid();
			bool result;
			if (iNodeLevel < this.m_oFontForRectangles.Count)
			{
				FontForRectangle fontForRectangle = (FontForRectangle)this.m_oFontForRectangles[iNodeLevel];
				string text = oNode.Text;
				if (fontForRectangle.CanFitInRectangle(text, oNode.Rectangle, oGraphics))
				{
					oFont = fontForRectangle.Font;
					sTextToDraw = text;
					result = true;
					return result;
				}
			}
			oFont = null;
			sTextToDraw = null;
			result = false;
			return result;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool bDisposing)
		{
			if (!this.m_bDisposed && bDisposing)
			{
				if (this.m_oFontForRectangles != null)
				{
					IEnumerator enumerator = this.m_oFontForRectangles.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							FontForRectangle fontForRectangle = (FontForRectangle)enumerator.Current;
							fontForRectangle.Dispose();
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
				}
				this.m_oFontForRectangles = null;
			}
			this.m_bDisposed = true;
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_oFontForRectangles != null);
		}
	}
}
