using Microsoft.Research.CommunityTechnologies.AppLib;
using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class MaximizingFontMapper : IFontMapper, IDisposable
	{
		private ArrayList m_oFontForRectangles;
		protected bool m_bDisposed;
		protected internal MaximizingFontMapper(string sFamily, float fMinSizePt, float fMaxSizePt, float fIncrementPt, Graphics oGraphics)
		{
			StringUtil.AssertNotEmpty(sFamily);
			Debug.Assert(oGraphics != null);
			MaximizingFontMapper.ValidateSizeRange(fMinSizePt, fMaxSizePt, fIncrementPt, "MaximizingFontMapper.Initialize()");
			this.m_oFontForRectangles = new ArrayList();
			for (float num = fMinSizePt; num <= fMaxSizePt; num += fIncrementPt)
			{
				FontForRectangle fontForRectangle = new FontForRectangle(sFamily, num, oGraphics);
				this.m_oFontForRectangles.Insert(0, fontForRectangle);
			}
			this.m_bDisposed = false;
			this.AssertValid();
		}
		~MaximizingFontMapper()
		{
			this.Dispose(false);
		}
		public bool NodeToFont(Node oNode, int iNodeLevel, Graphics oGraphics, out Font oFont, out string sTextToDraw)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(iNodeLevel >= 0);
			Debug.Assert(oGraphics != null);
			this.AssertValid();
			string text = oNode.Text;
			RectangleF rectangle = oNode.Rectangle;
			IEnumerator enumerator = this.m_oFontForRectangles.GetEnumerator();
			bool result;
			try
			{
				while (enumerator.MoveNext())
				{
					FontForRectangle fontForRectangle = (FontForRectangle)enumerator.Current;
					if (fontForRectangle.CanFitInRectangle(text, rectangle, oGraphics))
					{
						oFont = fontForRectangle.Font;
						sTextToDraw = text;
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
		protected internal static void ValidateSizeRange(float fMinSizePt, float fMaxSizePt, float fIncrementPt, string sCaller)
		{
			if (fMinSizePt <= 0f)
			{
				throw new ArgumentOutOfRangeException("fMinSizePt", fMinSizePt, sCaller + ": fMinSizePt must be > 0.");
			}
			if (fMaxSizePt <= 0f)
			{
				throw new ArgumentOutOfRangeException("fMaxSizePt", fMaxSizePt, sCaller + ": fMaxSizePt must be > 0.");
			}
			if (fMaxSizePt < fMinSizePt)
			{
				throw new ArgumentOutOfRangeException("fMaxSizePt", fMaxSizePt, sCaller + ": fMaxSizePt must be >= fMinSizePt.");
			}
			if (fIncrementPt <= 0f)
			{
				throw new ArgumentOutOfRangeException("fIncrementPt", fIncrementPt, sCaller + ": fIncrementPt must be > 0.");
			}
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
