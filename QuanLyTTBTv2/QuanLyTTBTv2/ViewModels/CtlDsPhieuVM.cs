using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.ViewModels;

public partial class CtlDsPhieuVM: ViewModelBase
{
    private readonly Stopwatch _stopwatch = new();
    private bool _isLoadingDockets = false;
    
    public WorkspaceVM? Workspace { get; }
    public MainViewModel MainVM { get; }
    
    #region Điều kiện
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
            
    /// <summary>
    /// Id của đơn hàng đang xem (ở tab phiếu)
    /// </summary>
    public long CurDonHangId { get; private set; } = -1;

    public ICommand FilterPhieuCommand { get; }
    
    public CtlDsPhieuVM()
    {
        FilterPhieuCommand = new RelayCommand(FilterPhieu);
        Init();
    }
    
    public CtlDsPhieuVM(WorkspaceVM ws, MainViewModel mainvm)
    {
        Workspace = ws;
        MainVM = mainvm;
        FilterPhieuCommand = new RelayCommand(FilterPhieu);
        Init();
    }
    
    private void Init()
    {
        DateTime fdm = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime ldm = new DateTime(fdm.Year, fdm.Month, 1).AddMonths(1).AddDays(-1);
        LocPhieuTuTg = fdm;
        LocPhieuDenTg = ldm;
        _phCond.Limit = 15;
    } 

    
    private async void FilterPhieu()
    {
        if (Workspace == null || Workspace.SelFactory == null || Workspace.SelectedDonHang == null || _isLoadingDockets) return;

        _isLoadingDockets = true;
        MainVM.LastAction = "Lấy phiếu";
        MainVM.LastExecTime = "...";
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
        MainVM.LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        _isLoadingDockets = false;
    }
        
    public async void ChangePhieuPage(int p)
    {
        if (Workspace == null) return;
        
        _stopwatch.Restart();
            
        _phCond.Offset = (p - 1) * _phCond.Limit; 
        await Workspace.LoadDsPhieu(_phCond);
        PhieuPage = p;

        _stopwatch.Stop();
        MainVM.LastExecTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
    }
    
            
    public async Task AutoLoadChiTietDonHang(bool setCurDhId)
    {
        if (Workspace == null) return;
        
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
        if (Workspace == null) return;
        
        if (Workspace.SelectedDonHang == null)
        {
            Workspace.ClearDsPhieu();
            return;
        }

        if (Workspace.SelectedDonHang.Id != CurDonHangId)
        {
            Workspace.ClearDsPhieu();
            
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
        
    public void SetTableIPP(int ipp, int tableId = 1)
    {
        if (ipp > 0)
            switch (tableId)
            {
                case 1: _phCond.Limit = ipp; break;
            }
    }
}