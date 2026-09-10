using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
        #endregion
    }
}