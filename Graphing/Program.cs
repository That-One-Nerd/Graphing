using Graphing.Forms;

namespace Graphing;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        Graph graph = new("Nice testing graph")
        {
            //Size = (4, 4)
        };
        GraphViewForm form = new(graph);

        Application.Run(form);
    }
}