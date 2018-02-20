using Microsoft.Research.CommunityTechnologies.AppLib;
using System;
using System.Diagnostics;
using System.Drawing;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public class FontForRectangle : IDisposable
	{
		protected const int MinTruncatableTextLength = 4;
		protected Font m_oFont;
		protected bool m_bDisposed;
		public Font Font
		{
			get
			{
				this.AssertValid();
				return this.m_oFont;
			}
		}
		protected internal FontForRectangle(string sFamily, float fEmSize, Graphics oGraphics)
		{
			StringUtil.AssertNotEmpty(sFamily);
			Debug.Assert(fEmSize > 0f);
			Debug.Assert(oGraphics != null);
			this.m_oFont = new Font(sFamily, fEmSize);
			this.m_bDisposed = false;
			this.AssertValid();
		}
		~FontForRectangle()
		{
			this.Dispose(false);
		}
		public bool CanFitInRectangle(string sText, RectangleF oRectangle, Graphics oGraphics)
		{
			Debug.Assert(sText != null);
			Debug.Assert(oGraphics != null);
			this.AssertValid();
			SizeF sizeF = oGraphics.MeasureString(sText, this.Font);
			return sizeF.Width < oRectangle.Width && sizeF.Height < oRectangle.Height;
		}
		public bool CanFitInRectangleTruncate(ref string sText, RectangleF oRectangle, Graphics oGraphics)
		{
			Debug.Assert(sText != null);
			Debug.Assert(oGraphics != null);
			this.AssertValid();
			bool result;
			if (this.CanFitInRectangle(sText, oRectangle, oGraphics))
			{
				result = true;
			}
			else
			{
				string text;
				if (this.TruncateText(sText, out text) && this.CanFitInRectangle(text, oRectangle, oGraphics))
				{
					sText = text;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected bool TruncateText(string sText, out string sTruncatedText)
		{
			Debug.Assert(sText != null);
			this.AssertValid();
			bool result;
			if (sText.Length < 4)
			{
				sTruncatedText = null;
				result = false;
			}
			else
			{
				sTruncatedText = sText.Substring(0, 3) + "...";
				result = true;
			}
			return result;
		}
		protected void Dispose(bool bDisposing)
		{
			if (!this.m_bDisposed && bDisposing && this.m_oFont != null)
			{
				this.m_oFont.Dispose();
				this.m_oFont = null;
			}
			this.m_bDisposed = true;
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_oFont != null);
		}
	}
}
