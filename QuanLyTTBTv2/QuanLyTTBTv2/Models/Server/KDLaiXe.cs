using System;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "kd_laixe")]
public class KDLaiXe
{
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
    public int Id { get; set; }

    [Column(Name = "local_id")]
    public int LocalId { get; set; }

    [Column(Name = "source_id")]
    public int SourceId { get; set; }

    // [Column(Name = "local_updated_at")]
    // public DateTime LocalUpdatedAt { get; set; }
    //
    // [Column(Name = "local_created_at")]
    // public DateTime LocalCreatedAt { get; set; }

    [Column(Name = "ten")]
    public string? Ten { get; set; }

    [Column(Name = "sdt")]
    public string? Sdt { get; set; }

    // [Column(Name = "updated_at")]
    // public DateTime UpdatedAt { get; set; }
    //
    // [Column(Name = "created_at")]
    // public DateTime CreatedAt { get; set; }
}