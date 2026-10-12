using System;
using System.Globalization;
using System.Threading.Tasks;
using FastReport;
using NMPrinting;
using QuanLyTTBTv2.Models.Local;
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
    
    public async Task<bool> Print(PrintInfo pinfo, PhieuInVM ph, bool showDialog = false, int solan = 1)
    {
        try
        {
            var ci = pinfo.Company;
            _reportDocket.SetParameterValue("Title.CongTy.Ten", ci.Ten);
            _reportDocket.SetParameterValue("Title.CongTy.DiaChi", ci.DiaChi);
            _reportDocket.SetParameterValue("Title.CongTy.Sdt1", ci.Sdt1);
            _reportDocket.SetParameterValue("Title.CongTy.Sdt2", ci.Sdt2);

            _reportDocket.SetParameterValue("Phieu.SoPhieu", ph.SoPhieu);
            _reportDocket.SetParameterValue("Phieu.SoPhieuIn", ph.SoPhieuIn);
            
            _reportDocket.SetParameterValue("KhachHang.Ten", ph.KhachHang);
            _reportDocket.SetParameterValue("KhachHang.DiaChi", ph.DiaChiKH);
            _reportDocket.SetParameterValue("CongTrinh.Ten", ph.CongTrinh);
            _reportDocket.SetParameterValue("CongTrinh.DiaChi", ph.DiaDiem);


            _reportDocket.SetParameterValue("Xe.BSX", ph.Bsx);
            _reportDocket.SetParameterValue("Xe.LaiXe", ph.LaiXe);

            _reportDocket.SetParameterValue("BeTong.Ten", ph.BeTongTen);
            _reportDocket.SetParameterValue("BeTong.Mac", ph.BeTongMac);
            _reportDocket.SetParameterValue("BeTong.DoSut", ph.BeTongDoSut);
            _reportDocket.SetParameterValue("BeTong.CuongDo", ph.BeTongCuongDo);
            _reportDocket.SetParameterValue("BeTong.CotLieuMax", ph.BeTongCotLieuMax);



            // Tối đa 5 thành phần
            // for (int i = 0; i < ph.DsThanhPhan.Count && i < 7; i++)
            // {
            //     _reportDocket.SetParameterValue($"TP.TP{i + 1}", ph.DsThanhPhan[i].CongThuc);
            //     _reportDocket.SetParameterValue($"TP.KL{i + 1}", ph.DsThanhPhan[i].KL.ToString("F0"));
            // }

            _reportDocket.SetParameterValue($"NgayXuat", ph.TgRoiTram);

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