using System;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Local;

[Table(Name = "in_phieu")]
public class DbInPhieu
{
    [Column(Name = "id", IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(Name = "don_ma")]
    public string? DonMa { get; set; }

    [Column(Name = "phieu_id")] public long PhieuId { get; set; }
    [Column(Name = "phieu_so")] public string? PhieuMa { get; set; }
    [Column(Name = "phieu_sodon")] public string? PhieuSttDon { get; set; }    

    [Column(Name = "ngay_tron")]
    public string? NgayTron { get; set; }

    [Column(Name = "ngay_roi_tram")]
    public string? NgayRoiTram { get; set; }
    [Column(Name = "gio_roi_tram")]
    public string? GioRoiTram { get; set; }

    [Column(Name = "kh_ten")] public string? KHTen { get; set; }
    [Column(Name = "kh_diachi")] public string? KHDiaChi { get; set; }
    [Column(Name = "kh_sdt")] public string? KHSdt { get; set; }


    [Column(Name = "cong_trinh")]
    public string? CongTrinh { get; set; }

    [Column(Name = "dia_diem")]
    public string? DiaDiem { get; set; }

    [Column(Name = "bsx")]
    public string? Bsx { get; set; }

    [Column(Name = "lai_xe")]
    public string? LaiXe { get; set; }

    [Column(Name = "bt_ma")]
    public string? BeTongMa { get; set; }
    [Column(Name = "bt_mac")]
    public string? BeTongMac { get; set; }

    [Column(Name = "bt_dosut")]
    public string? BeTongDoSut { get; set; }
    [Column(Name = "bt_cuongdo")]
    public string? BeTongCuongDo { get; set; }

    [Column(Name = "bt_clmax")]
    public string? BeTongCotLieuMax { get; set; }

    [Column(Name = "kep_chi")]
    public string? KepChi { get; set; }

    [Column(Name = "tt_tron")]
    public double TtTron { get; set; }

    [Column(Name = "tt_tich_luy")]
    public double TtTichLuy { get; set; }

    [Column(Name = "kl_tron")]
    public double KlTron { get; set; }

    [Column(Name = "don_gia")]
    public double DonGia { get; set; }
    [Column(Name = "tong_tien")]
    public double TongTien { get; set; }

    [Column(Name = "tgbd")]
    public DateTime TGBD { get; set; }
    [Column(Name = "tgkt")]
    public DateTime TGKT { get; set; }

    
    [Column(Name = "flags")]
    public int Flags { get; set; }
    
    [Column(Name = "updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [Column(Name = "created_at")]
    public DateTime CreatedAt { get; set; }
}