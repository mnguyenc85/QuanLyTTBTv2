using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Styling;
using Huskui.Avalonia.Controls;
using Huskui.Avalonia.Models;
using QuanLyTTBTv2.Services;
using QuanLyTTBTv2.Utilities;
using QuanLyTTBTv2.ViewModels;

namespace QuanLyTTBTv2.Views
{
    public partial class MainWindow : AppWindow
    {
        private readonly LocalDbBridge _ldb = LocalDbBridge.Instance;
        private readonly DbCache _dbCache = DbCache.Instance;

        private readonly MainViewModel _vm = new();
        
        #region Initialization & Startup & Closed

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm;
            
            LoadIcons();
        }

        private void Window_OnLoaded(object? sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Window_OnClosed(object? sender, EventArgs e)
        {
            CrashLogger.Shutdown();
        }

        /// <summary>
        /// Khởi tạo db (local & server) -> khởi tạo khác (async)
        /// </summary>
        private async void Init()
        {
            try
            {
                if (!File.Exists(_ldb.DbPath))
                    _ldb.CreateDatabase();
                _ldb.SyncSchema();
                
                await _dbCache.InitAsync();

                await _vm.CreateServerComm();
                _vm.InitData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        #endregion
        
        #region Menu & Buttons

        private void MniSysExit_OnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void MniSysConfig_OnClick(object? sender, RoutedEventArgs e)
        {
            WndConfig wnd = new WndConfig();
            await wnd.ShowDialog(this);
        }
        
        private async void MniDHNL_OnClick(object? sender, RoutedEventArgs e)
        {
            WndDataViewer wnd = new WndDataViewer();
            wnd.SetWorkspace(_vm.Workspace);
            await wnd.ShowDialog(this);
        }
        #endregion

        #region Advanced UI
        private AppSurface? GetAppSurface() => AppSurface.GetAppSurface(this);
        
        #region Icons

        // Chuyển màu icon theo theme Light/Dark
        private Bitmap? _bmpCTLight, _bmpCTDark;
        private Bitmap? _bmpPhieuLight, _bmpPhieuDark;

        private void OnSwitchThemeClick(object? sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            if (app is null) return;

            bool isDark = app.ActualThemeVariant == ThemeVariant.Dark; 
            
            // Đang Dark → chuyển Light, ngược lại
            app.RequestedThemeVariant = isDark ? ThemeVariant.Light : ThemeVariant.Dark;

            UpdateIcon();

            if (!isDark)
            {
                var appSurface = GetAppSurface();
                if (appSurface == null)
                {
                    System.Diagnostics.Debug.WriteLine("No app surface");
                    return;
                }
                var notification = new GrowlItem()
                {
                    Level = GrowlLevel.Warning,
                    Title = "Thông báo",
                    Content = "Tính năng này đang được thử nghiệm."
                };
                appSurface.PopGrowl(notification);
            }
        }
        
        private void LoadIcons()
        {
            LoadIcon("avares://QuanLyTTBTv2/Assets/congtrinh_1_64.png", out _bmpCTLight, out _bmpCTDark);
            LoadIcon("avares://QuanLyTTBTv2/Assets/concrete_truck_2_64.png", out _bmpPhieuLight, out _bmpPhieuDark);
        }

        private void UpdateIcon()
        {
            var isDark = Application.Current?.ActualThemeVariant == ThemeVariant.Dark;

            IconTabCongTrinh.Source = isDark ? _bmpCTLight : _bmpCTDark;
            IconTabPhieu.Source = isDark ? _bmpPhieuLight : _bmpPhieuDark;
        }

        private void LoadIcon(string path, out Bitmap? light, out Bitmap? dark)
        {
            var uri = new Uri(path);
            using var stream = AssetLoader.Open(uri);
            dark = new Bitmap(stream);
            light = ImageHelper.Invert(dark);
        }

        #endregion
        #endregion

        private int _lastTabIndex = -1;
        private async void TabMain_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;

            CtlDsPh.IsActive = TabMain.SelectedIndex == 1;
            
            if (TabMain.SelectedIndex == 1 && TabMain.SelectedIndex != _lastTabIndex)
            {
                System.Diagnostics.Debug.WriteLine("Selected tab 1: Phiếu");
                
                long isDHChanged = _vm.PhieuVM.CheckSelectedDonHangChanged(); 
                if (isDHChanged != 0)
                {
                    _vm.Workspace.ClearCurDonHangData();
                    _vm.Workspace.ClearDsPhieu();

                    if (isDHChanged > 0)
                    {
                        await _vm.PhieuVM.AutoLoadChiTietDonHang();
                        CtlDsPh.ReCreateTblMeColumns();
                        await _vm.PhieuVM.AutoLoadDsPhieu();
                    }
                }
            }
            

            _lastTabIndex = TabMain.SelectedIndex;
        }
        
        #region Test
        //
        #endregion
    }
}