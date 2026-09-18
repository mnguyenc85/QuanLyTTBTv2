using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Services;

namespace QuanLyTTBTv2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly DbCache _dbCache = DbCache.Instance;
        private readonly SrvComm _srvComm = new();

        private readonly Stopwatch _stopwatch = new();

        public CtlDsDonHangVM DhVM { get; }
        public CtlDsPhieuVM PhieuVM { get; }
        
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

        public MainViewModel()
        {
            Workspace = new(_srvComm.SrvDb);
            DhVM = new CtlDsDonHangVM(Workspace, this);
            PhieuVM = new CtlDsPhieuVM(Workspace, this);
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
        #endregion
    }
}
