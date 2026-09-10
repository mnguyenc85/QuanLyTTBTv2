using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Styling;
using QuanLyTTBTv2.Services;
using QuanLyTTBTv2.Utilities;
using QuanLyTTBTv2.ViewModels;

namespace QuanLyTTBTv2.Views
{
    public partial class MainWindow : Window
    {
        private readonly LocalDbBridge _ldb = LocalDbBridge.Instance;
        private readonly DbCache _dbCache = DbCache.Instance;

        private MainViewModel _vm = new();
        
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
        #endregion

        /// <summary>
        /// Khởi tạo db (local & server) -> khởi tạo khác (async)
        /// </summary>
        private async void Init()
        {
            try
            {
                await _dbCache.InitAsync();
                _ldb.SyncSchema();

                await _vm.CreateServerComm();
                _vm.InitData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        #region Menu
        private void MniSysExit_OnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void MniSysConfig_OnClick(object? sender, RoutedEventArgs e)
        {
            WndConfig wnd = new WndConfig();
            await wnd.ShowDialog(this);
        }
        
        private void OnSwitchThemeClick(object? sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            if (app is null) return;

            // Đang Dark → chuyển Light, ngược lại
            app.RequestedThemeVariant =
                app.ActualThemeVariant == ThemeVariant.Dark
                    ? ThemeVariant.Light
                    : ThemeVariant.Dark;
            
            UpdateIcon();
        }
        #endregion

        #region Icons
        private Bitmap? _bmpCTLight, _bmpCTDark;
        private Bitmap? _bmpPhieuLight, _bmpPhieuDark;

        private void LoadIcons()
        {
            LoadIcon("avares://QuanLyTTBTv2/Assets/congtrinh_1_64.png", out _bmpCTLight, out _bmpCTDark);
            LoadIcon("avares://QuanLyTTBTv2/Assets/docket_1_64.png", out _bmpPhieuLight, out _bmpPhieuDark);
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
    }
}