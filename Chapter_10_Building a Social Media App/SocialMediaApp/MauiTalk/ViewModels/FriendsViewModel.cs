using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTalk.Controls;
using MauiTalk.Models;
using MauiTalk.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace MauiTalk.ViewModels;

public partial class FriendsViewModel : BaseViewModel
{
    private SignalRService _signalRService;

    private DatabaseContext _databaseContext;

    [ObservableProperty]
    public ObservableCollection<NewsAppModel> newsPosts;

    public FriendsViewModel(SignalRService signalRService, DatabaseContext databaseContext)
    {
        _signalRService = signalRService;
        _databaseContext = databaseContext;

        NewsPosts = new ObservableCollection<NewsAppModel>();

        _signalRService.OnMessageReceived += (user, message) =>
        {
            if (!user.Contains("Post:")) return;

            var newMsg = new MessageModel { Text = $"{user}: {message}", IsMine = false };
            string json = JsonSerializer.Serialize(newMsg);
            try
            {
                var newPost = new NewsAppModel()
                {
                    UserName = user.Substring("Post:".Length),
                    Post = message,
                    Date = DateTime.Now,
                };
                NewsPosts.Add(newPost);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message: {ex.Message}");
            }
        };
    }

    [RelayCommand]
    public async Task BtnAddTap()
    {
        var newPost = string.Empty;
        var popup = new NewPostPopup();
        popup.MsgSaved += (msg) =>
        {
            newPost += msg;
        };
        await App.Current.MainPage.ShowPopupAsync(popup);
        await SendMessage(newPost);
    }

    private async Task SendMessage(string newPost)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(newPost))
            {
                if (_signalRService == null)
                {
                    Console.WriteLine("SignalRService is not initialized.");
                    return;
                }

                var user = (await _databaseContext.GetAllAsync<UserModel>())
                .Where(x => x.IsActive == true).FirstOrDefault();

                await _signalRService.SendMessage($"Post:{user?.UserName}", newPost);
                var newMsg = new MessageModel { Text = $"Post:{user?.UserName}", IsMine = true };
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

