using Graphing.Forms;
using System.Drawing;

namespace Graphing.Parts;

public record struct GraphUiText : IGraphPart
{
    public string text;
    public Float2 position;
    public bool background;

    public ContentAlignment alignment;
    public Int2 offsetPix;

    private readonly Font font;
    private readonly Brush? backgroundBrush;

    public GraphUiText(string text, Float2 position, ContentAlignment alignment,
                       bool background = true, Int2? offsetPix = null)
    {
        font = new Font("Segoe UI", 8, FontStyle.Bold);

        this.text = text;
        this.position = position;
        this.background = background;
        this.alignment = alignment;
        this.offsetPix = offsetPix ?? new();

        if (background) backgroundBrush = new SolidBrush(GraphForm.BackgroundColor);
    }

    public readonly void Render(in GraphForm form, in Graphics g, in Pen p)
    {
        Int2 posScreen = form.GraphSpaceToScreenSpace(position);
        SizeF size = g.MeasureString(text, font);

        // Adjust X position based on alignment.
        switch (alignment)
        {
            case ContentAlignment.TopLeft or
                 ContentAlignment.MiddleLeft or
                 ContentAlignment.BottomLeft: break; // Nothing to offset.

            case ContentAlignment.TopCenter or
                 ContentAlignment.MiddleCenter or
                 ContentAlignment.BottomCenter:
                posScreen.x -= (int)(size.Width / 2);
                break;

            case ContentAlignment.TopRight or
                 ContentAlignment.MiddleRight or
                 ContentAlignment.BottomRight:
                posScreen.x -= (int)size.Width;
                break;
        }

        // Adjust Y position based on alignment.
        switch (alignment)
        {
            case ContentAlignment.TopLeft or
                 ContentAlignment.TopCenter or
                 ContentAlignment.TopRight: break; // Nothing to offset.

            case ContentAlignment.MiddleLeft or
                 ContentAlignment.MiddleCenter or
                 ContentAlignment.MiddleRight:
                posScreen.y -= (int)(size.Height / 2);
                break;

            case ContentAlignment.BottomLeft or
                 ContentAlignment.BottomCenter or
                 ContentAlignment.BottomRight:
                posScreen.y -= (int)size.Height; 
                break;
        }

        posScreen.x += (int)(offsetPix.x * form.DpiFloat / 192);
        posScreen.y += (int)(offsetPix.y * form.DpiFloat / 192);

        if (background)
        {
            g.FillRectangle(backgroundBrush!, new Rectangle(posScreen.x, posScreen.y,
                                                            (int)size.Width, (int)size.Height));
        }
        g.DrawString(text, font, p.Brush, new Point(posScreen.x, posScreen.y));
    }
}
