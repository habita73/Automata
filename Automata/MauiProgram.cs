//using Android.Window;
using MauiIcons.Fluent;
using Microsoft.Extensions.Logging;

namespace Automata
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();            
            builder                
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("fa-brands-400.ttf", "FaBrands");
                    fonts.AddFont("fa-regular-400.ttf", "FaRegular");
                    fonts.AddFont("fa-solid-900.ttf", "FaSolid");
                    fonts.AddFont("fa-v4compatibility.ttf", "FaV4compatibility");
                });

            // Initialise the .Net Maui Icons - Fluent
            builder.UseMauiApp<App>().UseFluentMauiIcons();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
