using System;
using System.Globalization;

namespace OpenSpaceToolbox
{
    public class IsValidIndexConverter : BaseValueConverter<IsValidIndexConverter, int, bool>
    {
        public override bool ConvertValue(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return value > -1;
        }
    }
}