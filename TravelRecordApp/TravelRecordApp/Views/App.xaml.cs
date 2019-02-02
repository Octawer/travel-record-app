using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
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
        public static IMobileServiceSyncTable<Post> PostsSyncTable;
        public static User LoggedUser;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        public App(string dbPath) : this()
        {
            DatabasePath = dbPath;
            var store = new MobileServiceSQLiteStore(dbPath);
            store.DefineTable<Post>();
            // magic O.O
            MobileService.SyncContext.InitializeAsync(store);
            PostsSyncTable = MobileService.GetSyncTable<Post>();
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
