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

        Equation equ = new(Math.Sin);
        SlopeField sf = new(2, (x, y) => Math.Cos(x));
        TangentLine tl = new(2, 2, equ);
        graph.Graph(equ, sf, tl);

        // Now, when integrating equations, the result is much less jagged
        // and much faster. Try it out! You can also select points along
        // equations and such as well. Click on an equation to see for
        // yourself!

        Application.Run(graph);
    }
}
