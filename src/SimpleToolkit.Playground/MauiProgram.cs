﻿using SimpleToolkit.Core;

namespace SimpleToolkit.SimpleShell.Playground
{
    public static class MauiProgram
    {
        public const bool UseSimpleShell = true;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.UseSimpleToolkit();

            if (UseSimpleShell)
            {
                builder.UseSimpleShell();
            }

            return builder.Build();
        }
    }
}