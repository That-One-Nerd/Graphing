using Graphing.Graphs;
using System.ComponentModel;

namespace Graphing.Forms
{
    public partial class GraphViewForm : Form
    {
        private IGraphViewer viewer = null!;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GraphBase? Graph
        {
            get; set
            {
                IGraphViewer newViewer = CreateViewerFromGraph(value)
                    ?? throw new($"Cannot generate a viewer control for {value!.GetType()}!");
                newViewer.Graph = value;

                InitViewer(newViewer, value);
                viewer?.Dispose();
                viewer = newViewer;

                field = value;
            }
        }

        public GraphViewForm() : this(null!) { }
        public GraphViewForm(GraphBase graph)
        {
            SuspendLayout();

            //float scale = DeviceDpi / 96.0f;
            //Size = new((int)(720 * scale), (int)(480 * scale));
            StartPosition = FormStartPosition.WindowsDefaultBounds;

            Graph = graph;

            ResumeLayout(true);
        }

        private IGraphViewer CreateViewerFromGraph(GraphBase? graph)
        {
            if (graph is null) return viewer ?? new Graph2dViewer();

            // TODO: Use reflection to determine which viewer to load.
            return new Graph2dViewer();
        }

        private void InitViewer(IGraphViewer viewer, GraphBase? graph)
        {
            SuspendLayout();
            UserControl control = viewer as UserControl ?? throw new("Improperly configured GraphViewer.");
            control.Dock = DockStyle.Fill;
            viewer.Graph = graph;
            Controls.Add(control);
            ResumeLayout(true);
        }
    }
}
