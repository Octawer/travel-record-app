
using System.Collections.Generic;
using TravelRecordApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //using (var conn = new SQLite.SQLiteConnection(App.DatabasePath))
            //{
            //    conn.CreateTable<Post>();
            //    List<Post> posts = conn.Table<Post>().ToList();

            //    txtPostCount.Text = posts.Count.ToString();
            //    categoriesListView.ItemsSource = posts
            //        .Where(post => !string.IsNullOrEmpty(post.CategoryName))
            //        .GroupBy(post => post.CategoryName)
            //        .ToDictionary(g => g.Key, g => g.ToList().Count);
            //}

            List<Post> userPosts = await Post.GetUserPostsAsync(App.LoggedUser.ID);

            txtPostCount.Text = userPosts.Count.ToString();

            categoriesListView.ItemsSource = Post.GetCountByCategory(userPosts);
        }
    }
}