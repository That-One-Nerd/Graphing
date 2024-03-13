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

        Equation equ1 = new(x =>
        {
            // Demonstrate the caching abilities of the software.
            // This extra waiting is done every time the form requires a
            // calculation done. At the start, it'll be laggy, but as you
            // move around and zoom in, more pieces are cached, and when
            // you reset, the viewport will be a lot less laggy.

            // Remove this loop to make the equation fast again. I didn't
            // slow the engine down much more with this improvement, so any
            // speed decrease you might notice is likely this function.
            for (int i = 0; i < 1_000_000; i++) ;
            return -x * x + 2;
        });
        Equation equ2 = new(x => x);
        Equation equ3 = new(x => -Math.Sqrt(x));
        SlopeField sf = new(2, (x, y) => (x * x - y * y) / x);
        graph.Graph(equ1, equ2, equ3, sf);

        // You can also now view and reset caches in the UI by going to
        // Misc > View Caches.

        Application.Run(graph);
    }
}
