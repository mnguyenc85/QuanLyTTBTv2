using System;
using System.Globalization;
using System.Threading.Tasks;
using FastReport;
using NMPrinting;
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
    
    public async Task<bool> Print(PhieuInVM ph, bool showDialog = false, int solan = 1)
        {
            try
            {
                //_reportDocket.SetParameterValue("Title.CongTy.Ten", _c.Settings.GetValue("in.phieu.cty.ten"));
                //_reportDocket.SetParameterValue("Title.CongTy.DiaChi", _c.Settings.GetValue("in.phieu.cty.diachi"));
                //_reportDocket.SetParameterValue("Title.CongTy.Sdt1", _c.Settings.GetValue("in.phieu.cty.sdt1"));
                //_reportDocket.SetParameterValue("Title.CongTy.Sdt2", _c.Settings.GetValue("in.phieu.cty.sdt2"));

                _reportDocket.SetParameterValue("Phieu.SoPhieu", ph.Id);

                // if (ph.KhachHang != null)
                // {
                //     _reportDocket.SetParameterValue("KhachHang.Ten", ph.KhachHang.Ten);
                //     _reportDocket.SetParameterValue("KhachHang.DiaChi", ph.KhachHang.DiaChi);
                // }
                //if (ph.Xe != null)
                //    report.SetParameterValue("Xe.BSX", ph.Xe.BSX);
                //if (ph.LaiXe != null)
                //    report.SetParameterValue("Xe.LaiXe", ph.LaiXe.Ten);

                // Tối đa 5 thành phần
                // for (int i = 0; i < ph.DsThanhPhan.Count && i < 7; i++)
                // {
                //     _reportDocket.SetParameterValue($"TP.TP{i + 1}", ph.DsThanhPhan[i].CongThuc);
                //     _reportDocket.SetParameterValue($"TP.KL{i + 1}", ph.DsThanhPhan[i].KL.ToString("F0"));
                // }
                // _reportDocket.SetParameterValue($"NgayXuat", ph.Tgbd.ToString("dd/MM/yyyy HH:mm:ss"));

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