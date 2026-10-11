using System;
using System.Globalization;
using FastReport;
using Serilog;

namespace QuanLyTTBTv2.Services;

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
                Error = "Can't find template";
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
}