using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeSql;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.Services;

public class SrvDbBridge
{
    #region Singleton
    private static readonly Lazy<SrvDbBridge> _instance = new(() => new SrvDbBridge());
    public static SrvDbBridge Instance => _instance.Value;
    #endregion

    private IFreeSql? _db;

    #region Initialization

    private SrvDbBridge()
    {
        
    }
    
    public bool Initialize(string host, string db, string user, string pass)
    {
        try
        {
            ReleaseDb();

            string connStr = $"Server={host};Port=3306;Database={db};User Id={user};Password={pass};SslMode=none;";

            _db = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, connStr)
                //.UseAutoSyncStructure(true)
                //.UseMonitorCommand(cmd =>
                //{
                //    System.Diagnostics.Debug.WriteLine("SQL: " + cmd.CommandText);
                //})
                .Build();
        }
        catch
        {
            return false;
        }
        return true;
    }

    private void ReleaseDb()
    {
        if (_db != null)
        {
            _db?.Dispose();
            _db = null;
        }
    }
    #endregion

    #region Load data
    public async Task<List<SrvFactory>?> Factory_SelectAllAsync()
    {
        if (_db == null) return null;
        
        return await _db.Select<SrvFactory>()
            .ToListAsync();
    }
    #endregion
}