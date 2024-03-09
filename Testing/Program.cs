using Graphing.Forms;
using Graphing.Graphables;

namespace Graphing.Testing;

internal static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        GraphForm graph = new("One Of The Graphing Calculators Of All Time");

        Equation equ = new(x =>
        {
            // Demonstrate the caching abilities of the software.
            // This extra waiting is done every time the form requires a
            // calculation done. At the start, it'll be laggy, but as you
            // move around and zoom in, more pieces are cached, and when
            // you reset, the viewport will be a lot less laggy.
            for (int i = 0; i < 1_000_000; i++) ;
            return x * x;
        });
        graph.Graph(equ);

        Application.Run(graph);
    }
}
