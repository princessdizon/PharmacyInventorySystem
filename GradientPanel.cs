using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class GradientPanel : Panel
{
    public Color TopColor { get; set; } = Color.FromArgb(0, 102, 255);
    public Color BottomColor { get; set; } = Color.FromArgb(102, 178, 255);

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        LinearGradientBrush brush = new LinearGradientBrush(
            this.ClientRectangle,
            TopColor,
            BottomColor,
            90F);

        e.Graphics.FillRectangle(brush, this.ClientRectangle);
    }
}