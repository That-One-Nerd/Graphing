using Graphing.Graphs;
using Nerd_STF.Mathematics;
using System.ComponentModel;

namespace Graphing.Forms;

public partial class Graph2dViewer : GraphViewerBase<Graph2d>
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Float2 ScreenCenter { get; set { field = value; Invalidate(); } }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Float2 ScreenZoom { get; set { field = value; Invalidate(); } } = (1, 1);

    private float ScaleFactor => DeviceDpi / 96.0f;

    public Graph2dViewer() : base() { }
    public Graph2dViewer(Graph2d graph) : base(graph) { }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        Point zero = GraphToScreen((0, 0));
        g.DrawRectangle(new Pen(Color.Red), new(zero.X - 5, zero.Y - 5, 10, 10));
    }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        // True background wipe.
        g.FillRectangle(new SolidBrush(BackgroundColor), g.VisibleClipBounds);

        // Draw the background grid.
        double semiStep = 1, quarterStep = semiStep / 4;

        Float2 min = ScreenToGraph(new(0, ClientRectangle.Height - 1)),
               max = ScreenToGraph(new(ClientRectangle.Width - 1, 0));

        Float2 semiMin = (Math.Round(min.x / semiStep) * semiStep, Math.Round(min.y / semiStep) * semiStep);
        Float2 semiMax = (Math.Round(max.x / semiStep) * semiStep, Math.Round(max.y / semiStep) * semiStep);
        Float2 quarterMin = (Math.Round(min.x / quarterStep) * quarterStep, Math.Round(min.y / quarterStep) * quarterStep);
        Float2 quarterMax = (Math.Round(max.x / quarterStep) * quarterStep, Math.Round(max.y / quarterStep) * quarterStep);

        Pen quarterPen = new(QuarterAxisColor, 1 * ScaleFactor);
        for (double x = quarterMin.x; x <= quarterMax.x; x += quarterStep)
        {
            g.DrawLine(quarterPen, GraphToScreen((x, min.y)), GraphToScreen((x, max.y)));
        }
    }

    protected override void OnResize(EventArgs e) => Invalidate();

    public Point GraphToScreen(Float2 graph)
    {
        graph.y = -graph.y;

        graph.x -= ScreenCenter.x;
        graph.y -= ScreenCenter.y;

        graph.x *= DeviceDpi / ScreenZoom.x;
        graph.y *= DeviceDpi / ScreenZoom.y;

        graph.x += ClientRectangle.Width / 2.0;
        graph.y += ClientRectangle.Height / 2.0;

        return new((int)graph.x, (int)graph.y);
    }
    public Float2 ScreenToGraph(Point screen)
    {
        Float2 result = new(screen.X, screen.Y);

        result.x -= ClientRectangle.Width / 2.0;
        result.y -= ClientRectangle.Height / 2.0;

        result.x /= DeviceDpi / ScreenZoom.x;
        result.y /= DeviceDpi / ScreenZoom.y;

        result.x += ScreenCenter.x;
        result.y += ScreenCenter.y;

        result.y = -result.y;

        return result;
    }
}
