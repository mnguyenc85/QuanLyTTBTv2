using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace QuanLyTTBTv2.Utilities.Converters;

public class StringToClassesConverter : IValueConverter
{
    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        var result = new Classes();

        if (value is not string text || string.IsNullOrWhiteSpace(text))
            return result;

        foreach (var className in text.Split(
                     ' ',
                     StringSplitOptions.RemoveEmptyEntries | 
                     StringSplitOptions.TrimEntries))
        {
            result.Add(className);
        }

        return result;
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (value is Classes classes)
            return string.Join(" ", classes);

        return null;
    }
}