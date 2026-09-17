using System.Collections.Generic;
using QuanLyTTBTv2.Models.Server;

namespace QuanLyTTBTv2.ViewModels.Server;

public class HTMeVM
{
    public string? Stt { get; set; }
    public string? M3Tron { get; set; }
    public string? TongKL { get; set; }

    public string?[] TPs { get; set; } = new string?[15];

    public string? Tght { get; set; }
    public string? TrangThai { get; set; }

    public static HTMeVM FromCapPhoi(List<string> dsMaTP, Dictionary<string, HTThanhPhan> cttps)
    {
        HTMeVM tk = new()
        {
            M3Tron = "1 m³",
        };

        for (int i = 0; i < dsMaTP.Count; i++)
        {
            string ma = dsMaTP[i];
            if (cttps.TryGetValue(ma, out var cttp))
            {
                tk.TPs[i] = cttp.KlCongThuc.ToString();
            }
            else
            {
                tk.TPs[i] = ma;
            }
        }

        return tk;
    }
}