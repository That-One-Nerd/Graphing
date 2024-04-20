using System;
using System.Windows.Forms;

namespace Graphing.Forms;

public partial class SetZoomForm : Form
{
    private readonly GraphForm refForm;

    private bool boxSelectEnabled;

    public SetZoomForm(GraphForm refForm)
    {
        InitializeComponent();
        this.refForm = refForm;

        refForm.Paint += (o, e) => RedeclareValues();
        RedeclareValues();

        MinBoxX.Leave += MinBoxX_Finish;
        MinBoxX.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) MinBoxX_Finish(o, e);
        };
        MaxBoxX.Leave += MaxBoxX_Finish;
        MaxBoxX.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) MaxBoxX_Finish(o, e);
        };

        MinBoxY.Leave += MinBoxY_Finish;
        MinBoxY.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) MinBoxY_Finish(o, e);
        };
        MaxBoxY.Leave += MaxBoxY_Finish;
        MaxBoxY.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) MaxBoxY_Finish(o, e);
        };
    }

    private void EnableBoxSelect_Click(object? sender, EventArgs e)
    {
        boxSelectEnabled = !boxSelectEnabled;
        refForm.canBoxSelect = boxSelectEnabled;

        if (boxSelectEnabled)
        {
            EnableBoxSelect.Text = $"Cancel ...";
            refForm.Focus();
        }
        else
        {
            EnableBoxSelect.Text = "Box Select";
        }
    }
    private void MatchAspectButton_Click(object? sender, EventArgs e)
    {
        double zoomXFactor = refForm.ZoomLevel.x / refForm.ZoomLevel.y;
        double actualXFactor = refForm.ClientRectangle.Width / refForm.ClientRectangle.Height;

        double diff = actualXFactor / zoomXFactor;
        int newWidth = (int)(refForm.Width / diff);
        refForm.ZoomLevel = new(refForm.ZoomLevel.x * diff, refForm.ZoomLevel.y);

        int maxScreenWidth = Screen.FromControl(refForm).WorkingArea.Width;
        if (newWidth >= maxScreenWidth)
        {
            refForm.Location = new(0, refForm.Location.Y);

            double xScaleFactor = (double)maxScreenWidth / newWidth;
            newWidth = maxScreenWidth;
            refForm.Height = (int)(refForm.Height * xScaleFactor);
            refForm.ZoomLevel = new(refForm.ZoomLevel.x * xScaleFactor, refForm.ZoomLevel.y * xScaleFactor);
        }

        refForm.Width = newWidth;
    }
    private void NormalizeButton_Click(object? sender, EventArgs e)
    {
        double factor = 1 / Math.Min(refForm.ZoomLevel.x, refForm.ZoomLevel.y);
        refForm.ZoomLevel = new(factor * refForm.ZoomLevel.x, factor * refForm.ZoomLevel.y);
    }
    private void ResetButton_Click(object? sender, EventArgs e)
    {
        refForm.ResetAllViewport();
    }
    private void ViewportLock_CheckedChanged(object? sender, EventArgs e)
    {
        refForm.ViewportLocked = ViewportLock.Checked;
        RedeclareValues();
    }

    private void MinBoxX_Finish(object? sender, EventArgs e)
    {
        if (double.TryParse(MinBoxX.Text, out double minX))
        {
            Float2 min = refForm.MinVisibleGraph, max = refForm.MaxVisibleGraph;

            double newCenterX = (minX + max.x) / 2,
                   zoomFactorX = (max.x - minX) / (max.x - min.x);

            refForm.ScreenCenter = new(newCenterX, refForm.ScreenCenter.y);
            refForm.ZoomLevel = new(refForm.ZoomLevel.x * zoomFactorX, refForm.ZoomLevel.y);
        }

        refForm.Invalidate(false);
    }
    private void MaxBoxX_Finish(object? sender, EventArgs e)
    {
        if (double.TryParse(MaxBoxX.Text, out double maxX))
        {
            Float2 min = refForm.MinVisibleGraph, max = refForm.MaxVisibleGraph;

            double newCenterX = (min.x + maxX) / 2,
                   zoomFactorX = (maxX - min.x) / (max.x - min.x);

            refForm.ScreenCenter = new(newCenterX, refForm.ScreenCenter.y);
            refForm.ZoomLevel = new(refForm.ZoomLevel.x * zoomFactorX, refForm.ZoomLevel.y);
        }

        refForm.Invalidate(false);
    }
    private void MinBoxY_Finish(object? sender, EventArgs e)
    {
        if (double.TryParse(MinBoxY.Text, out double minY))
        {
            Float2 min = refForm.MinVisibleGraph, max = refForm.MaxVisibleGraph;

            double newCenterY = -(minY + max.y) / 2, // Keeping it positive flips it for some reason ???
                   zoomFactorY = (max.y - minY) / (max.y - min.y);

            refForm.ScreenCenter = new(refForm.ScreenCenter.x, newCenterY);
            refForm.ZoomLevel = new(refForm.ZoomLevel.x, refForm.ZoomLevel.y * zoomFactorY);
        }

        refForm.Invalidate(false);
    }
    private void MaxBoxY_Finish(object? sender, EventArgs e)
    {
        if (double.TryParse(MaxBoxY.Text, out double maxY))
        {
            Float2 min = refForm.MinVisibleGraph, max = refForm.MaxVisibleGraph;

            double newCenterY = -(min.y + maxY) / 2, // Keeping it positive flips it for some reason ???
                   zoomFactorY = (maxY - min.y) / (max.y - min.y);

            refForm.ScreenCenter = new(refForm.ScreenCenter.x, newCenterY);
            refForm.ZoomLevel = new(refForm.ZoomLevel.x, refForm.ZoomLevel.y * zoomFactorY);
        }

        refForm.Invalidate(false);
    }

    public void RedeclareValues()
    {
        bool enabled = !refForm.ViewportLocked;

        Float2 minGraph = refForm.MinVisibleGraph,
               maxGraph = refForm.MaxVisibleGraph;

        MinBoxX.Text = $"{minGraph.x:0.000}";
        MaxBoxX.Text = $"{maxGraph.x:0.000}";
        MinBoxY.Text = $"{minGraph.y:0.000}";
        MaxBoxY.Text = $"{maxGraph.y:0.000}";

        ViewportLock.Checked = !enabled;
        EnableBoxSelect.Enabled = enabled;
        MatchAspectButton.Enabled = enabled;
        NormalizeButton.Enabled = enabled;
        ResetButton.Enabled = enabled;
        MinBoxX.Enabled = enabled;
        MaxBoxX.Enabled = enabled;
        MinBoxY.Enabled = enabled;
        MaxBoxY.Enabled = enabled;
    }

    internal void CompleteBoxSelection()
    {
        if (boxSelectEnabled) EnableBoxSelect_Click(null, new());
    }
}
