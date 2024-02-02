using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using Avalonia.VisualTree;

namespace Ursa.Controls;

public class SelectionControl: ListBox
{
    private Control? _control;
    static SelectionControl()
    {
        SelectedItemProperty.Changed.AddClassHandler<SelectionControl, object?>((s, e) => s.OnSelectedItemChanged(e));
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _control = e.NameScope.Get<Control>("Indicator");
        SetUpAnimation();
        if (ElementComposition.GetElementVisual(_control) is { } v)
        {
            v.ImplicitAnimations = _implicitAnimations;
        }
    }
    private ImplicitAnimationCollection? _implicitAnimations;

    private void SetUpAnimation()
    {
        var compositor = ElementComposition.GetElementVisual(this)!.Compositor;
        var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
        offsetAnimation.Target = "Offset";
        offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
        offsetAnimation.Duration = TimeSpan.FromSeconds(0.3);
        var rotationAnimation = compositor.CreateScalarKeyFrameAnimation();
        rotationAnimation.Target = "RotationAngle";
        rotationAnimation.InsertKeyFrame(.5f, 0.160f);
        rotationAnimation.InsertKeyFrame(1f, 0f);
        rotationAnimation.Duration = TimeSpan.FromMilliseconds(400);
        var sizeAnimation = compositor.CreateVector2KeyFrameAnimation();
        sizeAnimation.Target = "Size";
        sizeAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
        sizeAnimation.Duration = TimeSpan.FromSeconds(0.3);
        var animationGroup = compositor.CreateAnimationGroup();
        animationGroup.Add(offsetAnimation);
        animationGroup.Add(sizeAnimation);

        _implicitAnimations = compositor.CreateImplicitAnimationCollection();
        _implicitAnimations["Offset"] = animationGroup;
    }
    
    private void OnSelectedItemChanged(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        var container = ContainerFromItem(args.NewValue.Value);
        if (container is null)
        {
            return;
        }
        _control?.Arrange(container.Bounds);
    }
}