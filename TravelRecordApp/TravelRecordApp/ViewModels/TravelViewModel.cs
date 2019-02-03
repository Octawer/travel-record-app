using System;
using System.ComponentModel;
using System.Windows.Input;
using TravelRecordApp.Models;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModels
{
    public class TravelViewModel : INotifyPropertyChanged
    {
        private string _experience;
        private Post _post;
        private Venue _venue;

        public Post Post
        {
            get => _post;
            set
            {
                _post = value;
                OnPropertyChanged(nameof(Post));
            }
        }

        public string Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                Post = new Post { Experience = Experience, Venue = Venue };
                OnPropertyChanged(nameof(Experience));
            }
        }
        
        public Venue Venue
        {
            get => _venue;
            set
            {
                _venue = value;
                Post = new Post { Experience = Experience, Venue = Venue };
                OnPropertyChanged(nameof(Venue));
            }
        }

        public ICommand PostCommand => new Command(PublishPostAsync, CanPublishPost);
        
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        private bool CanPublishPost(object parameter)
        {
            var post = (Post)parameter;
            return post != null &&
                !string.IsNullOrEmpty(post.Experience) &&
                post.Venue != null;
        }

        private async void PublishPostAsync(object parameter)
        {
            try
            {
                Post post = (Post)parameter;
                // syntax looks odd: post insert post ...??
                // also if we dont give this a Task return type we cannot await it, and thus we will
                // display alert before we complete the async insert
                await Post.InsertAsync(post);
                await App.Current.MainPage.DisplayAlert("Success", $"Experience saved successfully", "OK");
            }
            catch (NullReferenceException nre)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Error saving post: {nre.Message}", "Close");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Error saving post {ex.Message}", "Close");
            }
        }
    }
}
