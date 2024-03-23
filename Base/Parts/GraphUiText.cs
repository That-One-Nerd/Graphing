using Graphing.Forms;
using System.Drawing;

namespace Graphing.Parts;

public record struct GraphUiText : IGraphPart
{
    public string text;
    public Float2 position;
    public bool background;

    private readonly Font font;
    private readonly Brush? backgroundBrush;

    public GraphUiText(string text, Float2 position, bool background = true)
    {
        font = new Font("Segoe UI", 8, FontStyle.Bold);

        this.text = text;
        this.position = position;
        this.background = background;

        if (background) backgroundBrush = new SolidBrush(GraphForm.BackgroundColor);
    }

    public readonly void Render(in GraphForm form, in Graphics g, in Pen p)
    {
        Int2 posScreen = form.GraphSpaceToScreenSpace(position);
        posScreen.y -= (int)(form.DpiFloat * font.Size * 2 / 192);
        if (background)
        {
            SizeF size = g.MeasureString(text, font);
            g.FillRectangle(backgroundBrush!, new Rectangle(posScreen.x, posScreen.y,
                                                            (int)size.Width, (int)size.Height));
        }
        g.DrawString(text, font, p.Brush, new Point(posScreen.x, posScreen.y));
    }
}
