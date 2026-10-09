using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models;
using QuanLyTTBTv2.Models.Local;
using QuanLyTTBTv2.Models.Server;
using QuanLyTTBTv2.Services;
using QuanLyTTBTv2.ViewModels.Printing;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.ViewModels;

public partial class WorkspaceVM: ViewModelBase
{
    private readonly SrvDbBridge _srvDb;
    private readonly DbCache _cache = DbCache.Instance;
    private CancellationTokenSource? _ctsLoadDhTk;
    
    // ----- Factory
    public ObservableCollection<SrvFactory> SrvFactories { get; set; } = [];
    [ObservableProperty] private SrvFactory? _selFactory;

    // ----- Đơn hàng
    private readonly Dictionary<long, HTDonHangVM> _tudienDonHang = [];
    public ObservableCollection<HTDonHangVM> DsDonHang { get; set; } = [];
    [ObservableProperty] private HTDonHangVM? _selectedDonHang;
    
    /// <summary>
    /// Danh sách thành phần được dùng trong đơn
    /// </summary>
    public ObservableCollection<HTThanhPhan> DsThanhPhan { get; set; } = [];
    private Dictionary<string, HTThanhPhan> _dsUniqueThanhPhan { get; } = [];
    /// <summary>
    /// Danh sách mã thành phần trong bảng thống kê (unique, ordered)
    /// </summary>
    public List<CHThanhPhan> DsMaThanhPhan { get; } = [];
    
    // ----- Phiếu & mẻ 
    public ObservableCollection<HTPhieuVM> DsPhieu { get; set; } = [];
    [ObservableProperty] private HTPhieuVM? _selectedPhieu;
    
    /// <summary>
    /// Hiển thị mẻ
    /// </summary>
    public ObservableCollection<HTMeVM> TkMe { get; set; } = [];
    
    // ----- Phiếu in
    public ObservableCollection<PhieuInVM> DsPhieuIn { get; set; } = [];
    [ObservableProperty] private PhieuInVM? _selectedPhieuIn;
    public PhieuInVM CurPhieuIn { get; set; } = new PhieuInVM();

    
    // ----- Thống kê phiếu in
    public ObservableCollection<PhieuInVM> DsPhieuInTK { get; set; } = [];
    
    
    public WorkspaceVM()
    {
        _srvDb = new SrvDbBridge();
    }
    
    public WorkspaceVM(SrvDbBridge srv)
    {
        _srvDb = srv;
    }
    
    public async Task LoadSrvFactories(bool reset = true)
    { 
        if (reset) SrvFactories.Clear();
        var lst = await _srvDb.Factory_SelectAllAsync();
        if (lst != null)
            foreach (var f in lst)
            {
                SrvFactories.Add(f);
            }

        if (SrvFactories.Count > 0)
        {
            SelFactory = SrvFactories[0];
        }
    }
    
    /// <summary>
    /// Lấy ds đơn hàng
    /// </summary>
    /// <param name="cond"></param>
    public async Task DsDonHang_Load(HTDonHangCond cond) {
        if (_ctsLoadDhTk != null)
            await _ctsLoadDhTk.CancelAsync();

        // Load dữ liệu đơn hàng
        ClearDsDonHang();

        if (cond.Changed)
        {
            cond.Offset = 0;
            long total = await _srvDb.DonHang_CountAsync(cond);
            cond.Total = (int)total;
        }
        
        var lst = await _srvDb.DonHang_SelectAllAsync(cond);
        if (lst == null) return;
        int stt = cond.Offset;
        foreach (var dh in lst)
        {
            AddDonHang(dh, ++stt);
        }

        await LoadOrCalDhTk();
    }

    /// <summary>
    /// Lấy hoặc tính dữ liệu thống kê của đơn hàng (tổng phiếu, m3)
    /// </summary>
    private async Task LoadOrCalDhTk()
    {
        try
        {
            // Load dữ liệu tk kèm đơn hàng
            _ctsLoadDhTk = new CancellationTokenSource();

            await DonHang_LoadTKs(_ctsLoadDhTk.Token);
            await DonHang_CalTKs(_ctsLoadDhTk.Token);
        }
        // catch { }
        finally
        {
            if (_ctsLoadDhTk != null)
            {
                _ctsLoadDhTk.Dispose();
                _ctsLoadDhTk = null;
            }
        }
    }
    
    private async Task DonHang_LoadTKs(CancellationToken ct)
    {
        var dhtks = await _srvDb.DonHangTk_SelectByDhIdsAsync(_tudienDonHang.Keys.ToList(), ct);

        // Update tk vào DsDonHang
        if (dhtks != null)
            foreach (var tk in dhtks)
            {
                if (_tudienDonHang.TryGetValue(tk.DonHangId, out HTDonHangVM? value)) value.TK.FromDBO(tk);
            }
    }
    
    /// <summary>
    /// Tính thống kê cho đơn hàng
    /// </summary>
    public async void DonHang_CalTK()
    {
        if (SelFactory == null || SelectedDonHang == null) return;

        var tk = await _srvDb.Phieu_TinhTKAsync(SelFactory.Id, SelectedDonHang.LocalId);
        if (tk != null)
        {
            tk.DonHangId = SelectedDonHang.Id;
            await _srvDb.DonHangTk_SaveAsync(tk);

            SelectedDonHang.TK.FromDBO(tk);
        }
    }

    /// <summary>
    /// Tính thống kê cho các đơn hàng hiển thị
    /// </summary>
    public async Task DonHang_CalTKs(CancellationToken ct)
    {
        if (SelFactory == null) return;

        foreach (var dh in DsDonHang)
        {
            if (dh.TK.Id > 0) continue;
            
            var tk = await _srvDb.Phieu_TinhTKAsync(SelFactory.Id, dh.LocalId, ct);
            if (tk != null)
            {
                tk.DonHangId = dh.Id;
                await _srvDb.DonHangTk_SaveAsync(tk, ct);

                dh.TK.FromDBO(tk);
            }
        }
    }

    /// <summary>
    /// Lấy dữ liệu liên quan đến đơn hàng: thành phần, khách hàng, dự án
    /// </summary>
    public async Task LoadCurDonHangData()
    {
        // Load danh sách thành phần sử dụng
        if (SelectedDonHang == null || SelFactory == null) return;

        var lstPh = await _srvDb.PhieuFkey_SelectByDonHangAsync(SelFactory.Id, SelectedDonHang.LocalId);
        if (lstPh == null) return;

        
        DsThanhPhan.Clear();
        var ctids = lstPh.GroupBy(x => x.CongthucId).Select(x => x.Key).Distinct().ToList();
        var lsttp = await _srvDb.ThanhPhan_SelectDistinctAsync(SelFactory.Id, ctids);
        
        lsttp.Sort((x, y) =>
        {
            int comparePhanLoai = x.PhanLoai.CompareTo(y.PhanLoai);
            if (comparePhanLoai != 0) return comparePhanLoai;
            return x.Silo.CompareTo(y.Silo);
        });
        
        _dsUniqueThanhPhan.Clear();
        DsMaThanhPhan.Clear();
        int stt = 0;
        foreach (var tp in lsttp)
        {
            tp.Stt = ++stt;
            DsThanhPhan.Add(tp);
            if (tp.Ma != null)
            {
                if (_dsUniqueThanhPhan.TryAdd(tp.Ma, tp))
                    DsMaThanhPhan.Add(new CHThanhPhan(tp.Ma) { Ten = tp.Ten, PL = tp.PhanLoai, Silo = tp.Silo });
            }
        }
    }
    
    public async Task LoadDsPhieu(HTPhieuCond cond)
    {
        DsPhieu.Clear();

        if (cond.Changed)
        {
            cond.Offset = 0;
            long total = await _srvDb.Phieu_CountAsync(cond);
            cond.Total = (int)total;
        }

        var lst = await _srvDb.Phieu_SelectAllAsync(cond);
        if (lst == null) return;
        int stt = cond.Offset;
        foreach (var ph in lst)
        {
            stt++;
            DsPhieu.Add(new HTPhieuVM(ph) { Stt = stt });
        }
    }

    public async Task LoadChiTietPhieu()
    {
        System.Diagnostics.Debug.WriteLine($"LoadChiTietPhieu 1: {SelectedPhieu}, {SelFactory}");
        if (SelectedPhieu == null || SelFactory == null)
        {
            // Xóa nếu lấy thành phần theo phiếu
            // Hiện đang lấy theo đơn
            // DsThanhPhan.Clear();
            return;
        }

        System.Diagnostics.Debug.WriteLine($"LoadChiTietPhieu 2: {SelectedPhieu}, {SelFactory}");
        var cttps = await _srvDb.ThanhPhan_SelectByCtAsync(SelFactory.Id, SelectedPhieu.CtId);
        System.Diagnostics.Debug.WriteLine($"LoadChiTietPhieu 3: {SelectedPhieu}, {SelFactory}");
        var lstme = await _srvDb.Me_SelectByPhieuAsync(SelFactory.Id, SelectedPhieu.LocalId);

        if (cttps == null || lstme == null)
        {
            System.Diagnostics.Debug.WriteLine($"cttps == null? {cttps == null}, lstme == null? {lstme == null}");
            return;
        }
        
        var dcttps = new Dictionary<string, HTThanhPhan>();
        foreach (var tp in cttps) if (tp.Ma != null) dcttps.Add(tp.Ma, tp);
        
        TkMe.Clear();

        int sotp = dcttps.Count;
        TkMe.Add(HTMeVM.FromThanhPhan(DsMaThanhPhan));
        var mecp = HTMeVM.FromCapPhoi(DsMaThanhPhan, dcttps); 
        TkMe.Add(mecp);
        double ttme = (SelectedPhieu != null && SelectedPhieu.Medat > 0)
            ? SelectedPhieu.Ttdat / SelectedPhieu.Medat
            : 0;
        TkMe.Add(HTMeVM.FromCapPhoiMe(DsMaThanhPhan, mecp, ttme, sotp));

        int stt = 0;
        List<HTMeVM> dsmetmp = [];
        foreach (var m in lstme)
        {
            var me = HTMeVM.FromMe(DsMaThanhPhan, dcttps, m);
            me.Stt = (++stt).ToString();
            TkMe.Add(me);
            dsmetmp.Add(me);
        }
        
        TkMe.Add(HTMeVM.CreateMeTong(dsmetmp, sotp));
    }
    
    public long PhieuIn_LoadByPhieuTron()
    {
        if (SelectedPhieu == null || SelectedPhieu.Id <= 0)
        {
            CurPhieuIn.Clear();
            return -1;
        }

        if (SelectedPhieu.Id != CurPhieuIn.PhieuId)
        {
            CurPhieuIn.CopyFrom(SelectedDonHang, SelectedPhieu);
            return CurPhieuIn.Id;
        }

        return 0;
    }

    public void PhieuIn_LoadByPhieuIn()
    {
        if (SelectedPhieuIn == null)
        {
            CurPhieuIn.Clear();
            return;
        }
        
        CurPhieuIn.CopyFrom(SelectedPhieuIn);
    }

    /// <summary>
    /// Lưu phiếu in hiện tại
    /// </summary>
    /// <returns>True: nếu save</returns>
    public async Task<bool> PhieuInSave()
    {
        var o = CurPhieuIn.CreateDO();
        await _cache.LocalDB.PhieuIn_SaveAsync(o);
        CurPhieuIn.Changed = false;
        return true;
    }

    public async Task DsPhieuIn_Load(DbInPhieuCond cond, bool byDonHang = false)
    {
        var db = _cache.LocalDB;
        // Load dữ liệu đơn hàng
        ClearDsPhieuIn();

        if (byDonHang)
        {
            cond.PhieuIds.Clear();
            foreach (var ph in DsPhieu)
            {
                cond.PhieuIds.Add(ph.Id);
            }
            cond.DonHangId = SelectedDonHang?.Id ?? -1;
        }
        else
        {
            if (SelectedPhieu == null) return; 
            cond.PhieuIds.Clear();
            cond.PhieuId = SelectedPhieu.Id;
        }
        
        if (cond.Changed)
        {
            cond.Offset = 0;
            long total = await db.PhieuIn_CountAsync(cond);
            cond.Total = (int)total;
        }
        
        var lst = await _cache.LocalDB.PhieuIn_LoadAsync(cond);
        if (lst == null) return;
        int stt = cond.Offset;
        foreach (var ph in lst)
        {
            AddPhieuIn(ph, ++stt);
        }
    }
    
    /// <summary>
    /// Sử dụng hàm này để đảm bảo DsDonHang và _tudienDonHang
    /// </summary>
    public void AddDonHang(HTDonHang dh, int stt)
    {
        var vm = new HTDonHangVM(dh) { Stt = stt };
        DsDonHang.Add(vm);
        _tudienDonHang.TryAdd(dh.Id, vm);
    }

    public void AddPhieuIn(DbInPhieu ph, int stt)
    {
        var vm = new PhieuInVM(ph) { Stt = stt };
        DsPhieuIn.Add(vm);
    }
    
    /// <summary>
    /// Đảm báo xóa đồng bộ DsDonHang và _tudienDonHang
    /// </summary>
    public void ClearDsDonHang()
    {
        DsDonHang.Clear();
        _tudienDonHang.Clear();
        // TODO: check
        SelectedPhieu = null;
        CurPhieuIn?.Clear();
    }
    
    public void ClearCurDonHangData()
    {
        DsThanhPhan.Clear();
    }

    public void ClearDsPhieu()
    {
        SelectedPhieu = null;
        DsPhieu.Clear();
        TkMe.Clear();
        
        System.Diagnostics.Debug.WriteLine($"ClearDsPhieu: {SelectedPhieu}, {SelFactory}");
    }

    public void ClearDsPhieuIn()
    {
        DsPhieuIn.Clear();
    }
}