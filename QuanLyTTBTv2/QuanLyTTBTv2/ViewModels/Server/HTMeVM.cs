using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using QuanLyTTBTv2.Models;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.ViewModels.Server;

public class HTMeVM
{
    public string? Stt { get; set; }
    public string? M3Tron { get; set; }
    public string? TongKL { get; set; }
    public double KlTong { get; set; }
    
    public string?[] TPs { get; } = new string?[15];
    public double[] TpKls { get; } = new double[15];

    public string? Tght { get; set; }
    public string? TrangThai { get; set; }
    
    public string? Css { get; set; }

    public static HTMeVM FromCapPhoi(List<CHThanhPhan> dsMaTP, Dictionary<string, HTThanhPhan> cttps)
    {
        HTMeVM tk = new()
        {
            M3Tron = "CP chuẩn",
            Css = "cpc",
        };

        for (int i = 0; i < dsMaTP.Count; i++)
        {
            string ma = dsMaTP[i].Ma;
            if (cttps.TryGetValue(ma, out var cttp))
            {
                double kl = cttp.KlCongThuc;
                tk.TpKls[i] = kl;
                tk.TPs[i] = kl.ToString();
                tk.KlTong += kl;
            }
            else
            {
                tk.TPs[i] = "0";
            }
        }
        tk.TongKL = tk.KlTong.ToString("F0");
        
        return tk;
    }
    
    public static HTMeVM FromMe(List<CHThanhPhan> dsMaTP, Dictionary<string, HTThanhPhan> cttps, HtMe me)
    {
        HTMeVM tk = new()
        {
            M3Tron = me.M3Tron.ToString("F2"),
            Tght = me.LocalUpdatedAt.ToString("HH:mm:ss"),
            Css = "me",
        };

        for (int i = 0; i < dsMaTP.Count; i++)
        {
            string ma = dsMaTP[i].Ma;
            if (cttps.TryGetValue(ma, out var cttp))
            {
                var kl = me.GetKhoiLuong(cttp.PhanLoai, cttp.Silo);
                kl = cttp.PhanLoai == 3 ? Math.Round(kl, 2) : Math.Round(kl);
                
                tk.TpKls[i] = kl;
                tk.TPs[i] = kl.ToString();
                tk.KlTong += kl;
            }
            else
            {
                tk.TpKls[i] = 0;
                tk.TPs[i] = "0";
            }
        }

        tk.TongKL = tk.KlTong.ToString("F0");

        return tk;
    }

    public static HTMeVM CreateMeTong(List<HTMeVM> dsmetmp, int sotp = 15)
    {
        HTMeVM tk = new()
        {
            M3Tron = "Tổng",
            Css = "tong",
        };

        foreach (var me in dsmetmp)
        {
            for (int i = 0; i < sotp; i++)
                tk.TpKls[i] += me.TpKls[i];
            tk.KlTong += me.KlTong;
        }

        for (int i = 0; i < sotp; i++)
            tk.TPs[i] = tk.TpKls[i].ToString();
        
        
        tk.TongKL = tk.KlTong.ToString("F0");
        
        return tk;
    }

    public static HTMeVM FromThanhPhan(List<CHThanhPhan> dsMaThanhPhan)
    {
        HTMeVM tk = new()
        {
            Css = "tentp",
        };

        for (int i = 0; i < dsMaThanhPhan.Count; i++)
        {
            tk.TPs[i] = dsMaThanhPhan[i].Ten;
        }
        
        return tk;
    }

    public static HTMeVM FromCapPhoiMe(List<CHThanhPhan> dsMaThanhPhan, HTMeVM mecp, double ttme, int sotp = 15)
    {        
        HTMeVM tk = new()
        {
            M3Tron = "CP mẻ",
            Css = "cpc",
        };

        for (int i = 0; i < sotp; i++)
        {
            double kl = mecp.TpKls[i] * ttme;
            kl = dsMaThanhPhan[i].PL == 3 ? Math.Round(kl, 2) : Math.Round(kl);
            
            tk.TpKls[i] = kl;
            tk.TPs[i] = kl.ToString();
            tk.KlTong += kl;
        }
        tk.TongKL = tk.KlTong.ToString("F0");
        
        return tk;
    }
}