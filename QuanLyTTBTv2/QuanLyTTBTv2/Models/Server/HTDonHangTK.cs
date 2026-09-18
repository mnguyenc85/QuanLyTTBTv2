using System;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "ht_donhang_tk")]
public class HTDonHangTK
{
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
    public long Id { get; set; }
    
    /// <summary>
    /// fkey: ht_donhang.id
    /// </summary>
    [Column(Name = "donhang_id")]
    public long DonHangId { get; set; }
    
    [Column(Name = "tongphieu")]
    public int TongPhieu { get; set; }
    [Column(Name = "tongtt")]
    public double TongTT { get; set; }
    [Column(Name = "tongme")]
    public int TongMe { get; set; }
    [Column(Name = "tongkl")]
    public double TongKL { get; set; }
    [Column(Name =  "tght")]
    public DateTime Tght  { get; set; }
    
    [Column(Name = "updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column(Name = "created_at", ServerTime = DateTimeKind.Local, CanUpdate = false)]
    public DateTime CreatedAt { get; set; }
}