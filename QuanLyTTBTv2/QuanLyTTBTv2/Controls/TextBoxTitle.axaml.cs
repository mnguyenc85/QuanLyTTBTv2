using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace QuanLyTTBTv2.Controls;

public partial class TextBoxTitle : UserControl
{
    #region Styled Properties
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<TextBoxTitle, string?>(
            nameof(Title));

    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        AvaloniaProperty.Register<TextBoxTitle, string?>(
            nameof(PlaceholderText));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<TextBoxTitle, string?>(
            nameof(Text),
            defaultBindingMode: BindingMode.TwoWay);

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    #endregion
    
    public TextBoxTitle()
    {
        InitializeComponent();
    }
    
    private void ClearParentTextBox_Click(object? sender, RoutedEventArgs e)
    {
        Text = string.Empty;
    }
}