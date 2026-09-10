using System;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "sources")]
public class SrvFactory
{
    [Column(Name = "id", IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; } = -1;

    [Column(Name = "created_at", CanUpdate = false, ServerTime = DateTimeKind.Local, InsertValueSql = "now()")]
    public DateTime CreatedAt { get; set; }
    
    [Column(Name = "ten")]
    public string? Ten { get; set; }
    [Column(Name = "ma1")]
    public string? Ma1 { get; set; }
    [Column(Name = "ma2")]
    public string? Ma2 { get; set; }

    [Column(Name = "flags")]
    public string? Flags { get; set; }
}