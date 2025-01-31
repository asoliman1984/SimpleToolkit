﻿using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell;

public partial class SimpleShell
{
    public static readonly BindableProperty TransitionProperty =
        BindableProperty.CreateAttached("Transition", typeof(ISimpleShellTransition), typeof(Page), null);

    public static readonly BindableProperty ShellGroupContainerTemplateProperty =
        BindableProperty.CreateAttached("ShellGroupContainerTemplate", typeof(DataTemplate), typeof(ShellGroupItem), null, propertyChanged: OnShellGroupContainerTemplateChanged);

    public static readonly BindableProperty ShellGroupContainerProperty =
        BindableProperty.CreateAttached("ShellGroupContainer", typeof(IView), typeof(ShellGroupItem), null, propertyChanged: OnShellGroupContainerChanged);

    public static ISimpleShellTransition GetTransition(BindableObject item)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        return (ISimpleShellTransition)item.GetValue(TransitionProperty);
    }

    public static void SetTransition(BindableObject item, ISimpleShellTransition value)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        item.SetValue(TransitionProperty, value);
    }

    public static DataTemplate GetShellGroupContainerTemplate(BindableObject item)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        return (DataTemplate)item.GetValue(ShellGroupContainerTemplateProperty);
    }

    public static void SetShellGroupContainerTemplate(BindableObject item, DataTemplate value)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        item.SetValue(ShellGroupContainerTemplateProperty, value);
    }

    public static IView GetShellGroupContainer(BindableObject item)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        return (IView)item.GetValue(ShellGroupContainerProperty);
    }

    public static void SetShellGroupContainer(BindableObject item, IView value)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        item.SetValue(ShellGroupContainerProperty, value);
    }


    private static void OnShellGroupContainerTemplateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var group = bindable as ShellGroupItem;

        if (group.IsSet(ShellGroupContainerProperty))
        {
            SetShellGroupContainer(group, null);
        }
    }

    private static void OnShellGroupContainerChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var group = bindable as ShellGroupItem;
        var oldView = oldValue as Element;
        var newView = newValue as Element;

        var simpleShell = group.FindParentOfType<SimpleShell>();

        simpleShell?.UpdateLogicalChildren(oldView, newView);
    }
}