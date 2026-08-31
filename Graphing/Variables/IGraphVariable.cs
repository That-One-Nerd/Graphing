using Graphing.Graphs;

namespace Graphing.Variables;

public interface IGraphVariable
{
    IGraphVariable Evaluate(VariableContext context);
    TObject Evaluate<TObject>(VariableContext context) where TObject : IGraphVariable => (TObject)Evaluate(context);
}
