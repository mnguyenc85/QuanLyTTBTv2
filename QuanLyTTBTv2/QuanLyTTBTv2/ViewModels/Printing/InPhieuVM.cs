using CommunityToolkit.Mvvm.ComponentModel;

namespace QuanLyTTBTv2.ViewModels.Printing;

public partial class InPhieuVM: ViewModelBase
{
    public long Id { get; set; }
    
    public long PhieuId { get; set; }

    [ObservableProperty] private string? _soPhieu;
    [ObservableProperty] private string? _ngayTron;
    [ObservableProperty] private string? _tgRoiTram;
    
    [ObservableProperty] private string? _khachHang;
    [ObservableProperty] private string? _diaChiKH;
    
    [ObservableProperty] private string? _duAn;
    [ObservableProperty] private string? _congTrinh;
    [ObservableProperty] private string? _hangMuc;
    [ObservableProperty] private string? _diaDiem;
    
    [ObservableProperty] private string? _bsx;
    [ObservableProperty] private string? _laiXe;
    
    [ObservableProperty] private string? _macBeTong;
    [ObservableProperty] private string? _doSut;
    [ObservableProperty] private string? _cotLieuMax;
    [ObservableProperty] private string? _kepChi;
    
    [ObservableProperty] private double _ttTron;
    [ObservableProperty] private double _ttTichLuy;
    [ObservableProperty] private double _klTron;
}