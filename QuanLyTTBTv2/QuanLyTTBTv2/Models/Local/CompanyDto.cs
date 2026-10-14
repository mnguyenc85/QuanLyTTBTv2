using System;
using QuanLyTTBTv2.Services;

namespace QuanLyTTBTv2.Models.Local
{
    public class CompanyDto
    {
        public string? Ten1 { get; set; }
        public string? Ten2 { get; set; }
        public string? DiaChi { get; set; }
        public string? Sdt1 { get; set; }
        public string? Sdt2 { get; set; }

        public void Load(DbSettings s)
        {
            Ten1 = s.GetValue("in.phieu.cty.ten1");
            Ten2 = s.GetValue("in.phieu.cty.ten2");
            DiaChi = s.GetValue("in.phieu.cty.diachi");
            Sdt1 = s.GetValue("in.phieu.cty.sdt1");
            Sdt2 = s.GetValue("in.phieu.cty.sdt2");
        }

        public void Save(DbSettings s) {
            s.Update("in.phieu.cty.ten1", Ten1);
            s.Update("in.phieu.cty.ten2", Ten2);
            s.Update("in.phieu.cty.diachi", DiaChi);
            s.Update("in.phieu.cty.sdt1", Sdt1);
            s.Update("in.phieu.cty.sdt2", Sdt2);
        }
    }

    public class PrintInfo()
    {
        public string? TramTen { get; set; }
        public CompanyDto Company { get; set; } = new();
    }
}
