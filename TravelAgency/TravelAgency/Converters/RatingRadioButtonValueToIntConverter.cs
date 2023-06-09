using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TravelAgency.Converters
{
    public class RatingRadioButtonValueToIntConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            int targetValue = System.Convert.ToInt32(parameter);
            return intValue == targetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            int targetValue = System.Convert.ToInt32(parameter);
            return isChecked ? targetValue : Binding.DoNothing;
        }
    }
}
