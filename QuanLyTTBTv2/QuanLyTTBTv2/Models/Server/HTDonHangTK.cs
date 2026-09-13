using System;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "ht_donhang_tk")]
public class HTDonHangTK
{
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
    public int Id { get; set; }
    
    /// <summary>
    /// fkey: ht_donhang.id
    /// </summary>
    [Column(Name = "donhang_id")]
    public int DonHangId { get; set; }
    
    [Column(Name = "tongphieu")]
    public int TongPhieu { get; set; }
    [Column(Name = "tongtt")]
    public int TongTT { get; set; }
    [Column(Name = "tongme")]
    public int TongMe { get; set; }
    
    [Column(Name = "updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column(Name = "created_at", ServerTime = DateTimeKind.Local, CanUpdate = false)]
    public DateTime CreatedAt { get; set; }
}