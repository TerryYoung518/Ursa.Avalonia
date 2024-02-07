using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Ursa.Controls.Panels;

public class HorizontalCoercePanelRoot: Panel
{
    public Dictionary<string, GridLength> _maps;

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        foreach (var child in Children)
        {
            if (child is ICoercePanelContainer container)
            {
                container.CoercePanel.RequestCoerce += CoercePanel_RequestCoerce;
            }
        }
    }

    private void CoercePanel_RequestCoerce(object sender, object? e)
    {
        
        
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        _maps = new Dictionary<string, GridLength>();
        foreach (var child in Children)
        {
            if (child is ICoercePanelContainer container)
            {
                var length = container.CoercePanel.GetLength();
            }
        }
        return base.MeasureOverride(availableSize);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        foreach (var child in Children)
        {
            if (child is ICoercePanelContainer container)
            {
                container.CoercePanel.SetLength(new Dictionary<string, double>());
            }
        }
        return base.ArrangeOverride(finalSize);
    }
}