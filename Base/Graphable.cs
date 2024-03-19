using Graphing.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace Graphing;

public abstract class Graphable
{
    private static int defaultColorsUsed;
    public static readonly uint[] DefaultColors =
    [
        0xFF_B34D47, // Red
        0xFF_4769B3, // Blue
        0xFF_50B347, // Green
        0xFF_7047B3, // Purple
        0xFF_B38B47, // Orange
        0xFF_5B5B5B  // Black
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

    public virtual void EraseCache() { }
    public virtual long GetCacheBytes() => 0;
    public virtual void Preload(Float2 xRange, Float2 yRange, double step) { }

    public virtual bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor) => false;
    public virtual Float2 GetSelectedPoint(in GraphForm graph, Float2 graphMousePos) => default;
}
