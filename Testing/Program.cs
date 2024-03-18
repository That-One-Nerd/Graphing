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

        Equation possibleA = new(x => x * x * x);
        SlopeField sf = new(2, (x, y) => 3 * x * x);
        TangentLine tl = new(5, 2, possibleA);
        graph.Graph(possibleA, sf, tl);

        // You can also now view and reset caches in the UI by going to
        // Misc > View Caches.

        Application.Run(graph);
    }
}
