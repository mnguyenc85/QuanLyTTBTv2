using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using QuanLyTTBTv2.Services;

namespace QuanLyTTBTv2.Views;

public partial class WndConfig : Window
{
    private readonly LocalDbBridge _db = LocalDbBridge.Instance;
    private readonly DbCache _c = DbCache.Instance;
    
    public WndConfig()
    {
        InitializeComponent();
    }

    private void Window_OnLoaded(object? sender, RoutedEventArgs e)
    {
        Init();
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
    }
    
    private void Init()
    {
        LoadSettings();
    }
    
    private void LoadSettings()
    {
        var s = _c.Settings;

        TxtServer.Text = s.GetValue("srv.address");
        TxtAccount.Text = s.GetValue("srv.username");
        TxtPassword.Text = s.GetValue("srv.password");
    }
    
    private async void SaveSettings()
    {
        var s = _c.Settings;

        (int noins, int noupdate) = await _db.Settings_SaveAsync(s);
        // MessageBox.Show($"Lưu cài đặt: {noins} mới, {noupdate} cập nhật!");
    }

    private void BtAccep_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveSettings();
    }
}