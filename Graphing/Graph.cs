namespace Graphing;

public class Graph(string name)
{
    public string Name { get; } = name;
    public IReadOnlyList<string> Axes { get; init; } = ["x", "y"];

    public GraphPoint Center { get; set; } = [];
    public GraphPoint? Size { get; set; } = null;

    public GraphPoint? Min
    {
        get
        {
            if (!Size.HasValue) return null;
            else return -Size.Value / 2 + Center;
        }
    }
    public GraphPoint? Max
    {
        get
        {
            if (!Size.HasValue) return null;
            else return Size.Value / 2 + Center;
        }
    }
}
