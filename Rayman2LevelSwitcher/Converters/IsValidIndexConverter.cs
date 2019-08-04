using System;
using System.Globalization;

namespace Rayman2LevelSwitcher
{
    public class IsValidIndexConverter : BaseValueConverter<IsValidIndexConverter, int, bool>
    {
        public override bool ConvertValue(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return value > -1;
        }
    }
}