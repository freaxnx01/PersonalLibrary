using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	public class ColorModifier
	{
		protected internal ColorModifier()
		{
		}
		public void RGBToHSB(Color oColor, out float fHue, out float fSaturation, out float fBrightness)
		{
			int num;
			int num2;
			int num3;
			ColorModifier.ColorRGBToHLS(oColor.ToArgb(), out num, out num2, out num3);
			Debug.Assert(num >= 0);
			Debug.Assert(num <= 240);
			Debug.Assert(num3 >= 0);
			Debug.Assert(num3 <= 240);
			Debug.Assert(num2 >= 0);
			Debug.Assert(num2 <= 240);
			fHue = (float)((double)num * 1.5);
			fSaturation = (float)((double)num3 / 240.0);
			fBrightness = (float)((double)num2 / 240.0);
		}
		public Color HSBToRGB(float fHue, float fSaturation, float fBrightness)
		{
			Debug.Assert(fHue >= 0f);
			Debug.Assert(fHue <= 360f);
			Debug.Assert(fSaturation >= 0f);
			Debug.Assert(fSaturation <= 1f);
			Debug.Assert(fBrightness >= 0f);
			Debug.Assert(fBrightness <= 1f);
			Color baseColor = Color.FromArgb(ColorModifier.ColorHLSToRGB((int)((double)fHue * 0.66666666666666663), (int)((double)fBrightness * 240.0), (int)((double)fSaturation * 240.0)));
			return Color.FromArgb(255, baseColor);
		}
		public Color SetBrightness(Color oColor, float fBrightness)
		{
			Debug.Assert(fBrightness >= 0f);
			Debug.Assert(fBrightness <= 1f);
			float fHue;
			float fSaturation;
			float num;
			this.RGBToHSB(oColor, out fHue, out fSaturation, out num);
			return this.HSBToRGB(fHue, fSaturation, fBrightness);
		}
		[DllImport("shlwapi.dll")]
		protected static extern void ColorRGBToHLS(int clrRGB, out int pwHue, out int pwLuminance, out int pwSaturation);
		[DllImport("shlwapi.dll")]
		protected static extern int ColorHLSToRGB(int wHue, int wLuminance, int wSaturation);
	}
}
