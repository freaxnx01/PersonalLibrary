using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	public class TransparentBrushMapper
	{
		private ArrayList m_oTransparentBrushes;
		private int m_iTransparentBrushes;
		protected internal TransparentBrushMapper()
		{
			this.m_oTransparentBrushes = null;
			this.m_iTransparentBrushes = 0;
		}
		public void Initialize(Color oSolidColor, int iMinAlpha, int iMaxAlpha, int iAlphaIncrementPerLevel)
		{
			if (oSolidColor.A != 255)
			{
				throw new ArgumentOutOfRangeException("oSolidColor", oSolidColor, "TransparentBrushMapper.Initialize(): oSolidColor must not be transparent.");
			}
			TransparentBrushMapper.ValidateAlphaRange(iMinAlpha, iMaxAlpha, iAlphaIncrementPerLevel, "TransparentBrushMapper.Initialize()");
			this.m_oTransparentBrushes = new ArrayList();
			for (int i = iMinAlpha; i <= iMaxAlpha; i += iAlphaIncrementPerLevel)
			{
				Color color = Color.FromArgb(i, oSolidColor);
				Brush brush = new SolidBrush(color);
				this.m_oTransparentBrushes.Add(brush);
			}
			this.m_iTransparentBrushes = this.m_oTransparentBrushes.Count;
			this.AssertValid();
		}
		public Brush LevelToTransparentBrush(int iLevel)
		{
			this.AssertValid();
			if (iLevel < 0)
			{
				throw new ArgumentOutOfRangeException("iLevel", iLevel, "TransparentBrushMapper.LevelToTransparentBrush: iLevel must be >= 0.");
			}
			if (iLevel >= this.m_iTransparentBrushes)
			{
				iLevel = this.m_iTransparentBrushes - 1;
			}
			return (Brush)this.m_oTransparentBrushes[iLevel];
		}
		public void Dispose()
		{
			this.AssertValid();
			if (this.m_oTransparentBrushes != null)
			{
				IEnumerator enumerator = this.m_oTransparentBrushes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Brush brush = (Brush)enumerator.Current;
						brush.Dispose();
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
		}
		protected internal static void ValidateAlphaRange(int iMinAlpha, int iMaxAlpha, int iIncrementPerLevel, string sCaller)
		{
			if (iMinAlpha < 0 || iMinAlpha > 255)
			{
				throw new ArgumentOutOfRangeException("iMinAlpha", iMinAlpha, sCaller + ": iMinAlpha must be between 0 and 255.");
			}
			if (iMaxAlpha < 0 || iMaxAlpha > 255)
			{
				throw new ArgumentOutOfRangeException("iMaxAlpha", iMaxAlpha, sCaller + ": iMaxAlpha must be between 0 and 255.");
			}
			if (iMaxAlpha < iMinAlpha)
			{
				throw new ArgumentOutOfRangeException("iMaxAlpha", iMaxAlpha, sCaller + ": iMaxAlpha must be >= iMinAlpha.");
			}
			if (iIncrementPerLevel <= 0)
			{
				throw new ArgumentOutOfRangeException("iIncrementPerLevel", iIncrementPerLevel, sCaller + ": iIncrementPerLevel must be > 0.");
			}
		}
		[Conditional("DEBUG")]
		protected internal void AssertValid()
		{
			Debug.Assert(this.m_oTransparentBrushes != null);
			Debug.Assert(this.m_iTransparentBrushes != 0);
		}
	}
}
