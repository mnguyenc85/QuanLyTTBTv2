using System;
using System.Globalization;
using System.Threading.Tasks;
using FastReport;
using NMPrinting;
using QuanLyTTBTv2.Models.Local;
using QuanLyTTBTv2.Models.Server;
using QuanLyTTBTv2.Utilities;
using QuanLyTTBTv2.ViewModels.Printing;
using Serilog;

namespace QuanLyTTBTv2.Models;

public class ReportPrinting
{
    private readonly CultureInfo _culture;

    public string? ReportDocketTemplate { get; private set; }
    private readonly Report _reportDocket = new();
    public bool IsError { get; private set; }
    public string? Error { get; set; }
    
    public ReportPrinting(CultureInfo culture)
    {
        _culture = culture;
    }
    
    public void LoadTemplate(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            IsError = true;
            Error = "Chưa cài đặt mẫu in";
        }
        
        try
        {
            if (path == ReportDocketTemplate && !IsError) return;

            if (System.IO.File.Exists(path))
            {
                _reportDocket.Load(path);
                _reportDocket.Dictionary.Connections.Clear();
                ReportDocketTemplate = path;
                
                IsError = false;
                Error = null;
            }
            else
            {
                IsError = true;
                Error = "Không tìm thấy file mẫu in";
            }
        }
        catch (Exception ex)
        {
            IsError = true;
            Error = ex.Message;

            System.Diagnostics.Debug.WriteLine(ex.Message);
            Log.Error("DocketPrinting: {0}", ex.Message);
        }
    }
    
    /// <summary>
    /// In phiếu tổng hợp (không có chi tiết kl, cp, mẻ)
    /// </summary>
    public async Task<bool> Print(PrintInfo pinfo, PhieuInVM ph, HTPhieu? phCan = null, bool showDialog = false, int solan = 1)
    {
        try
        {
            var ci = pinfo.Company;
            _reportDocket.SetParameterValue("Title.CongTy.Ten", ci.Ten);
            _reportDocket.SetParameterValue("Title.CongTy.DiaChi", ci.DiaChi);
            _reportDocket.SetParameterValue("Title.CongTy.Sdt1", ci.Sdt1);
            _reportDocket.SetParameterValue("Title.CongTy.Sdt2", ci.Sdt2);

            _reportDocket.SetParameterValue("Tram.Ten", pinfo.TramTen);

            _reportDocket.SetParameterValue("Don.Ma", ph.DonMa);

            _reportDocket.SetParameterValue("Phieu.SoPhieu", ph.SoPhieu);
            _reportDocket.SetParameterValue("Phieu.SoPhieuDon", ph.SoPhieuIn);
            _reportDocket.SetParameterValue("Phieu.TG.BatDau", ph.TGBD);
            _reportDocket.SetParameterValue("Phieu.TG.KetThuc", ph.TGKT);
            _reportDocket.SetParameterValue("Phieu.TG.XuatTram.Ngay", ph.NgayRoiTram);
            _reportDocket.SetParameterValue("Phieu.TG.XuatTram.Gio", ph.GioRoiTram);

            _reportDocket.SetParameterValue("KH.Ten", ph.KHTen);
            _reportDocket.SetParameterValue("KH.DiaChi", ph.KHDiaChi);
            _reportDocket.SetParameterValue("KH.Sdt", ph.KHSdt);
            _reportDocket.SetParameterValue("CongTrinh.Ten", ph.CongTrinh);
            _reportDocket.SetParameterValue("CongTrinh.DiaChi", ph.DiaDiem);

            _reportDocket.SetParameterValue("Xe.BSX", ph.Bsx);
            _reportDocket.SetParameterValue("Xe.LaiXe", ph.LaiXe);

            _reportDocket.SetParameterValue("BT.CP.Ma", ph.BeTongMa);
            _reportDocket.SetParameterValue("BT.CP.Mac", ph.BeTongMac);
            _reportDocket.SetParameterValue("BT.CP.DoSut", ph.BeTongDoSut);
            _reportDocket.SetParameterValue("BT.CP.CuongDo", ph.BeTongCuongDo);
            _reportDocket.SetParameterValue("BT.CP.CotLieuMax", ph.BeTongCotLieuMax);


            
            // Tối đa 5 thành phần
            // for (int i = 0; i < ph.DsThanhPhan.Count && i < 7; i++)
            // {
            //     _reportDocket.SetParameterValue($"TP.TP{i + 1}", ph.DsThanhPhan[i].CongThuc);
            //     _reportDocket.SetParameterValue($"TP.KL{i + 1}", ph.DsThanhPhan[i].KL.ToString("F0"));
            // }


            await _reportDocket.PrepareAsync();
            using var ms = FastReportPrintUtils.GetReportAsPdfStreamAsync(_reportDocket);
            PrintUtils.PrintPdfMemoryStream(ms, showDialog, solan);

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return false;
        }
    }
}