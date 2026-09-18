using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using QuanLyTTBTv2.Models;
using QuanLyTTBTv2.ViewModels;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.Views;

public partial class CtlDsPhieu : UserControl
{
    private CtlDsPhieuVM? _vm;
    
    public CtlDsPhieu()
    {
        InitializeComponent();
        
        CboPhieuTblIPP.Items.Add("5");
        CboPhieuTblIPP.Items.Add("10");
        CboPhieuTblIPP.Items.Add("15");
        CboPhieuTblIPP.Items.Add("20");
        CboPhieuTblIPP.Items.Add("25");
        CboPhieuTblIPP.SelectedIndex = 2;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        _vm = DataContext as CtlDsPhieuVM;
    }
    
    private void NMPaginator_OnPageClicked(object? sender, int e)
    {
        if (sender == null) return;
        if (sender.Equals(PgPhieu))
        {
            _vm?.ChangePhieuPage(e);
        }
    }
    
    private void CboPhieuTblIPP_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            if (int.TryParse(CboPhieuTblIPP.Text, out int v))
                if (v >= 5 && v <= 50)
                    _vm?.SetTableIPP(v);
    }

    private void CboPhieuTblIPP_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // Đặt items per page cho bảng phiếu
        if (int.TryParse(CboPhieuTblIPP.Text, out int v))
            if (v >= 5 && v <= 50)
                _vm?.SetTableIPP(v);
    }
    
    private void TvwPhieu_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_vm == null || _vm.Workspace == null) return;
        
        _vm.Workspace.LoadChiTietPhieu();
    }
    
    #region Table mẻ

    public void ReCreateTblMeColumns()
    {
        if (_vm == null || _vm.Workspace == null) return;
        
        RemoveTPColumns();
        CreateTPColumns([.. _vm.Workspace.DsMaThanhPhan]);
    }
    
    private void RemoveTPColumns()
    {
        int totalColumns = TvwMe.Columns.Count;

        // Luôn giữ 3 cột đầu và 2 cột cuối
        int tpStartIndex = 3;
        int tpEndIndex = totalColumns - 2;

        // Xóa từ cuối về đầu để không bị thay đổi index
        for (int i = tpEndIndex - 1; i >= tpStartIndex; i--)
        {
            TvwMe.Columns.RemoveAt(i);
        }
    }

    private void CreateTPColumns(List<CHThanhPhan> headers)
    {
        for (int i = 0; i < headers.Count; i++)
        {
            int tpIndex = i;
            var column = new TableViewColumn
            {
                Header = headers[i].GetHeader(),
                Width = new GridLength(108),
                CellTemplate = new FuncDataTemplate<HTMeVM>((item, _) =>
                {
                    var textBlock = new TextBlock
                    {
                        // Text = item.TPs[tpIndex],
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };

                    textBlock.Bind(
                        TextBlock.TextProperty,
                        new Binding($"TPs[{tpIndex}]")
                    );

                    return textBlock;
                })                    
            };
            TvwMe.Columns.Insert(3 + i, column);
        }
    }
    #endregion
}