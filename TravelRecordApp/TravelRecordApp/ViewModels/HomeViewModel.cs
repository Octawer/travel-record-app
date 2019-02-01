using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModels
{
    public class HomeViewModel
    {
        public ICommand AddTravelCommand => new Command(AddTravel);

        private void AddTravel(object parameter) => App.Current.MainPage.Navigation.PushAsync(new TravelPage());
    }
}
