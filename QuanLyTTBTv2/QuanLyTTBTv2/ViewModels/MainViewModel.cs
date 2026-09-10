using System;
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

        private Stopwatch _stopwatch = new();
        
        [ObservableProperty]
        private string? _srvStatus = "None";

        [ObservableProperty]
        private string? _lastExecTime;
        
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
        public WorkspaceVM Workspace { get; private set; } = new();

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
        #endregion
    }
}
