using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models;
using QuanLyTTBTv2.Models.Server;
using QuanLyTTBTv2.Services;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.ViewModels;

public partial class WorkspaceVM: ViewModelBase
{
    private readonly SrvDbBridge _srvDb;
    private CancellationTokenSource? _ctsLoadDhTk;
    
    public ObservableCollection<SrvFactory> SrvFactories { get; set; } = [];
    [ObservableProperty] private SrvFactory? _selFactory;

    private Dictionary<long, HTDonHangVM> _tudienDonHang = [];
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
    
    public ObservableCollection<HTPhieuVM> DsPhieu { get; set; } = [];
    [ObservableProperty] private HTPhieuVM? _selectedPhieu;
    
    /// <summary>
    /// Hiển thị mẻ
    /// </summary>
    public ObservableCollection<HTMeVM> TkMe { get; set; } = [];
    
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
    
    public async Task LoadDsDonHang(HTDonHangCond cond) {
        _ctsLoadDhTk?.Cancel();

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

        await LoadDuLieuDhTk();
    }

    private async Task LoadDuLieuDhTk()
    {
        try
        {
            // Load dữ liệu tk kèm đơn hàng
            _ctsLoadDhTk = new CancellationTokenSource();
            var dhtks = await _srvDb.DonHangTk_SelectByDhIdsAsync(_tudienDonHang.Keys.ToList(), _ctsLoadDhTk.Token);

            // Update tk vào DsDonHang
            if (dhtks != null)
                foreach (var tk in dhtks)
                {
                    if (_tudienDonHang.TryGetValue(tk.DonHangId, out HTDonHangVM? value)) value.TK.FromDBO(tk);
                }
        }
        catch { }
        finally
        {
            if (_ctsLoadDhTk != null)
            {
                _ctsLoadDhTk.Dispose();
                _ctsLoadDhTk = null;
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

    /// <summary>
    /// Tính lại dữ liệu của đơn hàng
    /// </summary>
    public async void DonHangTkHt()
    {
        if (SelectedDonHang == null || SelFactory == null) return;

        var tk = await _srvDb.Phieu_TinhTKAsync(SelFactory.Id, SelectedDonHang.LocalId);
        tk.DonHangId = SelectedDonHang.Id;
        await _srvDb.DonHangTk_SaveAsync(tk);
        
        SelectedDonHang.TK.FromDBO(tk);
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
    
    public void ClearDsDonHang()
    {
        DsDonHang.Clear();
        _tudienDonHang.Clear();
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
}