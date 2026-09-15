using CommunityToolkit.Mvvm.ComponentModel;

namespace QuanLyTTBTv2.ViewModels;

public partial class WndDataViewerVM: ViewModelBase
{
    [ObservableProperty] private WorkspaceVM? _workspace;
}