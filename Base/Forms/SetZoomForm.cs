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

    private void RedeclareValues()
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

    private void ViewportLock_CheckedChanged(object? sender, EventArgs e)
    {
        refForm.ViewportLocked = ViewportLock.Checked;
        RedeclareValues();
    }
}
