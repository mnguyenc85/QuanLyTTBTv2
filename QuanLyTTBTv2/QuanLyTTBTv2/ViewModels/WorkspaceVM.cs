using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Server;
using QuanLyTTBTv2.Services;
using QuanLyTTBTv2.ViewModels.Server;

namespace QuanLyTTBTv2.ViewModels;

public partial class WorkspaceVM: ViewModelBase
{
    private readonly SrvDbBridge _srvDb;
    
    public ObservableCollection<SrvFactory> SrvFactories { get; set; } = [];
    [ObservableProperty] private SrvFactory? _selFactory;

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
    public List<string> DsMaThanhPhan { get; } = [];
    
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
        DsDonHang.Clear();

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
            stt++;
            DsDonHang.Add(new HTDonHangVM(dh) { Stt = stt });
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
            int comparePhanLoai = Nullable.Compare(x.PhanLoai, y.PhanLoai);
            if (comparePhanLoai != 0) return comparePhanLoai;
            return Nullable.Compare(x.Silo, y.Silo);
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
                    DsMaThanhPhan.Add(tp.Ma);
            }
        }
    }

    public void ClearCurDonHangData()
    {
        DsThanhPhan.Clear();
    }

    public void ClearDsPhieu()
    {
        SelectedPhieu = null;
        DsPhieu.Clear();
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

    public async void LoadChiTietPhieu()
    {
        if (SelectedPhieu == null || SelFactory == null)
        {
            // Xóa nếu lấy thành phần theo phiếu
            // Hiện đang lấy theo đơn
            // DsThanhPhan.Clear();
            return;
        }

        var cttps = await _srvDb.ThanhPhan_SelectByCtAsync(SelFactory.Id, SelectedPhieu.CtId);
        
        var dcttps = new Dictionary<string, HTThanhPhan>();
        foreach (var tp in cttps) if (tp.Ma != null) dcttps.Add(tp.Ma, tp);
        
        TkMe.Add(HTMeVM.FromCapPhoi(DsMaThanhPhan, dcttps));
    }
}