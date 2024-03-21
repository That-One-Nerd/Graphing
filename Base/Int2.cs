using System.Drawing;

namespace Graphing;

public record struct Int2
{
    public int x;
    public int y;

    public Int2()
    {
        x = 0;
        y = 0;
    }
    public Int2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Point(Int2 v) => new(v.x, v.y);
}
