using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TravelRecordApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TravelPage : ContentPage
	{

        public TravelPage ()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // TODO: move to viewModel (UpdateAsync) & ObservableCollection<Venue>

            try
            {
                PermissionStatus locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (!locationStatus.Equals(PermissionStatus.Granted))
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Location Needed", "We need permission to access device location to show you nearby venues", "OK");
                    }

                    Dictionary<Permission, PermissionStatus> permissionResults = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    if (permissionResults.ContainsKey(Permission.Location))
                    {
                        locationStatus = permissionResults[Permission.Location];
                    }
                }

                if (locationStatus.Equals(PermissionStatus.Granted))
                {
                    var geolocator = CrossGeolocator.Current;
                    var position = await geolocator.GetPositionAsync();

                    List<Venue> venues = await Venue.GetVenuesAsync(position.Latitude, position.Longitude);

                    venuesListView.ItemsSource = venues;
                }
                else
                {
                    await DisplayAlert("Location Denied", "We cannot show you venues because you denied location access permissions", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An unexpected error ocurred: {ex.Message}", "OK");
            }
        }
    }
}