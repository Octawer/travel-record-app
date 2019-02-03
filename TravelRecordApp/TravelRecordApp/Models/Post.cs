using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TravelRecordApp.Services;

namespace TravelRecordApp.Models
{
    public class Post : INotifyPropertyChanged
    {
        #region Properties

        private string _iD;
        private string _experience;
        private string _venueId;
        private string _venueName;
        private string _categoryId;
        private string _categoryName;
        private string _locationAddress;
        private double _locationLatitude;
        private double _locationLongitude;
        private int _locationDistance;
        private string _userID;
        private Venue _venue;
        private DateTimeOffset _createdAt;

        public string ID
        {
            get => _iD;
            set
            {
                _iD = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public string Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                OnPropertyChanged(nameof(Experience));
            }
        }

        public string VenueId
        {
            get => _venueId;
            set
            {
                _venueId = value;
                OnPropertyChanged(nameof(VenueId));
            }
        }

        public string VenueName
        {
            get => _venueName;
            set
            {
                _venueName = value;
                OnPropertyChanged(nameof(VenueName));
            }
        }

        public string CategoryId
        {
            get => _categoryId;
            set
            {
                _categoryId = value;
                OnPropertyChanged(nameof(CategoryId));
            }
        }

        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }

        public string LocationAddress
        {
            get => _locationAddress;
            set
            {
                _locationAddress = value;
                OnPropertyChanged(nameof(LocationAddress));
            }
        }

        public double LocationLatitude
        {
            get => _locationLatitude;
            set
            {
                _locationLatitude = value;
                OnPropertyChanged(nameof(LocationLatitude));
            }
        }

        public double LocationLongitude
        {
            get => _locationLongitude;
            set
            {
                _locationLongitude = value;
                OnPropertyChanged(nameof(LocationLongitude));
            }
        }

        public int LocationDistance
        {
            get => _locationDistance;
            set
            {
                _locationDistance = value;
                OnPropertyChanged(nameof(LocationDistance));
            }
        }

        public string UserID
        {
            get => _userID;
            set
            {
                _userID = value;
                OnPropertyChanged(nameof(UserID));
            }
        }

        [JsonIgnore]
        public Venue Venue
        {
            get => _venue;
            set
            {
                _venue = value;

                if (Venue != null)
                {
                    var selectedCategory = Venue.Categories.FirstOrDefault();

                    VenueId = Venue.Id;
                    VenueName = Venue.Name;
                    CategoryId = selectedCategory.Id;
                    CategoryName = selectedCategory.Name;
                    LocationAddress = Venue.Location.Address;
                    LocationDistance = Venue.Location.Distance;
                    LocationLatitude = Venue.Location.Lat;
                    LocationLongitude = Venue.Location.Lng;
                    UserID = App.LoggedUser.ID;
                }

                OnPropertyChanged(nameof(Venue));
            }
        }

        public DateTimeOffset CREATEDAT
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged(nameof(CREATEDAT));
            }
        }


        #endregion


        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion


        #region Static Methods

        // is this OK that the Model access cloud storage through http ?? (seems like coupling with web / UI layer...)
        public static async Task InsertAsync(Post post)
        {
            await App.PostsSyncTable.InsertAsync(post);
            await App.MobileService.SyncContext.PushAsync();
        }

        public static async Task<List<Post>> GetUserPostsAsync(string userID)
        {
            return await App.PostsSyncTable.Where(p => p.UserID == userID).ToListAsync();
        }

        public static Dictionary<string, int> GetCountByCategory(List<Post> userPosts)
        {
            return userPosts
                .Where(post => !string.IsNullOrEmpty(post.CategoryName))
                .GroupBy(post => post.CategoryName)
                .ToDictionary(g => g.Key, g => g.ToList().Count);
        }

        public static async Task DeleteAsync(Post post)
        {
            await App.PostsSyncTable.DeleteAsync(post);
            await App.MobileService.SyncContext.PushAsync();
        }

        #endregion

    }
}
