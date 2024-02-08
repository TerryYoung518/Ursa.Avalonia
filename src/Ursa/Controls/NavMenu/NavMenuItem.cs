using Avalonia.Controls.Primitives;
using Ursa.Controls.Panels;

namespace Ursa.Controls.NavMenu;

public class NavMenuItem: HeaderedItemsControl, ICoercePanelContainer
{
    public CoercePanel? CoercePanel => null;
}