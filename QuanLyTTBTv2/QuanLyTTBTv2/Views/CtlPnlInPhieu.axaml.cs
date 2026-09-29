using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using QuanLyTTBTv2.ViewModels;

namespace QuanLyTTBTv2.Views;

public partial class CtlPnlInPhieu : UserControl
{
    private CtlPnlInPhieuVM? _vm;
    
    public CtlPnlInPhieu()
    {
        InitializeComponent();
    }
    
    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        _vm = DataContext as CtlPnlInPhieuVM;
    }
    
    private void NMPaginator_OnPageClicked(object? sender, int e)
    {
    }
    
    private void CboPhieuTblIPP_OnKeyDown(object? sender, KeyEventArgs e)
    {
        // if (e.Key == Key.Enter)
        //     if (int.TryParse(CboInPhieuTblIPP.Text, out int v))
        //         if (v >= 5 && v <= 50)
        //             _vm?.SetTableIPP(v);
    }

    private void CboPhieuTblIPP_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // Đặt items per page cho bảng phiếu
        // if (int.TryParse(CboInPhieuTblIPP.Text, out int v))
        //     if (v >= 5 && v <= 50)
        //         _vm?.SetTableIPP(v);
    }
    
    private async void TvwPhieu_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // if (_vm == null || _vm.Workspace == null || !IsActive) return;
        //
        // try
        // {
        //     await _vm.Workspace.LoadChiTietPhieu();
        // }
        // catch (Exception ex)
        // {
        //     System.Diagnostics.Debug.WriteLine(ex.Message);
        // }
    }
}