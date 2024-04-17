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

        refForm.OnZoomLevelChanged += (o, e) => RedeclareValues();
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
    private void ResetButton_Click(object? sender, EventArgs e)
    {
        refForm.ResetAllViewport();
    }

    private void RedeclareValues()
    {
        Invalidate(false);
    }

    internal void CompleteBoxSelection()
    {
        if (boxSelectEnabled) EnableBoxSelect_Click(null, new());
    }

    private void NormalizeButton_Click(object sender, EventArgs e)
    {
        double factor = 1 / Math.Min(refForm.ZoomLevel.x, refForm.ZoomLevel.y);
        refForm.ZoomLevel = new(factor * refForm.ZoomLevel.x, factor * refForm.ZoomLevel.y);
    }
}
