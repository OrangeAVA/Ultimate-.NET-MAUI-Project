using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERP_APP.Data;
using ERP_APP.Models;

namespace ERP_APP.ViewModels;

public partial class MainViewModel : BaseViewModel
{

    private DatabaseContext _databaseContext;

    [ObservableProperty]
    private string? userName;

    [ObservableProperty]
    private string? userEmail;

    public MainViewModel(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [RelayCommand]
    public async Task TapButtonSingOut()
    {
        try
        {
            var result = (await _databaseContext.GetAllAsync<Users>())
                  .Where(x => x.ActiveSession == true).FirstOrDefault();
            if (result != null)
            {
                result.ActiveSession = false;
                var update = (await _databaseContext.UpdateItemAsync(result));
                if (update)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }
            }
        }
        catch(Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}