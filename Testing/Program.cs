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
        ParametricEquation equC = new(0, 20, t => 0.0375 * t * Math.Cos(t), t => 0.0625 * t * Math.Sin(t) + 3);
        graph.Graph(equA, equB, diff, equC, equA.ToColumnTable(-1, 1, 2));

        Application.Run(graph);
    }
}
