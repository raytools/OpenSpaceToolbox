using System;
using System.Globalization;
using OpenSpaceCore.Helpers.WPF;

namespace OpenSpaceCore.Converters
{
    public class IsValidIndexConverter : BaseValueConverter<IsValidIndexConverter, int, bool>
    {
        public override bool ConvertValue(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return value > -1;
        }
    }
}