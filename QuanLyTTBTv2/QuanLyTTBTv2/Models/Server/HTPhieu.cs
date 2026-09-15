using System;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "ht_phieu")]
public class HTPhieu
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

    [Column(Name = "sophieu")]
    public string? Sophieu { get; set; }

    [Column(Name = "donhang_id")]
    public int DonhangId { get; set; }

    [Column(Name = "xe_id")]
    public int XeId { get; set; }

    [Column(Name = "lx_id")]
    public int LxId { get; set; }

    [Column(Name = "congthuc_id")]
    public int CongthucId { get; set; }

    [Column(Name = "thetichdat")]
    public double Thetichdat { get; set; }

    [Column(Name = "kldat")]
    public double Kldat { get; set; }

    [Column(Name = "medat")]
    public int Medat { get; set; }

    [Column(Name = "thetichht")]
    public double Thetichht { get; set; }

    [Column(Name = "klht")]
    public double Klht { get; set; }

    [Column(Name = "meht")]
    public int Meht { get; set; }

    [Column(Name = "tgbd")]
    public DateTime Tgbd { get; set; }

    [Column(Name = "tght")]
    public DateTime Tght { get; set; }

    [Column(Name = "dongia")]
    public double Dongia { get; set; }

    [Column(Name = "don_stt")]
    public int DonStt { get; set; }

    [Column(Name = "don_tt")]
    public double DonTt { get; set; }

    [Column(Name = "don_stt_sim")]
    public int DonSttSim { get; set; }

    [Column(Name = "don_tt_sim")]
    public double DonTtSim { get; set; }

    [Column(Name = "trangthai")]
    public int Trangthai { get; set; }

    [Column(Name = "ghichu")]
    public string? Ghichu { get; set; }

    [Column(Name = "kepchi")]
    public string? Kepchi { get; set; }

    // [Column(Name = "updated_at")]
    // public DateTime UpdatedAt { get; set; }
    //
    // [Column(Name = "created_at")]
    // public DateTime CreatedAt { get; set; }
}

[Table(Name = "ht_phieu")]
public class HTPhieuFkey
{
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
    public int Id { get; set; }

    [Column(Name = "local_id")]
    public int LocalId { get; set; }

    [Column(Name = "source_id")]
    public int SourceId { get; set; }

    [Column(Name = "donhang_id")]
    public int DonhangId { get; set; }

    [Column(Name = "xe_id")]
    public int XeId { get; set; }

    [Column(Name = "lx_id")]
    public int LxId { get; set; }

    [Column(Name = "congthuc_id")]
    public int CongthucId { get; set; }
}