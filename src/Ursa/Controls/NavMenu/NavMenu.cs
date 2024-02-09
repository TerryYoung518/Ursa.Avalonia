using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Ursa.Controls.Panels;

namespace Ursa.Controls;

public class NavMenu: ItemsControl
{
    private static readonly ITemplate<Panel?> DefaultPanel = new FuncTemplate<Panel?>(() => new CoercePanel());
    static NavMenu()
    {
        ItemsPanelProperty.OverrideDefaultValue<NavMenu>(DefaultPanel);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<NavMenuItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new NavMenuItem();
    }
}