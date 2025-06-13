using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using System.Data;

namespace MyFirstAppMauiBlazor.ViewModels
{
    public partial class CalculateViewModel : ObservableObject
    {
        private string _pattern = "^[\\/*]+$";

        private string _message = "Invalid expression";

        [ObservableProperty]
        private string displayValue;

        [ObservableProperty]
        private List<string> buttonValues;

        public IRelayCommand<string?> OperationsCommand { get; set; }

        public CalculateViewModel()
        {
            DisplayValue = "";

            OperationsCommand = new RelayCommand<string?>(Operations);

            ButtonValues = new List<string>
            {
                "7", "8", "9", "+",
                "4", "5", "6", "-",
                "1", "2", "3", "*",
                "0", "C", "=", "/"
             };
        }

        private void Operations(string? value)
        {
            DisplayValue = DisplayValue.Equals(_message) ? string.Empty : DisplayValue ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                bool isValid = Regex.IsMatch(DisplayValue, _pattern);
                if (!isValid && value != "C")
                {
                    if (value == "=")
                    {
                        var data = new DataTable();
                        var result = data.Compute(DisplayValue, String.Empty).ToString();
                        DisplayValue = result ?? "";
                    }
                    else
                        DisplayValue += value;
                    return;
                }
            }
            DisplayValue = value == "C" ? string.Empty : _message;
        }

    }
}
