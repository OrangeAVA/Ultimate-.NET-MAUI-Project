using Microsoft.Extensions.Logging;
using FirstAppWithSQLiteAndApi.Services;
using FirstAppWithSQLiteAndApi.ViewModels;
using FirstAppWithSQLiteAndApi.Views;

namespace FirstAppWithSQLiteAndApi
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
                });

            var dbPath = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), 
                    "posts.db3");
            builder.Services.AddSingleton<IDatabase>(new Database(dbPath));
            builder.Services.AddSingleton<IApiConnection, ApiConnection>();
            builder.Services.AddSingleton<CommentsViewModel>();
            builder.Services.AddTransient<CommentsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
