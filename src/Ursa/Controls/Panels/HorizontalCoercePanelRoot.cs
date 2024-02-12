using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Remote.Protocol.Designer;

namespace Ursa.Controls.Panels;

public class HorizontalCoercePanelRoot: Panel
{
    public ColumnDefinitions ColumnDefinitions { get; set; } = new();
    private Dictionary<string, CoercePanelCell> _coercedValues = new();
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

    private bool _isCoercing;
    private void CoercePanel_RequestCoerce(object sender, object? e)
    {
        if (_isCoercing)
        {
            return;
        }
        _isCoercing = true;
        InvalidateMeasure();
        InvalidateArrange();
        _isCoercing = false;
        
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        List<Dictionary<string, double>> values = new();
        foreach (var child in Children)
        {
            if (child is ICoercePanelContainer container)
            {
                var length = container.CoercePanel?.GetLength();
                if (length is not null)
                {
                    values.Add(length);
                }
            }
        }

        var grid = new Grid();
        _coercedValues = CoerceLength(values, availableSize.Width);
        var size = base.MeasureOverride(availableSize);
        return new Size(Children.Select(a => a.Bounds.Width).Max(), Children.Select(a => a.Bounds.Height).Sum());
    }

    private Dictionary<string, CoercePanelCell> CoerceLength(List<Dictionary<string, double>> values, double width)
    {
        var result = new Dictionary<string, CoercePanelCell>();
        var columns = new Dictionary<string, Tuple<int, ColumnDefinition>>();
        var lengths = new Dictionary<string, double>();
        int i = 0;
        foreach (var column in ColumnDefinitions)
        {
            var key = column.SharedSizeGroup;
            if(key is null) continue;
            columns[key] = new Tuple<int, ColumnDefinition>(i, column);
            lengths[key] = 0;
            i++;
        }
        foreach (var value in values)
        {
            foreach (var kv in value)
            {
                if (lengths.ContainsKey(kv.Key))
                {
                    lengths[kv.Key] = Math.Max(lengths[kv.Key], kv.Value);
                }
            }
        }

        var autoKeys = columns
            .Where(a=>a.Value.Item2.Width.IsAuto)
            .Select(a=>a.Key)
            .ToList();
        foreach (var key in autoKeys)
        {
            result[key] = new CoercePanelCell(columns[key].Item1, lengths[key]);
        }
        var autoWidth = lengths.Where(a=>autoKeys.Contains(a.Key)).Sum(a=>a.Value);
        var remainingWidth = width - autoWidth;
        var stars = columns
            .Where(a => a.Value.Item2.Width.IsStar);
        var starKeys = stars.Select(a => a.Key).ToList();
        double? total = stars?.Select(a=>a.Value.Item2.Width.Value).Sum(); 
        foreach (var star in stars)
        {
            result[star.Key] = new CoercePanelCell(columns[star.Key].Item1,
                total * star.Value.Item2.Width.Value / remainingWidth ?? 0);
        }
        return result;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Rect rect = new Rect(new Point(), new Size());
        foreach (var child in Children)
        {
            if (child is ICoercePanelContainer container)
            {
                rect = rect.WithHeight(child.Bounds.Height);
                rect = rect.WithWidth(child.Bounds.Width);
                container.CoercePanel?.SetLength(_coercedValues);
                container.CoercePanel?.Arrange(rect);
                rect = rect.WithY(rect.Y + child.Bounds.Height);
            }
        }
        return finalSize;
    }
}