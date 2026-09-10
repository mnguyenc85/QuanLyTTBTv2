using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
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
        
        s.Update("srv.address", TxtServer.Text);
        s.Update("srv.username", TxtAccount.Text);
        s.Update("srv.password", TxtPassword.Text);

        (int noins, int noupdate) = await _db.Settings_SaveAsync(s);
        var box = MessageBoxManager
            .GetMessageBoxStandard("Lưu cấu hình", $"Lưu: {noins} mới, {noupdate} cập nhật!");
        var _ = await box.ShowAsync();
        Close();
    }

    private void BtAccep_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveSettings();
    }
}