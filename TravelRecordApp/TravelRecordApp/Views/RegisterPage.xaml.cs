using System;
using TravelRecordApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage ()
		{
			InitializeComponent ();

            var assembly = typeof(RegisterPage);
            var icon = ImageSource.FromResource("TravelRecordApp.Assets.Images.icons8-contact-details-64.png", assembly);
            registerIcon.Source = icon;
        }

    }
}