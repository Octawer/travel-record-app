using TravelRecordApp.Services;
using TravelRecordApp.ViewModels;
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is HistoryViewModel)
            {
                var viewModel = (HistoryViewModel)BindingContext;
                await viewModel.UpdatePostsAsync();

                await AzureMobileDatabaseService.OfflineSyncAsync();
            }
        }
    }
}