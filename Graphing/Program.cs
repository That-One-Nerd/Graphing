using Graphing.Forms;
using Graphing.Graphs;

namespace Graphing;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        Graph2d graph = new("Nice testing graph")
        {
            //Size = (4, 4)
        };
        GraphViewForm form = new(graph);

        Application.Run(form);
    }
}