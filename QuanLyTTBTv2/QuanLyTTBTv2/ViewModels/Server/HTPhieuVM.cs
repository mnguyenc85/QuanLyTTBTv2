using System;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.ViewModels.Server;

public partial class HTPhieuVM: ViewModelBase
{
    public long Id { get; set; }
    public long LocalId { get; set; }
    public long CtId { get; set; }
    
    [ObservableProperty] private int _stt;
    [ObservableProperty] private string? _sophieu;
    [ObservableProperty] private int _sttDon;
    
    [ObservableProperty] private string? _bsx;
    [ObservableProperty] private string? _lx;
    
    [ObservableProperty] private double _ttdat;
    [ObservableProperty] private double _kldat;
    [ObservableProperty] private int _medat;
    [ObservableProperty] private double _ttht;
    [ObservableProperty] private double _klht;
    [ObservableProperty] private int _meht;
    
    [ObservableProperty] private double _ttDon;
    [ObservableProperty] private int _trangthai;
    
    [ObservableProperty] private string? _ghiChu;
    [ObservableProperty] private string? _kepChi;
    
    [ObservableProperty] private DateTime _tgbd;
    [ObservableProperty] private DateTime _tgkt;

    public HTCongThuc? CongThuc { get; set; }
    
    public HTPhieuVM()
    {
        
    }

    public HTPhieuVM(HTPhieu ph)
    {
        Id = ph.Id;
        LocalId = ph.LocalId;
        CtId = ph.CongthucId;
        
        Sophieu = ph.Sophieu;
        SttDon = ph.DonStt;

        Bsx = ph.Bsx;
        Lx = ph.LaiXe;

        Ttdat = ph.Thetichdat;
        Ttht = Math.Round(ph.Thetichht, 2);
        Kldat  = ph.Kldat;
        Klht = ph.Klht;
        Medat = ph.Medat;
        Meht = ph.Meht;

        TtDon = Math.Round(ph.DonTt, 2);
        
        Trangthai = ph.Trangthai;
        GhiChu = ph.Ghichu;
        KepChi = ph.Kepchi;

        Tgbd = ph.CreatedAt;
        Tgkt = ph.UpdatedAt;
        
        CongThuc = ph.CongThuc;
    }
}