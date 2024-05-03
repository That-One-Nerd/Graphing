using Graphing.Graphables;

namespace Graphing.Abstract;

public interface IConvertEquation
{
    public bool UngraphWhenConvertedToEquation { get; }

    public Equation ToEquation();
}
