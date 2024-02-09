using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Ursa.Controls.Panels;

namespace Ursa.Controls;

[TemplatePart(PART_CoercePanel, typeof(CoercePanel))]
public class NavMenuItem: HeaderedItemsControl, ICoercePanelContainer
{
    public const string PART_CoercePanel = "PART_CoercePanel";
    public CoercePanel? CoercePanel { get; private set; }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        CoercePanel = e.NameScope.Get<CoercePanel>(PART_CoercePanel);
    }
}