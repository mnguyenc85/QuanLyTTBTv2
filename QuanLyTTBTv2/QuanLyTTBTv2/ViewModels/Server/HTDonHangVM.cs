using System;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.ViewModels.Server;

public partial class HTDonHangVM: ViewModelBase
{
    public long Id { get; set; }
    
    [ObservableProperty]
    private int _stt;

    [ObservableProperty]
    private string? _khachHang;
    
    [ObservableProperty]
    private string? _duAn;

    [ObservableProperty]
    private double _kl;

    [ObservableProperty]
    private DateTime _tgbd;

    [ObservableProperty]
    private DateTime _tgkt;

    public HTDonHangVM()
    {
        
    }

    public HTDonHangVM(HTDonHang dh)
    {
        Id = dh.Id;
        KhachHang = dh.KhachHang?.Ten;
        // DuAn = $"{dh.DaId} - {dh.CtId} - {dh.HmId}";
        DuAn = $"{dh.DuAn} - {dh.CongTrinh} - {dh.HangMuc}";
        Kl = dh.Klht;
        Tgbd = dh.CreatedAt;
        Tgkt = dh.Tght ?? DateTime.MinValue;
    }
}