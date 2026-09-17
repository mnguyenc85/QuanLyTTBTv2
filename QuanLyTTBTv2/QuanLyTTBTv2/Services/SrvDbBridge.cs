using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeSql;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.Services;

public class SrvDbBridge
{
    public string? LastError { get; private set; }
    private IFreeSql? _db;

    #region Initialization

    public SrvDbBridge()
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
            typeof(HTDonHangTK)
        );
    }
    #endregion

    #region Load data
    public async Task<List<SrvFactory>?> Factory_SelectAllAsync()
    {
        if (_db == null) return null;
        
        return await _db.Select<SrvFactory>()
            .ToListAsync();
    }

    #region Đơn hàng
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

        query.Where((dh, kh, da, ct, hm, dc) =>
            dh.SourceId == cond.SourceId
            );
        
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

    #region Phiếu

    public async Task<long> Phieu_CountAsync(HTPhieuCond cond)
    {
        if (_db == null) return 0;
        // TODO: use CreatePhieuQuery, hoặc dùng riêng
        return await _db.Select<HTPhieu>()
            .Where(p => p.SourceId == cond.SourceId && p.DonhangId == cond.DonHangId)
            .CountAsync();
    }
    
    private ISelect<HTPhieu, KDXe, KDLaiXe, HTCongThuc> CreatePhieuQuery(HTPhieuCond cond)
    {
        var query = _db
            .Select<HTPhieu, KDXe, KDLaiXe, HTCongThuc>()
            .LeftJoin((ph, xe, lx, ct) =>
                ph.SourceId == xe.SourceId &&
                ph.XeId == xe.LocalId)
            .LeftJoin((ph, xe, lx, ct) =>
                ph.SourceId == lx.SourceId &&
                ph.XeId == lx.LocalId)
            .LeftJoin((ph, xe, lx, ct) =>
                ph.SourceId == ct.SourceId &&
                ph.CongthucId == ct.LocalId);

        query.Where((ph, xe, lx, ct) => ph.DonhangId == cond.DonHangId && ph.SourceId == cond.SourceId);
        
        if (cond.UseFrom)
            query.Where((ph, xe, lx, ct) => 
                ph.CreatedAt >= cond.FromTime);
        if (cond.UseTo)
            query.Where((ph, xe, lx, ct) => 
                ph.CreatedAt < cond.ToTime);

        if (!string.IsNullOrWhiteSpace(cond.CongThuc))
        {
            query = query.Where((ph, xe, lx, ct) =>
                (ct.Ma ?? "").Contains(cond.CongThuc) ||
                (ct.Mac ?? "").Contains(cond.CongThuc)
            );
        }
        if (!string.IsNullOrWhiteSpace(cond.Xe))
        {
            query = query.Where((ph, xe, lx, ct) =>
                (xe.Bsx ?? "").Contains(cond.Xe)
            );
        }
        if (!string.IsNullOrWhiteSpace(cond.LaiXe))
        {
            query = query.Where((ph, xe, lx, ct) =>
                (lx.Ten ?? "").Contains(cond.LaiXe) ||
                (lx.Sdt ?? "").Contains(cond.LaiXe)
            );
        }

        return query;
    } 
    
    public async Task<List<HTPhieu>?> Phieu_SelectAllAsync(HTPhieuCond cond)
    {
        if (_db == null) return null;

        var query = CreatePhieuQuery(cond);

        query
            .OrderByDescending((ph, xe, lx, ct) => ph.CreatedAt)
            .Offset(cond.Offset)
            .Limit(cond.Limit);

        var rows = await query.ToListAsync((ph, xe, lx, ct) => new
        {
            Phieu = ph,
            Xe = xe,
            Lx = lx,
            Ct = ct
        });

        var result = rows.Select(x =>
        {
            x.Phieu.Bsx = x.Xe.Bsx;
            x.Phieu.LaiXe = x.Lx.Ten;
            x.Phieu.CongThuc = x.Ct;
            return x.Phieu;
        }).ToList();

        return result;
    }
    #endregion
    
    #region Công thức
    public async Task<List<HTCongThuc>> CongThuc_SelectAsync( int source_id, List<int> congthuc_ids)
    {
        if (_db == null || congthuc_ids.Count == 0)
            return [];

        // 1. Lấy danh sách công thức
        var congthucs = await _db
            .Select<HTCongThuc>()
            .Where(x =>
                x.SourceId == source_id &&
                congthuc_ids.Contains(x.LocalId))
            .ToListAsync();

        if (congthucs.Count == 0)  return [];
        // Các local_id công thức thực tế lấy được
        var ctIds = congthucs
            .Select(x => x.LocalId)
            .ToList();

        // 2. Lấy thành phần
        var rows = await _db
            .Select<HTCongThucThanhPhan, HTThanhPhan>()
            .InnerJoin<HTThanhPhan>(
                (cttp, tp) =>
                    cttp.SourceId == tp.SourceId &&
                    cttp.TpId == tp.LocalId)
            .Where((cttp, tp) =>
                cttp.SourceId == source_id &&
                congthuc_ids.Contains(cttp.CtId))
            .ToListAsync<CongThucThanhPhanDto>();

        var tpByCt = rows
            .GroupBy(x => x.CtId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ThanhPhan).ToList());
        
        foreach (var ct in congthucs)
        {
            if (tpByCt.TryGetValue(ct.LocalId, out var ds))
                ct.DsThanhPhan = ds;
        }
        
        return congthucs;
    }
    
    public async Task<List<HTThanhPhan>> ThanhPhan_SelectDistinctAsync(long source_id, List<int> congthuc_ids)
    {
        if (congthuc_ids.Count == 0) return [];

        var data = await _db
            .Select<HTCongThucThanhPhan, HTThanhPhan>()
            .InnerJoin<HTThanhPhan>((cttp, tp) =>
                cttp.SourceId == tp.SourceId &&
                cttp.TpId == tp.LocalId)
            .Where((cttp, tp) =>
                cttp.SourceId == source_id &&
                congthuc_ids.Contains(cttp.CtId))
            .ToListAsync<HTThanhPhan>();

        return data
            .GroupBy(x => x.LocalId)
            .Select(g => g.First())
            .ToList();
    }
    
        
    public async Task<List<HTThanhPhan>> ThanhPhan_SelectByCtAsync(long source_id, long ct_id)
    {
        var data = await _db
            .Select<HTCongThucThanhPhan, HTThanhPhan>()
            .InnerJoin<HTThanhPhan>((cttp, tp) =>
                cttp.SourceId == tp.SourceId &&
                cttp.TpId == tp.LocalId)
            .Where((cttp, tp) =>
                cttp.SourceId == source_id &&
                cttp.CtId == ct_id)
            .ToListAsync<HTThanhPhan>();

        return data
            .GroupBy(x => x.LocalId)
            .Select(g => g.First())
            .ToList();
    }
    
    public async Task<List<HTPhieuFkey>?> PhieuFkey_SelectByDonHangAsync(long source_id, long donhang_id)
    {
        if (_db == null) return null;
        
        var stmt = _db
            .Select<HTPhieuFkey>()
            .Where((ph) =>
                ph.SourceId == source_id &&
                ph.DonhangId == donhang_id);
        
        return await stmt.ToListAsync<HTPhieuFkey>();
    }
    #endregion
    
    #endregion
}