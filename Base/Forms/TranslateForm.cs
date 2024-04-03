using Graphing.Abstract;
using System;
using System.Windows.Forms;

namespace Graphing.Forms;

public partial class TranslateForm : Form
{
    private readonly GraphForm refForm;

    // These variables both represent the same graphable.
    private readonly ITranslatableX? ableTransX;
    private readonly ITranslatableY? ableTransY;

    private readonly bool useX;
    private readonly bool useY;

    private double minX, maxX, curX, minY, maxY, curY;

    public TranslateForm(GraphForm graph, Graphable ableRaw, ITranslatable ableTrans)
    {
        InitializeComponent();

        Text = $"Translate {ableRaw.Name}";
        TitleLabel.Text = $"Adjust Location for {ableRaw.Name}";

        MinBoxX.Leave += (o, e) => UpdateFromMinBoxX();
        MinBoxX.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) UpdateFromMinBoxX();
        };
        MaxBoxX.Leave += (o, e) => UpdateFromMaxBoxX();
        MaxBoxX.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) UpdateFromMaxBoxX();
        };
        ThisValueX.Leave += (o, e) => UpdateFromThisBoxX();
        ThisValueX.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) UpdateFromThisBoxX();
        };

        MinBoxY.Leave += (o, e) => UpdateFromMinBoxY();
        MinBoxY.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) UpdateFromMinBoxY();
        };
        MaxBoxY.Leave += (o, e) => UpdateFromMaxBoxY();
        MaxBoxY.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) UpdateFromMaxBoxY();
        };
        ThisValueY.Leave += (o, e) => UpdateFromThisBoxY();
        ThisValueY.KeyDown += (o, e) =>
        {
            if (e.KeyCode == Keys.Enter) UpdateFromThisBoxY();
        };

        refForm = graph;

        double curX = 0, curY = 0;
        if (ableTrans is ITranslatableX transX)
        {
            useX = true;
            ableTransX = transX;
            curX = transX.OffsetX;
        }
        else
        {
            LabelY.Location = LabelX.Location;
            TrackY.Location = TrackX.Location;
            MinBoxY.Location = MinBoxX.Location;
            MaxBoxY.Location = MaxBoxX.Location;
            ThisValueY.Location = ThisValueX.Location;

            LabelX.Dispose();
            TrackX.Dispose();
            MinBoxX.Dispose();
            MaxBoxX.Dispose();
            ThisValueX.Dispose();
        }

        if (ableTrans is ITranslatableY transY)
        {
            useY = true;
            ableTransY = transY;
            curY = transY.OffsetY;
        }
        else
        {
            LabelY.Dispose();
            TrackY.Dispose();
            MinBoxY.Dispose();
            MaxBoxY.Dispose();
            ThisValueY.Dispose();
        }

        if (!useX && !useY)
        {
            TitleLabel.Text = $"There doesn't seem to be anything you can translate for {ableRaw.Name}.";
        }

        // TODO: Maybe replace these default limits with what's visible on screen?
        //       Tried it and it got a bit confusing so maybe not.
        minX = -10;
        maxX = 10;
        minY = -10;
        maxY = 10;

        UpdateFromCurX(curX, false);
        UpdateFromCurY(curY, false);
    }

    private void UpdateFromCurX(double newCurX, bool invalidate)
    {
        curX = newCurX;
        if (curX < minX) minX = curX;
        else if (curX > maxX) maxX = curX;

        int step = (int)(1000 * InverseLerp(minX, maxX, curX));
        TrackX.Value = step;
        MinBoxX.Text = $"{minX:0.00}";
        MaxBoxX.Text = $"{maxX:0.00}";
        ThisValueX.Text = $"{curX:0.00}";

        if (invalidate) refForm.Invalidate(false);
    }
    private void UpdateFromSliderX(bool invalidate)
    {
        double t = InverseLerp(0, 1000, TrackX.Value);
        curX = Lerp(minX, maxX, t);

        ThisValueX.Text = $"{curX:0.00}";
        ableTransX!.OffsetX = curX;

        if (invalidate) refForm.Invalidate(false);
    }
    private void UpdateFromMinBoxX()
    {
        if (!double.TryParse(MinBoxX.Text, out double newMin))
        {
            MinBoxX.Text = $"{minX:0.00}";
            return;
        }
        minX = newMin;
        MinBoxX.Text = $"{minX:0.00}";

        if (minX > curX)
        {
            curX = minX;
            ThisValueX.Text = $"{curX:0.00}";
            ableTransX!.OffsetX = curX;
        }

        int step = (int)(1000 * InverseLerp(minX, maxX, curX));
        TrackX.Value = step;

        refForm.Invalidate(false);
    }
    private void UpdateFromMaxBoxX()
    {
        if (!double.TryParse(MaxBoxX.Text, out double newMax))
        {
            MaxBoxX.Text = $"{maxX:0.00}";
            return;
        }

        maxX = newMax;
        MaxBoxX.Text = $"{maxX:0.00}";

        if (maxX < curX)
        {
            curX = maxX;
            ThisValueX.Text = $"{curX:0.00}";
            ableTransX!.OffsetX = curX;
        }

        int step = (int)(1000 * InverseLerp(minX, maxX, curX));
        TrackX.Value = step;

        refForm.Invalidate(false);
    }
    private void UpdateFromThisBoxX()
    {
        if (!double.TryParse(ThisValueX.Text, out double newCur))
        {
            ThisValueX.Text = $"{curX:0.00}";
            return;
        }
        ableTransX!.OffsetX = newCur;
        UpdateFromCurX(newCur, true);
    }

    private void UpdateFromCurY(double newCurY, bool invalidate)
    {
        curY = newCurY;
        if (curY < minY) minY = curY;
        else if (curY > maxY) maxY = curY;

        int step = (int)(1000 * InverseLerp(minY, maxY, curY));
        TrackY.Value = step;
        MinBoxY.Text = $"{minY:0.00}";
        MaxBoxY.Text = $"{maxY:0.00}";
        ThisValueY.Text = $"{curY:0.00}";

        if (invalidate) refForm.Invalidate(false);
    }
    private void UpdateFromSliderY(bool invalidate)
    {
        double t = InverseLerp(0, 1000, TrackY.Value);
        curY = Lerp(minY, maxY, t);

        ThisValueY.Text = $"{curY:0.00}";
        ableTransY!.OffsetY = curY;

        if (invalidate) refForm.Invalidate(false);
    }
    private void UpdateFromMinBoxY()
    {
        if (!double.TryParse(MinBoxY.Text, out double newMin))
        {
            MinBoxY.Text = $"{minY:0.00}";
            return;
        }
        minY = newMin;
        MinBoxY.Text = $"{minY:0.00}";

        if (minY > curY)
        {
            curY = minY;
            ThisValueY.Text = $"{curY:0.00}";
            ableTransY!.OffsetY = curY;
        }

        int step = (int)(1000 * InverseLerp(minY, maxY, curY));
        TrackY.Value = step;

        refForm.Invalidate(false);
    }
    private void UpdateFromMaxBoxY()
    {
        if (!double.TryParse(MaxBoxY.Text, out double newMax))
        {
            MaxBoxY.Text = $"{maxY:0.00}";
            return;
        }

        maxY = newMax;
        MaxBoxY.Text = $"{maxY:0.00}";

        if (maxY < curY)
        {
            curY = maxY;
            ThisValueY.Text = $"{curY:0.00}";
            ableTransY!.OffsetY = curY;
        }

        int step = (int)(1000 * InverseLerp(minY, maxY, curY));
        TrackY.Value = step;

        refForm.Invalidate(false);
    }
    private void UpdateFromThisBoxY()
    {
        if (!double.TryParse(ThisValueY.Text, out double newCur))
        {
            ThisValueY.Text = $"{curY:0.00}";
            return;
        }
        ableTransY!.OffsetY = newCur;
        UpdateFromCurY(newCur, true);
    }

    private static double Lerp(double a, double b, double t) => a + t * (b - a);
    private static double InverseLerp(double a, double b, double c) => (c - a) / (b - a);

    private void TrackX_Scroll(object sender, EventArgs e)
    {
        UpdateFromSliderX(true);
    }
    private void TrackY_Scroll(object sender, EventArgs e)
    {
        UpdateFromSliderY(true);
    }
}
