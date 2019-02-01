using System;
using System.ComponentModel;
using System.Windows.Input;
using TravelRecordApp.Models;
using TravelRecordApp.Views;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _passwordConfirm;
        private string _password;
        private string _email;
        private User _user;

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
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                User = new User { Email = Email, Password = Password };
                OnPropertyChanged(nameof(PasswordConfirm));
            }
        }

        public ICommand RegisterCommand => new Command(RegisterAsync, CanRegister);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public RegisterViewModel()
        {
            User = new User();
        }

        private bool CanRegister(object parameter)
        {
            User user = parameter as User;
            return user != null && 
                !string.IsNullOrEmpty(user.Email) &&
                user.Email.Contains("@") &&
                !string.IsNullOrEmpty(user.Password) &&
                user.Password == PasswordConfirm;
        }

        private async void RegisterAsync(object parameter)
        {
            try
            {
                User user = parameter as User;
                // insert into Azure User easy table
                await User.InsertAsync(user);
                await App.Current.MainPage.DisplayAlert("Success", $"{user.Email} Registered correctly", "OK");
                await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Error registering user: {ex.Message}", "OK");
            }
        }

    }
}
