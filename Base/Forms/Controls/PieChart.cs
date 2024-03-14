using System.Drawing.Drawing2D;

namespace Graphing.Forms.Controls;

public partial class PieChart : UserControl
{
    public List<(Color, double)> Values { get; set; }

    public float DpiFloat { get; private set; }

    public PieChart()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);

        Graphics tempG = CreateGraphics();
        DpiFloat = (tempG.DpiX + tempG.DpiY) / 2;
        tempG.Dispose();

        Values = [];
        InitializeComponent();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.HighQuality;
        int size = Math.Min(Width, Height);
        Rectangle rect = new(5, 5, size - 10, size - 10);

        double sum = 0;
        foreach ((Color, double v) item in Values)
            sum += item.v;

        // Draw them.
        double current = 0;
        foreach ((Color color, double value) item in Values)
        {
            double start = 360 * current / sum,
                   end = 360 * (current + item.value) / sum;

            Brush filler = new SolidBrush(item.color);
            g.FillPie(filler, rect, (float)start, (float)(end - start));

            current += item.value;
        }

        // Draw the outline.
        Pen outlinePartsPen = new(Color.FromArgb(unchecked((int)0xFF_202020)), DpiFloat * 3 / 192);
        current = 0;
        foreach ((Color, double value) item in Values)
        {
            double start = 360 * current / sum,
                   end = 360 * (current + item.value) / sum;
            g.DrawPie(outlinePartsPen, rect, (float)start, (float)(end - start));

            current += item.value;
        }

        // Outline
        Pen outlinePen = new(Color.FromArgb(unchecked((int)0xFF_202020)), DpiFloat * 5 / 192);
        g.DrawEllipse(outlinePen, rect);
    }
}
