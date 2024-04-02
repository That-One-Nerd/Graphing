using Graphing.Graphables;

namespace Graphing.Abstract;

public interface IConvertSlopeField
{
    public bool UngraphWhenConvertedToSlopeField { get; }

    public SlopeField ToSlopeField(int detail);
}
