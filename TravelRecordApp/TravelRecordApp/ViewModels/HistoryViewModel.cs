using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelRecordApp.Models;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModels
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;

        public ObservableCollection<Post> Posts { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public ICommand DeletePostCommand => new Command(DeletePostAsync);
        public ICommand RefreshPostsCommand => new Command(RefreshPostsAsync);

        
        public HistoryViewModel()
        {
            Posts = new ObservableCollection<Post>();
            IsBusy = false;
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion



        private async void RefreshPostsAsync(object parameter)
        {
            IsBusy = true;
            await UpdatePostsAsync();
            IsBusy = false;
        }

        public async Task UpdatePostsAsync()
        {
            List<Post> userPosts = await Post.GetUserPostsAsync(App.LoggedUser.ID);
            if (userPosts != null)
            {
                Posts.Clear();
                userPosts.ForEach(p => Posts.Add(p));
            }
        }

        public async void DeletePostAsync(object parameter)
        {
            try
            {
                var post = parameter as Post;
                await Post.DeleteAsync(post);
                UpdatePostsAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error ocurred while deleting post: {ex.Message}", "OK");
            }
        }
    }
}
