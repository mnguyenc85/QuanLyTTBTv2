using System;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.ViewModels.Server;

public partial class HTDonHangTKVM: ViewModelBase
{
    public long Id { get; set; }
    public long DonHangId { get; set; }

    [ObservableProperty] private int _tongPhieu;
    [ObservableProperty] private double _tongTT;
    [ObservableProperty] private double _tongKL;
    [ObservableProperty] private int _tongMe;

    [ObservableProperty] private DateTime _tght;

    public HTDonHangTKVM() { }

    public HTDonHangTKVM(HTDonHangTK o)
    {
        FromDBO(o);
    }
    
    public void ToDBO(HTDonHangTK o)
    {
        o.Id = Id;
        o.DonHangId = DonHangId;
        o.TongPhieu = TongPhieu;
        o.TongKL = TongKL;
        o.TongTT = TongTT;
        o.TongMe = TongMe;
        o.Tght = Tght;
    }
    
    public HTDonHangTK CreateDBO()
    {
        var o = new HTDonHangTK();
        ToDBO(o);
        return o;
    }

    public void FromDBO(HTDonHangTK o)
    {
        Id = o.Id;
        DonHangId = o.DonHangId;
        
        TongPhieu = o.TongPhieu;
        TongTT = o.TongTT;
        TongKL = o.TongKL;
        TongMe = o.TongMe;
        
        Tght = o.Tght;
    }
}