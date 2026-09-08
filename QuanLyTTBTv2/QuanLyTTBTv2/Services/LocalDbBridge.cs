using System;
using System.IO;
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
    /// FreeSql instance.
    /// </summary>
    public IFreeSql Db => _db;

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
    }
    
    public void SyscSchema()
    {
        _db?.CodeFirst.SyncStructure(
            typeof(LocalSetting)
        );
    }
}