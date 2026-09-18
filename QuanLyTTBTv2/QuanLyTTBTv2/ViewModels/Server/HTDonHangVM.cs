using System;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.ViewModels.Server;

public partial class HTDonHangVM: ViewModelBase
{
    public long Id { get; set; }
    public long LocalId { get; set; }
    
    [ObservableProperty] private int _stt;
    [ObservableProperty] private string? _ma;
    
    [ObservableProperty] private string? _khachHang;
    [ObservableProperty] private string? _duAn;

    [ObservableProperty] private double _kl;

    [ObservableProperty] private DateTime _tgbd;
    [ObservableProperty] private DateTime _tgkt;

    public HTDonHangTKVM TK { get; set; } = new();
    
    public HTDonHangVM()
    {
        
    }

    public HTDonHangVM(HTDonHang o)
    {
        FromDBO(o);
    }

    public void FromDBO(HTDonHang o)
    {
        Id = o.Id;
        LocalId = o.LocalId;
        Ma = o.Ma;
        KhachHang = o.KhachHang?.Ten;
        // DuAn = $"{dh.DaId} - {dh.CtId} - {dh.HmId}";
        DuAn = $"{o.DuAn} - {o.CongTrinh} - {o.HangMuc}";
        Kl = o.Klht;
        Tgbd = o.CreatedAt;
        Tgkt = o.Tght ?? DateTime.MinValue;
    }
}