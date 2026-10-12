using System;
using QuanLyTTBTv2.Services;

namespace QuanLyTTBTv2.Models.Local
{
    public class CompanyDto
    {
        public string? Ten { get; set; }
        public string? DiaChi { get; set; }
        public string? Sdt1 { get; set; }
        public string? Sdt2 { get; set; }

        public void Load(DbSettings s)
        {
            Ten = s.GetValue("in.phieu.cty.ten");
            DiaChi = s.GetValue("in.phieu.cty.diachi");
            Sdt1 = s.GetValue("in.phieu.cty.sdt1");
            Sdt2 = s.GetValue("in.phieu.cty.sdt2");
        }

        public void Save(DbSettings s) {
            s.Update("in.phieu.cty.ten", Ten);
            s.Update("in.phieu.cty.diachi", DiaChi);
            s.Update("in.phieu.cty.sdt1", Sdt1);
            s.Update("in.phieu.cty.sdt2", Sdt2);
        }
    }

    public class PrintInfo()
    {
        public CompanyDto Company { get; set; } = new();
    }
}
