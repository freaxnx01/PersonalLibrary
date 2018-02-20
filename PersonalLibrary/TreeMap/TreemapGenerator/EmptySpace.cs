using System;
using System.Diagnostics;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	public class EmptySpace
	{
		protected TreemapGenerator m_oTreemapGenerator;
		protected float m_fSizeMetric;
		public float SizeMetric
		{
			get
			{
				this.AssertValid();
				return this.m_fSizeMetric;
			}
			set
			{
				Node.ValidateSizeMetric(value, "EmptySpace.SizeMetric");
				if (this.m_fSizeMetric != value)
				{
					this.m_fSizeMetric = value;
					this.FireRedrawRequired();
				}
			}
		}
		protected internal TreemapGenerator TreemapGenerator
		{
			set
			{
				this.m_oTreemapGenerator = value;
				this.AssertValid();
			}
		}
		protected internal EmptySpace()
		{
			this.m_oTreemapGenerator = null;
			this.m_fSizeMetric = 0f;
		}
		protected void FireRedrawRequired()
		{
			if (this.m_oTreemapGenerator != null)
			{
				this.m_oTreemapGenerator.FireRedrawRequired();
			}
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Node.ValidateSizeMetric(this.m_fSizeMetric, "EmptySpace.m_fSizeMetric");
		}
	}
}
