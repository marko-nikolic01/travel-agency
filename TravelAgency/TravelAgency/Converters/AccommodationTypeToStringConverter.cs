using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TravelAgency.Domain.Models;

namespace TravelAgency.Converters
{
    public class AccommodationTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((AccommodationType)value)
            {
                case AccommodationType.APARTMENT:
                    return "Appartment";
                case AccommodationType.HOUSE:
                    return "House";
                case AccommodationType.HUT:
                    return "Hut";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AccommodationTypeToStringConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((AccommodationType)value)
            {
                case AccommodationType.APARTMENT:
                    return "Apartman";
                case AccommodationType.HOUSE:
                    return "Kuća";
                case AccommodationType.HUT:
                    return "Koliba";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AccommodationTypeStringToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((string)value)
            {
                case "Apartman":
                    return "Appartment";
                case "Kuća":
                    return "House";
                case "Koliba":
                    return "Hut";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((string)value)
            {
                case "Appartment":
                    return "Apartman";
                case "House":
                    return "Kuća";
                case "Hut":
                    return "Koliba";
            }
            return null;
        }
    }

    public class EnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? parameter : Binding.DoNothing;
        }
    }
}
