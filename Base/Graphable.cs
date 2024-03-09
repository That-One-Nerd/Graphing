using Graphing.Forms;
using Graphing.Parts;

namespace Graphing;

public abstract class Graphable
{
    private static int defaultColorsUsed;
    public static readonly uint[] DefaultColors =
    [
        0xEF_B34D47, // Red
        0xEF_4769B3, // Blue
        0xEF_50B347, // Green
        0xEF_7047B3, // Purple
        0xEF_B38B47, // Orange
        0xEF_5B5B5B  // Black
    ];

    public Color Color { get; set; }
    public string Name { get; set; }

    public Graphable()
    {
        Color = Color.FromArgb((int)DefaultColors[defaultColorsUsed % DefaultColors.Length]);
        defaultColorsUsed++;

        Name = "Unnamed Graphable.";
    }

    public abstract IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph);

    public abstract Graphable DeepCopy();

    public abstract void EraseCache();
    public abstract long GetCacheBytes();
}
