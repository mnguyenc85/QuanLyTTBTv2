using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuanLyTTBTv2.ViewModels.Visual
{
    public partial class PhieuInConfigVM: ViewModelBase
    {
        [ObservableProperty] private bool _showPhieuIn = true;

        //[ObservableProperty] private bool _showNgayTron = true;
        [ObservableProperty] private bool _showNgayXuat = true;
        [ObservableProperty] private bool _showGioXuat = true;

        //[ObservableProperty] private bool _showKHTen = true;
        [ObservableProperty] private bool _showKHDiaChi = false;

        //[ObservableProperty] private bool _showCongTrinh = true;
        [ObservableProperty] private bool _showCTHangMuc = false;
        [ObservableProperty] private bool _showCTDiaChi = false;

        [ObservableProperty] private bool _showBTTen = true;
        [ObservableProperty] private bool _showBTMac = true;
        [ObservableProperty] private bool _showBTCuongDo = false;
        
        [ObservableProperty] private bool _showKepChi = false;
    }
}
