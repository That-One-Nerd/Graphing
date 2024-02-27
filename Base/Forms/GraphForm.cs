using Graphing.Extensions;
using Graphing.Graphables;
using System.Text;

namespace Graphing.Forms;

public partial class GraphForm : Form
{
    public Float2 ScreenCenter { get; private set; }
    public Float2 Dpi { get; private set; }

    public double ZoomLevel
    {
        get => _zoomLevel;
        set
        {
            double oldZoom = ZoomLevel;

            _zoomLevel = Math.Clamp(value, 1e-2, 1e3);

            int totalSegments = 0;
            foreach (Graphable able in ables) totalSegments += able.GetItemsToRender(this).Count();

            if (totalSegments > 10_000)
            {
                _zoomLevel = oldZoom;
                return; // Too many segments, stop.
            }
        }
    }
    private double _zoomLevel;

    private readonly Point initialWindowPos;
    private readonly Size initialWindowSize;

    public Graphable[] Graphables => ables.ToArray();

    public Float2 MinVisibleGraph => ScreenSpaceToGraphSpace(new Int2(0, ClientRectangle.Height));
    public Float2 MaxVisibleGraph => ScreenSpaceToGraphSpace(new Int2(ClientRectangle.Width, 0));

    private readonly List<Graphable> ables;

    public GraphForm(string title)
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);

        InitializeComponent();
        Text = title;

        Graphics tempG = CreateGraphics();
        Dpi = new(tempG.DpiX, tempG.DpiY);
        tempG.Dispose();
        ables = [];
        ZoomLevel = 1;
        initialWindowPos = Location;
        initialWindowSize = Size;
    }

    public Int2 GraphSpaceToScreenSpace(Float2 graphPoint)
    {
        graphPoint.y = -graphPoint.y;

        graphPoint.x -= ScreenCenter.x;
        graphPoint.y -= ScreenCenter.y;

        graphPoint.x *= Dpi.x / ZoomLevel;
        graphPoint.y *= Dpi.y / ZoomLevel;

        graphPoint.x += ClientRectangle.Width / 2.0;
        graphPoint.y += ClientRectangle.Height / 2.0;

        return new((int)graphPoint.x, (int)graphPoint.y);
    }
    public Float2 ScreenSpaceToGraphSpace(Int2 screenPoint)
    {
        Float2 result = new(screenPoint.x, screenPoint.y);

        result.x -= ClientRectangle.Width / 2.0;
        result.y -= ClientRectangle.Height / 2.0;

        result.x /= Dpi.x / ZoomLevel;
        result.y /= Dpi.y / ZoomLevel;

        result.x += ScreenCenter.x;
        result.y += ScreenCenter.y;

        result.y = -result.y;

        return result;
    }

    protected virtual void PaintGrid(Graphics g)
    {
        double axisScale = Math.Pow(2, Math.Round(Math.Log2(ZoomLevel)));

        // Draw horizontal/vertical quarter-axis.
        Brush quarterBrush = new SolidBrush(Color.FromArgb(unchecked((int)0xFF_E0E0E0)));
        Pen quarterPen = new(quarterBrush, 2);

        for (double x = Math.Ceiling(MinVisibleGraph.x * 4 / axisScale) * axisScale / 4; x <= Math.Floor(MaxVisibleGraph.x * 4 / axisScale) * axisScale / 4; x += axisScale / 4)
        {
            Int2 startPos = GraphSpaceToScreenSpace(new Float2(x, MinVisibleGraph.y)),
                 endPos = GraphSpaceToScreenSpace(new Float2(x, MaxVisibleGraph.y));
            g.DrawLine(quarterPen, startPos, endPos);
        }
        for (double y = Math.Ceiling(MinVisibleGraph.y * 4 / axisScale) * axisScale / 4; y <= Math.Floor(MaxVisibleGraph.y * 4 / axisScale) * axisScale / 4; y += axisScale / 4)
        {
            Int2 startPos = GraphSpaceToScreenSpace(new Float2(MinVisibleGraph.x, y)),
                 endPos = GraphSpaceToScreenSpace(new Float2(MaxVisibleGraph.x, y));
            g.DrawLine(quarterPen, startPos, endPos);
        }

        // Draw horizontal/vertical semi-axis.
        Brush semiBrush = new SolidBrush(Color.FromArgb(unchecked((int)0xFF_999999)));
        Pen semiPen = new(semiBrush, 2);

        for (double x = Math.Ceiling(MinVisibleGraph.x / axisScale) * axisScale; x <= Math.Floor(MaxVisibleGraph.x / axisScale) * axisScale; x += axisScale)
        {
            Int2 startPos = GraphSpaceToScreenSpace(new Float2(x, MinVisibleGraph.y)),
                 endPos = GraphSpaceToScreenSpace(new Float2(x, MaxVisibleGraph.y));
            g.DrawLine(semiPen, startPos, endPos);
        }
        for (double y = Math.Ceiling(MinVisibleGraph.y / axisScale) * axisScale; y <= Math.Floor(MaxVisibleGraph.y / axisScale) * axisScale; y += axisScale)
        {
            Int2 startPos = GraphSpaceToScreenSpace(new Float2(MinVisibleGraph.x, y)),
                 endPos = GraphSpaceToScreenSpace(new Float2(MaxVisibleGraph.x, y));
            g.DrawLine(semiPen, startPos, endPos);
        }

        Brush mainLineBrush = new SolidBrush(Color.Black);
        Pen mainLinePen = new(mainLineBrush, 3);

        // Draw the main axis (on top of the semi axis).
        Int2 startCenterY = GraphSpaceToScreenSpace(new Float2(0, MinVisibleGraph.y)),
             endCenterY = GraphSpaceToScreenSpace(new Float2(0, MaxVisibleGraph.y)),
             startCenterX = GraphSpaceToScreenSpace(new Float2(MinVisibleGraph.x, 0)),
             endCenterX = GraphSpaceToScreenSpace(new Float2(MaxVisibleGraph.x, 0));

        g.DrawLine(mainLinePen, startCenterX, endCenterX);
        g.DrawLine(mainLinePen, startCenterY, endCenterY);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        Brush background = new SolidBrush(Color.White);
        g.FillRectangle(background, e.ClipRectangle);

        PaintGrid(g);

        // Draw the actual graphs.
        for (int i = 0; i < ables.Count; i++)
        {
            IEnumerable<Line2d> lines = ables[i].GetItemsToRender(this);
            Brush graphBrush = new SolidBrush(ables[i].Color);
            Pen penBrush = new(graphBrush, 3);

            foreach (Line2d l in lines)
            {
                if (!double.IsNormal(l.a.x) || !double.IsNormal(l.a.y) ||
                    !double.IsNormal(l.b.x) || !double.IsNormal(l.b.y)) continue;

                Int2 start = GraphSpaceToScreenSpace(l.a),
                     end = GraphSpaceToScreenSpace(l.b);
                g.DrawLine(penBrush, start, end);
            }
        }

        base.OnPaint(e);
    }
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        Invalidate(false);
    }

    public void Graph(Graphable able)
    {
        ables.Add(able);
        RegenerateMenuItems();
        Invalidate(false);
    }

    private bool mouseDrag = false;
    private Int2 initialMouseLocation;
    private Float2 initialScreenCenter;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        mouseDrag = true;
        initialMouseLocation = new Int2(Cursor.Position.X, Cursor.Position.Y);
        initialScreenCenter = ScreenCenter;
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (mouseDrag)
        {
            Int2 pixelDiff = new(initialMouseLocation.x - Cursor.Position.X,
                             initialMouseLocation.y - Cursor.Position.Y);
            Float2 graphDiff = new(pixelDiff.x * ZoomLevel / Dpi.x, pixelDiff.y * ZoomLevel / Dpi.y);
            ScreenCenter = new(initialScreenCenter.x + graphDiff.x,
                               initialScreenCenter.y + graphDiff.y);
            Invalidate(false);
        }
        mouseDrag = false;
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (mouseDrag)
        {
            Int2 pixelDiff = new(initialMouseLocation.x - Cursor.Position.X,
                             initialMouseLocation.y - Cursor.Position.Y);
            Float2 graphDiff = new(pixelDiff.x * ZoomLevel / Dpi.x, pixelDiff.y * ZoomLevel / Dpi.y);
            ScreenCenter = new(initialScreenCenter.x + graphDiff.x,
                               initialScreenCenter.y + graphDiff.y);
            Invalidate(false);
        }
    }
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        ZoomLevel *= 1 - e.Delta * 0.00075; // Zoom factor.
        Invalidate(false);
    }

    private void ResetViewportButton_Click(object? sender, EventArgs e)
    {
        ScreenCenter = new Float2(0, 0);
        ZoomLevel = 1;
        Invalidate(false);
    }

    private void GraphColorPickerButton_Click(Graphable able)
    {
        GraphColorPickerForm picker = new(this, able)
        {
            StartPosition = FormStartPosition.Manual
        };
        picker.Location = new Point(Location.X + ClientRectangle.Width + 10,
                                    Location.Y + (ClientRectangle.Height - picker.ClientRectangle.Height) / 2);
        picker.ShowDialog();
        RegenerateMenuItems();
    }

    private void RegenerateMenuItems()
    {
        MenuColors.DropDownItems.Clear();
        MenuEquationsDerivative.DropDownItems.Clear();
        MenuEquationsIntegral.DropDownItems.Clear();

        foreach (Graphable able in ables)
        {
            ToolStripMenuItem colorItem = new()
            {
                ForeColor = able.Color,
                Text = able.Name
            };
            colorItem.Click += (o, e) => GraphColorPickerButton_Click(able);
            MenuColors.DropDownItems.Add(colorItem);

            if (able is Equation)
            {
                ToolStripMenuItem derivativeItem = new()
                {
                    ForeColor = able.Color,
                    Text = able.Name
                };
                derivativeItem.Click += (o, e) => EquationComputeDerivative_Click((able as Equation)!);
                MenuEquationsDerivative.DropDownItems.Add(derivativeItem);

                ToolStripMenuItem integralItem = new()
                {
                    ForeColor = able.Color,
                    Text = able.Name
                };
                integralItem.Click += (o, e) => EquationComputeIntegral_Click((able as Equation)!);
                MenuEquationsIntegral.DropDownItems.Add(integralItem);
            }
        }
    }

    private void ButtonViewportSetZoom_Click(object? sender, EventArgs e)
    {
        SetZoomForm picker = new(this)
        {
            StartPosition = FormStartPosition.Manual,
        };
        picker.Location = new Point(Location.X + ClientRectangle.Width + 10,
                                    Location.Y + (ClientRectangle.Height - picker.ClientRectangle.Height) / 2);
        picker.ShowDialog();
    }

    private void ButtonViewportSetCenter_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("TODO", "Set Center Position", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void ButtonViewportReset_Click(object? sender, EventArgs e)
    {
        ScreenCenter = new Float2(0, 0);
        ZoomLevel = 1;
        Invalidate(false);
    }

    private void ButtonViewportResetWindow_Click(object? sender, EventArgs e)
    {
        Location = initialWindowPos;
        Size = initialWindowSize;
        WindowState = FormWindowState.Normal;
    }

    private void EquationComputeDerivative_Click(Equation equation)
    {
        EquationDelegate equ = equation.GetDelegate();
        string oldName = equation.Name, newName;
        if (oldName.StartsWith("Derivative of ")) newName = "Second Derivative of " + oldName[14..];
        else if (oldName.StartsWith("Second Derivative of ")) newName = "Third Derivative of " + oldName[21..];
        else newName = "Derivative of " + oldName;
        // TODO: anti-integrate (maybe).

        Graph(new Equation(DerivativeAtPoint(equ))
        {
            Name = newName
        });

        static EquationDelegate DerivativeAtPoint(EquationDelegate e)
        {
            const double step = 1e-3;
            return x => (e(x + step) - e(x)) / step;
        }
    }

    private void EquationComputeIntegral_Click(Equation equation)
    {
        EquationDelegate equ = equation.GetDelegate();
        string oldName = equation.Name, newName;
        if (oldName.StartsWith("Integral of ")) newName = "Second Integral of " + oldName[12..];
        else if (oldName.StartsWith("Second Integral of ")) newName = "Third Integral of " + oldName[19..];
        else newName = "Integral of " + oldName;
        // TODO: anti-derive (maybe)

        Graph(new Equation(x => Integrate(equ, 0, x))
        {
            Name = newName
        });

        static double Integrate(EquationDelegate e, double lower, double upper)
        {
            // TODO: a better rendering method could make this much faster.
            const double step = 1e-2;

            double factor = 1;
            if (upper < lower)
            {
                factor = -1;
                (lower, upper) = (upper, lower);
            }

            double sum = 0;
            for (double x = lower; x <= upper; x += step)
            {
                sum += e(x) * step;
            }

            return sum * factor;
        }
    }

    private void MenuMiscCaches_Click(object? sender, EventArgs e)
    {
        // TODO: Replace with a form with a pie chart of the use by equation
        //       and the ability to reset them.
        StringBuilder message = new();
        long total = 0;
        foreach (Graphable able in ables)
        {
            if (able is Equation equ)
            {
                long size = equ.GetCacheBytes();
                message.AppendLine($"{able.Name}: {size.FormatAsBytes()}");

                total += size;
            }
        }

        message.AppendLine($"\nTotal: {total.FormatAsBytes()}\n\nClick \"No\" to erase caches.");

        DialogResult result = MessageBox.Show(message.ToString(), "Graph Caches", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (result == DialogResult.No)
        {
            foreach (Graphable able in ables)
            {
                if (able is Equation equ) equ.EraseCache();
            }
        }
    }
}
