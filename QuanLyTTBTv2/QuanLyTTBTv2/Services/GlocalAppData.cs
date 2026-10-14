using System;
using System.Collections.Generic;
using System.Globalization;
using QuanLyTTBTv2.Models;
using QuanLyTTBTv2.ViewModels.Visual;

namespace QuanLyTTBTv2.Services
{
    /// <summary>
    /// Chứa thông tin dùng chung cho toàn bộ ứng dụng
    /// </summary>
    public class GlocalAppData
    {
        #region Singleton
        private static readonly Lazy<GlocalAppData> _instance = new(() => new GlocalAppData());
        public static GlocalAppData Instance => _instance.Value;

        private GlocalAppData()
        {
            _vnCulture.NumberFormat.NumberDecimalSeparator = ",";
            _vnCulture.NumberFormat.NumberGroupSeparator = "."; // nếu muốn ngăn cách hàng nghìn

            DocketPrint = new ReportPrinting(_vnCulture);
            DetailDocketPrint = new ReportPrinting(_vnCulture);

        }
        #endregion

        private readonly DbCache _c = DbCache.Instance;

        // Tạo CultureInfo dựa trên "en-US" nhưng đổi dấu thập phân thành ','
        private readonly CultureInfo _vnCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        public ReportPrinting DocketPrint { get; private set; }
        public ReportPrinting DetailDocketPrint { get; private set; }

        public PhieuInConfigVM ViewPhieuInConfig { get; private set; } = new PhieuInConfigVM();

        public int InitPrinting()
        {
            var t = _c.Settings.GetValue("print.phieu.coban");
            var path = System.IO.Path.Join(_c.DocketPath, t);
            DocketPrint.LoadTemplate(path);

            t = _c.Settings.GetValue("print.phieu.chitiet");
            path = System.IO.Path.Join(_c.DocketPath, t);
            DetailDocketPrint.LoadTemplate(path);

            return (DocketPrint.IsError ? 1 : 0) | (DetailDocketPrint.IsError ? 2 : 0);
        }
    }
}
