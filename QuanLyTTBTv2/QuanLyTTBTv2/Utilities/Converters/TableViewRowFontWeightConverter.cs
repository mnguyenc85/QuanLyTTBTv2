using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.Utilities.Converters;

public class TableViewRowFontWeightConverter: IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is HTMeVM me)
            switch (me.Css)
            {
                case "tentp": return FontWeight.DemiBold;
                // case "cpc": return FontWeight.Normal;
                // case "me": return FontWeight.Normal;
                case "tong": return FontWeight.Bold;
            }

        return FontWeight.Normal;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}