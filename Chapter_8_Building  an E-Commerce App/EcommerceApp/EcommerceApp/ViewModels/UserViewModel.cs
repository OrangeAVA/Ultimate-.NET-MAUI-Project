
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Data;
using EcommerceApp.Models;
using ColorMaui = Microsoft.Maui.Graphics;


namespace EcommerceApp.ViewModels;

public partial class UserViewModel : BaseViewModel
{
	private DatabaseContext _databaseContext;

    private string _idUser;

    [ObservableProperty]
    public string userName;

    [ObservableProperty]
    public string email;

    [ObservableProperty]
    public string pass;

    [ObservableProperty]
    public bool createSession;

    [ObservableProperty]
    public bool isVisibleButtonLogin;

    [ObservableProperty]
    public string message;

    [ObservableProperty]
    public Color messageTextColor;

    [ObservableProperty]
    public string? textButtonSaved;

    public UserViewModel(DatabaseContext databaseContext)
	{
		_databaseContext = databaseContext;
		UserName = string.Empty;
        Email = string.Empty;
        Pass = string.Empty;
        CreateSession = false;
        IsVisibleButtonLogin = true;
        Message = string.Empty;
        _idUser = string.Empty;
        MessageTextColor = new ColorMaui.Color(1, 0, 0); // RED
        TextButtonSaved = "Create Session";
    }

    public async Task OnAppearing()
    {
        try
        {
            var result = (await _databaseContext.GetAllAsync<Users>())
                .Where(x => x.ActiveSession == true).FirstOrDefault();


            if (result != null)
            {
                UserName = result.Name ?? string.Empty;
                Email = result.Email ?? string.Empty;
                Pass = result.Password ?? string.Empty;
                CreateSession = true;
                IsVisibleButtonLogin = false;
                _idUser = result.Id.ToString() ?? string.Empty;
                TextButtonSaved = "Edit";
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }   
    }

    [RelayCommand]
    public async Task TapSaved()
    {
        try
        {
            if (!CreateSession)
            {
                UserName = string.Empty;
                Email = string.Empty;
                Pass = string.Empty;
                CreateSession = true;
                IsVisibleButtonLogin = false;
                Message = string.Empty;
                return;
            }

            var user = string.IsNullOrWhiteSpace(UserName);
            var email = string.IsNullOrWhiteSpace(Email);
            var pass = string.IsNullOrWhiteSpace(Pass);

            if (!user && !email && !pass)
            {
                var newUser = new Users()
                {
                    Id = Guid.NewGuid(),
                    Name = UserName,
                    Email = Email,
                    Password = Pass,
                    ActiveSession = TextButtonSaved == "Edit" ? true : false
                };

                if (string.IsNullOrWhiteSpace(_idUser))
                {
                    var exist = (await _databaseContext.GetAllAsync<Users>())
                            .Any(x => x.Email == Email);
                    if(!exist)
                    {
                        if (await _databaseContext.AddItemAsync(newUser))
                        {
                            Message = "User created successfully";
                            MessageTextColor = new ColorMaui.Color(0, 1, 0); // green
                            IsVisibleButtonLogin = true;
                            return;
                        }
                    }                                          
                }
                else
                {
                    if (TextButtonSaved == "Edit")
                    {
                        newUser.Id = new Guid(_idUser);
                        if (await _databaseContext.UpdateItemAsync(newUser))
                        {
                            Message = "User update successfully";
                            MessageTextColor = new ColorMaui.Color(0, 1, 0); // green
                            return;
                        }
                    }
                }
                Message = "The user was not created because it already exists";
                MessageTextColor = new ColorMaui.Color(1, 0, 0); // red
            }
        }
        catch (Exception ex)
        {
           Console.WriteLine(ex.ToString());
        }
    }

    [RelayCommand]
    public async void TapLogin()
    {
        try
        {
            if (!CreateSession)
            {
                var user = string.IsNullOrWhiteSpace(UserName);
                var pass = string.IsNullOrWhiteSpace(Pass);

                if (!user && !pass)
                {
                    var userLogin = (await _databaseContext.GetAllAsync<Users>())
                                .Where(x => x.Name == UserName && x.Password == Pass)
                                .FirstOrDefault();
                    if (userLogin != null)
                    {
                        userLogin.ActiveSession = true;

                        if (await _databaseContext.UpdateItemAsync(userLogin))
                        {
                            await Shell.Current.GoToAsync("../");
                        }
                        else
                        {
                            Message = "I didn't log in";
                            MessageTextColor = new ColorMaui.Color(1, 0, 0); // Red
                        }
                    }
                    else
                    {
                        Message = "wrong password or username";
                        MessageTextColor = new ColorMaui.Color(1, 0, 0); // Red
                    }
                }
                return;
            }
            CreateSession = false;
            Message = string.Empty;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString()); 
        }
    }
}