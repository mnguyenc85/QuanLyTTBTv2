using FreeSql.DataAnnotations;
using System;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "pm_strings")]
[Index("uq_sync", "SourceId,LocalId", IsUnique = true)]
public class PMString
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

    [Column(Name = "vanban", StringLength = 255)]
    public string? Vanban { get; set; }

    [Column(Name = "phanloai")]
    public int? Phanloai { get; set; }

    [Column(Name = "sudung")]
    public int? Sudung { get; set; }

    [Column(Name = "updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column(Name = "created_at")]
    public DateTime CreatedAt { get; set; }
}

[Table(Name = "pm_strings")]
public class PMStringLookUp
{
    [Column(Name = "local_id")]
    public int LocalId { get; set; }

    [Column(Name = "source_id")]
    public int SourceId { get; set; }

    [Column(Name = "vanban", StringLength = 255)]
    public string? Vanban { get; set; }
}