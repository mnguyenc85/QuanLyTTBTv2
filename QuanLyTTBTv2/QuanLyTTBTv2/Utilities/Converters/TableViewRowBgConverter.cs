using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.Utilities.Converters;

public class TableViewRowBgConverter: IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is HTMeVM me)
            switch (me.Css)
            {
                case "tentp": return Brushes.Beige;
                case "cpc": return Brushes.WhiteSmoke;
                case "me": return Brushes.White;
                case "tong": return Brushes.Beige;
            }
            
        return Brushes.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}