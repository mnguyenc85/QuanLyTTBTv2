using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.ViewModels.Printing;
using QuanLyTTBTv2.ViewModels.Server;

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
                SoPhieu = "Test000"
            }
        };
    }
    
    public CtlPnlInPhieuVM(MainViewModel mainvm)
    {
        MainVM = mainvm;
        Workspace = mainvm.Workspace!;
    }
}