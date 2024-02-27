namespace Graphing.Forms;

public partial class GraphColorPickerForm : Form
{
    public Color Result
    {
        get => _result;
        set
        {
            _result = value;

            if (able is not null) able.Color = value;
            graph?.Invalidate(false);
        }
    }
    private Color _result;

    private readonly Color initialColor;

    private readonly GraphForm graph;
    private readonly Graphable able;

    public GraphColorPickerForm(GraphForm graph, Graphable able)
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);

        Result = able.Color;
        initialColor = Result;

        InitializeComponent();
        this.graph = graph;
        this.able = able;

        Text = $"{able.Name} Color";
        MessageLabel.Text = $"Pick a color for {able.Name}.";

        // Add preset buttons.
        const int size = 48;
        int position = 0;
        foreach (uint cId in Graphable.DefaultColors)
        {
            Color color = Color.FromArgb((int)cId);
            Button button = new()
            {
                BackColor = color,
                Height = size,
                Location = new Point(0, position),
                Parent = PresetButtonPanel,
                Text = "",
                Width = PresetButtonPanel.Width,
            };
            button.Click += (o, e) => SetPresetColor(button.BackColor);
            position += size;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        UpdateResult(false);
    }

    private void SetPresetColor(Color preset)
    {
        Result = preset;
        UpdateResult(true);
    }

    private void UpdateResult(bool invalidate)
    {
        RedTrackBar.Value = Result.R;
        GreenTrackBar.Value = Result.G;
        BlueTrackBar.Value = Result.B;
        ResultView.BackColor = Result;

        RedValueBox.Text = Result.R.ToString();
        GreenValueBox.Text = Result.G.ToString();
        BlueValueBox.Text = Result.B.ToString();

        if (invalidate) Invalidate(true);
    }

    private void UpdateColorFromTrackBars()
    {
        int a = 0xEF, r = RedTrackBar.Value, g = GreenTrackBar.Value, b = BlueTrackBar.Value;
        Result = Color.FromArgb(b + (g << 8) + (r << 16) + (a << 24));
        ResultView.BackColor = Result;
        RedValueBox.Text = Result.R.ToString();
        GreenValueBox.Text = Result.G.ToString();
        BlueValueBox.Text = Result.B.ToString();
        ResultView.Invalidate();
    }

    private void RedTrackBar_Scroll(object? sender, EventArgs e)
    {
        UpdateColorFromTrackBars();
    }

    private void GreenTrackBar_Scroll(object? sender, EventArgs e)
    {
        UpdateColorFromTrackBars();
    }

    private void BlueTrackBar_Scroll(object? sender, EventArgs e)
    {
        UpdateColorFromTrackBars();
    }

    private void CancelButton_Click(object? sender, EventArgs e)
    {
        Result = initialColor;
        UpdateResult(true);

        Close();
    }

    private void OkButton_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void RedValueBox_TextChanged(object? sender, EventArgs e)
    {
        int original = RedTrackBar.Value;
        try
        {
            int value;
            if (string.IsNullOrWhiteSpace(RedValueBox.Text))
            {
                return;
            }
            else
            {
                value = int.Parse(RedValueBox.Text);
                if (value < 0 || value > 255) throw new();
            }

            RedTrackBar.Value = value;
        }
        catch
        {
            RedTrackBar.Value = original;
            RedValueBox.Text = RedTrackBar.Value.ToString();
        }
        UpdateColorFromTrackBars();
    }

    private void GreenValueBox_TextChanged(object? sender, EventArgs e)
    {
        int original = GreenTrackBar.Value;
        try
        {
            int value;
            if (string.IsNullOrWhiteSpace(GreenValueBox.Text))
            {
                value = 0;
            }
            else
            {
                value = int.Parse(GreenValueBox.Text);
                if (value < 0 || value > 255) throw new();
            }

            GreenTrackBar.Value = value;
        }
        catch
        {
            GreenTrackBar.Value = original;
            GreenValueBox.Text = GreenTrackBar.Value.ToString();
        }
        UpdateColorFromTrackBars();
    }

    private void BlueValueBox_TextChanged(object sender, EventArgs e)
    {
        int original = BlueTrackBar.Value;
        try
        {
            int value;
            if (string.IsNullOrWhiteSpace(BlueValueBox.Text))
            {
                value = 0;
            }
            else
            {
                value = int.Parse(BlueValueBox.Text);
                if (value < 0 || value > 255) throw new();
            }

            BlueTrackBar.Value = value;
        }
        catch
        {
            BlueTrackBar.Value = original;
            BlueValueBox.Text = BlueTrackBar.Value.ToString();
        }
        UpdateColorFromTrackBars();
    }
}
