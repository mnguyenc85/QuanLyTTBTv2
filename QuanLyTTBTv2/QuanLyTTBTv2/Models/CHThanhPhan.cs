namespace QuanLyTTBTv2.Models;

public class CHThanhPhan(string ma)
{
    public string Ma { get; set; } = ma;
    public string? Ten { get; set; }
    public int PL { get; set; }
    public int Silo { get; set; }

    public string GetHeader()
    {
        switch (PL)
        {
            case 1: return $"CL {Silo + 1}";
            case 2: return $"XM {Silo + 1}";
            case 3: return $"PG {Silo + 1}";
            case 4: return $"Nước";
            default: return "";
        }
    }
}