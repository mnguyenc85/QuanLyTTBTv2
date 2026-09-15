using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using QuanLyTTBTv2.ViewModels;

namespace QuanLyTTBTv2.Views;

public partial class WndDataViewer : Window
{
    public WndDataViewerVM _vm { get; private set; } = new();
    
    public WndDataViewer()
    {
        InitializeComponent();

        DataContext = _vm;
    }

    public void SetWorkspace(WorkspaceVM workspace)
    {
        _vm.Workspace = workspace;
    }
}