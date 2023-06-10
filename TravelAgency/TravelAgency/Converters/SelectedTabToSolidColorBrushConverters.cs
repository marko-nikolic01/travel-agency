using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;

namespace TravelAgency.Converters
{
    public class SelectedTabToSolidColorBrushConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string selectedTab = (string)value;
            if (selectedTab == "Home" || selectedTab == "HomeDemo")
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#999999");
            }
            return (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedTabToSolidColorBrushConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string selectedTab = (string)value;
            if (selectedTab == "AccommodationsReservations" || selectedTab == "AccommodationsReservationsDemo")
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#999999");
            }
            return (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedTabToSolidColorBrushConverter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string selectedTab = (string)value;
            if (selectedTab == "Reviews" || selectedTab == "ReviewsDemo")
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#999999");
            }
            return (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedTabToSolidColorBrushConverter4 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string selectedTab = (string)value;
            if (selectedTab == "Forums" || selectedTab == "ForumsDemo")
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#999999");
            }
            return (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedTabToSolidColorBrushConverter5 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string selectedTab = (string)value;
            if (selectedTab == "Notifications" || selectedTab == "NotificationsDemo")
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#999999");
            }
            return (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedTabToSolidColorBrushConverter6 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string selectedTab = (string)value;
            if (selectedTab == "UserProfile" || selectedTab == "UserProfileDemo")
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#999999");
            }
            return (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
