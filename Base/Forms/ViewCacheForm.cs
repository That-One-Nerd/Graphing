using Graphing.Extensions;

namespace Graphing.Forms;

public partial class ViewCacheForm : Form
{
    private readonly GraphForm refForm;

    private readonly List<Label> labelCache;
    private readonly List<Button> buttonCache;

    public ViewCacheForm(GraphForm thisForm)
    {
        InitializeComponent();

        refForm = thisForm;
        refForm.Paint += (o, e) => UpdatePieChart();
        labelCache = [];
        buttonCache = [];
        UpdatePieChart();
    }

    private void UpdatePieChart()
    {
        CachePie.Values.Clear();

        long totalBytes = 0;
        int index = 0;
        foreach (Graphable able in refForm.Graphables)
        {
            long thisBytes = able.GetCacheBytes();
            CachePie.Values.Add((able.Color, thisBytes));
            totalBytes += thisBytes;

            if (index < labelCache.Count)
            {
                Label reuseLabel = labelCache[index];
                reuseLabel.ForeColor = able.Color;
                reuseLabel.Text = $"{able.Name}: {thisBytes.FormatAsBytes()}";
            }
            else
            {
                Label newText = new()
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                    AutoEllipsis = true,
                    ForeColor = able.Color,
                    Location = new Point(0, labelCache.Count * 46),
                    Parent = SpecificCachePanel,
                    Size = new Size(SpecificCachePanel.Width - 98, 46),
                    Text = $"{able.Name}: {thisBytes.FormatAsBytes()}",
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                labelCache.Add(newText);
            }

            if (index >= buttonCache.Count)
            {
                Button newButton = new()
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(SpecificCachePanel.Width - 92, buttonCache.Count * 46),
                    Parent = SpecificCachePanel,
                    Size = new Size(92, 46),
                    Text = "Clear"
                };
                newButton.Click += (o, e) => EraseSpecificGraphable_Click(able);
                buttonCache.Add(newButton);
            }

            index++;
        }

        TotalCacheText.Text = $"Total Cache: {totalBytes.FormatAsBytes()}";

        Invalidate(true);
    }

    private void EraseAllCacheButton_Click(object? sender, EventArgs e)
    {
        foreach (Graphable able in refForm.Graphables) able.EraseCache();
        refForm.Invalidate(false);
    }
    private void EraseSpecificGraphable_Click(Graphable able)
    {
        able.EraseCache();
        refForm.Invalidate(false);
    }
}
