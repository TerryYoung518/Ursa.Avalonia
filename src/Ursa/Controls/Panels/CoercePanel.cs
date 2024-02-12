using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls.Panels;

public class CoercePanel: Panel
{
    #region Attached Property
    public static readonly AttachedProperty<string?> CoerceGroupProperty =
        AvaloniaProperty.RegisterAttached<CoercePanel, Control, string?>("CoerceGroup");

    public static void SetCoerceGroup(Control obj, string? value) => obj.SetValue(CoerceGroupProperty, value);
    public static string? GetCoerceGroup(Control obj) => obj.GetValue(CoerceGroupProperty);
    #endregion
    
    
    
    
    private Dictionary<string, CoercePanelCell> _coercedValues = new Dictionary<string, CoercePanelCell>();
    private Dictionary<string, double> _actualValues = new Dictionary<string, double>();

    private bool _inCoercing;
    
    public event EventHandler<object?>? RequestCoerce;
    
    internal void SetLength(Dictionary<string, CoercePanelCell> values)
    {
        _coercedValues = values;
        _inCoercing = true;
        InvalidateMeasure();
        InvalidateArrange();
        _inCoercing = false;
    }
    
    internal Dictionary<string, double> GetLength()
    {
        return _actualValues;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var grid = new Grid();
        var size= base.MeasureOverride(availableSize);
        foreach (var child in Children)
        {
            if(GetCoerceGroup(child) is { } group)
            {
                _actualValues[group] = child.Bounds.Width;
            }
        }
        RequestCoerce?.Invoke(this, null);
        return size;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var size = base.ArrangeOverride(finalSize);
        double x = 0;
        var grid = new Grid();
        foreach (var child in Children)
        {
            var key = GetCoerceGroup(child);
            if (key is null)
            {
                child.Arrange(new Rect(new Point(), child.DesiredSize));
            }
            else if (_coercedValues.TryGetValue(key, out CoercePanelCell cell))
            {
                child.Arrange(child.Bounds.WithX(x).WithWidth(80));
                x += 40;
            }
        }
        return size;
    }
}