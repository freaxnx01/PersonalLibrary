using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	public class ColorGradientMapper
	{
		protected Color[] m_aoDiscreteColors;
		protected Brush[] m_aoDiscreteBrushes;
		protected int m_iDiscreteColorCount;
		protected float m_fMinColorMetric;
		protected float m_fMaxColorMetric;
		protected float m_fColorMetricsPerDivision;
		protected internal ColorGradientMapper()
		{
			this.m_aoDiscreteColors = null;
			this.m_aoDiscreteBrushes = null;
			this.m_iDiscreteColorCount = 0;
			this.m_fMinColorMetric = 0f;
			this.m_fMaxColorMetric = 0f;
			this.m_fColorMetricsPerDivision = 0f;
		}
		public void Initialize(Graphics oGraphics, float fMinColorMetric, float fMaxColorMetric, Color oMinColor, Color oMaxColor, int iDiscreteColorCount, bool bCreateBrushes)
		{
			Debug.Assert(oGraphics != null);
			if (fMaxColorMetric <= fMinColorMetric)
			{
				throw new ArgumentOutOfRangeException("fMaxColorMetric", fMaxColorMetric, "ColorGradientMapper.Initialize: fMaxColorMetric must be > fMinColorMetric.");
			}
			if (iDiscreteColorCount < 2 || iDiscreteColorCount > 256)
			{
				throw new ArgumentOutOfRangeException("iDiscreteColorCount", iDiscreteColorCount, "ColorGradientMapper.Initialize: iDiscreteColorCount must be between 2 and 256.");
			}
			this.m_fMinColorMetric = fMinColorMetric;
			this.m_fMaxColorMetric = fMaxColorMetric;
			this.m_iDiscreteColorCount = iDiscreteColorCount;
			this.m_fColorMetricsPerDivision = (this.m_fMaxColorMetric - this.m_fMinColorMetric) / (float)this.m_iDiscreteColorCount;
			this.m_aoDiscreteColors = this.CreateDiscreteColors(oGraphics, oMinColor, oMaxColor, iDiscreteColorCount);
			if (bCreateBrushes)
			{
				this.m_aoDiscreteBrushes = this.CreateDiscreteBrushes(this.m_aoDiscreteColors);
			}
		}
		public Color ColorMetricToColor(float fColorMetric)
		{
			if (this.m_iDiscreteColorCount == 0)
			{
				throw new InvalidOperationException("ColorGradientMapper.ColorMetricToColor: Must call Initialize() first.");
			}
			int num = this.ColorMetricToArrayIndex(fColorMetric);
			return this.m_aoDiscreteColors[num];
		}
		public Brush ColorMetricToBrush(float fColorMetric)
		{
			if (this.m_iDiscreteColorCount == 0)
			{
				throw new InvalidOperationException("ColorGradientMapper.ColorMetricToBrush: Must call Initialize() first.");
			}
			if (this.m_aoDiscreteBrushes == null)
			{
				throw new InvalidOperationException("ColorGradientMapper.ColorMetricToBrush: Must specify bCreateBrushes=true in Initialize() call.");
			}
			int num = this.ColorMetricToArrayIndex(fColorMetric);
			return this.m_aoDiscreteBrushes[num];
		}
		public void Dispose()
		{
			if (this.m_aoDiscreteBrushes != null)
			{
				Brush[] aoDiscreteBrushes = this.m_aoDiscreteBrushes;
				for (int i = 0; i < aoDiscreteBrushes.Length; i++)
				{
					Brush brush = aoDiscreteBrushes[i];
					brush.Dispose();
				}
			}
		}
		protected Color[] CreateDiscreteColors(Graphics oGraphics, Color oMinColor, Color oMaxColor, int iDiscreteColorCount)
		{
			Debug.Assert(oGraphics != null);
			Debug.Assert(iDiscreteColorCount > 1);
			Color[] array = new Color[iDiscreteColorCount];
			Bitmap bitmap = new Bitmap(1, iDiscreteColorCount, oGraphics);
			Graphics graphics = Graphics.FromImage(bitmap);
			Rectangle rect = Rectangle.FromLTRB(0, 0, 1, iDiscreteColorCount - 1);
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, oMinColor, oMaxColor, LinearGradientMode.Vertical);
			graphics.FillRectangle(linearGradientBrush, new Rectangle(Point.Empty, bitmap.Size));
			linearGradientBrush.Dispose();
			int i;
			for (i = 0; i < iDiscreteColorCount - 1; i++)
			{
				array[i] = bitmap.GetPixel(0, i);
			}
			array[i] = oMaxColor;
			bitmap.Dispose();
			return array;
		}
		protected Brush[] CreateDiscreteBrushes(Color[] aoDiscreteColors)
		{
			int num = aoDiscreteColors.Length;
			Brush[] array = new Brush[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new SolidBrush(aoDiscreteColors[i]);
			}
			return array;
		}
		protected int ColorMetricToArrayIndex(float fColorMetric)
		{
			Debug.Assert(this.m_iDiscreteColorCount != 0);
			int num;
			if (fColorMetric <= this.m_fMinColorMetric)
			{
				num = 0;
			}
			else
			{
				if (fColorMetric >= this.m_fMaxColorMetric)
				{
					num = this.m_iDiscreteColorCount - 1;
				}
				else
				{
					num = (int)((fColorMetric - this.m_fMinColorMetric) / this.m_fColorMetricsPerDivision);
				}
			}
			Debug.Assert(num >= 0);
			Debug.Assert(num < this.m_iDiscreteColorCount);
			return num;
		}
	}
}
