using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace QuanLyTTBTv2.Services;

public class DbCache
{
    #region Singleton
    private static readonly Lazy<DbCache> _instance = new(() => new DbCache());
    public static DbCache Instance => _instance.Value;
    #endregion

    // Tạo CultureInfo dựa trên "en-US" nhưng đổi dấu thập phân thành ','
    private readonly CultureInfo _vnCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
    
    private readonly SqliteDbBridge _db = SqliteDbBridge.Instance;
    private readonly LocalDbBridge _localdb = new LocalDbBridge();
    public LocalDbBridge LocalDB => _localdb;

    public ReportPrinting DocketPrint { get; private set; }
    public ReportPrinting DetailDocketPrint { get; private set; }
    
    public string AppPath { get; set; }
    public string DocketPath { get; set; }
    public string ReportsPath { get; set; }

    public DbSettings Settings { get; set; } = new DbSettings();
    
    private DbCache()
    {
        _vnCulture.NumberFormat.NumberDecimalSeparator = ",";
        _vnCulture.NumberFormat.NumberGroupSeparator = "."; // nếu muốn ngăn cách hàng nghìn
        AppPath = AppDomain.CurrentDomain.BaseDirectory;
        ReportsPath = AppPath + "reports\\";
        if (!Directory.Exists(ReportsPath)) Directory.CreateDirectory(ReportsPath);
        DocketPath = AppPath + "reports\\dockets\\";
        if (!Directory.Exists(DocketPath)) Directory.CreateDirectory(DocketPath);
        
        DocketPrint = new ReportPrinting(_vnCulture);
        DetailDocketPrint = new ReportPrinting(_vnCulture);
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

        _localdb.Initialize();
    }

    public int InitPrinting()
    {
        var t = Settings.GetValue("print.phieu.coban");
        var path = System.IO.Path.Join(DocketPath, t);
        DocketPrint.LoadTemplate(path);

        t = Settings.GetValue("print.phieu.chitiet");
        path = System.IO.Path.Join(DocketPath, t);
        DetailDocketPrint.LoadTemplate(path);
        
        return (DocketPrint.IsError ? 1 : 0) | (DetailDocketPrint.IsError ? 2 : 0);
    }
}