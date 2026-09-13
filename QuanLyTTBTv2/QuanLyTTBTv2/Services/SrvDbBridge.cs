using System;
using System.Collections.Generic;
using System.Linq;
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
                .UseMonitorCommand(cmd =>
                {
                    System.Diagnostics.Debug.WriteLine("SQL: " + cmd.CommandText);
                })
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

    private ISelect<HTDonHang, KDKhachHang, PMStringLookUp, PMStringLookUp, PMStringLookUp, PMStringLookUp>
        CreateDonHangQuery(HTDonHangCond cond)
    {
        var query = _db
            .Select<HTDonHang, KDKhachHang, PMStringLookUp, PMStringLookUp, PMStringLookUp, PMStringLookUp>()
            .LeftJoin((dh, kh, da, ct, hm, dc) =>
                dh.SourceId == kh.SourceId &&
                dh.KhId == kh.LocalId)
            .LeftJoin((dh, kh, da, ct, hm, dc) =>
                dh.SourceId == da.SourceId &&
                dh.DaId == da.LocalId)
            .LeftJoin((dh, kh, da, ct, hm, dc) =>
                dh.SourceId == ct.SourceId &&
                dh.CtId == ct.LocalId)
            .LeftJoin((dh, kh, da, ct, hm, dc) =>
                dh.SourceId == hm.SourceId &&
                dh.HmId == hm.LocalId)
            .LeftJoin((dh, kh, da, ct, hm, dc) =>
                dh.SourceId == dc.SourceId &&
                dh.DiachiId == dc.LocalId);

        if (cond.UseFrom)
            query.Where((dh, kh, da, ct, hm, dc) => 
                dh.CreatedAt >= cond.FromTime);
        if (cond.UseTo)
            query.Where((dh, kh, da, ct, hm, dc) => 
                dh.CreatedAt < cond.ToTime);
        
        if (!string.IsNullOrWhiteSpace(cond.KhachHang))
        {
            var khten = cond.KhachHang;
            query = query.Where((dh, kh, da, ct, hm, dc) =>
                (kh.Ten ?? "").Contains(khten) ||
                (kh.Sdt ?? "").Contains(khten)
            );
        }
        if (!string.IsNullOrWhiteSpace(cond.DuAn))
        {
            var duan = cond.DuAn;
            query = query.Where((dh, kh, da, ct, hm, dc) =>
                (da.Vanban ?? "").Contains(duan) ||
                (ct.Vanban ?? "").Contains(duan) ||
                (hm.Vanban ?? "").Contains(duan) ||
                (dc.Vanban ?? "").Contains(duan)
            );
        }

        return query;
    } 
    
    public async Task<List<HTDonHang>?> DonHang_SelectAllAsync(HTDonHangCond cond)
    {
        if (_db == null) return null;

        var query = CreateDonHangQuery(cond);

        query
            .OrderByDescending((dh, kh, da, ct, hm, dc) => dh.CreatedAt)
            .Offset(cond.Offset)
            .Limit(cond.Limit);

        var rows = await query.ToListAsync((dh, kh, da, ct, hm, dc) => new
        {
            Dh = dh,
            Kh = kh,
            Da = da,
            Ct = ct,
            Hm = hm,
            Dc = dc
        });

        var result = rows.Select(x =>
        {
            x.Dh.KhachHang = x.Kh;
            x.Dh.DuAn = x.Da.Vanban;
            x.Dh.CongTrinh = x.Ct.Vanban;
            x.Dh.HangMuc = x.Hm.Vanban;
            x.Dh.DiaChi = x.Dc.Vanban;
            return x.Dh;
        }).ToList();

        return result;
    }
    
    public async Task<long> DonHang_CountAsync(HTDonHangCond cond)
    {
        if (_db == null) return 0;

        var query = CreateDonHangQuery(cond);

        return await query.CountAsync();
    }
    #endregion
}