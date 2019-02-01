using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using TravelRecordApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Position = Plugin.Geolocator.Abstractions.Position;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        private bool hasLocationPermission;
        private readonly double DEFAULT_LATITUDE_DEGREES = 1;
        private readonly double DEFAULT_LONGITUDE_DEGREES = 1;
        private readonly double MINIMUM_DISANCE_TO_TRACK = 100;

        public MapPage ()
		{
			InitializeComponent();
            GetLocationPermissionsAsync();
		}


        private async void GetLocationPermissionsAsync()
        {
            try
            {
                PermissionStatus locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (!locationStatus.Equals(PermissionStatus.Granted))
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Location Needed", "We need permission to access device location to show you the map", "OK");
                    }

                    Dictionary<Permission, PermissionStatus> permissionResults = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    if (permissionResults.ContainsKey(Permission.Location))
                    {
                        locationStatus = permissionResults[Permission.Location];
                        
                        SetLocationAsync();
                    }
                }

                travelsMap.IsShowingUser = locationStatus.Equals(PermissionStatus.Granted);
                hasLocationPermission = locationStatus.Equals(PermissionStatus.Granted);

                if (!locationStatus.Equals(PermissionStatus.Granted))
                {
                    await DisplayAlert("Location Denied", "We cannot show you map because you denied location access permissions", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"We ran into a issue showing you the map :( {ex.Message}", "OK");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (IsLocationAvailable() && hasLocationPermission)
            {
                var geolocator = CrossGeolocator.Current;
                geolocator.PositionChanged += Geolocator_PositionChanged;
                await geolocator.StartListeningAsync(TimeSpan.Zero, MINIMUM_DISANCE_TO_TRACK);
            }

            SetLocationAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var geolocator = CrossGeolocator.Current;
            geolocator.StopListeningAsync();    // why dont we await this ??
            geolocator.PositionChanged -= Geolocator_PositionChanged;

        }

        private void Geolocator_PositionChanged(object sender, PositionEventArgs e)
        {
            MoveMap(e.Position);
        }

        private async void SetLocationAsync()
        {
            if (IsLocationAvailable() && hasLocationPermission)
            {
                var geolocator = CrossGeolocator.Current;
                Position position = await geolocator.GetPositionAsync();
                MoveMap(position);

                SetVenuesPinsAsync(position);
            }
        }

        private async void SetVenuesPinsAsync(Position position)
        {
            //using (var conn = new SQLite.SQLiteConnection(App.DatabasePath))
            //{
            //    conn.CreateTable<Post>();
            //    List<Post> posts = conn.Table<Post>().ToList();

            //    posts.ForEach(post => PutPinInMap(post));
            //}

            List<Post> userPosts = await Post.GetUserPostsAsync(App.LoggedUser.ID);
            userPosts.ForEach(post => PutPinInMap(post));
        }

        private void PutPinInMap(Post post)
        {
            try
            {
                var pin = new Pin
                {
                    Type = PinType.SavedPin,
                    Label = post.VenueName,
                    Address = post.LocationAddress,
                    Position = new Xamarin.Forms.Maps.Position(post.LocationLatitude, post.LocationLongitude)
                };

                travelsMap.Pins.Add(pin);
            }
            catch (Exception ex)
            {
            }
        }

        private void MoveMap(Position position)
        {
            Xamarin.Forms.Maps.Position center = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
            Xamarin.Forms.Maps.MapSpan mapSpan = new Xamarin.Forms.Maps.MapSpan(center, DEFAULT_LATITUDE_DEGREES, DEFAULT_LONGITUDE_DEGREES);
            travelsMap.MoveToRegion(mapSpan);
        }

        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }
    }
}