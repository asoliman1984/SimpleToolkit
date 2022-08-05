﻿#if __IOS__ || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Radek.SimpleShell.Handlers
{ 
    public partial class SimpleNavigationHostHandler : ViewHandler<ISimpleNavigationHost, UIView>
    {
        public static IPropertyMapper<ISimpleNavigationHost, SimpleNavigationHostHandler> Mapper = new PropertyMapper<ISimpleNavigationHost, SimpleNavigationHostHandler>(ViewHandler.ViewMapper)
        {
        };

        public static CommandMapper<ISimpleNavigationHost, SimpleNavigationHostHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
        {
        };

        public SimpleNavigationHostHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }

        public SimpleNavigationHostHandler()
            : base(Mapper, CommandMapper)
        {
        }

        public UIView Container { get; protected set; }

        protected override UIView CreatePlatformView()
        {
            // TODO: Use Microsoft.Maui.Platform.ContentView if just UIView won't work: ViewHandler<ISimpleNavigationHost, Microsoft.Maui.Platform.ContentView>
            var container = new UIView();
            return container;
        }

        public virtual void SetContent(UIView view)
        {
            view.Frame = PlatformView.Bounds;
            Container.ClearSubviews();
            Container.AddSubview(view);
        }
    }
}

#endif