namespace Graphing;

public record struct Range2d
{
    public double minX;
    public double minY;
    public double maxX;
    public double maxY;

    public Range2d()
    {
        minX = 0;
        minY = 0;
        maxX = 0;
        maxY = 0;
    }
    public Range2d(double minX, double minY, double maxX, double maxY)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
    }

    public readonly bool Contains(Float2 p) =>
        p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY;
}
