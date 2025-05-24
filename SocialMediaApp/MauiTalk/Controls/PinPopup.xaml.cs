using CommunityToolkit.Maui.Views;

namespace MauiTalk.Controls;

public partial class PinPopup : Popup
{
    public event Action<string> PinSubmitted;
    private string? _comparePin;
    public PinPopup(string? comparePin)
	{
		InitializeComponent();
        _comparePin = comparePin;
	}
    private void OnSubmitClicked(object sender, EventArgs e)
    {
        var pin = pinEntry.Text?.Trim();
        if (string.IsNullOrEmpty(pin) || pin.Length < 4 || _comparePin != pin)
        {
            Application.Current.MainPage.DisplayAlert("Error",
                 _comparePin != pin ? "PIN is not valid" :
                 "PIN must be 4 digits", "OK");
            return;
        }


        PinSubmitted?.Invoke(pin);
        Close();
    }
}