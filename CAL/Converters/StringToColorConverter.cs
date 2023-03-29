using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value.ToString().ToLower();
            switch (color)
            {
                case "green":
                    return Colors.Green;
                case "red":
                    return Colors.Red;
                case "blue":
                    return Colors.Blue;
                case "orange":
                    return Colors.Orange;
                case "yellow":
                    return Colors.Yellow;
                case "purple":
                    return Colors.Purple;
                case "pink":
                    return Colors.Pink;
                default:
                    throw new Exception($"No color found for: {color}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
