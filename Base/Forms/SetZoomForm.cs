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

    internal void CompleteBoxSelection()
    {
        if (boxSelectEnabled) EnableBoxSelect_Click(null, new());
    }
}
