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
        SlopeField sf = new(2, (x, y) => 1 / x);
        TangentLine tl = new(2, 2, possibleA);
        graph.Graph(possibleA, sf, tl);

        // You can preload graphs in by going Misc > Preload Cache.
        // Keep in mind this uses more memory than usual and can take
        // some time.

        Application.Run(graph);
    }
}
