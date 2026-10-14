using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using QuanLyTTBTv2.Services;

namespace QuanLyTTBTv2.Views;

public partial class WndConfig : Window
{
    private readonly SqliteDbBridge _db = SqliteDbBridge.Instance;
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
        ListAllTemplates(CboPhieuIn, _c.DocketPath);
        ListAllTemplates(CboPhieuInChiTiet, _c.DocketPath);
        
        LoadSettings();
    }
    
    private void LoadSettings()
    {
        try
        {
            var s = _c.Settings;

            TxtServer.Text = s.GetValue("srv.address");
            TxtAccount.Text = s.GetValue("srv.username");
            TxtPassword.Text = s.GetValue("srv.password");

            CboPhieuIn.Text = s.GetValue("print.phieu.coban");
            CboPhieuInChiTiet.Text = s.GetValue("print.phieu.chitiet");

            var ci = _c.PrintInfo.Company;
            ci.Load(s);
            TxtCTyTen1.Text = ci.Ten1;
            TxtCTyTen2.Text = ci.Ten2;
            TxtCTyDiaChi.Text = ci.DiaChi;
            TxtCTySdt1.Text = ci.Sdt1;
            TxtCTySdt2.Text = ci.Sdt2;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadSettings: {ex.Message}");
        }
        System.Diagnostics.Debug.WriteLine(CboPhieuIn.Text);
    }
    
    private async void SaveSettings()
    {
        var s = _c.Settings;
        
        s.Update("srv.address", TxtServer.Text);
        s.Update("srv.username", TxtAccount.Text);
        s.Update("srv.password", TxtPassword.Text);
        
        s.Update("print.phieu.coban", CboPhieuIn.Text);
        s.Update("print.phieu.chitiet", CboPhieuInChiTiet.Text);

        var ci = _c.PrintInfo.Company;
        ci.Ten1 = TxtCTyTen1.Text;
        ci.Ten2 = TxtCTyTen2.Text;
        ci.DiaChi = TxtCTyDiaChi.Text;
        ci.Sdt1 = TxtCTySdt1.Text;
        ci.Sdt2 = TxtCTySdt2.Text;
        ci.Save(s);

        (int noins, int noupdate) = await _db.Settings_SaveAsync(s);
        var box = MessageBoxManager
            .GetMessageBoxStandard("Lưu cấu hình", $"Lưu: {noins} mới, {noupdate} cập nhật!");
        await box.ShowAsync();
        Close();
    }

    private void BtAccep_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveSettings();
    }

    private async void BtTestSrv_OnClick(object? sender, RoutedEventArgs e)
    {
        SrvComm comm = new();
        string? srv = TxtServer.Text;
        string? user = TxtAccount.Text;
        string? pass = TxtPassword.Text;

        if (srv == null || user == null || pass == null)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Kết nối server", $"Lỗi: thông tin kết nối!");
            await box.ShowAsync();
            return;
        }
            
        await comm.Connect(srv, user, pass);
        comm.SyncDb();
    }

    private async void BtCreateCSDL_OnClick(object? sender, RoutedEventArgs e)
    {
        var ldb = new LocalDbBridge();
        try
        {
            ldb.CreateDb();
            ldb.Initialize();
            ldb.SyncSchema();

            var box = MessageBoxManager
                .GetMessageBoxStandard("Kết nối csdl", $"Kết nối thành công!");
            await box.ShowAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            var box = MessageBoxManager
                .GetMessageBoxStandard("Kết nối csdl", $"Lỗi: không kết nối được!");
            await box.ShowAsync();
        }
    }
    
    private void ListAllTemplates(ComboBox cbo, string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath, "*.frx", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                string filename = Path.GetFileName(file);
                cbo.Items.Add(filename);
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("ListAllTemplates: Thư mục không tồn tại!");
        }
    }
}