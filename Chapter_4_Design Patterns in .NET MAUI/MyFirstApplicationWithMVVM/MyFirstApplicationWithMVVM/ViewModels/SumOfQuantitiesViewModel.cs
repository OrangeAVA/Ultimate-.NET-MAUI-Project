using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyFirstApplicationWithMVVM.ViewModels
{
    public partial class SumOfQuantitiesViewModel : ObservableObject
    {

        [ObservableProperty]
        private double _numberOne;

        [ObservableProperty]
        private double _numberTwo;

        [ObservableProperty]
        private double _result;

        public IRelayCommand AddCommand { get; set; }

        public SumOfQuantitiesViewModel() => AddCommand = new RelayCommand(CalculateSum);

        private void CalculateSum()
        {
            Result = NumberOne + NumberTwo; 
        }

    }   

}
