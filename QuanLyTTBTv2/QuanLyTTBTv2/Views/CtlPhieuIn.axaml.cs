using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using QuanLyTTBTv2.ViewModels.Visual;

namespace QuanLyTTBTv2.Views;

public partial class CtlPhieuIn : UserControl
{
    public CtlPhieuIn()
    {
        InitializeComponent();
    }

    public void SetFieldVisibilities(PhieuInConfigVM cfg)
    {
        PnlKHDiaChi.IsVisible = cfg.ShowKHDiaChi;
        PnlBTCLMax.IsVisible = cfg.ShowBTCLMax;
        PnlKepChi.IsVisible = cfg.ShowKepChi;
    }
}