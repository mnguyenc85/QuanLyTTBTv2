using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using QuanLyTTBTv2.Services;

namespace QuanLyTTBTv2.Views;

public partial class WndAppInfo : Window
{
    private DbCache _cache = DbCache.Instance;
    public AppConfig? PrintingService { get; set; }

    public WndAppInfo()
    {
        InitializeComponent();
    }

    private void Window_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (PrintingService != null)
        {

            if (PrintingService.DocketPrint.IsError)
            {
                LstInfo.Items.Add($"Lỗi in phiếu: {PrintingService.DocketPrint.Error}");
            }
            if (PrintingService.DetailDocketPrint.IsError)
            {
                LstInfo.Items.Add($"Lỗi in phiếu chi tiết: {PrintingService.DetailDocketPrint.Error}");
            }
        }
        else
        {
            LstInfo.Items.Add("Không có PrintingServer");
        }
    }
}