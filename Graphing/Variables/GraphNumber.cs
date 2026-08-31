namespace Graphing.Variables;

public class GraphNumber(double value) : IGraphVariable
{
    public double Value { get; set; } = value;

    IGraphVariable IGraphVariable.Evaluate(VariableContext context) => this;

    public static implicit operator double(GraphNumber num) => num.Value;
    public static implicit operator GraphNumber(double num) => new(num);
}
