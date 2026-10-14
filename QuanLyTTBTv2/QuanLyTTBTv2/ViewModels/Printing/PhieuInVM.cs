using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Local;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.ViewModels.Printing;

public partial class PhieuInVM: ViewModelBase
{
    public int Stt { get; set; }

    private long _id;
    public long Id { 
        get { return _id; } 
        set { _id = value;
            TieuDe = Id > 0 ? "Sửa phiếu in" : "Phiếu in mới";
        }
    }
    
    public long PhieuId { get; set; }

    public string? DonMa { get; set; }

    [ObservableProperty] private string? _soPhieu;
    [ObservableProperty] private string? _soPhieuIn;
    
    [ObservableProperty] private string? _ngayTron;
    [ObservableProperty] private string? _ngayRoiTram;
    [ObservableProperty] private string? _gioRoiTram;
    
    [ObservableProperty] private string? _kHTen;
    [ObservableProperty] private string? _kHDiaChi;
    [ObservableProperty] private string? _kHSdt;

    [ObservableProperty] private string? _congTrinh;
    [ObservableProperty] private string? _diaDiem;
    
    [ObservableProperty] private string? _bsx;
    [ObservableProperty] private string? _laiXe;
    
    [ObservableProperty] private string? _beTongMa;
    [ObservableProperty] private string? _beTongMac;
    [ObservableProperty] private string? _beTongCuongDo;
    [ObservableProperty] private string? _beTongDoSut;
    [ObservableProperty] private string? _beTongCotLieuMax;
    
    [ObservableProperty] private string? _kepChi;
    
    [ObservableProperty] private double _ttTron;
    [ObservableProperty] private double _ttTichLuy;
    [ObservableProperty] private double _klTron;

    [ObservableProperty] private double _donGia;
    [ObservableProperty] private double _tongTien;

    public DateTime TGBD { get; set; }
    public DateTime TGKT { get; set; }

    /// <summary>
    /// True khi các property thay đổi
    /// False khi copy từ DbInPhieu hoặc PhieuInVM khác
    /// </summary>
    public bool Changed { get; set; }
    [ObservableProperty] private string? _tieuDe;
    
    public PhieuInVM() { }

    public PhieuInVM(DbInPhieu o)
    {
        FromDO(o);
    }

    #region Track changed
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName != nameof(Changed) && e.PropertyName != nameof(TieuDe))
        {
            Changed = true;
        }
        if (e.PropertyName == nameof(TtTron) || (e.PropertyName == nameof(DonGia)))
        {
            TongTien = TtTron * DonGia;
        }
    }
    #endregion
    
    public void FromDO(DbInPhieu o)
    {
        Id = o.Id;
        PhieuId = o.PhieuId;

        DonMa = o.DonMa;

        SoPhieu = o.PhieuMa;
        SoPhieuIn = o.PhieuSttDon;
        
        NgayTron = o.NgayTron;
        NgayRoiTram = o.NgayRoiTram;
        GioRoiTram = o.GioRoiTram;

        KHTen = o.KHTen;
        KHDiaChi = o.KHDiaChi;
        KHSdt = o.KHSdt;

        CongTrinh = o.CongTrinh;
        DiaDiem = o.DiaDiem;

        Bsx = o.Bsx;
        LaiXe = o.LaiXe;

        BeTongMa = o.BeTongMa;
        BeTongMac = o.BeTongMac;
        BeTongDoSut = o.BeTongDoSut;
        BeTongCuongDo = o.BeTongCuongDo;
        BeTongCotLieuMax = o.BeTongCotLieuMax;

        KepChi = o.KepChi;

        TtTron = o.TtTron;
        TtTichLuy = o.TtTichLuy;
        KlTron = o.KlTron;

        DonGia = o.DonGia;
        TongTien = o.TongTien;

        TGBD = o.TGBD;
        TGKT = o.TGKT;

        Changed = false;
    }

    public void ToDO(DbInPhieu o)
    {
        o.Id = Id;
        o.PhieuId = PhieuId;

        o.DonMa = DonMa;

        o.PhieuMa = SoPhieu;
        o.PhieuSttDon = SoPhieuIn;
        
        o.NgayTron = NgayTron;
        o.NgayRoiTram = NgayRoiTram;
        o.GioRoiTram = GioRoiTram;

        o.KHTen = KHTen;
        o.KHDiaChi = KHDiaChi;
        o.KHSdt = KHSdt;

        o.CongTrinh = CongTrinh;
        o.DiaDiem = DiaDiem;

        o.Bsx = Bsx;
        o.LaiXe = LaiXe;

        o.BeTongMa = BeTongMa;
        o.BeTongMac = BeTongMac;
        o.BeTongDoSut = BeTongDoSut;
        o.BeTongCuongDo = BeTongCuongDo;
        o.BeTongCotLieuMax = BeTongCotLieuMax;

        o.KepChi = KepChi;

        o.TtTron = TtTron;
        o.TtTichLuy = TtTichLuy;
        o.KlTron = KlTron;

        o.TongTien = TongTien;
        o.DonGia = DonGia;

        o.TGBD = TGBD;
        o.TGKT = TGKT;
    }

    public void Clear()
    {
        Id = -1;
        PhieuId = -1;

        DonMa = "";

        SoPhieu = "";
        SoPhieuIn = "";
        
        NgayTron = "";
        NgayRoiTram = "";
        GioRoiTram = "";

        KHTen = ""; KHDiaChi = ""; KHSdt = "";

        CongTrinh = "";
        DiaDiem = "";

        Bsx = "";
        LaiXe = "";

        BeTongMa = "";
        BeTongMac = "";
        BeTongCotLieuMax = "";
        BeTongDoSut = "";
        BeTongCuongDo = "";

        KepChi = "";

        TtTron = 0;
        TtTichLuy = 0;
        KlTron = 0;

        DonGia = 0;
        TongTien = 0;

        TieuDe = "(Không)";
        Changed = false;
    }

    public void CopyFrom(HTDonHangVM? dh, HTPhieuVM ph)
    {
        Id = -1;
        PhieuId = ph.Id;

        DonMa = dh?.Ma;

        SoPhieu = ph.Sophieu;
        SoPhieuIn = ph.SttDon.ToString();
        
        NgayTron = ph.Tgkt.ToString("MM/dd/yyyy HH:mm:ss");
        NgayRoiTram = DateTime.Now.ToString("dd/MM/yyyy");
        GioRoiTram = DateTime.Now.ToString("HH:mm:ss");

        if (dh != null)
        {
            KHTen = dh.KhachHang;
            KHDiaChi = dh.KHDiaChi;
            KHSdt = dh.KHSdt;

            CongTrinh = dh.DuAn;
            DiaDiem = dh.DADiaChi;
        }
        else
        {
            KHTen = "";
            KHDiaChi = "";
            KHSdt = "";
            CongTrinh = "";
            DiaDiem = "";
        }

        Bsx = ph.Bsx;
        LaiXe = ph.Lx;

        if (ph.CongThuc != null)
        {
            BeTongMa = ph.CongThuc.Ma;
            BeTongMac = ph.CongThuc.Mac;
            BeTongDoSut = ph.CongThuc.Slump;
            BeTongCotLieuMax = ph.CongThuc.KichThuocHat.ToString();
            //BeTongCuongDo = ?;
        }
        else
        {
            BeTongMa = "";
            BeTongMac = "";
            BeTongCotLieuMax = "";
            BeTongDoSut = "";
            BeTongCuongDo = "";
        }

        KepChi = "";

        TtTron = ph.Ttht;
        TtTichLuy = ph.TtDon;
        KlTron = ph.Klht;

        DonGia = 0;
        TongTien = 0;

        TGBD = ph.Tgbd;
        TGKT = ph.Tgkt;

        Changed = false;
    }

    public void CopyFrom(PhieuInVM o)
    {
        Id = o.Id;
        PhieuId = o.PhieuId;

        DonMa = o.DonMa;

        SoPhieu = o.SoPhieu;
        SoPhieuIn = o.SoPhieuIn;
        
        NgayTron = o.NgayTron;
        NgayRoiTram = o.NgayRoiTram;
        GioRoiTram = o.GioRoiTram;

        KHTen = o.KHTen;
        KHDiaChi = o.KHDiaChi;
        KHSdt = o.KHSdt;

        CongTrinh = o.CongTrinh;
        DiaDiem = o.DiaDiem;

        Bsx = o.Bsx;
        LaiXe = o.LaiXe;

        BeTongMa = o.BeTongMa;
        BeTongMac = o.BeTongMac;
        BeTongDoSut = o.BeTongDoSut;
        BeTongCuongDo = o.BeTongCuongDo;
        BeTongCotLieuMax = o.BeTongCotLieuMax;

        KepChi = o.KepChi;

        TtTron = o.TtTron;
        TtTichLuy = o.TtTichLuy;
        KlTron = o.KlTron;

        DonGia = o.DonGia;
        TongTien = o.TongTien;

        TGBD = TGBD;
        TGKT = TGKT;

        Changed = false;
    }
    
    public DbInPhieu CreateDO()
    {
        var o = new DbInPhieu();
        ToDO(o);
        return o;
    }
}
