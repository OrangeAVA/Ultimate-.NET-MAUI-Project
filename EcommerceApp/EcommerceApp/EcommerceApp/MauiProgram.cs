using CommunityToolkit.Maui;
using EcommerceApp.Data;
using Microsoft.Extensions.Logging;
using EcommerceApp.Views;
using EcommerceApp.ViewModels;
using EcommerceApp.Services;

namespace EcommerceApp
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

            Routing.RegisterRoute("ProductPage", typeof(ProductDescriptionView));
            Routing.RegisterRoute("UserPage", typeof(UserView));
            Routing.RegisterRoute("PurchasePage", typeof(PurchaseView));

#if DEBUG
            builder.Logging.AddDebug();
#endif
            //DataBase
            builder.Services.AddSingleton<DatabaseContext>();
            //Pages and ViewModels
            builder.Services.AddTransient<HomeView>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<AccountView>();
            builder.Services.AddTransient<AccountViewModel>();
            builder.Services.AddTransient<ShoppingCartView>();
            builder.Services.AddTransient<ShoppingCartViewModel>();
            builder.Services.AddTransient<MenuView>();
            builder.Services.AddTransient<MenuViewModel>();
            builder.Services.AddTransient<ProductDescriptionView>();
            builder.Services.AddTransient<ProductDescriptionViewModel>();
            builder.Services.AddTransient<UserView>();
            builder.Services.AddTransient<UserViewModel>();
            builder.Services.AddTransient<PurchaseView>();
            builder.Services.AddTransient<PurchaseViewModel>();
            //Services
            builder.Services.AddSingleton<IProductsServices, ProductsService>();

            return builder.Build();
        }
    }
}
