using System;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Local;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.ViewModels.Printing;

public partial class InPhieuVM: ViewModelBase
{
    public int Stt { get; set; }
    
    public long Id { get; set; }
    
    public long PhieuId { get; set; }

    [ObservableProperty] private string? _soPhieu;
    [ObservableProperty] private string? _soPhieuIn;
    
    [ObservableProperty] private string? _ngayTron;
    [ObservableProperty] private string? _tgRoiTram;
    
    [ObservableProperty] private string? _khachHang;
    [ObservableProperty] private string? _diaChiKH;
    
    [ObservableProperty] private string? _congTrinh;
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
    
    public InPhieuVM() { }

    public InPhieuVM(DbInPhieu o)
    {
        FromDO(o);
    }
    
    public void FromDO(DbInPhieu o)
    {
        Id = o.Id;
        PhieuId = o.PhieuId;

        SoPhieu = o.SoPhieu;
        SoPhieuIn = o.SoPhieuIn;
        
        NgayTron = o.NgayTron;
        TgRoiTram = o.TgRoiTram;

        KhachHang = o.KhachHang;
        DiaChiKH = o.DiaChiKH;

        CongTrinh = o.CongTrinh;
        DiaDiem = o.DiaDiem;

        Bsx = o.Bsx;
        LaiXe = o.LaiXe;

        MacBeTong = o.MacBeTong;
        DoSut = o.DoSut;
        CotLieuMax = o.CotLieuMax;
        KepChi = o.KepChi;

        TtTron = o.TtTron;
        TtTichLuy = o.TtTichLuy;
        KlTron = o.KlTron;
    }


    public void ToDO(DbInPhieu o)
    {
        o.Id = Id;
        o.PhieuId = PhieuId;

        o.SoPhieu = SoPhieu;
        o.SoPhieuIn = SoPhieuIn;
        
        o.NgayTron = NgayTron;
        o.TgRoiTram = TgRoiTram;

        o.KhachHang = KhachHang;
        o.DiaChiKH = DiaChiKH;

        o.CongTrinh = CongTrinh;
        o.DiaDiem = DiaDiem;

        o.Bsx = Bsx;
        o.LaiXe = LaiXe;

        o.MacBeTong = MacBeTong;
        o.DoSut = DoSut;
        o.CotLieuMax = CotLieuMax;
        o.KepChi = KepChi;

        o.TtTron = TtTron;
        o.TtTichLuy = TtTichLuy;
        o.KlTron = KlTron;
    }

    public void Clear()
    {
        Id = -1;
        PhieuId = -1;

        SoPhieu = "";
        SoPhieuIn = "";
        
        NgayTron = "";
        TgRoiTram = "";

        KhachHang = "";
        DiaChiKH = "";

        CongTrinh = "";
        DiaDiem = "";

        Bsx = "";
        LaiXe = "";

        MacBeTong = "";
        DoSut = "";
        CotLieuMax = "";
        KepChi = "";

        TtTron = 0;
        TtTichLuy = 0;
        KlTron = 0;
    }

    public void CopyFrom(HTDonHangVM? dh, HTPhieuVM ph)
    {
        Id = -1;
        PhieuId = ph.Id;

        SoPhieu = ph.Sophieu;
        SoPhieuIn = ph.SttDon.ToString();
        
        NgayTron = ph.Tgkt.ToString("MM/dd/yyyy HH:mm:ss");
        TgRoiTram = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        if (dh != null)
        {
            KhachHang = dh.KhachHang;
            DiaChiKH = dh.DiaChiKH;

            CongTrinh = dh.DuAn;
            DiaDiem = dh.DADiaChi;
        }
        else
        {
            KhachHang = "";
            DiaChiKH = "";
            CongTrinh = "";
            DiaDiem = "";
        }

        Bsx = ph.Bsx;
        LaiXe = ph.Lx;

        if (ph.CongThuc != null)
        {
            MacBeTong = ph.CongThuc.Mac;
            DoSut = ph.CongThuc.Slump;
            CotLieuMax = ph.CongThuc.KichThuocHat.ToString();
        }
        else
        {
            MacBeTong = "";
            DoSut = "";
            CotLieuMax = "";
        }

        KepChi = "";

        TtTron = ph.Ttht;
        TtTichLuy = ph.TtDon;
        KlTron = ph.Klht;    
    }

    public DbInPhieu CreateDO()
    {
        var o = new DbInPhieu();
        ToDO(o);
        return o;
    }
}
