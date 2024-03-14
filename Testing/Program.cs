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

        Equation equ1 = new(x => -x * x + 2);
        Equation equ2 = new(x => x);
        Equation equ3 = new(x => -Math.Sqrt(x));
        graph.Graph(equ1, equ2, equ3);

        // You can also now view and reset caches in the UI by going to
        // Misc > View Caches.

        Application.Run(graph);
    }
}
