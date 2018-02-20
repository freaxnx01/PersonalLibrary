using Microsoft.Research.CommunityTechnologies.Treemap;
using System;
using System.Drawing;
namespace Microsoft.Research.CommunityTechnologies.TreemapNoDoc
{
	public interface IFontMapper : IDisposable
	{
		bool NodeToFont(Node oNode, int iNodeLevel, Graphics oGraphics, out Font oFont, out string sTextToDraw);
	}
}
