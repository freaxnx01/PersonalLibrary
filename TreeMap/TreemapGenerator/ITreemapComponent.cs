using System;
using System.Drawing;
namespace Microsoft.Research.CommunityTechnologies.Treemap
{
	public interface ITreemapComponent
	{
		Nodes Nodes
		{
			get;
		}
		string NodesXml
		{
			get;
			set;
		}
		LayoutAlgorithm LayoutAlgorithm
		{
			get;
			set;
		}
		int PaddingPx
		{
			get;
			set;
		}
		int PaddingDecrementPerLevelPx
		{
			get;
			set;
		}
		int PenWidthPx
		{
			get;
			set;
		}
		int PenWidthDecrementPerLevelPx
		{
			get;
			set;
		}
		Color BackColor
		{
			get;
			set;
		}
		Color BorderColor
		{
			get;
			set;
		}
		NodeColorAlgorithm NodeColorAlgorithm
		{
			get;
			set;
		}
		Color MinColor
		{
			get;
			set;
		}
		Color MaxColor
		{
			get;
			set;
		}
		float MinColorMetric
		{
			get;
			set;
		}
		float MaxColorMetric
		{
			get;
			set;
		}
		int DiscreteNegativeColors
		{
			get;
			set;
		}
		int DiscretePositiveColors
		{
			get;
			set;
		}
		string FontFamily
		{
			get;
			set;
		}
		Color FontSolidColor
		{
			get;
			set;
		}
		Color SelectedFontColor
		{
			get;
			set;
		}
		Color SelectedBackColor
		{
			get;
			set;
		}
		NodeLevelsWithText NodeLevelsWithText
		{
			get;
			set;
		}
		TextLocation TextLocation
		{
			get;
			set;
		}
		EmptySpaceLocation EmptySpaceLocation
		{
			get;
			set;
		}
		void GetNodeLevelsWithTextRange(out int minLevel, out int maxLevel);
		void SetNodeLevelsWithTextRange(int minLevel, int maxLevel);
		void GetFontSizeRange(out float minSizePt, out float maxSizePt, out float incrementPt);
		void SetFontSizeRange(float minSizePt, float maxSizePt, float incrementPt);
		void GetFontAlphaRange(out int minAlpha, out int maxAlpha, out int incrementPerLevel);
		void SetFontAlphaRange(int minAlpha, int maxAlpha, int incrementPerLevel);
		void BeginUpdate();
		void EndUpdate();
		void Clear();
	}
}
