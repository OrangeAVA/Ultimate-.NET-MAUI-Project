using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTalk.Data;
using MauiTalk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTalk.ViewModels;
public partial class ProfileViewModel : BaseViewModel
{
    private DatabaseContext _databaseContext;

    [ObservableProperty]
    private bool isPass;

    [ObservableProperty]
    private string? userName;

    [ObservableProperty]
    private string? phoneNumber;

    [ObservableProperty]
    private string? btnText;

    [ObservableProperty]
    private string? pin;
    
    public ProfileViewModel(DatabaseContext databaseContext) 
    {
        _databaseContext = databaseContext;
        IsPass = true;
        _ = FillForm();
    }

    [RelayCommand]
    public void TapImageEye()
    {
        IsPass = IsPass ? false : true;
    }

    private async Task FillForm()
    {
        var exist = (await _databaseContext.GetAllAsync<UserModel>()).Where(x => x.IsActive == true).FirstOrDefault();
        BtnText = exist == null ? "Create": "Update";
        UserName = exist?.UserName ?? string.Empty;
        PhoneNumber = exist?.PhoneNumber ?? string.Empty;
        Pin = exist?.Pin ?? string.Empty;
    }

    [RelayCommand]
    public async Task EditTapBtn()
    {
        try
        {
            var newUser = new UserModel()
            {
                UserName = UserName ?? string.Empty,
                PhoneNumber = PhoneNumber ?? string.Empty,
                Pin = Pin ?? string.Empty,
                IsActive = true
            };

            var exist = (await _databaseContext.GetAllAsync<UserModel>())
                        .Where(x => x.PhoneNumber == PhoneNumber).FirstOrDefault();
            if (exist == null)
            {
                if (await _databaseContext.AddItemAsync(newUser))
                {

                    Application.Current.MainPage.DisplayAlert(
                        "Account created", 
                        "Account successfully created!", 
                        "OK");
                    return;
                }
            }
            else
            {
                if (exist == null) return; 
                newUser.Id = exist.Id;
                if (await _databaseContext.UpdateItemAsync(newUser))
                {
                    Application.Current.MainPage.DisplayAlert(
                        "Account updated",
                        "The account was successfully updated.", 
                        "OK");

                    return;
                }
            }

            Application.Current.MainPage.DisplayAlert("Warning", "invalid operation", "OK");
        }
        catch(Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Exception", ex.Message, "OK");
        }
    }


}
