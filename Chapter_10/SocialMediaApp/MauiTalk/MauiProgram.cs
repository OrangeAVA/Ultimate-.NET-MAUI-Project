using CommunityToolkit.Maui;
using MauiTalk.Data;
using MauiTalk.ViewModels;
using MauiTalk.Views;
using Microsoft.Extensions.Logging;

namespace MauiTalk
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<ContactsViewModel>();
            builder.Services.AddTransient<ContactsPage>();
            builder.Services.AddSingleton<MessagesViewModel>();
            builder.Services.AddTransient<MessagesPage>();
            builder.Services.AddSingleton<SignalRService>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<ChatViewModel>();
            builder.Services.AddTransient<ChatPage>();
            builder.Services.AddTransient<FriendsPage>();
            builder.Services.AddSingleton<FriendsViewModel>();


            Routing.RegisterRoute("MessagesPage", typeof(MessagesPage));
            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));

            return builder.Build();
        }
    }
}
