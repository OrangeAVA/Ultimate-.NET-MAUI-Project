using CommunityToolkit.Maui;
using ERP_APP.ViewModels;
using ERP_APP.Data;
using ERP_APP.Views;
using Microsoft.Extensions.Logging;

namespace ERP_APP
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


            Routing.RegisterRoute(nameof(DashBoardView), typeof(DashBoardView));
            Routing.RegisterRoute("SupplersPage", typeof(SuppliersView));
            Routing.RegisterRoute("PurchasePage", typeof(PurchaseView));
            Routing.RegisterRoute("SalePage", typeof(SaleView));
            Routing.RegisterRoute("WarehousePage", typeof(WarehouseView));
            Routing.RegisterRoute("CustomerPage", typeof(CustomerView));
            Routing.RegisterRoute("FormPage", typeof(FormView));

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<LoginView, LoginViewModel>();
            builder.Services.AddSingleton<DashBoardView, DashBoardViewModel>();
            builder.Services.AddSingleton<UserView, UserViewModel>();
            builder.Services.AddSingleton<SuppliersView, SuppliersViewModel>();
            builder.Services.AddSingleton<PurchaseView, PurchaseViewModel>();
            builder.Services.AddSingleton<SaleView, SaleViewModel>();
            builder.Services.AddSingleton<WarehouseView, WarehouseViewModel>();
            builder.Services.AddSingleton<CustomerView, CustomerViewModel>();
            builder.Services.AddSingleton<FormView, FormViewModel>();

            return builder.Build();
        }
    }
}
