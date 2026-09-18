using System;
using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Server;

[Table(Name = "ht_me")]
public class HtMe
{
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
    public int Id { get; set; }

    [Column(Name = "local_id")]
    public int LocalId { get; set; }

    [Column(Name = "source_id")]
    public int SourceId { get; set; }
    
    [Column(Name = "phieu_id")]
    public int PhieuId { get; set; }

    [Column(Name = "stt")]
    public int Stt { get; set; }

    [Column(Name = "m3tron")]
    public double M3Tron { get; set; }

    [Column(Name = "cl1")]
    public double Cl1 { get; set; }

    [Column(Name = "cl2")]
    public double Cl2 { get; set; }

    [Column(Name = "cl3")]
    public double Cl3 { get; set; }

    [Column(Name = "cl4")]
    public double Cl4 { get; set; }

    [Column(Name = "cl5")]
    public double Cl5 { get; set; }

    [Column(Name = "cl6")]
    public double Cl6 { get; set; }

    [Column(Name = "xi1")]
    public double Xi1 { get; set; }

    [Column(Name = "xi2")]
    public double Xi2 { get; set; }

    [Column(Name = "xi3")]
    public double Xi3 { get; set; }

    [Column(Name = "xi4")]
    public double Xi4 { get; set; }

    [Column(Name = "xi5")]
    public double Xi5 { get; set; }

    [Column(Name = "pg1")]
    public double Pg1 { get; set; }

    [Column(Name = "pg2")]
    public double Pg2 { get; set; }

    [Column(Name = "pg3")]
    public double Pg3 { get; set; }

    [Column(Name = "pg4")]
    public double Pg4 { get; set; }

    [Column(Name = "pg5")]
    public double Pg5 { get; set; }

    [Column(Name = "pg6")]
    public double Pg6 { get; set; }

    [Column(Name = "pg7")]
    public double Pg7 { get; set; }

    [Column(Name = "pg8")]
    public double Pg8 { get; set; }

    [Column(Name = "hu1")]
    public double Hu1 { get; set; }

    [Column(Name = "hu2")]
    public double Hu2 { get; set; }

    [Column(Name = "hu3")]
    public double Hu3 { get; set; }

    [Column(Name = "hu4")]
    public double Hu4 { get; set; }

    [Column(Name = "hu5")]
    public double Hu5 { get; set; }

    [Column(Name = "hu6")]
    public double Hu6 { get; set; }

    [Column(Name = "nuoc")]
    public double Nuoc { get; set; }

    [Column(Name = "flags")]
    public int Flags { get; set; }

    [Column(Name = "local_updated_at")]
    public DateTime LocalUpdatedAt { get; set; }
    
    public double GetKhoiLuong(int pl, int silo)
    {
        return (pl, silo) switch
        {
            (1, 0) => Cl1,
            (1, 1) => Cl2,
            (1, 2) => Cl3,
            (1, 3) => Cl4,
            (1, 4) => Cl5,
            (1, 5) => Cl6,

            (2, 0) => Xi1,
            (2, 1) => Xi2,
            (2, 2) => Xi3,
            (2, 3) => Xi4,
            (2, 4) => Xi5,

            (3, 0) => Pg1,
            (3, 1) => Pg2,
            (3, 2) => Pg3,
            (3, 3) => Pg4,
            (3, 4) => Pg5,
            (3, 5) => Pg6,
            (3, 6) => Pg7,
            (3, 7) => Pg8,

            (4, _) => Nuoc,

            _ => 0
        };
    }
}

[Table(Name = "ht_me")]
public class HtMeFull: HtMe
{
    [Column(Name = "local_created_at")]
    public DateTime LocalCreatedAt { get; set; }
    
    [Column(Name = "updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column(Name = "created_at")]
    public DateTime CreatedAt { get; set; }
}
