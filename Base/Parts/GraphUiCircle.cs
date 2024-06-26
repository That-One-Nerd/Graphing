﻿using Graphing.Forms;
using System.Drawing;

namespace Graphing.Parts;

public record struct GraphUiCircle : IGraphPart
{
    public Float2 center;
    public int radius;

    public GraphUiCircle()
    {
        center = new();
        radius = 1;
    }
    public GraphUiCircle(Float2 center, int radius = 8)
    {
        this.center = center;
        this.radius = radius;
    }

    public readonly void Render(in GraphForm form, in Graphics g, in Pen pen)
    {
        if (!double.IsFinite(center.x) || !double.IsFinite(center.y) ||
            !double.IsFinite(radius) || radius == 0) return;

        int rad = (int)(form.DpiFloat * radius / 192);

        Int2 centerPix = form.GraphSpaceToScreenSpace(center);
        g.FillEllipse(pen.Brush, new Rectangle(new Point(centerPix.x - rad,
                                                         centerPix.y - rad),
                                               new Size(rad * 2, rad * 2)));
    }
}
