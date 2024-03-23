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
        graph.Graph(equA, equB, diff);

        Application.Run(graph);
    }
}
