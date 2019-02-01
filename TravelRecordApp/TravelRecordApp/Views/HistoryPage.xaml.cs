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

            List<Post> userPosts = await Post.GetUserPostsAsync(App.LoggedUser.ID);
            travelsListView.ItemsSource = userPosts;
        }
    }
}