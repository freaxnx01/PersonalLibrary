using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct NodeColor
	{
		[FieldOffset(0)]
		private float m_fColorMetric;
		[FieldOffset(0)]
		private Color m_oAbsoluteColor;
		public float ColorMetric
		{
			get
			{
				this.AssertValid();
				float result;
				if (float.IsNaN(this.m_fColorMetric))
				{
					result = 0f;
				}
				else
				{
					result = this.m_fColorMetric;
				}
				return result;
			}
			set
			{
				Debug.Assert(!float.IsNaN(value));
				this.m_fColorMetric = value;
				this.AssertValid();
			}
		}
		public Color AbsoluteColor
		{
			get
			{
				this.AssertValid();
				return this.m_oAbsoluteColor;
			}
			set
			{
				this.m_oAbsoluteColor = value;
				this.AssertValid();
			}
		}
		public NodeColor(float fColorMetric)
		{
			this.m_oAbsoluteColor = Color.Black;
			this.m_fColorMetric = fColorMetric;
			this.AssertValid();
		}
		public NodeColor(Color oAbsoluteColor)
		{
			this.m_fColorMetric = 0f;
			this.m_oAbsoluteColor = oAbsoluteColor;
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
		}
	}
}
