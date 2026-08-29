using Nerd_STF.Mathematics;
using System.ComponentModel;

namespace Graphing.Forms;

public partial class Graph2dViewer : GraphViewerBase
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Float2 ScreenCenter { get; set { field = value; Invalidate(); } }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Float2 ScreenZoom { get; set { field = value; Invalidate(); } } = (1, 1);

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ViewportLocked { get; set; }

    private float ScaleFactor => DeviceDpi / 96.0f;

    private string xAxis = "x", yAxis = "y";

    public Graph2dViewer() : base()
    {
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    }
    public Graph2dViewer(Graph graph) : base(graph)
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
        if (Graph is null) return;
        string xAxis = Graph.Axes[0], yAxis = Graph.Axes[1];

        // Draw the background grid.
        double semiStepX = Math.Pow(2, Math.Round(Math.Log2(ScreenZoom.x))), quarterStepX = semiStepX / 4,
               semiStepY = Math.Pow(2, Math.Round(Math.Log2(ScreenZoom.y))), quarterStepY = semiStepY / 4;

        GraphPoint min = ScreenToGraph(new(0, ClientRectangle.Height - 1)),
                   max = ScreenToGraph(new(ClientRectangle.Width - 1, 0));

        GraphPoint semiMin = [("x", Math.Floor(min[xAxis] / semiStepX) * semiStepX), ("y", Math.Floor(min[yAxis] / semiStepY) * semiStepY)];
        GraphPoint semiMax = [("x", Math.Ceiling(max[xAxis] / semiStepX) * semiStepX), ("y", Math.Ceiling(max[yAxis] / semiStepY) * semiStepY)];

        if (Graph.Size.HasValue)
        {
            semiMin[xAxis] = Math.Max(semiMin[xAxis], Graph.Min!.Value[xAxis]); semiMin[yAxis] = Math.Max(semiMin[yAxis], Graph.Min!.Value[yAxis]);
            semiMax[xAxis] = Math.Min(semiMax[xAxis], Graph.Max!.Value[xAxis]); semiMax[yAxis] = Math.Min(semiMax[yAxis], Graph.Max!.Value[yAxis]);
        }

        PointF semiStepScreen;
        {
            PointF b = GraphToScreen(semiMin + (semiStepX, semiStepY));
            PointF a = GraphToScreen(semiMin);
            semiStepScreen = new(b.X - a.X, b.Y - a.Y);
        }
        PointF semiMinScreen = GraphToScreen(semiMin), semiMaxScreen = GraphToScreen(semiMax);

        GraphPoint quarterMin = [("x", Math.Floor(min[xAxis] / quarterStepX) * quarterStepX), ("y", Math.Floor(min[yAxis] / quarterStepX) * quarterStepX)];
        GraphPoint quarterMax = [("x", Math.Ceiling(max[xAxis] / quarterStepY) * quarterStepY), ("y", Math.Ceiling(max[yAxis] / quarterStepY) * quarterStepY)];

        if (Graph.Size.HasValue)
        {
            quarterMin[xAxis] = Math.Max(quarterMin[xAxis], Graph.Min!.Value[xAxis]); quarterMin[yAxis] = Math.Max(quarterMin[yAxis], Graph.Min!.Value[yAxis]);
            quarterMax[xAxis] = Math.Min(quarterMax[xAxis], Graph.Max!.Value[xAxis]); quarterMax[yAxis] = Math.Min(quarterMax[yAxis], Graph.Max!.Value[yAxis]);
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

        GraphPoint mouseOver = ScreenToGraph(e.Location);

        Float2 newZoom = ScreenZoom;
        newZoom.x *= 1 - e.Delta * 0.00075;
        newZoom.y *= 1 - e.Delta * 0.00075;
        ScreenZoom = newZoom;

        GraphPoint newOver = ScreenToGraph(e.Location);
        GraphPoint diff = mouseOver - newOver;
        ScreenCenter += (diff[xAxis], -diff[yAxis]);
    }

    public PointF GraphToScreen(GraphPoint graph)
    {
        graph[yAxis] = -graph[yAxis];

        graph[xAxis] -= ScreenCenter.x;
        graph[yAxis] -= ScreenCenter.y;

        graph[xAxis] *= DeviceDpi / ScreenZoom.x;
        graph[yAxis] *= DeviceDpi / ScreenZoom.y;

        graph[xAxis] += ClientRectangle.Width / 2.0;
        graph[yAxis] += ClientRectangle.Height / 2.0;

        return new((float)graph[xAxis], (float)graph[yAxis]);
    }
    public GraphPoint ScreenToGraph(PointF screen)
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

    protected override void UpdateActiveGraph()
    {
        base.UpdateActiveGraph();
        xAxis = Graph?.Axes[0] ?? "x";
        yAxis = Graph?.Axes[1] ?? "y";
    }

    private enum ClickState
    {
        None,
        GraphPan,
    }
}
