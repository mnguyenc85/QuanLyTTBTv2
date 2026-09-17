using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Styling;
using Huskui.Avalonia.Controls;
using Huskui.Avalonia.Models;
using QuanLyTTBTv2.Services;
using QuanLyTTBTv2.Utilities;
using QuanLyTTBTv2.ViewModels;
using QuanLyTTBTv2.ViewModels.Server;

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
            CboDHTblIPP.Items.Add(10);
            CboDHTblIPP.Items.Add(15);
            CboDHTblIPP.Items.Add(20);
            CboDHTblIPP.Items.Add(23);
            CboDHTblIPP.Items.Add(25);
            CboDHTblIPP.SelectedIndex = 2;

            CboPhieuTblIPP.Items.Add(5);
            CboPhieuTblIPP.Items.Add(10);
            CboPhieuTblIPP.Items.Add(15);
            CboPhieuTblIPP.Items.Add(20);
            CboPhieuTblIPP.Items.Add(25);
            CboPhieuTblIPP.SelectedIndex = 2;

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

        private void NMPaginator_OnPageClicked(object? sender, int e)
        {
            if (sender == PgDonHang)
            {
                _vm?.ChangeDonHangPage(e);
            }
            else if (sender == PgPhieu)
            {
                _vm.ChangePhieuPage(e);
            }
        }

        private void CboDHTblIPP_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            // Đặt items per page cho bảng đơn hàng
            if (CboDHTblIPP.SelectedValue is int v)
                _vm.SetTableIPP(1, v);
        }

        private void CboPhieuTblIPP_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            // Đặt items per page cho bảng phiếu
            if (CboPhieuTblIPP.SelectedValue is int v)
                _vm.SetTableIPP(2, v);
        }

        private async void TabMain_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            if (TabMain.SelectedIndex == 1)
            {
                await _vm.AutoLoadChiTietDonHang(false);
                RemoveTPColumns();
                CreateTPColumns([.. _vm.Workspace.DsMaThanhPhan]);
                _vm.AutoLoadDsPhieu(true);
            }
        }

        #region Table mẻ
        
        private void RemoveTPColumns()
        {
            int totalColumns = TvwMe.Columns.Count;

            // Luôn giữ 3 cột đầu và 2 cột cuối
            int tpStartIndex = 3;
            int tpEndIndex = totalColumns - 2;

            // Xóa từ cuối về đầu để không bị thay đổi index
            for (int i = tpEndIndex - 1; i >= tpStartIndex; i--)
            {
                TvwMe.Columns.RemoveAt(i);
            }
        }

        private void CreateTPColumns(List<string> headers)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                int tpIndex = i;
                var column = new TableViewColumn
                {
                    Header = headers[i],
                    Width = new GridLength(108),
                    CellTemplate = new FuncDataTemplate<HTMeVM>((item, scope) =>
                    {
                        var textBlock = new TextBlock
                        {
                            Text = item.TPs[tpIndex],
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                        };

                        // textBlock.Bind(
                        //     TextBlock.TextProperty,
                        //     new Binding($"TPs[{i}]")
                        // );

                        return textBlock;
                    })                    
                };
                TvwMe.Columns.Insert(3 + i, column);
            }
        }
        #endregion
        
        #region Test
        private void BtTestMe_OnClick(object? sender, RoutedEventArgs e)
        {
            _vm.Workspace.LoadChiTietPhieu();
        }
        #endregion
    }
}