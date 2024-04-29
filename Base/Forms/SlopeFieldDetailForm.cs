using Graphing.Graphables;
using System;
using System.Windows.Forms;

namespace Graphing.Forms;

public partial class SlopeFieldDetailForm : Form
{
    private readonly GraphForm refForm;
    private readonly SlopeField slopeField;

    private double minDetail, maxDetail;

    public SlopeFieldDetailForm(GraphForm form, SlopeField sf)
    {
        InitializeComponent();

        refForm = form;
        slopeField = sf;

        refForm.Paint += (o, e) => RedeclareValues();
        RedeclareValues();

        TrackSlopeDetail.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Right) IncrementButton_Click(o, e);
            else if (e.KeyCode == Keys.Left) DecrementButton_Click(o, e);
        };

        MinDetailBox.Leave += MinDetailBox_Finish;
        MinDetailBox.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) MinDetailBox_Finish(o, e);
        };
        MaxDetailBox.Leave += MaxDetailBox_Finish;
        MaxDetailBox.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) MaxDetailBox_Finish(o, e);
        };
        CurrentDetailBox.Leave += CurrentDetailBox_Finish;
        CurrentDetailBox.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) CurrentDetailBox_Finish(o, e);
        };

        minDetail = sf.Detail / 2;
        maxDetail = sf.Detail * 2;

        Message.Text = Message.Text.Replace("%name%", sf.Name);
    }

    // Exponential interpolations are better than simple lerps here since
    // we're scaling a multiple rather than an additive.
    private double Interp(double t)
    {
        // This is weird. I don't like the +1s and -1s, I don't think I wrote this right.
        // But it seems to get the job done.
        return minDetail + Math.Pow(2, t * Math.Log2(maxDetail - minDetail + 1)) - 1;
    }
    private double InverseInterp(double c)
    {
        return Math.Log2(c - minDetail + 1) / Math.Log2(maxDetail - minDetail + 1);
    }

    private void RedeclareValues()
    {
        double detail = slopeField.Detail;
        if (detail < minDetail) minDetail = detail;
        else if (detail > maxDetail) maxDetail = detail;

        double t = InverseInterp(detail);
        TrackSlopeDetail.Value = (int)(TrackSlopeDetail.Minimum + t * (TrackSlopeDetail.Maximum - TrackSlopeDetail.Minimum));

        MinDetailBox.Text = $"{minDetail:0.00}";
        MaxDetailBox.Text = $"{maxDetail:0.00}";
        CurrentDetailBox.Text = $"{detail:0.00}";
    }

    private void TrackSlopeDetail_Scroll(object? sender, EventArgs e)
    {
        double t = (double)(TrackSlopeDetail.Value - TrackSlopeDetail.Minimum) / (TrackSlopeDetail.Maximum - TrackSlopeDetail.Minimum);
        double newDetail = Interp(t);

        slopeField.Detail = newDetail;
        refForm.Invalidate(false);
    }
    private void MinDetailBox_Finish(object? sender, EventArgs e)
    {
        if (double.TryParse(MinDetailBox.Text, out double newMinDetail))
        {
            minDetail = newMinDetail;
            if (minDetail > slopeField.Detail) slopeField.Detail = newMinDetail;
        }
        refForm.Invalidate(false);
    }
    private void MaxDetailBox_Finish(object? sender, EventArgs e)
    {
        if (double.TryParse(MaxDetailBox.Text, out double newMaxDetail))
        {
            maxDetail = newMaxDetail;
            if (maxDetail < slopeField.Detail) slopeField.Detail = newMaxDetail;
        }
        refForm.Invalidate(false);
    }
    private void CurrentDetailBox_Finish(object? sender, EventArgs e)
    {
        if (double.TryParse(CurrentDetailBox.Text, out double newDetail))
        {
            if (newDetail < minDetail) minDetail = newDetail;
            else if (newDetail > maxDetail) maxDetail = newDetail;
            slopeField.Detail = newDetail;
        }
        refForm.Invalidate(false);
    }

    private void IncrementButton_Click(object? sender, EventArgs e)
    {
        double newDetail = slopeField.Detail * 1.0625f;
        if (newDetail > maxDetail) maxDetail = newDetail;
        slopeField.Detail = newDetail;
        refForm.Invalidate(false);
    }
    private void DecrementButton_Click(object? sender, EventArgs e)
    {
        double newDetail = slopeField.Detail / 1.0625f;
        if (newDetail < minDetail) minDetail = newDetail;
        slopeField.Detail = newDetail;
        refForm.Invalidate(false);
    }
}
