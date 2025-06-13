using CommunityToolkit.Mvvm.ComponentModel;
using MauiTalk.Models;
using MauiTalk.Data;
using MauiTalk.Controls;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;

namespace MauiTalk.ViewModels;
public partial class ChatViewModel : BaseViewModel
{
    private UserModel _user;

    private DatabaseContext _databaseContext;

    [ObservableProperty]
    public ObservableCollection<ContactData?> chatsList;

    [ObservableProperty]
    private bool isVisible;

    public ChatViewModel(DatabaseContext databaseContext)
    {
       _databaseContext = databaseContext;
        ChatsList = new ObservableCollection<ContactData?>();
        IsVisible = false;
        _ = OpenPinPopup();
        _ = LoadContacts();        
    }

    [RelayCommand]
    public async Task TapContact(ContactData? contact)
    {
        if (contact == null)
            return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "Open", contact }
        };

        await Shell.Current.GoToAsync("MessagesPage", navigationParameter);
    }


    private async Task OpenPinPopup()
    {
        try
        {
            var user = (await _databaseContext.GetAllAsync<UserModel>())
                .Where(x => x.IsActive == true).FirstOrDefault();
            if (user != null)
            {
                var pinPopup = new PinPopup(user.Pin);
                await App.Current.MainPage.ShowPopupAsync(pinPopup);
            }
            else
            {
                await Shell.Current.GoToAsync("ProfilePage");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task LoadContacts()
    {
        IsVisible = true;
        try
        {
            ChatsList.Clear();
            _user = (await _databaseContext.GetAllAsync<UserModel>())
                .Where(x => x.IsActive == true).FirstOrDefault();

            if (_user?.UserName != null)
            {
                var contacts = (await _databaseContext.GetAllAsync<ContactsAppModel>())
               .Where(x => x.FkUser == _user.Id);

                foreach(var contact in contacts)
                {
                    if (contact != null)
                    {
                        var numView = (await _databaseContext.GetAllAsync<MessagesModel>())
                                .Where(x => x.FkContactApp == contact.Id && x.Viewed == false).Count();

                        ChatsList.Add(new ContactData()
                        {
                            Id = contact.Id,
                            DisplayName = contact.Name,
                            PhoneNumer = contact.PhoneNumber,
                            numView = numView == 0 ? string.Empty : numView.ToString(),
                        });
                    }
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert(
                    "Warning", "I couldn't find any saved contacts", "OK");
            }
           
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert(
                    "Error", ex.Message, "OK");
        }
        finally
        {
            IsVisible = false;
        }
    }

}
