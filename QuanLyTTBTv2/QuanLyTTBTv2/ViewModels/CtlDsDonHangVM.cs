using System;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.ViewModels;

public partial class CtlDsDonHangVM: ViewModelBase
{
    private readonly Stopwatch _stopwatch = new();
    
    public WorkspaceVM? Workspace { get; }
    public MainViewModel MainVM { get; }
    
    #region Điều kiện lọc
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
    
    /// <summary>
    /// Id của đơn hàng đang xem (ở tab phiếu)
    /// </summary>
    public long CurDonHangId { get; private set; } = -1;
        
    public ICommand FilterDonHangCommand { get; }

    public CtlDsDonHangVM()
    {
        FilterDonHangCommand = new RelayCommand(FilterDonHang);
        Init();
    }
    
    public CtlDsDonHangVM(WorkspaceVM ws, MainViewModel mainvm)
    {
        Workspace = ws;
        MainVM = mainvm;
        FilterDonHangCommand = new RelayCommand(FilterDonHang);
        Init();
    }

    private void Init()
    {
        DateTime fdm = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime ldm = new DateTime(fdm.Year, fdm.Month, 1).AddMonths(1).AddDays(-1);
        LocDHTuTg = fdm;
        LocDHDenTg = ldm;
        _dhCond.Limit = 20;
    } 
    
    private async void FilterDonHang()
    {
        if (Workspace == null || Workspace.SelFactory == null) return;

        MainVM.LastAction = "Lấy đơn hàng";
        MainVM.LastExecTime = "...";
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
        MainVM.LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
    }

    public async void ChangeDonHangPage(int p)
    {
        if (Workspace == null) return;
        
        _stopwatch.Restart();
            
        _dhCond.Offset = (p - 1) * _dhCond.Limit; 
        await Workspace.LoadDsDonHang(_dhCond);
        DhPage = p;

        _stopwatch.Stop();
        MainVM.LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
    }
    
    public void SetTableIPP(int ipp, int tableId = 1)
    {
        if (ipp > 0)
            switch (tableId)
            {
                case 1: _dhCond.Limit = ipp; break;
            }
    }
}