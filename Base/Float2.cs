namespace Graphing;

public record struct Float2
{
    public double x;
    public double y;

    public Float2()
    {
        x = 0;
        y = 0;
    }
    public Float2(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator PointF(Float2 v) => new((float)v.x, (float)v.y);
}
