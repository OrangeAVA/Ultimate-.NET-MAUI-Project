using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ERP_APP.Data;
using ERP_APP.Models;
using ERP_APP.Views;

namespace ERP_APP.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private DatabaseContext _databaseContext;

    [ObservableProperty]
	private string? userName;

    [ObservableProperty]
    private string? passUser;

    public LoginViewModel(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task OnAppearing()
    {
        try
        {

            var result = (await _databaseContext.GetAllAsync<Users>())
                .Where(x => x.ActiveSession == true).FirstOrDefault();

            if (result != null)
            {
                App.ViewModel.UserName = result.Name;
                App.ViewModel.UserEmail = result.Email;
                await Shell.Current.GoToAsync("//AccountsPageView");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    [RelayCommand]
	public async Task TapButtonSignIn()
	{
        try
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(PassUser))
            {
                var result = (await _databaseContext.GetAllAsync<Users>())
                   .Where(x => x.Name == UserName && x.Password == PassUser).FirstOrDefault();
                if(result != null)
                {
                    result.ActiveSession = true;
                    var update = (await _databaseContext.UpdateItemAsync(result));
                    if(update)
                    {
                        App.ViewModel.UserName = result.Name;
                        App.ViewModel.UserEmail = result.Email;
                        await Shell.Current.GoToAsync("//AccountsPageView");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        Application.Current.MainPage.DisplayAlert("Invalid user", "Incorrect username or password", "OK");
    }

    [RelayCommand]
    public async Task TapButtonCreateUser()
    {
        await Shell.Current.GoToAsync("//CreateUserPage");

    }
}