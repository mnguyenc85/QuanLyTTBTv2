using System;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Local;
using QuanLyTTBTv2.ViewModels.Printing;

namespace QuanLyTTBTv2.ViewModels;

public partial class CtlPnlInPhieuVM: ViewModelBase
{
    public WorkspaceVM Workspace { get; }
    public MainViewModel MainVM { get; }
    
    [ObservableProperty] private int _phieuTotal = 1;
    [ObservableProperty] private int _phieuPage = 1;
    
    public CtlPnlInPhieuVM()
    {
        // Chỉ để dùng khi designer
        MainVM = new MainViewModel();
        Workspace = MainVM.Workspace;
        Workspace.CurInPhieu = new InPhieuVM()
        {
            Id = -1,
            SoPhieu = "Test000",
            NgayTron = "01/01/0001",
            TgRoiTram = "00:00 AM",
            KhachHang = "Khách hàng",
            DiaChiKH = "Địa chỉ KH",
            CongTrinh = "Dự án - công trình - hạng mục"
        };
    }
    
    public CtlPnlInPhieuVM(MainViewModel mainvm)
    {
        MainVM = mainvm;
        Workspace = mainvm.Workspace!;
    }

    public async void DsPhieuIn_LoadByDH()
    {
        try
        {
            var cond = new DbInPhieuCond();
            cond.Offset = PhieuPage * cond.Limit;
            if (!cond.Changed) return;

            await Workspace.DsPhieuIn_Load(cond, true);
            if (cond.Changed)
            {
                PhieuTotal = (cond.Total - 1) / cond.Limit + 1;
                PhieuPage = 1;
                cond.Changed = false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}