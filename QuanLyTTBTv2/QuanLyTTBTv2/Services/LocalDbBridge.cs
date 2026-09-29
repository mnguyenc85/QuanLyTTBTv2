using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeSql;
using MySqlConnector;
using QuanLyTTBTv2.Models;
using QuanLyTTBTv2.Models.Local;

namespace QuanLyTTBTv2.Services;

public class LocalDbBridge
{
    public string? LastError { get; private set; }
    private IFreeSql? _db;

    public string? DbSrcName { get; set; }
    private readonly Dictionary<int, DbConfig> _dbConfigs = new();
    private DbConfig? _selConfig;
    
    #region Initialization

    public LocalDbBridge()
    {
        _dbConfigs.Add(0, new DbConfig("localhost", -1, "tronbetong3_ql", "root", "ncmanh191"));
        _dbConfigs.Add(1, new DbConfig("localhost", 13306, "tronbetong3_ql", "root", "ncmanh191"));
    }

    public void CreateDb()
    {
        _selConfig ??= _dbConfigs[0];

        using var conn = new MySqlConnection(_selConfig.CreateConnStr(true));
        conn.Open();
        var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {_selConfig.Db} CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;", conn);
        cmd.ExecuteNonQuery();
    }
    
    public bool Initialize()
    {
        try
        {
            _selConfig ??= _dbConfigs[0];
            string connStr = _selConfig.CreateConnStr();
            
            ReleaseDb();

            _db = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, connStr)
                //.UseAutoSyncStructure(true)
                // .UseMonitorCommand(cmd =>
                // {
                //     System.Diagnostics.Debug.WriteLine("SQL: " + cmd.CommandText);
                // })
                .Build();
        }
        catch (Exception ex)
        {
            LastError = ex.Message;
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

    public void SyncSchema()
    {
        _db?.CodeFirst.SyncStructure(
            typeof(DbInPhieu)
        );
    }
    #endregion
}