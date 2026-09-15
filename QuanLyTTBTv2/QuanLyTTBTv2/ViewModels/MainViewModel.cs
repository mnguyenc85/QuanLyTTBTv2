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
        
        [ObservableProperty]
        private string? _srvStatus = "None";

        [ObservableProperty]
        private string? _lastExecTime;
        
        public ICommand FilterDonHangCommand { get; }
        /// <summary>
        /// Ấn nút hoặc chọn từ bảng
        /// </summary>
        public ICommand ChangeCurDHCommand { get; }

        public MainViewModel()
        {
            Workspace = new(_srvComm.SrvDb);
            
            FilterDonHangCommand = new RelayCommand(FilterDonHang);
            ChangeCurDHCommand = new RelayCommand(ChangeCurDH);
        }

        public void SetTableIPP(int tableId, int ipp)
        {
            if (ipp > 0)
                switch (tableId)
                {
                    case 1: _dhCond.Limit = ipp; break;
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
        
        #region Workspace
        public WorkspaceVM Workspace { get; private set; }

        /// <summary>
        /// Khởi tạo (load) dữ liệu ban đầu
        /// </summary>
        public async void InitData()
        {
            _stopwatch.Restart();
            
            await Workspace.LoadSrvFactories();
            
            _stopwatch.Stop();
            LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        }

        
        private async void FilterDonHang()
        {
            if (Workspace.SelFactory == null) return;
            
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

        private async void ChangeCurDH()
        {
            await Workspace.LoadCurDonHang();
        }
        #endregion
    }
}
