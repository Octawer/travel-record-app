using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRecordApp.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _iD;
        private string _email;
        private string _password;

        public string ID
        {
            get => _iD;
            set
            {
                _iD = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static async Task InsertAsync(User user)
        {
            await App.MobileService.GetTable<User>().InsertAsync(user);
        }

        public static async Task<User> GetUserByEmailAsync(string email)
        {
            return (await App.MobileService.GetTable<User>().ToListAsync()).FirstOrDefault(u => u.Email.Equals(email));
        }

        public static async Task<bool> IsValidUserAsync(string email, string password)
        {
            // fail fast
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return false;

            User user = await User.GetUserByEmailAsync(email);
            return user != null && user.Password.Equals(password);

        }
    }
}
