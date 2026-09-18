using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLyTTBTv2.Models.Server;
using QuanLyTTBTv2.Services;

namespace QuanLyTTBTv2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly DbCache _dbCache = DbCache.Instance;
        private readonly SrvComm _srvComm = new();

        private readonly Stopwatch _stopwatch = new();
        
        /// <summary>
        /// Trạng thái kết nối Server
        /// </summary>
        [ObservableProperty] 
        private string? _srvStatus = "None";

        /// <summary>
        /// Hành động gần nhất
        /// </summary>
        [ObservableProperty] private string? _lastAction;
        
        /// <summary>
        /// Thời gian thực hiện hành động gần nhất
        /// </summary>
        [ObservableProperty]
        private string? _lastExecTime;
        
        /// <summary>
        /// Id của đơn hàng đang xem (ở tab phiếu)
        /// </summary>
        public long CurDonHangId { get; private set; } = -1;
        
        public ICommand FilterDonHangCommand { get; }
        public ICommand FilterPhieuCommand { get; }

        public MainViewModel()
        {
            Workspace = new(_srvComm.SrvDb);
            
            FilterDonHangCommand = new RelayCommand(FilterDonHang);
            FilterPhieuCommand = new RelayCommand(FilterPhieu);
            
            DateTime fdm = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime ldm = new DateTime(fdm.Year, fdm.Month, 1).AddMonths(1).AddDays(-1);
            LocDHTuTg = fdm;
            LocDHDenTg = ldm;
            
            LocPhieuTuTg = fdm;
            LocPhieuDenTg = ldm;
        }

        public void SetTableIPP(int tableId, int ipp)
        {
            if (ipp > 0)
                switch (tableId)
                {
                    case 1: _dhCond.Limit = ipp; break;
                    case 2: _phCond.Limit = ipp; break;
                }
        }
        
        public async Task CreateServerComm()
        {
            var s = _dbCache.Settings;
            string? srv = s.GetValue("srv.address");
            string? user = s.GetValue("srv.username");
            string? pass = s.GetValue("srv.password");

            if (srv == null || user == null || pass == null)
            {
                SrvStatus = "None";
                return;
            }
            
            string? _ = await _srvComm.Connect(srv, user, pass);

            if (_srvComm.IsServerDbOk)
            {
                SrvStatus = "OK!";
            }
            else
                SrvStatus = _srvComm.IsServerOk ? "OK" : "Error";
        }

        #region Đơn hàng
        private readonly HTDonHangCond _dhCond = new();

        [ObservableProperty] private bool _locDHTu;
        [ObservableProperty] private bool _locDHDen;
        [ObservableProperty] private DateTime _locDHTuTg;
        [ObservableProperty] private DateTime _locDHDenTg;
        
        [ObservableProperty] private string? _locDHKH;

        [ObservableProperty] private string? _locDHDuAn;

        [ObservableProperty] private int _dhTotal = 1;
        [ObservableProperty] private int _dhPage = 1;
        #endregion

        #region Phiếu
        private readonly HTPhieuCond _phCond = new();
        
        [ObservableProperty] private bool _locPhieuTu;
        [ObservableProperty] private bool _locPhieuDen;
        [ObservableProperty] private DateTime _locPhieuTuTg;
        [ObservableProperty] private DateTime _locPhieuDenTg;
        
        [ObservableProperty] private string? _locPhieuXe;
        [ObservableProperty] private string? _locPhieuLaiXe;
        
        [ObservableProperty] private string? _locPhieuCapPhoi;

        [ObservableProperty] private int _phieuTotal = 1;
        [ObservableProperty] private int _phieuPage = 1;
        #endregion
        
        #region Workspace
        public WorkspaceVM Workspace { get; private set; }

        /// <summary>
        /// Khởi tạo (load) dữ liệu ban đầu
        /// </summary>
        public async void InitData()
        {
            LastAction = "Khởi động";
            _stopwatch.Restart();
            
            await Workspace.LoadSrvFactories();
            
            _stopwatch.Stop();
            LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        }
        
        private async void FilterDonHang()
        {
            if (Workspace.SelFactory == null) return;

            LastAction = "Lấy đơn hàng";
            LastExecTime = "...";
            _stopwatch.Restart();
            
            // Điều kiện:
            _dhCond.SourceId = Workspace.SelFactory.Id;
            // - Thời gian
            _dhCond.UseFrom = LocDHTu;
            _dhCond.FromTime = LocDHTuTg;
            _dhCond.UseTo = LocDHDen;
            _dhCond.ToTime = LocDHDenTg;
            // - Khách hàng & dự án
            _dhCond.KhachHang = LocDHKH;
            _dhCond.DuAn = LocDHDuAn;
            
            _dhCond.Offset = DhPage * _dhCond.Limit;
            // Điều kiện không đổi
            if (!_dhCond.Changed) return;       
            
            await Workspace.LoadDsDonHang(_dhCond);
            if (_dhCond.Changed)
            {
                DhTotal = (_dhCond.Total - 1) / _dhCond.Limit + 1;
                DhPage = 1;
                _dhCond.Changed = false;
            }
            
            _stopwatch.Stop();
            LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        }

        public async void ChangeDonHangPage(int p)
        {
            _stopwatch.Restart();
            
            _dhCond.Offset = (p - 1) * _dhCond.Limit; 
            await Workspace.LoadDsDonHang(_dhCond);
            DhPage = p;

            _stopwatch.Stop();
            LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        }

        private bool _isLoadingDockets = false;
        private async void FilterPhieu()
        {
            if (Workspace.SelFactory == null || Workspace.SelectedDonHang == null || _isLoadingDockets) return;

            _isLoadingDockets = true;
            LastAction = "Lấy phiếu";
            LastExecTime = "...";
            _stopwatch.Restart();
            
            // Điều kiện:
            _phCond.SourceId = Workspace.SelFactory.Id;
            _phCond.DonHangId = Workspace.SelectedDonHang.LocalId;
            // - Thời gian
            _phCond.UseFrom = LocPhieuTu;
            _phCond.FromTime = LocPhieuTuTg;
            _phCond.UseTo = LocPhieuDen;
            _phCond.ToTime = LocPhieuDenTg;
            // - Khách hàng & dự án
            _phCond.Xe = LocPhieuXe;
            _phCond.LaiXe = LocPhieuLaiXe;
            
            _phCond.Offset = PhieuPage * _phCond.Limit;
            if (!_phCond.Changed) return;

            await Workspace.LoadDsPhieu(_phCond);
            if (_phCond.Changed)
            {
                PhieuTotal = (_phCond.Total - 1) / _phCond.Limit + 1;
                PhieuPage = 1;
                _phCond.Changed = false;
            }
            
            _stopwatch.Stop();
            LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
            _isLoadingDockets = false;
        }
        
        public async void ChangePhieuPage(int p)
        {
            _stopwatch.Restart();
            
            _phCond.Offset = (p - 1) * _phCond.Limit; 
            await Workspace.LoadDsPhieu(_phCond);
            PhieuPage = p;

            _stopwatch.Stop();
            LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        }

        
        public async Task AutoLoadChiTietDonHang(bool setCurDhId)
        {
            if (Workspace.SelectedDonHang == null)
            {
                Workspace.ClearCurDonHangData();
                return;
            }

            if (Workspace.SelectedDonHang.Id != CurDonHangId)
            {
                await Workspace.LoadCurDonHangData();

                if (setCurDhId) CurDonHangId = Workspace.SelectedDonHang.Id;
            }
        }

        public void AutoLoadDsPhieu(bool setCurDhId)
        {
            if (Workspace.SelectedDonHang == null)
            {
                Workspace.ClearDsPhieu();
                return;
            }

            if (Workspace.SelectedDonHang.Id != CurDonHangId)
            {
                // Reset điều kiện
                LocPhieuTu = false;
                LocPhieuDen = false;
                LocPhieuXe = null;
                LocPhieuLaiXe = null;
                _phCond.Offset = 0;
                FilterPhieu();
                
                if (setCurDhId) CurDonHangId = Workspace.SelectedDonHang.Id;
            }            
        }
        #endregion
    }
}
