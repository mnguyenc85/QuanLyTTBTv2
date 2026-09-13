using System;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "ht_donhang")]
public class HTDonHang
{
    #region Fields
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
    public int Id { get; set; }

    [Column(Name = "local_id")]
    public int LocalId { get; set; }

    [Column(Name = "source_id")]
    public int SourceId { get; set; }

    [Column(Name = "local_updated_at")]
    public DateTime? LocalUpdatedAt { get; set; }

    [Column(Name = "local_created_at")]
    public DateTime? LocalCreatedAt { get; set; }

    [Column(Name = "ma", StringLength = 63)]
    public string? Ma { get; set; } = "";

    [Column(Name = "kh_id")]
    public int KhId { get; set; }

    [Column(Name = "da_id")]
    public int DaId { get; set; }

    [Column(Name = "ct_id")]
    public int CtId { get; set; }

    [Column(Name = "hm_id")]
    public int HmId { get; set; }

    [Column(Name = "diachi_id")]
    public int DiachiId { get; set; }

    [Column(Name = "thetichdh")]
    public double Thetichdh { get; set; }

    [Column(Name = "ghichu", StringLength = 255)]
    public string? Ghichu { get; set; } = "";

    [Column(Name = "trangthai")]
    public int Trangthai { get; set; }

    [Column(Name = "thetichht")]
    public double Thetichht { get; set; }

    [Column(Name = "klht")]
    public double Klht { get; set; }

    [Column(Name = "meht")]
    public int Meht { get; set; }

    [Column(Name = "tght")]
    public DateTime? Tght { get; set; }

    [Column(Name = "updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column(Name = "created_at")]
    public DateTime CreatedAt { get; set; }
    #endregion

    [Column(IsIgnore = true)]
    public KDKhachHang? KhachHang { get; set; }
    [Column(IsIgnore = true)]
    public string? DuAn { get; set; }
    [Column(IsIgnore = true)]
    public string? CongTrinh { get; set; }
    [Column(IsIgnore = true)]
    public string? HangMuc { get; set; }
    [Column(IsIgnore = true)]
    public string? DiaChi { get; set; }
}