using System;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.ViewModels.Server;

public partial class HTDonHangVM: ViewModelBase
{
    public long Id { get; set; }
    public long LocalId { get; set; }
    
    [ObservableProperty] private int _stt;
    [ObservableProperty] private string? _ma;
    
    [ObservableProperty] private string? _khachHang;
    public string? DiaChiKH { get; set; }
    /// <summary>
    /// Chuỗi gộp dự án - công trình - hang mục
    /// </summary>
    [ObservableProperty] private string? _duAn;
    public string? DADuAn { get; set; }
    public string? DACongTrinh { get; set; }
    public string? DAHangMuc { get; set; }
    public string? DADiaChi { get; set; }

    [ObservableProperty] private double _kl;

    [ObservableProperty] private DateTime _tgbd;
    [ObservableProperty] private DateTime _tgkt;

    public HTDonHangTKVM TK { get; set; } = new();
    
    public HTDonHangVM()
    {
        
    }

    public HTDonHangVM(HTDonHang o)
    {
        FromDBO(o);
    }

    public void FromDBO(HTDonHang o)
    {
        Id = o.Id;
        LocalId = o.LocalId;
        Ma = o.Ma;
        if (o.KhachHang != null)
        {
            KhachHang = o.KhachHang.Ten;
            DiaChiKH = o.KhachHang.Diachi;
        }
        else
        {
            KhachHang = "";
            DiaChiKH = "";
        }

        if (o.DuAn != null)
        {
            // DuAn = $"{dh.DaId} - {dh.CtId} - {dh.HmId}";
            DuAn = $"{o.DuAn} - {o.CongTrinh} - {o.HangMuc}";
            DADuAn = o.DuAn;
            DACongTrinh = o.CongTrinh;
            DAHangMuc = o.HangMuc;
            DADiaChi = o.DiaChi;
        }
        else
        {
            DADuAn = "";
            DACongTrinh = "";
            DAHangMuc = "";
            DADiaChi = "";
        }

        Kl = o.Klht;
        Tgbd = o.CreatedAt;
        Tgkt = o.Tght ?? DateTime.MinValue;
    }
}