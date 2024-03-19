using System;
using System.Windows.Forms;

namespace Graphing.Forms;

public partial class SetZoomForm : Form
{
    private double minZoomRange;
    private double maxZoomRange;

    private double zoomLevel;

    private readonly GraphForm form;

    public SetZoomForm(GraphForm form)
    {
        InitializeComponent();

        minZoomRange = 1 / (form.ZoomLevel * 2);
        maxZoomRange = 2 / form.ZoomLevel;
        zoomLevel = 1 / form.ZoomLevel;

        ZoomTrackBar.Value = (int)(ZoomToFactor(zoomLevel) * (ZoomTrackBar.Maximum - ZoomTrackBar.Minimum) + ZoomTrackBar.Minimum);

        this.form = form;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        ZoomMaxValue.Text = maxZoomRange.ToString("0.00");
        ZoomMinValue.Text = minZoomRange.ToString("0.00");

        ValueLabel.Text = $"{zoomLevel:0.00}x";

        base.OnPaint(e);

        form.ZoomLevel = 1 / zoomLevel;
        form.Invalidate(false);
    }

    private double FactorToZoom(double factor)
    {
        return minZoomRange + (factor * factor) * (maxZoomRange - minZoomRange);
    }
    private double ZoomToFactor(double zoom)
    {
        double sqrValue = (zoom - minZoomRange) / (maxZoomRange - minZoomRange);
        return Math.Sign(sqrValue) * Math.Sqrt(Math.Abs(sqrValue));
    }

    private void ZoomTrackBar_Scroll(object? sender, EventArgs e)
    {
        double factor = (ZoomTrackBar.Value - ZoomTrackBar.Minimum) / (double)(ZoomTrackBar.Maximum - ZoomTrackBar.Minimum);
        zoomLevel = FactorToZoom(factor);

        Invalidate(true);
    }

    private void ZoomMinValue_TextChanged(object? sender, EventArgs e)
    {
        double original = minZoomRange;
        try
        {
            double value;
            if (string.IsNullOrWhiteSpace(ZoomMinValue.Text) ||
                ZoomMinValue.Text.EndsWith('.'))
            {
                return;
            }
            else
            {
                value = double.Parse(ZoomMinValue.Text);
                if (value < 1e-2 || value > 1e3 || value > maxZoomRange) throw new();
            }

            minZoomRange = value;
            ZoomTrackBar.Value = (int)Math.Clamp(ZoomToFactor(zoomLevel) * (ZoomTrackBar.Maximum - ZoomTrackBar.Minimum) + ZoomTrackBar.Minimum, ZoomTrackBar.Minimum, ZoomTrackBar.Maximum);
            double factor = (ZoomTrackBar.Value - ZoomTrackBar.Minimum) / (double)(ZoomTrackBar.Maximum - ZoomTrackBar.Minimum);
            double newZoom = FactorToZoom(factor);

            zoomLevel = newZoom;
            if (newZoom != factor) Invalidate(true);
        }
        catch
        {
            minZoomRange = original;
            ZoomMinValue.Text = minZoomRange.ToString("0.00");
        }
    }

    private void ZoomMaxValue_TextChanged(object sender, EventArgs e)
    {
        double original = maxZoomRange;
        try
        {
            double value;
            if (string.IsNullOrWhiteSpace(ZoomMaxValue.Text) ||
                ZoomMaxValue.Text.EndsWith('.'))
            {
                return;
            }
            else
            {
                value = double.Parse(ZoomMaxValue.Text);
                if (value < 1e-2 || value > 1e3 || value < minZoomRange) throw new();
            }

            maxZoomRange = value;
            ZoomTrackBar.Value = (int)Math.Clamp(ZoomToFactor(zoomLevel) * (ZoomTrackBar.Maximum - ZoomTrackBar.Minimum) + ZoomTrackBar.Minimum, ZoomTrackBar.Minimum, ZoomTrackBar.Maximum);
            double factor = (ZoomTrackBar.Value - ZoomTrackBar.Minimum) / (double)(ZoomTrackBar.Maximum - ZoomTrackBar.Minimum);
            double newZoom = FactorToZoom(factor);

            zoomLevel = newZoom;
            if (newZoom != factor) Invalidate(true);
        }
        catch
        {
            maxZoomRange = original;
            ZoomMaxValue.Text = maxZoomRange.ToString("0.00");
        }
    }
}
