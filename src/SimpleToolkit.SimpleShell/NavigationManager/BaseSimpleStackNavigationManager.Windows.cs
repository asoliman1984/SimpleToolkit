﻿#if WINDOWS

using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
using NavFrame = Microsoft.UI.Xaml.Controls.Grid;
using Microsoft.UI.Xaml;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected virtual void AddPlatformPageToContainer(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
    {
        var newPageView = GetPlatformView(newPage);

        if (newPageView is null)
            return;

        if (isCurrentPageRoot)
            AddPlatformRootPage(onTop, newPageView);
        else
            AddToContainer(newPageView, navigationFrame, onTop);
    }

    protected virtual void RemovePlatformPageFromContainer(IView oldPage, IView oldShellItemContainer, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
    {
        var oldPageView = GetPlatformView(oldPage);

        if (oldPageView is null)
            return;

        if (oldPageView?.Parent is NavFrame parent)
        {
            parent.Children.Remove(oldPageView);
        }
        else
        {
            if (GetPageContainerNavHost(oldShellSectionContainer) is NavFrame sectionNavHost)
                sectionNavHost.Children.Remove(oldPageView);
            if (GetPageContainerNavHost(oldShellItemContainer) is NavFrame itemNavHost)
                itemNavHost.Children.Remove(oldPageView);
            if (GetPageContainerNavHost(this.rootPageContainer) is NavFrame rootNavHost)
                rootNavHost.Children.Remove(oldPageView);
            navigationFrame.Children.Remove(oldPageView);
        }

        if (oldShellSectionContainer is not null && currentShellSectionContainer != oldShellSectionContainer)
            RemoveShellGroupContainer(oldShellSectionContainer);

        if (oldShellItemContainer is not null && currentShellItemContainer != oldShellItemContainer)
            RemoveShellGroupContainer(oldShellItemContainer);

        if (!isCurrentPageRoot && isPreviousPageRoot && this.rootPageContainer is not null)
            RemoveRootPageContainer(this.rootPageContainer);
    }

    protected private void AddPlatformRootPage(bool onTop, PlatformView newPageView)
    {
        var r = AddToContainer(this.rootPageContainer, navigationFrame, onTop);
        var i = AddToContainer(currentShellItemContainer, r, onTop);
        var s = AddToContainer(currentShellSectionContainer, i, onTop);
        AddToContainer(newPageView, s, onTop);
    }

    private NavFrame AddToContainer(IView childContainer, NavFrame parentNavHost, bool onTop)
    {
        _ = parentNavHost ?? throw new ArgumentNullException(nameof(parentNavHost), $"{nameof(SimpleNavigationHost)} is missing");

        if (childContainer is null)
            return parentNavHost;

        var platformChildContainer = GetPlatformView(childContainer);

        AddToContainer(platformChildContainer, parentNavHost, onTop);

        if (GetPageContainerNavHost(childContainer) is NavFrame navHost)
            return navHost;

        return null;
    }

    private void AddToContainer(PlatformView child, NavFrame parentNavHost, bool onTop)
    {
        _ = parentNavHost ?? throw new ArgumentNullException(nameof(parentNavHost), $"{nameof(SimpleNavigationHost)} is missing");

        if (parentNavHost.Children.Contains(child))
            parentNavHost.Children.Remove(child);

        if (onTop)
            parentNavHost.Children.Add(child);
        else
            parentNavHost.Children.Insert(0, child);
    }

    protected virtual void ReplaceRootPageContainer(IView rootPageContainer, bool isCurrentPageRoot)
    {
        var oldContainer = GetPlatformView(this.rootPageContainer);
        var newContainer = GetPlatformView(rootPageContainer);
        IList<UIElement> oldChildren = new List<UIElement>();

        if (oldContainer is not null)
            oldChildren = RemoveRootPageContainer(this.rootPageContainer);

        // Old container is being replaced or added
        if (newContainer is not null && isCurrentPageRoot)
        {
            // New container is being added
            if (oldContainer is null && navigationFrame.Children.Any())
            {
                foreach (var child in navigationFrame.Children)
                    oldChildren.Add(child);
                navigationFrame.Children.Clear();
            }

            navigationFrame.Children.Add(newContainer);

            if (GetPageContainerNavHost(rootPageContainer) is NavFrame newNavHost)
            {
                foreach (var child in oldChildren)
                    newNavHost.Children.Add(child);
            }
        }

        // Old container is being removed
        if (oldContainer is not null && newContainer is null && isCurrentPageRoot)
        {
            foreach (var child in oldChildren)
                navigationFrame.Children.Add(child);
        }
    }

    protected private IList<UIElement> RemoveRootPageContainer(IView oldRootContainer)
    {
        var oldContainer = GetPlatformView(oldRootContainer);
        var oldChildren = new List<UIElement>();

        if (GetPageContainerNavHost(oldRootContainer) is NavFrame oldNavHost)
        {
            oldChildren = oldNavHost.Children.ToList();
            oldNavHost.Children.Clear();
        }

        if (navigationFrame.Children.Contains(oldContainer))
            navigationFrame.Children.Remove(oldContainer);

        return oldChildren;
    }

    protected private void RemoveShellGroupContainer(IView oldShellGroupContainer)
    {
        var oldContainer = GetPlatformView(oldShellGroupContainer);

        if (GetPageContainerNavHost(oldShellGroupContainer) is NavFrame oldNavHost)
            oldNavHost.Children.Clear();

        if (oldContainer.Parent is NavFrame parent)
            parent.Children.Remove(oldContainer);
    }
}

#endif