using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Huskui.Avalonia.Controls;
using Huskui.Avalonia.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using QuanLyTTBTv2.Models;
using QuanLyTTBTv2.Models.Local;
using QuanLyTTBTv2.ViewModels;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.Views;

public partial class CtlDsPhieu : UserControl
{
    private CtlDsPhieuVM? _vm;
    public bool IsActive { get; set; }

    public event EventHandler<GrowlItem>? PopGrowl; 
    
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

    #region Table: phiếu
    /// <summary>
    /// Kích đổi trang
    /// </summary>
    private void NMPaginator_OnPageClicked(object? sender, int e)
    {
        if (sender == null) return;
        if (sender.Equals(PgPhieu))
        {
            _vm?.ChangePhieuPage(e);
        }
    }

    /// <summary>
    /// Số phiếu / trang
    /// </summary>
    private void CboPhieuTblIPP_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            if (int.TryParse(CboPhieuTblIPP.Text, out int v))
                if (v >= 5 && v <= 50)
                    _vm?.SetTableIPP(v);
    }

    /// <summary>
    /// Số phiếu / trang
    /// </summary>
    private void CboPhieuTblIPP_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // Đặt items per page cho bảng phiếu
        if (int.TryParse(CboPhieuTblIPP.Text, out int v))
            if (v >= 5 && v <= 50)
                _vm?.SetTableIPP(v);
    }

    private bool _isLoadPhieu = false;
    private async void TvwPhieu_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_isLoadPhieu) return;
        _isLoadPhieu = true;
        
        System.Diagnostics.Debug.WriteLine("CtlDsPhieu: chọn phiếu");

        if (_vm != null && IsActive)
        {
            try
            {
                await _vm.Workspace.LoadChiTietPhieu();

                await DsPhieuIn_LoadByCurPhieu();

                await PhieuIn2PhieuCan();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        _isLoadPhieu = false;
    }
    
    /// <summary>
    /// Lấy dữ liệu phiếu in từ phiếu cân
    /// </summary>
    private async void BtFromPhCan_OnClick(object? sender, RoutedEventArgs e)
    {
        await PhieuIn2PhieuCan();
    }

    private async Task PhieuIn2PhieuCan()
    {
        var ws = _vm?.Workspace;
        if (ws == null) return;
     
        BtFromPhCan.IsEnabled = false;
        if (ws.CurPhieuIn != null && ws.CurPhieuIn.Changed)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Lưu phiếu in", "Bạn có muốn lưu thông tin đã sửa?", ButtonEnum.YesNo);
            await box.ShowAsync();
        }
        
        ws.PhieuIn_LoadByPhieuTron();
        BtFromPhCan.IsEnabled = true;
    }
    #endregion
    
    #region Table: mẻ
    public void ReCreateTblMeColumns()
    {
        if (_vm == null) return;
        
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
    
    #region Table: phiếu in
    /// <summary>
    /// Load ds phiếu in từ phiếu cân được chọn
    /// </summary>
    public async Task DsPhieuIn_LoadByCurPhieu()
    {
        if (_vm == null) return;
        try
        {
            var cond = new DbInPhieuCond();
            cond.Offset = 0 * cond.Limit;
            if (!cond.Changed) return;

            await _vm.Workspace.DsPhieuIn_Load(cond);
            if (cond.Changed)
            {
                // PhieuTotal = (cond.Total - 1) / cond.Limit + 1;
                // PhieuPage = 1;
                cond.Changed = false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    private async void TvwPhieuIn_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_isLoadPhieu) return;
        _isLoadPhieu = true;

        await PhieuIn2PhieuIn();

        _isLoadPhieu = false;
    }
    
    private async Task PhieuIn2PhieuIn()
    {
        var ws = _vm?.Workspace;
        if (ws == null) return;
     
        BtFromPhIn.IsEnabled = false;
        if (ws.CurPhieuIn.Changed)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Lưu phiếu in", "Bạn có muốn lưu thông tin đã sửa?", ButtonEnum.YesNo, Icon.Info);
            await box.ShowAsync();
        }
        
        ws.PhieuIn_LoadByPhieuIn();
        BtFromPhIn.IsEnabled = true;
    }
    #endregion

    #region Edit: phiếu in
    private async void BtPhieuInSave_OnClick(object? sender, RoutedEventArgs e)
    {
        var ws = _vm?.Workspace;
        if (ws == null) return;

        await ws.PhieuInSave();

        PopGrowl?.Invoke(this, new GrowlItem()
        {
            Level = GrowlLevel.Information,
            Content = "Phiếu in đã được lưu.",
            IsTabStop = false,
        });
    }
    #endregion
}