using System.Collections.Generic;
using TravelRecordApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
		public HistoryPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //using (var conn = new SQLite.SQLiteConnection(App.DatabasePath))
            //{
            //    conn.CreateTable<Post>();
            //    var posts = conn.Table<Post>().ToList();

            //    lvTravels.ItemsSource = posts;
            //}

            List<Post> userPosts = await Post.GetUserPostsAsync(App.LoggedUser.ID);
            lvTravels.ItemsSource = userPosts;
        }
    }
}