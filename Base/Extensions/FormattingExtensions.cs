namespace Graphing.Extensions;

public static class FormattingExtensions
{
    private static readonly string[] sizeUnits =
    [
        " bytes",
        " KB",
        " MB",
        " GB",
        " TB",
        " PB",
    ];

    public static string FormatAsBytes(this long bytes)
    {
        double val = bytes;
        int unitIndex = 0;

        while (val > 1024)
        {
            unitIndex++;
            val /= 1024;
        }

        string result;
        if (unitIndex == 0) result = val.ToString("0");
        else result = val.ToString("0.00");

        return result + sizeUnits[unitIndex];
    }
}
