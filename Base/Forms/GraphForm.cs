using Graphing.Abstract;
using Graphing.Helpers;
using Graphing.Parts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace Graphing.Forms;

public partial class GraphForm : Form
{
    public static readonly Color BackgroundColor = Color.White;
    public static readonly Color MainAxisColor = Color.Black;
    public static readonly Color SemiAxisColor = Color.FromArgb(unchecked((int)0xFF_999999));
    public static readonly Color QuarterAxisColor = Color.FromArgb(unchecked((int)0xFF_E0E0E0));
    public static readonly Color UnitsTextColor = Color.Black;
    public static readonly Color ZoomBoxColor = Color.Black;

    public Float2 ScreenCenter { get; private set; }
    public Float2 Dpi { get; private set; }

    public float DpiFloat { get; private set; }

    public Float2 ZoomLevel
    {
        get => _zoomLevel;
        set
        {
            _zoomLevel = new(Math.Clamp(value.x, 1e-5, 1e3),
                             Math.Clamp(value.y, 1e-5, 1e3));
            OnZoomLevelChanged(this, new());
            Invalidate(false);
        }
    }
    private Float2 _zoomLevel;

    private readonly Point initialWindowPos;
    private readonly Size initialWindowSize;

    public Graphable[] Graphables => ables.ToArray();

    public Float2 MinVisibleGraph => ScreenSpaceToGraphSpace(new Int2(0, ClientRectangle.Height));
    public Float2 MaxVisibleGraph => ScreenSpaceToGraphSpace(new Int2(ClientRectangle.Width, 0));

    private readonly List<Graphable> ables;

    public event EventHandler OnZoomLevelChanged = delegate { };

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
        ZoomLevel = new(1, 1);
        initialWindowPos = Location;
        initialWindowSize = Size;

        RunUpdateChecker();
    }

    public Int2 GraphSpaceToScreenSpace(Float2 graphPoint)
    {
        graphPoint.y = -graphPoint.y;

        graphPoint.x -= ScreenCenter.x;
        graphPoint.y -= ScreenCenter.y;

        graphPoint.x *= Dpi.x / ZoomLevel.x;
        graphPoint.y *= Dpi.y / ZoomLevel.y;

        graphPoint.x += ClientRectangle.Width / 2.0;
        graphPoint.y += ClientRectangle.Height / 2.0;

        return new((int)graphPoint.x, (int)graphPoint.y);
    }
    public Float2 ScreenSpaceToGraphSpace(Int2 screenPoint)
    {
        Float2 result = new(screenPoint.x, screenPoint.y);

        result.x -= ClientRectangle.Width / 2.0;
        result.y -= ClientRectangle.Height / 2.0;

        result.x /= Dpi.x / ZoomLevel.x;
        result.y /= Dpi.y / ZoomLevel.y;

        result.x += ScreenCenter.x;
        result.y += ScreenCenter.y;

        result.y = -result.y;

        return result;
    }

    protected virtual void PaintGrid(Graphics g)
    {
        double axisScaleX = Math.Pow(2, Math.Round(Math.Log2(ZoomLevel.x))),
               axisScaleY = Math.Pow(2, Math.Round(Math.Log2(ZoomLevel.y)));

        // Draw horizontal/vertical quarter-axis.
        Brush quarterBrush = new SolidBrush(QuarterAxisColor);
        Pen quarterPen = new(quarterBrush, DpiFloat * 2 / 192);

        for (double x = Math.Ceiling(MinVisibleGraph.x * 4 / axisScaleX) * axisScaleX / 4; x <= Math.Floor(MaxVisibleGraph.x * 4 / axisScaleX) * axisScaleX / 4; x += axisScaleX / 4)
        {
            Int2 startPos = GraphSpaceToScreenSpace(new Float2(x, MinVisibleGraph.y)),
                 endPos = GraphSpaceToScreenSpace(new Float2(x, MaxVisibleGraph.y));
            g.DrawLine(quarterPen, startPos, endPos);
        }
        for (double y = Math.Ceiling(MinVisibleGraph.y * 4 / axisScaleY) * axisScaleY / 4; y <= Math.Floor(MaxVisibleGraph.y * 4 / axisScaleY) * axisScaleY / 4; y += axisScaleY / 4)
        {
            Int2 startPos = GraphSpaceToScreenSpace(new Float2(MinVisibleGraph.x, y)),
                 endPos = GraphSpaceToScreenSpace(new Float2(MaxVisibleGraph.x, y));
            g.DrawLine(quarterPen, startPos, endPos);
        }

        // Draw horizontal/vertical semi-axis.
        Brush semiBrush = new SolidBrush(SemiAxisColor);
        Pen semiPen = new(semiBrush, DpiFloat * 2 / 192);

        for (double x = Math.Ceiling(MinVisibleGraph.x / axisScaleX) * axisScaleX; x <= Math.Floor(MaxVisibleGraph.x / axisScaleX) * axisScaleX; x += axisScaleX)
        {
            Int2 startPos = GraphSpaceToScreenSpace(new Float2(x, MinVisibleGraph.y)),
                 endPos = GraphSpaceToScreenSpace(new Float2(x, MaxVisibleGraph.y));
            g.DrawLine(semiPen, startPos, endPos);
        }
        for (double y = Math.Ceiling(MinVisibleGraph.y / axisScaleY) * axisScaleY; y <= Math.Floor(MaxVisibleGraph.y / axisScaleY) * axisScaleY; y += axisScaleY)
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
        double axisScaleX = Math.Pow(2, Math.Round(Math.Log2(ZoomLevel.x))),
               axisScaleY = Math.Pow(2, Math.Round(Math.Log2(ZoomLevel.y)));
        Brush textBrush = new SolidBrush(UnitsTextColor);
        Font textFont = new(Font.Name, 9, FontStyle.Regular);

        // X-axis
        int minX = (int)(DpiFloat * 50 / 192),
            maxX = ClientRectangle.Height - (int)(DpiFloat * 40 / 192);
        for (double x = Math.Ceiling(MinVisibleGraph.x / axisScaleX) * axisScaleX; x <= MaxVisibleGraph.x; x += axisScaleX)
        {
            if (x == 0) x = 0; // Fixes -0

            Int2 screenPos = GraphSpaceToScreenSpace(new Float2(x, 0));

            if (screenPos.y < minX) screenPos.y = minX;
            else if (screenPos.y > maxX) screenPos.y = maxX;

            g.DrawString($"{x}", textFont, textBrush, screenPos.x, screenPos.y);
        }

        // Y-axis
        int minY = (int)(DpiFloat * 10 / 192);
        for (double y = Math.Ceiling(MinVisibleGraph.y / axisScaleY) * axisScaleY; y <= MaxVisibleGraph.y; y += axisScaleY)
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
        if (selectState == SelectionState.GraphSelect)
        {
            for (int i = 0; i < ables.Count; i++)
            {
                if (ables[i].ShouldSelectGraphable(this, graphMousePos, 2.5))
                {
                    IEnumerable<IGraphPart> selectionParts = ables[i].GetSelectionItemsToRender(this, graphMousePos);
                    foreach (IGraphPart selPart in selectionParts) selPart.Render(this, g, graphPens[i]);
                }
            }
        }
        else if (selectState == SelectionState.ZoomBox)
        {
            // Draw the current box selection.
            Int2 boxPosA = GraphSpaceToScreenSpace(boxSelectA),
                 boxPosB = GraphSpaceToScreenSpace(boxSelectB);

            if (boxPosA.x > boxPosB.x) (boxPosA.x, boxPosB.x) = (boxPosB.x, boxPosA.x);
            if (boxPosA.y > boxPosB.y) (boxPosA.y, boxPosB.y) = (boxPosB.y, boxPosA.y);

            Pen boxPen = new(ZoomBoxColor, 2 * DpiFloat / 192);
            g.DrawRectangle(boxPen, new(boxPosA.x, boxPosA.y,
                                        boxPosB.x - boxPosA.x,
                                        boxPosB.y - boxPosA.y));
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

    public bool IsGraphPointVisible(Float2 point)
    {
        Int2 pixelPos = GraphSpaceToScreenSpace(point);
        return pixelPos.x >= 0 && pixelPos.x < ClientRectangle.Width &&
               pixelPos.y >= 0 && pixelPos.y < ClientRectangle.Height;
    }

    private SelectionState selectState = SelectionState.None;
    internal bool canBoxSelect;
    private SetZoomForm? setZoomForm;

    private Int2 initialMouseLocation;
    private Float2 initialScreenCenter;

    private Float2 boxSelectA, boxSelectB;

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (selectState == SelectionState.None && canBoxSelect)
        {
            Point clientMousePos = PointToClient(Cursor.Position);
            Float2 graphMousePos = ScreenSpaceToGraphSpace(new(clientMousePos.X,
                                                               clientMousePos.Y));

            boxSelectA = graphMousePos;
            boxSelectB = graphMousePos;

            selectState = SelectionState.ZoomBox;
        }

        if (selectState == SelectionState.None)
        {
            Point clientMousePos = PointToClient(Cursor.Position);
            Float2 graphMousePos = ScreenSpaceToGraphSpace(new(clientMousePos.X,
                                                               clientMousePos.Y));
            foreach (Graphable able in Graphables)
            {
                if (able.ShouldSelectGraphable(this, graphMousePos, 1))
                    selectState = SelectionState.GraphSelect;
            }
            if (selectState == SelectionState.GraphSelect) Invalidate(false);
        }

        if (selectState == SelectionState.None)
        {
            selectState = SelectionState.ViewportDrag;
            initialMouseLocation = new Int2(Cursor.Position.X, Cursor.Position.Y);
            initialScreenCenter = ScreenCenter;
        }
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (selectState == SelectionState.ViewportDrag)
        {
            Int2 pixelDiff = new(initialMouseLocation.x - Cursor.Position.X,
                             initialMouseLocation.y - Cursor.Position.Y);
            Float2 graphDiff = new(pixelDiff.x * ZoomLevel.x / Dpi.x, pixelDiff.y * ZoomLevel.y / Dpi.y);
            ScreenCenter = new(initialScreenCenter.x + graphDiff.x,
                               initialScreenCenter.y + graphDiff.y);
        }
        else if (selectState == SelectionState.ZoomBox)
        {
            Point clientMousePos = PointToClient(Cursor.Position);
            Float2 graphMousePos = ScreenSpaceToGraphSpace(new(clientMousePos.X,
                                                               clientMousePos.Y));
            boxSelectB = graphMousePos;

            // Set center.
            ScreenCenter = new((boxSelectA.x + boxSelectB.x) * 0.5,
                              -(boxSelectA.y + boxSelectB.y) * 0.5);

            // Set zoom. Kind of weird but it works.
            Float2 minGraph = MinVisibleGraph, maxGraph = MaxVisibleGraph;
            Float2 oldDist = new(maxGraph.x - minGraph.x,
                                 maxGraph.y - minGraph.y);
            Float2 newDist = new(Math.Abs(boxSelectB.x - boxSelectA.x),
                                 Math.Abs(boxSelectB.y - boxSelectA.y));
            ZoomLevel = new(ZoomLevel.x * newDist.x / oldDist.x,
                            ZoomLevel.y * newDist.y / oldDist.y);

            setZoomForm!.CompleteBoxSelection();

            boxSelectA = new(0, 0);
            boxSelectB = new(0, 0);
        }
        selectState = SelectionState.None;
        Invalidate(false);
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (selectState == SelectionState.ViewportDrag)
        {
            Int2 pixelDiff = new(initialMouseLocation.x - Cursor.Position.X,
                             initialMouseLocation.y - Cursor.Position.Y);
            Float2 graphDiff = new(pixelDiff.x * ZoomLevel.x / Dpi.x, pixelDiff.y * ZoomLevel.y / Dpi.y);
            ScreenCenter = new(initialScreenCenter.x + graphDiff.x,
                               initialScreenCenter.y + graphDiff.y);
        }
        else if (selectState == SelectionState.ZoomBox)
        {
            Point clientMousePos = PointToClient(Cursor.Position);
            Float2 graphMousePos = ScreenSpaceToGraphSpace(new(clientMousePos.X,
                                                               clientMousePos.Y));
            boxSelectB = graphMousePos;
        }
        Invalidate(false);
    }
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        Float2 newZoom = ZoomLevel;
        newZoom.x *= 1 - e.Delta * 0.00075; // Zoom factor.
        newZoom.y *= 1 - e.Delta * 0.00075;
        ZoomLevel = newZoom;

        Invalidate(false);
    }

    private void ResetViewportButton_Click(object? sender, EventArgs e)
    {
        ResetAllViewport();
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
        MenuConvertSlopeField.DropDownItems.Clear();
        MenuOperationsTranslate.DropDownItems.Clear();
        // At some point, we'll have a Convert To Column Table button,
        // but I'll need to make a form for the ranges when I do that.

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
            if (able is IConvertEquation equConvert)
            {
                ToolStripMenuItem equItem = new()
                {
                    ForeColor = able.Color,
                    Text = able.Name
                };
                equItem.Click += (o, e) =>
                {
                    if (equConvert.UngraphWhenConvertedToEquation) Ungraph(able);
                    Graph(equConvert.ToEquation());
                };
                MenuConvertEquation.DropDownItems.Add(equItem);
            }
            if (able is IConvertSlopeField sfConvert)
            {
                ToolStripMenuItem sfItem = new()
                {
                    ForeColor = able.Color,
                    Text = able.Name
                };
                sfItem.Click += (o, e) =>
                {
                    if (sfConvert.UngraphWhenConvertedToSlopeField) Ungraph(able);
                    Graph(sfConvert.ToSlopeField(2));
                };
                MenuConvertSlopeField.DropDownItems.Add(sfItem);
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
        if (setZoomForm is not null)
        {
            setZoomForm.Focus();
            return;
        }

        SetZoomForm zoomForm = new(this)
        {
            StartPosition = FormStartPosition.Manual,
        };
        zoomForm.Location = new Point(Location.X + ClientRectangle.Width + 10,
                                    Location.Y + (ClientRectangle.Height - zoomForm.ClientRectangle.Height) / 2);

        if (zoomForm.Location.X + zoomForm.Width > Screen.FromControl(this).WorkingArea.Width)
        {
            zoomForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
        }

        setZoomForm = zoomForm;
        zoomForm.Show();
        zoomForm.FormClosing += (o, e) =>
        {
            zoomForm.CompleteBoxSelection();
            setZoomForm = null;
        };
    }
    private void ButtonViewportSetCenter_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("TODO", "Set Center Position", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    private void ButtonViewportReset_Click(object? sender, EventArgs e)
    {
        ScreenCenter = new Float2(0, 0);
        ZoomLevel = new(1, 1);
        Invalidate(false);
    }
    private void ButtonViewportResetWindow_Click(object? sender, EventArgs e)
    {
        Location = initialWindowPos;
        Size = initialWindowSize;
        WindowState = FormWindowState.Normal;
    }

    public void ResetAllViewport()
    {
        ScreenCenter = new Float2(0, 0);
        ZoomLevel = new(1, 1);
        Location = initialWindowPos;
        Size = initialWindowSize;
        WindowState = FormWindowState.Normal;
        Invalidate(false);
    }

    private ViewCacheForm? cacheForm;
    private void MenuMiscCaches_Click(object? sender, EventArgs e)
    {
        if (this.cacheForm is not null)
        {
            this.cacheForm.Focus();
            return;
        }

        ViewCacheForm cacheForm = new(this)
        {
            StartPosition = FormStartPosition.Manual
        };
        this.cacheForm = cacheForm;

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

    private static async void RunUpdateChecker()
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

                string? project = UpdaterHelper.GetProjectPath();
                if (project is null)
                {
                    MessageBox.Show("Cannot find project root. You'll likely have to update manually.",
                                    "Error running automatic updates.", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                if (await UpdaterHelper.UsingNugetPackage(project))
                {
                    Console.WriteLine($"Attempting to update via the NuGet Package Manager.");
                    bool status = await UpdaterHelper.UpdateProjectByNuget(latest["tag_name"]!.GetValue<string>(), project);
                    if (!status)
                    {
                        MessageBox.Show("Failed to update with the NuGet Package Manager. " +
                                        "You'll likely have to update manually.",
                                        "Error running automatic updates.", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"Attempting to update via a GitHub asset download.");
                    bool status = await UpdaterHelper.UpdateProjectByGitHub(latest["tag_name"]!.GetValue<string>(), project);
                    if (status)
                    {
                        MessageBox.Show("Update ready. Restart the project to complete the update.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to update with an automatic download. " +
                                        "You'll likely have to update manually.",
                                        "Error running automatic updates.", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                }
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

    private enum SelectionState
    {
        None = 0,
        ViewportDrag,
        GraphSelect,
        ZoomBox,
    }
}
