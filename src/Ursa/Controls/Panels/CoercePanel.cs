using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls.Panels;

public class CoercePanel: Panel
{

    public static readonly AttachedProperty<string> CoerceGroupProperty =
        AvaloniaProperty.RegisterAttached<CoercePanel, Control, string>("CoerceGroup");

    public static void SetCoerceGroup(Control obj, string value) => obj.SetValue(CoerceGroupProperty, value);
    public static string GetCoerceGroup(Control obj) => obj.GetValue(CoerceGroupProperty);
    
    private Dictionary<string, double> _coercedValues = new Dictionary<string, double>();
    private Dictionary<string, double> _actualValues = new Dictionary<string, double>();

    private bool _inCoercing;
    
    public event EventHandler<object?>? RequestCoerce;
    
    public void SetLength(Dictionary<string, double> values)
    {
        _coercedValues = values;
        _inCoercing = true;
        InvalidateArrange();
        _inCoercing = false;
    }
    
    public Dictionary<string, double> GetLength()
    {
        return _actualValues;
    }

    protected override void OnMeasureInvalidated()
    {
        if (_inCoercing)
        {
            return;
        }
        base.OnMeasureInvalidated();

        foreach (var child in Children)
        {
            if(GetCoerceGroup(child) is { } group)
            {
                _actualValues[group] = child.Bounds.Width;
            }
        }
        RequestCoerce?.Invoke(this, null);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var size = base.ArrangeOverride(finalSize);
        foreach (var child in Children)
        {
            var key = GetCoerceGroup(child);
            if (key is null)
            {
                child.Arrange(new Rect(new Point(), child.DesiredSize));
            }
            else if (_coercedValues.TryGetValue(GetCoerceGroup(child), out double width))
            {
                child.Arrange(child.Bounds.WithWidth(width));
            }
        }
        return size;
    }
}