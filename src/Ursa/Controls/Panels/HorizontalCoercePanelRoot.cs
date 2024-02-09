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
                if (container.CoercePanel is not null)
                {
                    container.CoercePanel.RequestCoerce += CoercePanel_RequestCoerce;
                }
            }
        }
    }

    private void CoercePanel_RequestCoerce(object sender, object? e)
    {
        InvalidateMeasure();
        InvalidateArrange();
        
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        _maps = new Dictionary<string, GridLength>();
        foreach (var child in Children)
        {
            if (child is ICoercePanelContainer container)
            {
                var length = container.CoercePanel?.GetLength();
            }
        }
        return new Size(100, 120);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Rect rect = new Rect(new Point(), new Size());
        foreach (var child in Children)
        {
            if (child is ICoercePanelContainer container)
            {
                rect = rect.WithHeight(40);
                rect = rect.WithWidth(100);
                container.CoercePanel?.SetLength(new Dictionary<string, double>());
                child.Arrange(rect);
                rect = rect.WithY(rect.Y + 40);
            }
        }
        return finalSize;
    }
}