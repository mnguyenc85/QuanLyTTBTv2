using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace NCMAvaControls;

public partial class NMPaginator : UserControl
{
    #region CurIndex

    public static readonly StyledProperty<int> CurIndexProperty =
        AvaloniaProperty.Register<NMPaginator, int>(nameof(CurIndex), defaultValue: 0);

    public int CurIndex
    {
        get => GetValue(CurIndexProperty);
        set => SetValue(CurIndexProperty, value);
    }

    private void UpdateButtons(int index)
    {
        BtPrev2.Content = $"{index - 2}";
        BtPrev1.Content = $"{index - 1}";
        BtCur.Content = $"{index}";
        BtNext1.Content = $"{index + 1}";
        BtNext2.Content = $"{index + 2}";
    }

    #endregion

    #region Total

    public static readonly StyledProperty<int> TotalProperty =
        AvaloniaProperty.Register<NMPaginator, int>(nameof(Total), defaultValue: 1);

    public int Total
    {
        get => GetValue(TotalProperty);
        set => SetValue(TotalProperty, value);
    }

    private void UpdateTotal(int n)
    {
        BtLast.Content = $"{n}";
    }

    private void SetVisibility()
    {
        int i = Math.Min(CurIndex, Total);
        BtFirst.IsEnabled = i > 1;
        LblDotsPrev.IsVisible = i > 4;
        BtPrev2.IsVisible = i > 3;
        BtPrev1.IsVisible = i > 2;
        BtCur.IsVisible = i > 1 && i < Total;

        BtNext1.IsVisible = i < Total - 1;
        BtNext2.IsVisible = i < Total - 2;
        LblDotsNext.IsVisible = i < Total - 3;
        BtLast.IsVisible = Total > 1;
        BtLast.IsEnabled = i < Total;
    }

    #endregion

    public event EventHandler<int>? PageClicked;

    public NMPaginator()
    {
        InitializeComponent();
    }

    static NMPaginator()
    {
        CurIndexProperty.Changed.AddClassHandler<NMPaginator>((control, args) =>
        {
            if (args.NewValue is int index)
                control.UpdateButtons(index);
            control.SetVisibility();
        });
        TotalProperty.Changed.AddClassHandler<NMPaginator>((control, args) =>
        {
            if (args.NewValue is int n)
                control.UpdateTotal(n);
            control.SetVisibility();
        });
    }

    private void BtPage_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button bt)
        {
            if (int.TryParse(bt.Content?.ToString(), out int page))
            {
                PageClicked?.Invoke(this, page);
            }
        }
    }
}