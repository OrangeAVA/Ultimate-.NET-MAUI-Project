using EducationalApp.ViewModels;
using EducationalApp.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using EducationalApp.Services;
using EducationalApp.Data;

namespace EducationalApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkitMediaElement()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        Routing.RegisterRoute("CourseDescriptionView", typeof(CourseDescriptionView));
        Routing.RegisterRoute("CourseContentView", typeof(CourseContentView));

        builder.Services.AddSingleton<DatabaseContext>();

        builder.Services.AddTransient<LoginView>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<UserView>();
        builder.Services.AddTransient<UserViewModel>();
        builder.Services.AddTransient<MyCoursesView>();
        builder.Services.AddTransient<MyCoursesViewModel>();
        builder.Services.AddTransient<SearchView>();
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<WishListView>();
        builder.Services.AddTransient<WishListViewModel>();
        builder.Services.AddTransient<AccountView>();
        builder.Services.AddTransient<AccountViewModel>();
        builder.Services.AddTransient<CourseDescriptionView>();
        builder.Services.AddTransient<CourseDescriptionViewModel>();
        builder.Services.AddTransient<CourseContentView>();
        builder.Services.AddTransient<CourseContentViewModel>();

        builder.Services.AddSingleton<ICourseService, CourseService>();

        return builder.Build();
	}
}
