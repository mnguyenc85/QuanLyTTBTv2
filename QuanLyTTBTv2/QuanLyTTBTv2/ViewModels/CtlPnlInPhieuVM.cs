using QuanLyTTBTv2.ViewModels.Printing;

namespace QuanLyTTBTv2.ViewModels;

public partial class CtlPnlInPhieuVM: ViewModelBase
{
    public WorkspaceVM Workspace { get; }
    public MainViewModel MainVM { get; }
    
    public CtlPnlInPhieuVM()
    {
        // Chỉ để dùng khi designer
        Workspace = new WorkspaceVM
        {
            CurInPhieu = new InPhieuVM()
            {
                Id = -1,
                SoPhieu = "Test000",
                NgayTron = "01/01/0001",
                TgRoiTram = "00:00 AM",
                KhachHang = "Khách hàng",
                DiaChiKH = "Địa chỉ KH",
                CongTrinh = "Dự án - công trình - hạng mục"
            }
        };
    }
    
    public CtlPnlInPhieuVM(MainViewModel mainvm)
    {
        MainVM = mainvm;
        Workspace = mainvm.Workspace!;
    }
}