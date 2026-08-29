using System.ComponentModel;

namespace Graphing.Forms;

internal interface IGraphViewer : IDisposable
{
    Graph? Graph { get; set; }
}
public abstract class GraphViewerBase(Graph graph) : UserControl, IGraphViewer
{
    public static readonly Color BackgroundColor = Color.White;
    public static readonly Color MainAxisColor = Color.Black;
    public static readonly Color SemiAxisColor = Color.FromArgb(unchecked((int)0xFF_999999));    // Grayish
    public static readonly Color QuarterAxisColor = Color.FromArgb(unchecked((int)0xFF_E0E0E0)); // Lighter grayish
    public static readonly Color UnitsTextColor = Color.Black;
    public static readonly Color ZoomBoxColor = Color.Black;


    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Graph? Graph
    {
        get; set
        {
            field = value;
            UpdateActiveGraph();
        }
    } = graph;

    public GraphViewerBase() : this(null!) { }

    protected virtual void UpdateActiveGraph()
    {
        Name = Graph?.Name ?? "Empty Graph";
        Invalidate();
    }
}
