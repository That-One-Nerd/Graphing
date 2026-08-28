using Graphing.Graphs;
using System.ComponentModel;

namespace Graphing.Forms;

internal interface IGraphViewer : IDisposable
{
    GraphBase? Graph { get; set; }
    Type GraphType { get; }
}
public abstract class GraphViewerBase<TGraph> : UserControl, IGraphViewer
    where TGraph : GraphBase
{
    public static readonly Color BackgroundColor = Color.White;
    public static readonly Color MainAxisColor = Color.Black;
    public static readonly Color SemiAxisColor = Color.FromArgb(unchecked((int)0xFF_999999));    // Grayish
    public static readonly Color QuarterAxisColor = Color.FromArgb(unchecked((int)0xFF_E0E0E0)); // Lighter grayish
    public static readonly Color UnitsTextColor = Color.Black;
    public static readonly Color ZoomBoxColor = Color.Black;


    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TGraph? Graph
    {
        get; set
        {
            field = value;
            UpdateActiveGraph();
        }
    }
    GraphBase? IGraphViewer.Graph { get => Graph; set => Graph = (TGraph)value!; }
    Type IGraphViewer.GraphType => typeof(TGraph);

    public GraphViewerBase() => Graph = default;
    public GraphViewerBase(TGraph graph) => Graph = graph;

    protected virtual void UpdateActiveGraph()
    {
        Name = Graph?.Name ?? "Empty Graph";
        Invalidate();
    }
}
