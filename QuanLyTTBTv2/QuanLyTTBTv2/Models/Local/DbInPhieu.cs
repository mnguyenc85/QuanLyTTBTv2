using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Local;

[Table(Name = "in_phieu")]
public class DbInPhieu
{
    [Column(Name = "id", IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(Name = "phieu_id")]
    public long PhieuId { get; set; }

    [Column(Name = "so_phieu")]
    public string? SoPhieu { get; set; }

    [Column(Name = "ngay_tron")]
    public string? NgayTron { get; set; }

    [Column(Name = "tg_roi_tram")]
    public string? TgRoiTram { get; set; }

    [Column(Name = "khach_hang")]
    public string? KhachHang { get; set; }

    [Column(Name = "dia_chi_kh")]
    public string? DiaChiKH { get; set; }

    [Column(Name = "du_an")]
    public string? DuAn { get; set; }

    [Column(Name = "cong_trinh")]
    public string? CongTrinh { get; set; }

    [Column(Name = "hang_muc")]
    public string? HangMuc { get; set; }

    [Column(Name = "dia_diem")]
    public string? DiaDiem { get; set; }

    [Column(Name = "bsx")]
    public string? Bsx { get; set; }

    [Column(Name = "lai_xe")]
    public string? LaiXe { get; set; }

    [Column(Name = "mac_be_tong")]
    public string? MacBeTong { get; set; }

    [Column(Name = "do_sut")]
    public string? DoSut { get; set; }

    [Column(Name = "cot_lieu_max")]
    public string? CotLieuMax { get; set; }

    [Column(Name = "kep_chi")]
    public string? KepChi { get; set; }

    [Column(Name = "tt_tron")]
    public double TtTron { get; set; }

    [Column(Name = "tt_tich_luy")]
    public double TtTichLuy { get; set; }

    [Column(Name = "kl_tron")]
    public double KlTron { get; set; }
}