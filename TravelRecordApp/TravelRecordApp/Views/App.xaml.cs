using Microsoft.WindowsAzure.MobileServices;
using TravelRecordApp.Models;
using TravelRecordApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TravelRecordApp
{
    public partial class App : Application
    {
        public static string DatabasePath;
        public static MobileServiceClient MobileService =  new MobileServiceClient("https://travelrecordappomg.azurewebsites.net");
        public static User LoggedUser;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        public App(string dbPath) : this()
        {
            DatabasePath = dbPath;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
