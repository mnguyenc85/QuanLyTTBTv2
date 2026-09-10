using FreeSql.DataAnnotations;
using System;

namespace QuanLyTTBTv2.Models.Local;

public enum SettingValueTypes { String = 0, Number = 1, Boolean = 2 }

[Table(Name = "pm_settings")]
public class DbSettingDto
{
    [Column(Name = "id", IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; } = -1;

    [Column(Name = "updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column(Name = "created_at", CanUpdate = false)]
    public DateTime CreatedAt { get; set; }

    [Column(Name = "ten")]
    public string Name { get; set; }

    private string? _val;
    [Column(Name = "giatri")]
    public string? Value
    {
        get => _val;
        set { 
            if (_val != value)
            {
                _val = value;
                Changed = true;
            } 
        }
    }

    [Column(Name = "kieu", DbType = "INTEGER")]
    public SettingValueTypes Kieu { get; set; }

    [Column(IsIgnore = true)]
    public bool Changed { get; set; }

    public DbSettingDto(string name)
    {
        Name = name;
    }
}
