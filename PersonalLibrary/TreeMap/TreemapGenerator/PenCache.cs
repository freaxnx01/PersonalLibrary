using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	public class PenCache
	{
		private Hashtable m_oPens;
		private Color m_oPenColor;
		protected internal PenCache()
		{
			this.m_oPens = null;
		}
		public void Initialize(Color oPenColor)
		{
			if (this.m_oPens != null)
			{
				this.Dispose();
			}
			this.m_oPens = new Hashtable();
			this.m_oPenColor = oPenColor;
			this.AssertValid();
		}
		public Pen GetPen(int iWidthPx)
		{
			if (iWidthPx <= 0)
			{
				throw new ArgumentOutOfRangeException("iWidthPx", iWidthPx, "PenCache.GetPen(): iWidthPx must be > 0.");
			}
			Pen pen = (Pen)this.m_oPens[iWidthPx];
			if (pen == null)
			{
				pen = new Pen(this.m_oPenColor, (float)iWidthPx);
				pen.Alignment = PenAlignment.Inset;
				this.m_oPens[iWidthPx] = pen;
			}
			return pen;
		}
		public void Dispose()
		{
			this.AssertValid();
			if (this.m_oPens != null)
			{
				IDictionaryEnumerator enumerator = this.m_oPens.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
						((Pen)dictionaryEntry.Value).Dispose();
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
				this.m_oPens = null;
			}
		}
		[Conditional("DEBUG")]
		protected internal void AssertValid()
		{
			Debug.Assert(this.m_oPens != null);
		}
	}
}
