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

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ViewportLocked { get; set; }

    private float ScaleFactor => DeviceDpi / 96.0f;

    public Graph2dViewer() : base()
    {
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    }
    public Graph2dViewer(Graph2d graph) : base(graph)
    {
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    }

    private readonly Queue<double> debugRenderTimes = [];
    protected override void OnPaint(PaintEventArgs e)
    {
        // TODO: We can maybe apply optimizations here someday. But apparently the main lag point
        //       was a lack of double-buffering (how on earth does that work??) so it's not necessary.

        DateTime start = DateTime.Now;

        Graphics g = e.Graphics;

        // True background wipe.
        g.Clear(BackgroundColor);

        if (Graph is null) return;

        PaintGrid(g);

        PointF zero = GraphToScreen((0, 0));
        g.DrawRectangle(new Pen(Color.Red), new RectangleF(zero.X - 5, zero.Y - 5, 10, 10));

        // Debug frame time.
        DateTime end = DateTime.Now;
        TimeSpan time = end - start;

        double msec = time.TotalMilliseconds;
        debugRenderTimes.Enqueue(msec);
        while (debugRenderTimes.Count > 150) debugRenderTimes.Dequeue();

        ParentForm.Text = $"Frame Time {msec:0.0} msec ({1000 / msec:0.0} fps, {1000 / debugRenderTimes.Average():0.0} avg). Center {ScreenCenter.ToString("0.0e0")}, Zoom {ScreenZoom.ToString("0.0e0")}";
    }

    private void PaintGrid(Graphics g)
    {
        // Draw the background grid.
        double semiStepX = Math.Pow(2, Math.Round(Math.Log2(ScreenZoom.x))), quarterStepX = semiStepX / 4,
               semiStepY = Math.Pow(2, Math.Round(Math.Log2(ScreenZoom.y))), quarterStepY = semiStepY / 4;

        Float2 min = ScreenToGraph(new(0, ClientRectangle.Height - 1)),
               max = ScreenToGraph(new(ClientRectangle.Width - 1, 0));

        Float2 semiMin = (Math.Floor(min.x / semiStepX) * semiStepX, Math.Floor(min.y / semiStepY) * semiStepY);
        Float2 semiMax = (Math.Ceiling(max.x / semiStepX) * semiStepX, Math.Ceiling(max.y / semiStepY) * semiStepY);

        if (Graph.Size.HasValue)
        {
            semiMin.x = Math.Max(semiMin.x, Graph.Min!.Value.x); semiMin.y = Math.Max(semiMin.y, Graph.Min!.Value.y);
            semiMax.x = Math.Min(semiMax.x, Graph.Max!.Value.x); semiMax.y = Math.Min(semiMax.y, Graph.Max!.Value.y);
        }

        PointF semiStepScreen;
        {
            PointF b = GraphToScreen(semiMin + (semiStepX, semiStepY));
            PointF a = GraphToScreen(semiMin);
            semiStepScreen = new(b.X - a.X, b.Y - a.Y);
        }
        PointF semiMinScreen = GraphToScreen(semiMin), semiMaxScreen = GraphToScreen(semiMax);

        Float2 quarterMin = (Math.Floor(min.x / quarterStepX) * quarterStepX, Math.Floor(min.y / quarterStepX) * quarterStepX);
        Float2 quarterMax = (Math.Ceiling(max.x / quarterStepY) * quarterStepY, Math.Ceiling(max.y / quarterStepY) * quarterStepY);

        if (Graph.Size.HasValue)
        {
            quarterMin.x = Math.Max(quarterMin.x, Graph.Min!.Value.x); quarterMin.y = Math.Max(quarterMin.y, Graph.Min!.Value.y);
            quarterMax.x = Math.Min(quarterMax.x, Graph.Max!.Value.x); quarterMax.y = Math.Min(quarterMax.y, Graph.Max!.Value.y);
        }

        PointF quarterStepScreen;
        {
            PointF b = GraphToScreen(quarterMin + (quarterStepX, quarterStepY));
            PointF a = GraphToScreen(quarterMin);
            quarterStepScreen = new(b.X - a.X, b.Y - a.Y);
        }
        PointF quarterMinScreen = GraphToScreen(quarterMin), quarterMaxScreen = GraphToScreen(quarterMax);

        PointF minLine = new(0, 0), maxLine = new(ClientRectangle.Width - 1, ClientRectangle.Height - 1);
        if (Graph.Size.HasValue)
        {
            PointF possibleMin = GraphToScreen(Graph.Min!.Value), possibleMax = GraphToScreen(Graph.Max!.Value);
            minLine.X = Math.Max(0, possibleMin.X); minLine.Y = Math.Max(0, possibleMin.Y);
            maxLine.X = Math.Min(ClientRectangle.Width - 1, possibleMax.X); maxLine.Y = Math.Min(ClientRectangle.Height - 1, possibleMax.Y);
        }

        // Quarter axis
        Pen quarterPen = new(QuarterAxisColor, 1 * ScaleFactor);
        for (float xS = quarterMinScreen.X; xS <= quarterMaxScreen.X; xS += quarterStepScreen.X)
        {
            g.DrawLine(quarterPen, new PointF(xS, minLine.Y), new PointF(xS, maxLine.Y));
        }
        for (float yS = quarterMaxScreen.Y; yS <= quarterMinScreen.Y; yS -= quarterStepScreen.Y)
        {
            g.DrawLine(quarterPen, new PointF(minLine.X, yS), new PointF(maxLine.X, yS));
        }

        // Semi axis
        Pen semiPen = new(SemiAxisColor, 1 * ScaleFactor);
        for (float xS = semiMinScreen.X; xS <= semiMaxScreen.X; xS += semiStepScreen.X)
        {
            g.DrawLine(semiPen, new PointF(xS, minLine.Y), new PointF(xS, maxLine.Y));
        }
        for (float yS = semiMaxScreen.Y; yS <= semiMinScreen.Y; yS -= semiStepScreen.Y)
        {
            g.DrawLine(semiPen, new PointF(minLine.X, yS), new PointF(maxLine.X, yS));
        }

        // Main axis
    }

    protected override void OnResize(EventArgs e) => Invalidate();

    // Click events.
    private ClickState click = ClickState.None;
    private Float2 graphPanInitial;
    private Point graphPanScreen;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (!ViewportLocked)
        {
            click = ClickState.GraphPan;
            graphPanInitial = ScreenCenter;
            graphPanScreen = e.Location;
        }
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (click == ClickState.GraphPan)
        {
            Point pan = e.Location;
            Int2 diff = (pan.X - graphPanScreen.X, pan.Y - graphPanScreen.Y);
            Float2 diffGraph = diff * ScreenZoom / DeviceDpi;
            ScreenCenter = graphPanInitial - diffGraph;
        }
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        click = ClickState.None;
    }
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        if (ViewportLocked) return;

        Float2 mouseOver = ScreenToGraph(e.Location);

        Float2 newZoom = ScreenZoom;
        newZoom.x *= 1 - e.Delta * 0.00075;
        newZoom.y *= 1 - e.Delta * 0.00075;
        ScreenZoom = newZoom;

        Float2 newOver = ScreenToGraph(e.Location);
        Float2 diff = mouseOver - newOver;
        ScreenCenter += (diff.x, -diff.y);
    }

    public PointF GraphToScreen(Float2 graph)
    {
        graph.y = -graph.y;

        graph.x -= ScreenCenter.x;
        graph.y -= ScreenCenter.y;

        graph.x *= DeviceDpi / ScreenZoom.x;
        graph.y *= DeviceDpi / ScreenZoom.y;

        graph.x += ClientRectangle.Width / 2.0;
        graph.y += ClientRectangle.Height / 2.0;

        return new((float)graph.x, (float)graph.y);
    }
    public Float2 ScreenToGraph(PointF screen)
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

    private enum ClickState
    {
        None,
        GraphPan,
    }
}
