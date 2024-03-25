using Graphing.Abstract;
using Graphing.Parts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphing.Forms;

public partial class GraphForm : Form
{
    public static readonly Color BackgroundColor = Color.White;
    public static readonly Color MainAxisColor = Color.Black;
    public static readonly Color SemiAxisColor = Color.FromArgb(unchecked((int)0xFF_999999));
    public static readonly Color QuarterAxisColor = Color.FromArgb(unchecked((int)0xFF_E0E0E0));
    public static readonly Color UnitsTextColor = Color.Black;

    public Float2 ScreenCenter { get; private set; }
    public Float2 Dpi { get; private set; }

    public float DpiFloat { get; private set; }

    public double ZoomLevel
    {
        get => _zoomLevel;
        set
        {
            double oldZoom = ZoomLevel;

            _zoomLevel = Math.Clamp(value, 1e-5, 1e3);

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

        DpiFloat = (float)((Dpi.x + Dpi.y) / 2);

        ables = [];
        ZoomLevel = 1;
        initialWindowPos = Location;
        initialWindowSize = Size;

        RunUpdateChecker();
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
        Brush quarterBrush = new SolidBrush(QuarterAxisColor);
        Pen quarterPen = new(quarterBrush, DpiFloat * 2 / 192);

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
        Brush semiBrush = new SolidBrush(SemiAxisColor);
        Pen semiPen = new(semiBrush, DpiFloat * 2 / 192);

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

        Brush mainLineBrush = new SolidBrush(MainAxisColor);
        Pen mainLinePen = new(mainLineBrush, DpiFloat * 3 / 192);

        // Draw the main axis (on top of the semi axis).
        Int2 startCenterY = GraphSpaceToScreenSpace(new Float2(0, MinVisibleGraph.y)),
             endCenterY = GraphSpaceToScreenSpace(new Float2(0, MaxVisibleGraph.y)),
             startCenterX = GraphSpaceToScreenSpace(new Float2(MinVisibleGraph.x, 0)),
             endCenterX = GraphSpaceToScreenSpace(new Float2(MaxVisibleGraph.x, 0));

        g.DrawLine(mainLinePen, startCenterX, endCenterX);
        g.DrawLine(mainLinePen, startCenterY, endCenterY);
    }
    protected virtual void PaintUnits(Graphics g)
    {
        double axisScale = Math.Pow(2, Math.Round(Math.Log(ZoomLevel, 2)));
        Brush textBrush = new SolidBrush(UnitsTextColor);
        Font textFont = new(Font.Name, 9, FontStyle.Regular);

        // X-axis
        int minX = (int)(DpiFloat * 50 / 192),
            maxX = ClientRectangle.Height - (int)(DpiFloat * 40 / 192);
        for (double x = Math.Ceiling(MinVisibleGraph.x / axisScale) * axisScale; x <= MaxVisibleGraph.x; x += axisScale)
        {
            if (x == 0) x = 0; // Fixes -0

            Int2 screenPos = GraphSpaceToScreenSpace(new Float2(x, 0));

            if (screenPos.y < minX) screenPos.y = minX;
            else if (screenPos.y > maxX) screenPos.y = maxX;

            g.DrawString($"{x}", textFont, textBrush, screenPos.x, screenPos.y);
        }

        // Y-axis
        int minY = (int)(DpiFloat * 10 / 192);
        for (double y = Math.Ceiling(MinVisibleGraph.y / axisScale) * axisScale; y <= MaxVisibleGraph.y; y += axisScale)
        {
            if (y == 0) continue;

            Int2 screenPos = GraphSpaceToScreenSpace(new Float2(0, y));

            string result = y.ToString();
            int maxY = ClientRectangle.Width - (int)(DpiFloat * (textFont.Height * result.Length * 0.40 + 15) / 192);

            if (screenPos.x < minY) screenPos.x = minY;
            else if (screenPos.x > maxY) screenPos.x = maxY;

            g.DrawString($"{y}", textFont, textBrush, screenPos.x, screenPos.y);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.HighQuality;

        Brush background = new SolidBrush(BackgroundColor);
        g.FillRectangle(background, e.ClipRectangle);

        PaintGrid(g);
        PaintUnits(g);

        Point clientMousePos = PointToClient(Cursor.Position);
        Float2 graphMousePos = ScreenSpaceToGraphSpace(new(clientMousePos.X,
                                                           clientMousePos.Y));

        // Draw the actual graphs.
        Pen[] graphPens = new Pen[ables.Count];
        for (int i = 0; i < ables.Count; i++)
        {
            IEnumerable<IGraphPart> lines = ables[i].GetItemsToRender(this);
            Brush graphBrush = new SolidBrush(ables[i].Color);
            Pen graphPen = new(graphBrush, DpiFloat * 3 / 192);
            graphPens[i] = graphPen;
            foreach (IGraphPart gp in lines) gp.Render(this, g, graphPen);
        }

        // Equation selection detection.
        // This system lets you select multiple graphs, and that's cool by me.
        if (ableDrag)
        {
            Font textFont = new(Font.Name, 8, FontStyle.Bold);
            for (int i = 0; i < ables.Count; i++)
            {
                if (ables[i].ShouldSelectGraphable(this, graphMousePos, 2.5))
                {
                    Float2 selectedPoint = ables[i].GetSelectedPoint(this, graphMousePos);
                    GraphUiCircle select = new(selectedPoint);

                    Int2 textPos = GraphSpaceToScreenSpace(select.center);
                    textPos.y -= (int)(DpiFloat * 32 / 192);

                    string content = $"({selectedPoint.x:0.00}, {selectedPoint.y:0.00})";

                    SizeF textSize = g.MeasureString(content, textFont);
                    g.FillRectangle(background, new Rectangle(textPos.x, textPos.y,
                                         (int)textSize.Width, (int)textSize.Height));
                    g.DrawString(content, textFont, graphPens[i].Brush, new Point(textPos.x, textPos.y));
                    select.Render(this, g, graphPens[i]);
                }
            }
        }

        base.OnPaint(e);
    }
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        Invalidate(false);
    }

    public void Graph(params Graphable[] newAbles)
    {
        ables.AddRange(newAbles);
        RegenerateMenuItems();
        Invalidate(false);
    }
    public void Ungraph(params Graphable[] ables)
    {
        this.ables.RemoveAll(x => ables.Contains(x));
        RegenerateMenuItems();
        Invalidate(false);
    }

    private bool mouseDrag = false;
    private Int2 initialMouseLocation;
    private Float2 initialScreenCenter;

    private bool ableDrag = false;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (!mouseDrag)
        {
            Point clientMousePos = PointToClient(Cursor.Position);
            Float2 graphMousePos = ScreenSpaceToGraphSpace(new(clientMousePos.X,
                                                               clientMousePos.Y));
            foreach (Graphable able in Graphables)
            {
                if (able.ShouldSelectGraphable(this, graphMousePos, 1)) ableDrag = true;
            }
            if (ableDrag) Invalidate(false);
        }

        if (!ableDrag)
        {
            mouseDrag = true;
            initialMouseLocation = new Int2(Cursor.Position.X, Cursor.Position.Y);
            initialScreenCenter = ScreenCenter;
        }
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
        }
        mouseDrag = false;
        ableDrag = false;
        Invalidate(false);
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
        else if (ableDrag) Invalidate(false);
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

        if (picker.Location.X + picker.Width > Screen.FromControl(this).WorkingArea.Width)
        {
            picker.StartPosition = FormStartPosition.WindowsDefaultLocation;
        }

        picker.TopMost = true;
        picker.ShowDialog();
        RegenerateMenuItems();
    }

    private void RegenerateMenuItems()
    {
        MenuElementsColors.DropDownItems.Clear();
        MenuElementsRemove.DropDownItems.Clear();
        MenuOperationsDerivative.DropDownItems.Clear();
        MenuOperationsIntegral.DropDownItems.Clear();
        MenuConvertEquation.DropDownItems.Clear();
        MenuOperationsTranslate.DropDownItems.Clear();

        foreach (Graphable able in ables)
        {
            ToolStripMenuItem colorItem = new()
            {
                ForeColor = able.Color,
                Text = able.Name
            };
            colorItem.Click += (o, e) => GraphColorPickerButton_Click(able);
            MenuElementsColors.DropDownItems.Add(colorItem);

            ToolStripMenuItem removeItem = new()
            {
                ForeColor = able.Color,
                Text = able.Name
            };
            removeItem.Click += (o, e) => Ungraph(able);
            MenuElementsRemove.DropDownItems.Add(removeItem);

            if (able is IDerivable derivable)
            {
                ToolStripMenuItem derivativeItem = new()
                {
                    ForeColor = able.Color,
                    Text = able.Name
                };
                derivativeItem.Click += (o, e) => Graph(derivable.Derive());
                MenuOperationsDerivative.DropDownItems.Add(derivativeItem);
            }
            if (able is IIntegrable integrable)
            {
                ToolStripMenuItem integralItem = new()
                {
                    ForeColor = able.Color,
                    Text = able.Name
                };
                integralItem.Click += (o, e) => Graph(integrable.Integrate());
                MenuOperationsIntegral.DropDownItems.Add(integralItem);
            }
            if (able is IEquationConvertible equConvert)
            {
                ToolStripMenuItem equItem = new()
                {
                    ForeColor = able.Color,
                    Text = able.Name
                };
                equItem.Click += (o, e) =>
                {
                    Ungraph(able);
                    Graph(equConvert.ToEquation());
                };
                MenuConvertEquation.DropDownItems.Add(equItem);
            }
            if (able is ITranslatable translatable)
            {
                ToolStripMenuItem transItem = new()
                {
                    ForeColor = able.Color,
                    Text = able.Name
                };
                transItem.Click += (o, e) => ElementsOperationsTranslate_Click(able, translatable);
                MenuOperationsTranslate.DropDownItems.Add(transItem);
            }
        }
    }

    private void ButtonViewportSetZoom_Click(object? sender, EventArgs e)
    {
        SetZoomForm zoomer = new(this)
        {
            StartPosition = FormStartPosition.Manual,
        };
        zoomer.Location = new Point(Location.X + ClientRectangle.Width + 10,
                                    Location.Y + (ClientRectangle.Height - zoomer.ClientRectangle.Height) / 2);
        if (zoomer.Location.X + zoomer.Width > Screen.FromControl(this).WorkingArea.Width)
        {
            zoomer.StartPosition = FormStartPosition.WindowsDefaultLocation;
        }
        zoomer.ShowDialog();
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

    private void MenuMiscCaches_Click(object? sender, EventArgs e)
    {
        ViewCacheForm cacheForm = new(this)
        {
            StartPosition = FormStartPosition.Manual
        };

        cacheForm.Location = new Point(Location.X + ClientRectangle.Width + 10,
                                       Location.Y + (ClientRectangle.Height - cacheForm.ClientRectangle.Height) / 2);

        if (cacheForm.Location.X + cacheForm.Width > Screen.FromControl(this).WorkingArea.Width)
        {
            cacheForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
        }
        cacheForm.TopMost = true;
        cacheForm.Show();
    }
    private void MiscMenuPreload_Click(object? sender, EventArgs e)
    {
        Float2 min = MinVisibleGraph, max = MaxVisibleGraph;
        Float2 add = new(max.x - min.x, max.y - min.y);
        add.x *= 0.75; // Expansion
        add.y *= 0.75; // Screen + 75%

        Float2 xRange = new(min.x - add.x, max.x + add.x),
               yRange = new(min.y - add.y, max.y + add.y);

        double step = ScreenSpaceToGraphSpace(new Int2(1, 0)).x
                    - ScreenSpaceToGraphSpace(new Int2(0, 0)).x;
        step /= 10;

        foreach (Graphable able in Graphables) able.Preload(xRange, yRange, step);
        Invalidate(false);
    }

    private void ElementsOperationsTranslate_Click(Graphable ableRaw, ITranslatable ableTrans)
    {
        TranslateForm shifter = new(this, ableRaw, ableTrans)
        {
            StartPosition = FormStartPosition.Manual,
        };
        shifter.Location = new Point(Location.X + ClientRectangle.Width + 10,
                                     Location.Y + (ClientRectangle.Height - shifter.ClientRectangle.Height) / 2);
        if (shifter.Location.X + shifter.Width > Screen.FromControl(this).WorkingArea.Width)
        {
            shifter.StartPosition = FormStartPosition.WindowsDefaultLocation;
        }
        shifter.Show();
    }

    private async void RunUpdateChecker()
    {
        try
        {
            HttpClient http = new();
            HttpRequestMessage request = new(HttpMethod.Get, "https://api.github.com/repos/That-One-Nerd/Graphing/releases");
            request.Headers.Add("User-Agent", "ThatOneNerd.Graphing-Update-Checker");

            HttpResponseMessage result = await http.SendAsync(request);
            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to check for updates.");
                return;
            }

            JsonArray arr = JsonSerializer.Deserialize<JsonArray>(await result.Content.ReadAsStreamAsync())!;
            JsonObject latest = arr[0]!.AsObject();

            Version curVersion = Version.Parse(Assembly.GetAssembly(typeof(GraphForm))!.FullName!.Split(',')[1].Trim()[8..^2]);
            Version newVersion = Version.Parse(latest["tag_name"]!.GetValue<string>());

            if (newVersion > curVersion)
            {
                // Updates are required.
                DialogResult button = MessageBox.Show(
                    $"A new update is available!\n{curVersion} -> {newVersion}\nWould you like to download the update?",
                    "Graphing Calculator Update", MessageBoxButtons.YesNo);

                if (button == DialogResult.No) return;

                ProcessStartInfo website = new()
                {
                    FileName = latest["html_url"]!.GetValue<string>(),
                    UseShellExecute = true
                };
                Process.Start(website);
            }
            else
            {
                Console.WriteLine($"Up-to-date.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to check for updates:\n{ex}");
        }
    }
}
