using Graphing.Forms;
using System.Drawing;

namespace Graphing;

public interface IGraphPart
{
    public void Render(in GraphForm form, in Graphics g, in Pen pen);
}
