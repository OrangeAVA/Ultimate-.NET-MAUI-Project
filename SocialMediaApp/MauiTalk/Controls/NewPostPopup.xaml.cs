using CommunityToolkit.Maui.Views;
using System.Xml.Linq;

namespace MauiTalk.Controls;

public partial class NewPostPopup : Popup
{
    public MsgSavedEventHandler MsgSaved;

    public NewPostPopup()
    {
        InitializeComponent();
    }

    public delegate void MsgSavedEventHandler(string msg);

    private void OnCloseClicked(object sender, EventArgs e)
    {
        string msg = txtMsg.Text?.Trim(); // Elimina espacios extra

        if (string.IsNullOrWhiteSpace(msg))
        {
            Application.Current.MainPage.DisplayAlert("Info", "The message cannot be empty.", "OK");
            return;
        }

        MsgSaved?.Invoke(msg);

        Close();
    }
}