using Plugin.Geolocator;
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

            var geolocator = CrossGeolocator.Current;
            var position = await geolocator.GetPositionAsync();

            List<Venue> venues = await Venue.GetVenuesAsync(position.Latitude, position.Longitude);

            venuesListView.ItemsSource = venues;
        }
    }
}