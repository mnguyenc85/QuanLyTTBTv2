using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FreeSql;
using QuanLyTTBTv2.Models.Local;

namespace QuanLyTTBTv2.Services;

public class LocalDbBridge
{
    #region Singleton
    private static readonly Lazy<LocalDbBridge> _instance = new Lazy<LocalDbBridge>(() => new LocalDbBridge());
    public static LocalDbBridge Instance => _instance.Value;
    #endregion
    
    private readonly IFreeSql _db;

    /// <summary>
    /// Đường dẫn tới file SQLite.
    /// </summary>
    public string DbPath =>
        Path.Combine(AppContext.BaseDirectory, "local.db");

    private LocalDbBridge()
    {
        string dbPath = Path.Combine(AppContext.BaseDirectory, "local.db");

        _db = new FreeSqlBuilder()
            .UseConnectionString(
                DataType.Sqlite,
                $"Data Source={dbPath};Pooling=true")
            .Build();

        //CreateDatabase();
    }
    
    public void SyncSchema()
    {
        _db?.CodeFirst.SyncStructure(
            typeof(DbSettingDto)
        );
    }

    /// <summary>
    /// Sử dụng thay cho SyncSchema
    /// </summary>
    public void CreateDatabase()
    {
        _db.Ado.ExecuteNonQuery("""
            CREATE TABLE IF NOT EXISTS pm_settings (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                updated_at DATETIME NOT NULL,
                created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                ten TEXT NOT NULL,
                giatri TEXT,
                kieu INTEGER NOT NULL
            );
        """);
    }

    #region pm_settings
    public async Task<List<DbSettingDto>?> Settings_SelectAllAsync()
    {
        return await _db.Select<DbSettingDto>()
            .ToListAsync();
    }

    public async Task<(int, int)> Settings_SaveAsync(DbSettings settings)
    {
        int noins = 0, noupdate = 0;

        foreach (DbSettingDto s in settings.Data.Values)
        {
            if (s.Id <= 0)
            {
                var id = await _db.Insert(s).ExecuteIdentityAsync();
                s.Id = id;
                noins++;
            }
            else if (s.Changed)
            {
                await _db.Update<DbSettingDto>()
                    .SetSource(s)
                    .ExecuteAffrowsAsync();
                noupdate++;
            }
        }

        return (noins, noupdate);
    }
    #endregion
}