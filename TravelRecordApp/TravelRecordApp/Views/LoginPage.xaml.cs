using Xamarin.Forms;

namespace TravelRecordApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            var assembly = typeof(LoginPage);
            var appMainIcon = ImageSource.FromResource("TravelRecordApp.Assets.Images.icons8-globe-earth-512.png", assembly);
            mainIcon.Source = appMainIcon;
        }
    }
}
