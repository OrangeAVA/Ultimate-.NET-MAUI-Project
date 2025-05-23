
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Data;
using EcommerceApp.Models;

namespace EcommerceApp.ViewModels;

public partial class MenuViewModel : BaseViewModel
{
	private DatabaseContext _databaseContext;

	private Users? _userLogin;

	[ObservableProperty]
	public string? textLogin;
	public MenuViewModel(DatabaseContext databaseContext)
	{
		_databaseContext = databaseContext;
		TextLogin = string.Empty;
	}

	public async void OnAppearing()
	{
		try
		{
            _userLogin = (await _databaseContext.GetAllAsync<Users>())
								.Where(x => x.ActiveSession == true)
								.FirstOrDefault();
            TextLogin = _userLogin != null ? "Log out" : "Login";

        }
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
	}

	[RelayCommand]
	public async Task TapButtonLogin()
	{
		try 
		{
			if (string.IsNullOrWhiteSpace(TextLogin)) 
				return;

			if(TextLogin.Equals("Login"))
			{
                await Shell.Current.GoToAsync("UserPage");
            }
			else if(_userLogin != null)
			{
                _userLogin.ActiveSession = false;
                if (await _databaseContext.UpdateItemAsync(_userLogin))
                {
					TextLogin = TextLogin.Equals("Log out") ? "Login" : "Log out";
                }
            }
        }
		catch(Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
	}

    [RelayCommand]
    public async Task TapButtonAccount()
    {
        await Shell.Current.GoToAsync("UserPage");
    }
}