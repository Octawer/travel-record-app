using System;
using System.Globalization;
using Xamarin.Forms;

namespace TravelRecordApp.ViewModels.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTimeOffset date = (DateTimeOffset)value;
            DateTimeOffset now = DateTimeOffset.Now;

            TimeSpan timePassed = now - date;

            if (timePassed < TimeSpan.FromSeconds(60))
            {
                return $"{timePassed.Seconds} seconds ago";
            }

            if (timePassed < TimeSpan.FromMinutes(60))
            {
                return $"{timePassed.Minutes} minutes ago";
            }

            if (timePassed < TimeSpan.FromHours(24))
            {
                return $"{timePassed.Hours} hours ago";
            }

            if (timePassed >= TimeSpan.FromHours(24) && timePassed < TimeSpan.FromHours(48))
            {
                return "yesterday";
            }

            return date.ToString("dd/MM/yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
