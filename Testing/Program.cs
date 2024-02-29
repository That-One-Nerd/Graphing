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

        Equation equ = new(x => x * x);
        TangentLine tangent = new(5, 2, equ);

        graph.Graph(equ);
        graph.Graph(tangent);

        Application.Run(graph);
    }

    private static double PopulationGraph(double max, double k, double A, double t)
    {
        return max / (1 + A * Math.Exp(-k * t));
    }
}
