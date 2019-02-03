using System.ComponentModel;
using System.Windows.Input;
using TravelRecordApp.Models;
using TravelRecordApp.Views;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private User _user;
        private string _email;
        private string _password;

        public ICommand LoginCommand => new Command(LoginAsync, CanLogin);
        public ICommand RegisterCommand => new Command(NavigateToRegister);

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                User = new User { Email = Email, Password = Password };
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                User = new User { Email = Email, Password = Password };
                OnPropertyChanged(nameof(Password));
            }
        }


        public LoginViewModel()
        {
            User = new User();
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion


        private bool CanLogin(object parameter)
        {
            User user = parameter as User;
            return !string.IsNullOrEmpty(user?.Email) && !string.IsNullOrEmpty(user?.Password);
        }

        private async void LoginAsync(object parameter)
        {
            User user = parameter as User;
            if (await User.IsValidUserAsync(user.Email, user.Password))
            {
                App.LoggedUser = await User.GetUserByEmailAsync(user.Email);
                await App.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Username / Pwd Incorrect", "OK");
            }
        }

        public async void NavigateToRegister()
        {
            // Coupling with View (RegisterPage, MainPage through App ...) Is this ok ??
            await App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
    }
}
