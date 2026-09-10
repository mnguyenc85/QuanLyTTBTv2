using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Server;
using QuanLyTTBTv2.Services;

namespace QuanLyTTBTv2.ViewModels;

public partial class WorkspaceVM: ViewModelBase
{
    private readonly SrvDbBridge _srvDb = SrvDbBridge.Instance;
    
    public ObservableCollection<SrvFactory> SrvFactories { get; set; } = [];
    [ObservableProperty]
    private SrvFactory? _selFactory;

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
}