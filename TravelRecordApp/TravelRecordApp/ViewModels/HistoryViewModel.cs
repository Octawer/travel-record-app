using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Models;

namespace TravelRecordApp.ViewModels
{
    public class HistoryViewModel
    {
        public ObservableCollection<Post> Posts { get; set; }

        public HistoryViewModel()
        {
            Posts = new ObservableCollection<Post>();
        }

        internal async void UpdatePostsAsync()
        {
            List<Post> userPosts = await Post.GetUserPostsAsync(App.LoggedUser.ID);
            if (userPosts != null)
            {
                Posts.Clear();
                userPosts.ForEach(p => Posts.Add(p));
            }
        }
    }
}
