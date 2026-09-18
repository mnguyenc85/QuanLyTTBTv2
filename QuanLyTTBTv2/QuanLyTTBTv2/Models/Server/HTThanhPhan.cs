using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "ht_thanhphan")] 
public class HTThanhPhan { 
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)] public int Id { get; set; } 
    [Column(Name = "source_id")] public int SourceId { get; set; } 
    [Column(Name = "local_id")] public int LocalId { get; set; } 
    [Column(Name = "ma")] public string? Ma { get; set; } 
    [Column(Name = "ten")] public string? Ten { get; set; } 
    [Column(Name = "phanloai")] public int PhanLoai { get; set; } 
    [Column(Name = "silo")] public int Silo { get; set; } 
    [Column(Name = "klcongthuc")] public double KlCongThuc { get; set; } 
    [Column(Name = "kltong")] public double KlTong { get; set; } 
    [Column(Name = "klme")] public double KlMe { get; set; }
    
    [Column(IsIgnore =  true)] public int Stt { get; set; }
}
