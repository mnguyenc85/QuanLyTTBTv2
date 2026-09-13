using FreeSql.DataAnnotations;
using System;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "kd_khachhang")]
[Index("uq_sync", "SourceId,LocalId", IsUnique = true)]
public class KDKhachHang
{
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
    public string? Ma { get; set; }

    [Column(Name = "ten", StringLength = 255)]
    public string? Ten { get; set; }

    [Column(Name = "diachi", StringLength = 255)]
    public string? Diachi { get; set; }

    [Column(Name = "sdt", StringLength = 31)]
    public string? Sdt { get; set; }

    [Column(Name = "email", StringLength = 63)]
    public string? Email { get; set; }

    [Column(Name = "mst", StringLength = 63)]
    public string? Mst { get; set; }

    [Column(Name = "lienhe", StringLength = 63)]
    public string? Lienhe { get; set; }

    [Column(Name = "ghichu", StringLength = 255)]
    public string? Ghichu { get; set; }

    [Column(Name = "updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column(Name = "created_at")]
    public DateTime CreatedAt { get; set; }
}