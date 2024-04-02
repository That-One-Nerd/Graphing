using Graphing.Forms;
using Graphing.Graphables;
using System;
using System.Windows.Forms;

namespace Graphing.Testing;

internal static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        
        GraphForm graph = new("One Of The Graphing Calculators Of All Time");

        Equation equA = new(Math.Sin),
                 equB = new(Math.Cos);
        EquationDifference diff = new(2, equA, equB);
        ParametricEquation equC = new(0, 4, t => t * t - 1, t => Math.Sqrt(t) + t + 1);
        graph.Graph(equA, equB, diff, equC);

        Application.Run(graph);
    }
}
