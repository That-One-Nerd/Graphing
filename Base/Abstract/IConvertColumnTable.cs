using Graphing.Graphables;

namespace Graphing.Abstract;

public interface IConvertColumnTable
{
    public bool UngraphWhenConvertedToColumnTable { get; }

    public ColumnTable ToColumnTable(double start, double end, int detail);
}
