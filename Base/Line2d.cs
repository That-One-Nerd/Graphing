namespace Graphing;

public record struct Line2d
{
    public Float2 a;
    public Float2 b;

    public Line2d()
    {
        a = new();
        b = new();
    }
    public Line2d(Float2 a, Float2 b)
    {
        this.a = a;
        this.b = b;
    }
}
