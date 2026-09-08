using System;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models;

public abstract class DbBaseDto
{
    [Column(Name = "id", IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; } = -1;

    [Column(Name = "updated_at", ServerTime = DateTimeKind.Local, InsertValueSql = "now()")]
    public DateTime UpdatedAt { get; set; }

    [Column(Name = "created_at", CanUpdate = false, ServerTime = DateTimeKind.Local, InsertValueSql = "now()")]
    public DateTime CreatedAt { get; set; }
}
