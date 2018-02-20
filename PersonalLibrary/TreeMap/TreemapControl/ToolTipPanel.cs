using Microsoft.Research.CommunityTechnologies.ControlLib;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace Microsoft.Research.CommunityTechnologies.AppLib
{
	public class ToolTipPanel : Panel
	{
		protected const int InternalMargin = 1;
		protected string m_sText;
		public ToolTipPanel()
		{
			this.m_sText = null;
			this.ForeColor = SystemColors.InfoText;
			base.Visible = false;
			base.Enabled = false;
			this.AssertValid();
		}
		public void ShowToolTip(string sText, Control oParentControl)
		{
			Debug.Assert(oParentControl != null);
			this.AssertValid();
			this.m_sText = sText;
			Rectangle rectangle = this.ComputePanelRectangle(sText, oParentControl);
			base.Location = rectangle.Location;
			base.Size = rectangle.Size;
			base.Visible = true;
		}
		public void HideToolTip()
		{
			this.AssertValid();
			base.Visible = false;
		}
		protected Rectangle ComputePanelRectangle(string sText, Control oParentControl)
		{
			Debug.Assert(oParentControl != null);
			this.AssertValid();
			Rectangle clientRectangle = oParentControl.ClientRectangle;
			PointF pointF = ControlUtil.GetClientMousePosition(oParentControl);
			Size size = oParentControl.Cursor.Size;
			int num = (int)pointF.X;
			int num2 = (int)pointF.Y + size.Height;
			Size size2 = clientRectangle.Size;
			num = Math.Max(num, 0);
			num = Math.Min(size2.Width, num);
			num2 = Math.Max(num2, 0);
			num2 = Math.Min(size2.Height, num2);
			Graphics graphics = base.CreateGraphics();
			SizeF sizeF = graphics.MeasureString(sText, this.Font);
			graphics.Dispose();
			Rectangle result = new Rectangle(num, num2, (int)Math.Ceiling((double)(sizeF.Width + 2f + 2f)), (int)Math.Ceiling((double)(sizeF.Height + 2f + 2f)));
			if (result.Bottom + 1 > size2.Height)
			{
				result.Offset(0, size2.Height - result.Bottom - 1);
			}
			if (result.Right + 1 > size2.Width)
			{
				result.Offset(size2.Width - result.Right - 1, 0);
			}
			result.Intersect(clientRectangle);
			Debug.Assert(result.Left >= 0);
			Debug.Assert(result.Top >= 0);
			Debug.Assert(result.Right <= size2.Width);
			Debug.Assert(result.Bottom <= size2.Height);
			return result;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			Debug.Assert(e != null);
			this.AssertValid();
			Graphics graphics = e.Graphics;
			Brush brush = new SolidBrush(this.ForeColor);
			graphics.DrawString(this.m_sText, this.Font, brush, new Point(1, 1));
			brush.Dispose();
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
		}
	}
}
