using System.Collections.Generic;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "ht_congthuc")] 
public class HTCongThuc 
{ 
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)] public int Id { get; set; } 
    [Column(Name = "source_id")] public int SourceId { get; set; } 
    [Column(Name = "local_id")] public int LocalId { get; set; } 
    [Column(Name = "ma")] public string? Ma { get; set; } 
    [Column(Name = "mac")] public string? Mac { get; set; } 
    [Column(Name = "slump")] public string? Slump { get; set; } 
    [Column(Name = "wcratio")] public double? WcRatio { get; set; } 
    [Column(Name = "kthat")] public double? KThat { get; set; } 
    [Column(Name = "klnuoc")] public double? KlNuoc { get; set; } 
    [Column(Name = "sotp")] public int? SoTp { get; set; }
    
    [Column(IsIgnore = true)] public List<HTThanhPhan> DsThanhPhan { get; set; }
}