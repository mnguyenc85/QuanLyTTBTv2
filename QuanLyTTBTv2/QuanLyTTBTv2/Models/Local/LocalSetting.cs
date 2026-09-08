using FreeSql.DataAnnotations;

namespace QuanLyTTBTv2.Models.Local;

public class LocalSetting
{
    public enum SettingValueTypes { String = 0, Number = 1, Boolean = 2 }

    [Table(Name = "pm_settings")]
    public class DbSettingDto: DbBaseDto
    {
        [Column(Name = "ten")]
        public string Name { get; set; }

        [Column(Name = "giatri")]
        public string? Value { get; set; }

        [Column(Name = "kieu", DbType = "INT")]
        public SettingValueTypes Kieu { get; set; }

        [Column(IsIgnore = true)]
        public bool Changed { get; set; }

        public DbSettingDto(string name)
        {
            Name = name;
        }
    }
}