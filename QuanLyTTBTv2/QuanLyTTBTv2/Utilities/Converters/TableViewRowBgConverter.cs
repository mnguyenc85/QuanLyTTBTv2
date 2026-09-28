using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.Utilities.Converters;

public class TableViewRowBgConverter: IValueConverter
{
    private static SolidColorBrush Brush(byte alpha, byte r, byte g, byte b)
        => new(Color.FromArgb(alpha, r, g, b));

    private static readonly IBrush TentpBrush = Brush(64, 230, 230, 160);
    private static readonly IBrush CpcBrush   = Brush(64, 210, 210, 210);
    private static readonly IBrush MeBrush    = Brush(64, 255, 255, 255);
    private static readonly IBrush TongBrush  = Brush(64, 230, 230, 160);

    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (value is HTMeVM me)
        {
            return me.Css switch
            {
                "tentp" => TentpBrush,
                "cpc"   => CpcBrush,
                "me"    => MeBrush,
                "tong"  => TongBrush,
                _       => Brushes.Transparent
            };
        }

        return Brushes.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}