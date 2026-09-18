using Avalonia.Controls;
using Avalonia.Interactivity;
using QuanLyTTBTv2.ViewModels;

namespace QuanLyTTBTv2.Views;

public partial class CtlDsDonHang : UserControl
{
    private CtlDsDonHangVM? _vm;
    
    public CtlDsDonHang()
    {
        InitializeComponent();
        
        CboDHTblIPP.Items.Add("10");
        CboDHTblIPP.Items.Add("15");
        CboDHTblIPP.Items.Add("20");
        CboDHTblIPP.Items.Add("23");
        CboDHTblIPP.Items.Add("25");
        CboDHTblIPP.SelectedIndex = 2;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        _vm = DataContext as CtlDsDonHangVM;
    }
    
    private void NMPaginator_OnPageClicked(object? sender, int e)
    {
        if (sender == null) return;
        if (sender.Equals(PgDonHang))
        {
            _vm?.ChangeDonHangPage(e);
        }
    }
    
    private void CboDHTblIPP_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // Đặt items per page cho bảng đơn hàng
        if (int.TryParse(CboDHTblIPP.Text, out int v))
            if (v >= 5 && v < 50)
                _vm?.SetTableIPP(v);
    }
}