using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "ht_congthuc_thanhphan")]
public class HTCongThucThanhPhan
{
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
    public int Id { get; set; }

    [Column(Name = "source_id")]
    public int SourceId { get; set; }

    [Column(Name = "local_id")]
    public int LocalId { get; set; }

    [Column(Name = "ct_id")]
    public int CtId { get; set; }

    [Column(Name = "tp_id")]
    public int TpId { get; set; }
}