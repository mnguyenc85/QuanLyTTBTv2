using System.Collections.ObjectModel;
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
    [ObservableProperty]
    private SrvFactory? _selFactory;

    public ObservableCollection<HTDonHangVM> DsDonHang { get; set; } = [];
    
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
    
    public async Task LoadDonHang(HTDonHangCond cond) {
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
}