namespace Ursa.Controls.Panels;

internal struct CoercePanelCell
{
    public int Column;
    public double Width;

    public CoercePanelCell( int column, double width)
    {
        Column = column;
        Width = width;
    }
}