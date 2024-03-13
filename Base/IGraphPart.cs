using Graphing.Forms;

namespace Graphing;

public interface IGraphPart
{
    public void Render(in GraphForm form, in Graphics g, in Brush brush);
}
