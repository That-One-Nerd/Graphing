using Graphing.Abstract;
using System.Windows.Forms;

namespace Graphing.Forms;

public partial class TranslateForm : Form
{
    private readonly GraphForm refForm;

    // These variables both represent the same graphable.
    private readonly Graphable ableRaw;
    private readonly ITranslatable ableTrans;

    public TranslateForm(GraphForm graph, Graphable ableRaw, ITranslatable ableTrans)
    {
        refForm = graph;
        this.ableRaw = ableRaw;
        this.ableTrans = ableTrans;

        if (ableTrans is ITranslatableX transX) transX.OffsetX = 1;
        if (ableTrans is ITranslatableY transY) transY.OffsetY = 1;

        graph.Invalidate(false);
    }
}
