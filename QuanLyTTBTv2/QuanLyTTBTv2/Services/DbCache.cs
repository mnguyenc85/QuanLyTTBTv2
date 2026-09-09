using System;
using System.IO;
using System.Threading.Tasks;

namespace QuanLyTTBTv2.Services;

public class DbCache
{
    #region Singleton
    private static readonly Lazy<DbCache> _instance = new(() => new DbCache());
    public static DbCache Instance => _instance.Value;
    #endregion

    private readonly LocalDbBridge _db = LocalDbBridge.Instance;

    public string AppPath { get; set; }
    public string DocketPath { get; set; }
    public string ReportsPath { get; set; }

    public DbSettings Settings { get; set; } = new DbSettings();
    
    private DbCache()
    {
        AppPath = AppDomain.CurrentDomain.BaseDirectory;
        ReportsPath = AppPath + "reports\\";
        if (!Directory.Exists(ReportsPath)) Directory.CreateDirectory(ReportsPath);
        DocketPath = AppPath + "reports\\dockets\\";
        if (!Directory.Exists(DocketPath)) Directory.CreateDirectory(DocketPath);
    }
    
    public async Task InitAsync()
    {
        var settings = await _db.Settings_SelectAllAsync();
        if (settings != null)
        {
            foreach (var s in settings)
            {
                Settings.LoadSetting(s);
            }
        }
    }
}